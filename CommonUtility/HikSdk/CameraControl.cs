using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using OpenCvSharp;

namespace CommonUtility.HikSdk
{
    public class CameraControl : IDisposable
    {
        public List<MyCamera> CameraList = new List<MyCamera>();

        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        private MvCameraSdk.MV_CC_DEVICE_INFO_LIST _mPDeviceList;
        private bool _isDisposed;

        public bool IsUseCustomSize = true;

        public CameraControl()
        {
            //AdjustNetworkSettings();

            Cv2.SetUseOptimized(true);
            Cv2.SetNumThreads(Environment.ProcessorCount);

            // 获取所有网络接口
            var interfaces = GetNetworkInterfaces();

            // 为每个接口创建并执行命令
            // foreach (var command in interfaces.Select(interfaceName => string.Format(
            //              "interface ipv4 set subinterface \"{0}\" mtu=1500 store=persistent", interfaceName)))
            //     ExecuteNetshCommand(command);
            foreach (var command in interfaces.Select(interfaceName => string.Format(
                         "interface ipv4 set subinterface \"{0}\" mtu=9014 store=persistent", interfaceName)))
                ExecuteNetshCommand(command);
        }

        private static IEnumerable<string> GetNetworkInterfaces()
        {
            var interfaces = new List<string>();

            try
            {
                var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter WHERE NetEnabled='True'");

                foreach (var o in searcher.Get())
                {
                    var mo = (ManagementObject)o;
                    if (mo["NetConnectionID"] == null)
                        continue;
                    var interfaceName = mo["NetConnectionID"].ToString();
                    interfaces.Add(interfaceName);
                    //Console.WriteLine($"Found interface: {interfaceName}");
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error getting network interfaces: {ex.Message}");
            }

            return interfaces;
        }

        private static void ExecuteNetshCommand(string arguments)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "netsh",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            process.StartInfo = startInfo;
            process.Start();

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            //Console.WriteLine($"Command: netsh {arguments}");
            //Console.WriteLine($"Output: {output}");
            if (!string.IsNullOrEmpty(error))
            {
                //Console.WriteLine($"Error: {error}");
            }
            //Console.WriteLine();
        }

        ~CameraControl() => Dispose(false);

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposeing)
        {
            if (!isDisposeing)
                return;

            // ...
            _isDisposed = true;
            CloseAllCamera();
        }

        /// <summary>
        /// 枚举设备
        /// </summary>
        public void DeviceListAcq()
        {
            GC.Collect();

            // Adjust network MTU size and other settings
            Process.Start("netsh", "interface ipv4 set subinterface \"Local Area Connection\" mtu=9000 store=persistent");

            var nRet = MvCameraSdk.MV_CC_EnumDevices_NET(MvCameraSdk.MV_GIGE_DEVICE | MvCameraSdk.MV_USB_DEVICE, ref _mPDeviceList);
            if (0 != nRet)
            {
                Console.WriteLine(@"Enumerate devices fail!");
                return;
            }

            for (var i = 0; i < _mPDeviceList.nDeviceNum; i++)
            {
                //ch:获取选择的设备信息 | en:Get Selected Device Information
                var device =
                    (MvCameraSdk.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(_mPDeviceList.pDeviceInfo[i], typeof(MvCameraSdk.MV_CC_DEVICE_INFO));

                if (device.nTLayerType != MvCameraSdk.MV_GIGE_DEVICE && device.nTLayerType != MvCameraSdk.MV_USB_DEVICE)
                    continue;

                var gigeInfo = (MvCameraSdk.MV_GIGE_DEVICE_INFO)MvCameraSdk.ByteToStruct(device.SpecialInfo.stGigEInfo, typeof(MvCameraSdk.MV_GIGE_DEVICE_INFO));

                if (gigeInfo.chSerialNumber.StartsWith("00D"))
                    continue;

                if (CameraList.Any(f => f.GigeInfo.chSerialNumber == gigeInfo.chSerialNumber))
                    continue;

                CameraList.Add(new MyCamera(_mPDeviceList.pDeviceInfo[i], gigeInfo));
            }
        }

        /// <summary>
        /// 打开所有摄像头
        /// </summary>
        public void OpenAllCamera()
        {
            foreach (var c in CameraList)
                c.OpenCamera();
        }

        /// <summary>
        /// 关闭所有摄像头
        /// </summary>
        public void CloseAllCamera()
        {
            foreach (var c in CameraList.Where(c => c != null))
                c.CloseCamera();
        }

        public void OpenCamearBySerialNo(string serialNo)
        {
            var c = CameraList.Find(f => f.GigeInfo.chSerialNumber == serialNo);
            if (c != null)
                c.OpenCamera();
        }

        public void OpenCamearByUserDefinedName(string userDefinedName)
        {
            var c = CameraList.Find(f => f.GigeInfo.chUserDefinedName == userDefinedName);
            if (c != null)
                c.OpenCamera();
        }

        public void SetCameraExposureTimeBySerialNo(string serialNo, int exposureTime)
        {
            var c = CameraList.Find(f => f.GigeInfo.chSerialNumber == serialNo);
            if (c != null)
                c.SetExposureTime(exposureTime);
        }

        public void SetCameraGainBySerialNo(string serialNo, int gain)
        {
            var c = CameraList.Find(f => f.GigeInfo.chSerialNumber == serialNo);
            if (c != null)
                c.SetGain(gain);
        }

        public void StartCameraGrabBySerialNo(string serialNo)
        {
            var c = CameraList.Find(f => f.GigeInfo.chSerialNumber == serialNo);
            if (c != null)
                c.StartGrab();
        }

        public void StopCameraGrabBySerialNo(string serialNo)
        {
            var c = CameraList.Find(f => f.GigeInfo.chSerialNumber == serialNo);
            if (c != null)
                c.StopGrab();
        }

        public void CaptureBySerialNo(string serialNo)
        {
            var c = CameraList.Find(f => f.GigeInfo.chSerialNumber == serialNo);
            if (c != null)
            {
                c.Capture(1, 500000);
                //c.StartGrab();
                //while (true)
                //{
                //    Thread.Sleep(500);

                //    if (c._isGotAllImg)
                //        break;
                //}
                //c.StopGrab();
            }
        }

        public void ClearBufferBySerialNo(string serialNo)
        {
            var c = CameraList.Find(f => f.GigeInfo.chSerialNumber == serialNo);
            if (c != null)
            {
                c.ClearBuffer();
            }
        }

        public void SaveImages(string serialNo)
        {
            var c = CameraList.Find(f => f.GigeInfo.chSerialNumber == serialNo);
            if (c != null)
            {
                c.SaveAllImg();
            }
        }

        /// <summary>
        /// 取流回调函数
        /// </summary>
        /// <param name="pData"></param>
        /// <param name="pFrameInfo"></param>
        /// <param name="pUser"></param>
        private void ImageCallBack(IntPtr pData, ref MvCameraSdk.MV_FRAME_OUT_INFO_EX pFrameInfo, IntPtr pUser)
        {
            int nIndex = (int)pUser;

            // ch:抓取的帧数 | en:Aquired Frame Number
            //++m_nFrames[nIndex];

            //lock (m_BufForSaveImageLock[nIndex])
            //{
            //    if (m_pSaveImageBuf[nIndex] == IntPtr.Zero || pFrameInfo.nFrameLen > m_nSaveImageBufSize[nIndex])
            //    {
            //        if (m_pSaveImageBuf[nIndex] != IntPtr.Zero)
            //        {
            //            Marshal.Release(m_pSaveImageBuf[nIndex]);
            //            m_pSaveImageBuf[nIndex] = IntPtr.Zero;
            //        }

            //        m_pSaveImageBuf[nIndex] = Marshal.AllocHGlobal((Int32)pFrameInfo.nFrameLen);
            //        if (m_pSaveImageBuf[nIndex] == IntPtr.Zero)
            //        {
            //            return;
            //        }
            //        m_nSaveImageBufSize[nIndex] = pFrameInfo.nFrameLen;
            //    }

            //    m_stFrameInfo[nIndex] = pFrameInfo;
            //    CopyMemory(m_pSaveImageBuf[nIndex], pData, pFrameInfo.nFrameLen);
            //}
        }
    }
}
