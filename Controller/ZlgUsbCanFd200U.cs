using CommonUtility;
using CommonUtility.BusLoader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Controller
{
    [Description("CAN-Device")]
    public sealed class ZlgUsbCanFd200U : ControllerBase
    {
        public int CustomMaxLen = -1;

        public CanBus ZlgCanChannel0;
        public CanBus ZlgCanChannel1;

        private readonly IntPtr _deviceHandle;
        private const int Null = 0;
        private readonly IntPtr _channelHandle0;
        private readonly IntPtr _channelHandle1;
        private Property _property;
        private readonly Recvdatathread _can0RecvDataThread;
        private readonly Recvdatathread _can1RecvDataThread;

        /// <summary>
        /// EFF/SFF is set in the MSB
        /// </summary>
        private const uint CanEffFlag = 0x80000000U;

        /// <summary>
        /// remote transmission request
        /// </summary>
        private const uint CanRtrFlag = 0x40000000U;

        /// <summary>
        /// error message frame
        /// </summary>
        private const uint CanErrFlag = 0x20000000U;

        /// <summary>
        /// id
        /// </summary>
        private const uint CanIdFlag = 0x1FFFFFFFU;

        public ZlgUsbCanFd200U(string name)
            : base(name)
        {
            var deviceInfo = new DeviceInfo(Define.ZcanUsbcanfd200U, 2);
            var type = deviceInfo.DeviceType;
            _deviceHandle = Method.ZCAN_OpenDevice(type, 0, 0);

            var pcieCanfd = type == Define.ZcanPciecanfd100U ||
                type == Define.ZcanPciecanfd200U ||
                type == Define.ZcanPciecanfd400U;
            var usbCanfd = type == Define.ZcanUsbcanfd100U ||
                type == Define.ZcanUsbcanfd200U ||
                type == Define.ZcanUsbcanfdMini;
            var canfdDevice = usbCanfd || pcieCanfd;

            var ptr = Method.GetIProperty(_deviceHandle);
            if (Null == (int)ptr)
                return;

            _property = (Property)Marshal.PtrToStructure((IntPtr)(uint)ptr, typeof(Property));

            var config = new ZcanChannelInitConfig { canfd = { mode = 0x00 } };

            if (canfdDevice)
            {
                config.can_type = Define.TypeCanfd;
                config.canfd.abit_timing = 0x0001975E;
                config.canfd.dbit_timing = 0x0041020A;
            }

            var pConfig = Marshal.AllocHGlobal(Marshal.SizeOf(config));
            Marshal.StructureToPtr(config, pConfig, true);

            if (usbCanfd)
            {
                if (!SetCanfdStandard(0, 0)) //设置CANFD标准
                    return;

                //if (!setFdBaudrate(1000000, 1000000, 0))
                //{
                //    return;
                //}
            }
            else
            {
                if (!canfdDevice && !SetBaudrate(500000, 0))
                {
                    return;
                }
            }
            _channelHandle0 = Method.ZCAN_InitCAN(_deviceHandle, 0, pConfig);

            if (usbCanfd)
            {
                if (!SetCanfdStandard(0, 1)) //设置CANFD标准
                    return;
            }
            else
            {
                if (!canfdDevice && !SetBaudrate(500000, 1))
                {
                    return;
                }
            }
            _channelHandle1 = Method.ZCAN_InitCAN(_deviceHandle, 1, pConfig);
            Marshal.FreeHGlobal(pConfig);

            if (Null != (int)_channelHandle0)
            {
                if (usbCanfd)
                    if (!SetResistanceEnable(0))
                        return;

                if (canfdDevice && !SetFilter(0))
                    return;

                if (Method.ZCAN_StartCAN(_channelHandle0) != Define.StatusOk)
                {

                }
                else
                {
                    _can0RecvDataThread = new Recvdatathread();
                    _can0RecvDataThread.SetChannelHandle(_channelHandle0);
                    _can0RecvDataThread.SetStart(true);
                    _can0RecvDataThread.RecvCanData += AddCan0Data;
                    _can0RecvDataThread.RecvFdData += AddCan0Data;

                    ZlgCanChannel0 = new ZlgUsbCanChannel(this, name + "_can0", 0);
                }
            }

            if (Null != (int)_channelHandle1)
            {
                if (usbCanfd)
                    if (!SetResistanceEnable(1))
                        return;

                if (canfdDevice && !SetFilter(1))
                    return;

                if (Method.ZCAN_StartCAN(_channelHandle1) != Define.StatusOk)
                {

                }
                else
                {
                    _can1RecvDataThread = new Recvdatathread();
                    _can1RecvDataThread.SetChannelHandle(_channelHandle1);
                    _can1RecvDataThread.SetStart(true);
                    _can1RecvDataThread.RecvCanData += AddCan1Data;
                    _can1RecvDataThread.RecvFdData += AddCan1Data;

                    ZlgCanChannel1 = new ZlgUsbCanChannel(this, name + "_can1", 1);
                }
            }
        }

        ~ZlgUsbCanFd200U()
        {
            if (_can0RecvDataThread != null)
            {
                _can0RecvDataThread.RecvCanData -= AddCan0Data;
                _can0RecvDataThread.RecvFdData -= AddCan0Data;
            }

            Method.ZCAN_CloseDevice(_deviceHandle);
            Dispose();
        }

        private void AddCan0Data(ZcanReceiveData[] data, uint len)
        {
            if (ZlgCanChannel0 == null)
                return;

            var canPackages = new List<CanBus.CanDataPackage>();

            for (uint i = 0; i < len; ++i)
            {
                var can = data[i];
                var id = data[i].frame.can_id;
                var canId = GetId(id);
                var eff = IsEff(id) ? CanBus.CanType.Extended : CanBus.CanType.Standard;
                var rtr = IsRtr(id) ? CanBus.CanFormat.Remote : CanBus.CanFormat.Data;

                var canData = new List<byte>();

                for (uint j = 0; j < can.frame.can_dlc; ++j)
                    canData.Add(Convert.ToByte(can.frame.data[j]));

                canPackages.Add(
                    new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can, eff, rtr, canData.ToArray()));
            }

            ZlgCanChannel0.ReceiveCanDatas(canPackages.ToArray());
        }

        private void AddCan0Data(ZcanReceiveFdData[] data, uint len)
        {
            if (ZlgCanChannel0 == null)
                return;

            var canPackages = new List<CanBus.CanDataPackage>();

            for (uint i = 0; i < len; ++i)
            {
                var can = data[i];
                var id = data[i].frame.can_id;
                var canId = GetId(id);
                var eff = IsEff(id) ? CanBus.CanType.Extended : CanBus.CanType.Standard;
                var rtr = IsRtr(id) ? CanBus.CanFormat.Remote : CanBus.CanFormat.Data;

                var canData = new List<byte>();

                for (uint j = 0; j < can.frame.len; ++j)
                    canData.Add(Convert.ToByte(can.frame.data[j]));

                canPackages.Add(new CanBus.CanDataPackage(canId, CanBus.CanProtocol.CanFd, eff, rtr, canData.ToArray()));
            }

            ZlgCanChannel0.ReceiveCanDatas(canPackages.ToArray());
        }

        private void AddCan1Data(ZcanReceiveData[] data, uint len)
        {
            if (ZlgCanChannel1 == null)
                return;

            var canPackages = new List<CanBus.CanDataPackage>();

            for (uint i = 0; i < len; ++i)
            {
                var can = data[i];
                var id = data[i].frame.can_id;
                var canId = GetId(id);
                var eff = IsEff(id) ? CanBus.CanType.Extended : CanBus.CanType.Standard;
                var rtr = IsRtr(id) ? CanBus.CanFormat.Remote : CanBus.CanFormat.Data;

                var canData = new List<byte>();

                for (uint j = 0; j < can.frame.can_dlc; ++j)
                    canData.Add(Convert.ToByte(can.frame.data[j]));

                canPackages.Add(
                    new CanBus.CanDataPackage(canId, CanBus.CanProtocol.Can, eff, rtr, canData.ToArray()));
            }

            ZlgCanChannel1.ReceiveCanDatas(canPackages.ToArray());
        }

        private void AddCan1Data(ZcanReceiveFdData[] data, uint len)
        {
            if (ZlgCanChannel1 == null)
                return;

            var canPackages = new List<CanBus.CanDataPackage>();

            for (uint i = 0; i < len; ++i)
            {
                var can = data[i];
                var id = data[i].frame.can_id;
                var canId = GetId(id);
                var eff = IsEff(id) ? CanBus.CanType.Extended : CanBus.CanType.Standard;
                var rtr = IsRtr(id) ? CanBus.CanFormat.Remote : CanBus.CanFormat.Data;

                var canData = new List<byte>();

                for (uint j = 0; j < can.frame.len; ++j)
                    canData.Add(Convert.ToByte(can.frame.data[j]));

                canPackages.Add(new CanBus.CanDataPackage(canId, CanBus.CanProtocol.CanFd, eff, rtr, canData.ToArray()));
            }

            ZlgCanChannel1.ReceiveCanDatas(canPackages.ToArray());
        }

        /// <summary>
        /// 设置CANFD标准
        /// </summary>
        /// <param name="canfdStandard"></param>
        /// <param name="channelIndex"></param>
        /// <returns></returns>
        private bool SetCanfdStandard(int canfdStandard, int channelIndex)
        {
            var path = channelIndex + "/canfd_standard";
            var value = canfdStandard.ToString();
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            return 1 == _property.SetValue(path, value);
        }

        private bool setFdBaudrate(UInt32 abaud, UInt32 dbaud, int channel_index_)
        {
            string path = channel_index_ + "/canfd_abit_baud_rate";
            string value = abaud.ToString();
            if (1 != _property.SetValue(path, value))
            {
                return false;
            }
            path = channel_index_ + "/canfd_dbit_baud_rate";
            value = dbaud.ToString();
            if (1 != _property.SetValue(path, value))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 设置波特率
        /// </summary>
        /// <param name="baud"></param>
        /// <param name="channelIndex"></param>
        /// <returns></returns>
        private bool SetBaudrate(uint baud, int channelIndex)
        {
            var path = channelIndex + "/baud_rate";
            var value = baud.ToString();
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            return 1 == _property.SetValue(path, value);
        }

        /// <summary>
        /// 设置终端电阻使能
        /// </summary>
        /// <returns></returns>
        private bool SetResistanceEnable(int channelIndex)
        {
            var path = channelIndex + "/initenal_resistance";
            var value = "1";
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            return 1 == _property.SetValue(path, value);
        }

        /// <summary>
        /// 设置滤波
        /// </summary>
        /// <returns></returns>
        private bool SetFilter(int channelIndex)
        {
            var path = channelIndex + "/filter_clear";//清除滤波
            var value = "0";
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            if (0 == _property.SetValue(path, value))
            {
                return false;
            }

            path = channelIndex + "/filter_mode";
            value = 2.ToString();
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            if (0 == _property.SetValue(path, value))
            {
                return false;
            }

            path = channelIndex + "/filter_start";
            value = "0";
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            if (0 == _property.SetValue(path, value))
            {
                return false;
            }

            path = channelIndex + "/filter_end";
            value = "0";
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            if (0 == _property.SetValue(path, value))
            {
                return false;
            }

            path = channelIndex + "/filter_ack";//滤波生效
            value = "0";
            //char* pathCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(path).ToPointer();
            //char* valueCh = (char*)System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(value).ToPointer();
            if (0 == _property.SetValue(path, value))
            {
                return false;
            }

            //如果要设置多条滤波，在清除滤波和滤波生效之间设置多条滤波即可
            return true;
        }

        //private uint SendCanDatas(int canChIndex, IReadOnlyList<CanBus.CanDataPackage> datas)
        //{
        //    // 加分组，一次最多发500条

        //    var zcanTransmitDatas = new ZcanTransmitData[datas.Count];
        //    for (var i = 0; i < datas.Count; i++)
        //    {
        //        var p = datas[i];
        //        var frameTypeIndex = p.CanType == CanBus.CanType.Standard ? 0 : 1;
        //        const int sendTypeIndex = 0;

        //        var canData = new ZcanTransmitData
        //        {
        //            frame =
        //            {
        //                can_id = MakeCanId(p.CanId, frameTypeIndex, 0, 0),
        //                data = new byte[8]
        //            }
        //        };

        //        var canDataStr =
        //                p.CanData.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b) + " ")
        //                    .TrimEnd(' ');

        //        canData.frame.can_dlc = (byte)SplitData(canDataStr, ref canData.frame.data, 8);
        //        canData.transmit_type = sendTypeIndex;

        //        zcanTransmitDatas[i] = canData;
        //    }

        //    if (zcanTransmitDatas.Any())
        //    {
        //        var len = zcanTransmitDatas.Length;
        //        var size = Marshal.SizeOf(typeof(ZcanTransmitData));
        //        var ptr = Marshal.AllocHGlobal(len * size);
        //        for (var i = 0; i < len; i++)
        //        {
        //            var currentPtr = (IntPtr)((uint)ptr + i * size);
        //            Marshal.StructureToPtr(zcanTransmitDatas[i], currentPtr, true);
        //        }

        //        if (canChIndex == 0)
        //        {
        //            var result = Method.ZCAN_Transmit(_channelHandle0, ptr, (uint)len);
        //            Marshal.FreeHGlobal(ptr);

        //            return result;
        //        }
        //        else
        //        {
        //            var result = Method.ZCAN_Transmit(_channelHandle1, ptr, (uint)len);
        //            Marshal.FreeHGlobal(ptr);

        //            return result;
        //        }
        //    }

        //    // >1 = 发送成功
        //    // 0 = 发送失败
        //    return 0;
        //}

        private uint SendCanDatas(int canChIndex, IReadOnlyList<CanBus.CanDataPackage> datas)
        {
            // 加分组，一次最多发500条

            var canfdData = datas.ToList().FindAll(f => f.CanProtocol == CanBus.CanProtocol.CanFd);
            var speedCanFdData = datas.ToList().FindAll(f => f.CanProtocol == CanBus.CanProtocol.SpeedCanFd);
            var normalCanData = datas.ToList().FindAll(f => f.CanProtocol == CanBus.CanProtocol.Can);

            var toSendData = new List<CanBus.CanDataPackage[]> { canfdData.ToArray(), speedCanFdData.ToArray(), normalCanData.ToArray() };

            var errorCount = 0;

            foreach (var tsd in toSendData)
            {
                if (!tsd.Any())
                    continue;

                if (tsd.Any(f => f.CanProtocol == CanBus.CanProtocol.Can))
                {
                    var zcanTransmitDatas = new ZcanTransmitData[tsd.Length];
                    for (var i = 0; i < tsd.Length; i++)
                    {
                        var p = tsd[i];
                        var frameTypeIndex = p.CanType == CanBus.CanType.Standard ? 0 : 1;
                        const int sendTypeIndex = 0;

                        var canData = new ZcanTransmitData
                        {
                            frame =
                                {
                                    can_id = MakeCanId(p.CanId, frameTypeIndex, 0, 0),
                                    data = new byte[8]
                                }
                        };

                        var canDataStr =
                                p.CanData.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b) + " ")
                                    .TrimEnd(' ');

                        canData.frame.can_dlc = (byte)SplitData(canDataStr, ref canData.frame.data, 8);
                        canData.transmit_type = sendTypeIndex;

                        zcanTransmitDatas[i] = canData;
                    }

                    if (zcanTransmitDatas.Any())
                    {
                        var len = zcanTransmitDatas.Length;
                        var size = Marshal.SizeOf(typeof(ZcanTransmitData));
                        var ptr = Marshal.AllocHGlobal(len * size);
                        for (var i = 0; i < len; i++)
                        {
                            var currentPtr = (IntPtr)((uint)ptr + i * size);
                            Marshal.StructureToPtr(zcanTransmitDatas[i], currentPtr, true);
                        }

                        if (canChIndex == 0)
                        {
                            var result = Method.ZCAN_Transmit(_channelHandle0, ptr, (uint)len);
                            Marshal.FreeHGlobal(ptr);

                            //return result;
                            if (result != tsd.Length)
                                errorCount++;
                        }
                        else
                        {
                            var result = Method.ZCAN_Transmit(_channelHandle1, ptr, (uint)len);
                            Marshal.FreeHGlobal(ptr);

                            //return result;
                            if (result != tsd.Length)
                                errorCount++;
                        }
                    }
                }
                else
                {
                    var zcanTransmitDatas = new ZcanTransmitFdData[tsd.Length];
                    for (var i = 0; i < tsd.Length; i++)
                    {
                        var p = tsd[i];
                        var frameTypeIndex = p.CanType == CanBus.CanType.Standard ? 0 : 1;
                        const int sendTypeIndex = 0;

                        var canData = new ZcanTransmitFdData
                        {
                            frame = new CanfdFrame
                            {
                                can_id = MakeCanId(p.CanId, frameTypeIndex, 0, 0),
                                data = new byte[64]
                            }
                        };

                        var canDataStr =
                                p.CanData.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b) + " ")
                                    .TrimEnd(' ');

                        canData.frame.len = (byte)SplitData(canDataStr, ref canData.frame.data, 64);
                        canData.transmit_type = sendTypeIndex;
                        canData.frame.flags = p.CanProtocol == CanBus.CanProtocol.SpeedCanFd ? (byte)1 : (byte)0;

                        zcanTransmitDatas[i] = canData;
                    }

                    if (zcanTransmitDatas.Any())
                    {
                        var len = zcanTransmitDatas.Length;
                        var size = Marshal.SizeOf(typeof(ZcanTransmitFdData));
                        var ptr = Marshal.AllocHGlobal(len * size);
                        for (var i = 0; i < len; i++)
                        {
                            var currentPtr = (IntPtr)((uint)ptr + i * size);
                            Marshal.StructureToPtr(zcanTransmitDatas[i], currentPtr, true);
                        }

                        if (canChIndex == 0)
                        {
                            var result = Method.ZCAN_TransmitFD(_channelHandle0, ptr, (uint)len);
                            Marshal.FreeHGlobal(ptr);

                            //return result;
                            if (result != tsd.Length)
                                errorCount++;
                        }
                        else
                        {
                            var result = Method.ZCAN_TransmitFD(_channelHandle1, ptr, (uint)len);
                            Marshal.FreeHGlobal(ptr);

                            //return result;
                            if (result != tsd.Length)
                                errorCount++;
                        }
                    }
                }
            }

            return (uint)(errorCount == 0 ? 1 : 0);
        }

        private void NergeSendDatas(int canChIndex, IReadOnlyList<CanBus.CanDataPackage> datas)
        {
            var FRAME_COUNT = datas.Count;
            ZCANDataObj[] dataObjs = new ZCANDataObj[FRAME_COUNT];
            IntPtr pDataObjs = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ZCANDataObj)) * FRAME_COUNT);

            // canfd_frame
            var baseFrame = new CanfdFrame
            {
                can_id = MakeCanId(0x10, 0, 0, 0),  // ID
                len = 64,                           // 长度
                flags = 0x01,                       // 1-CANFD加速
                data = new byte[64]
            };

            for (int i = 0; i < baseFrame.data.Length; i++)
            {
                baseFrame.data[i] = (byte)(i % 0xFF);   // 数据
            }

            for (int i = 0; i < FRAME_COUNT; i++)
            {
                // ZCANCANFDData
                var canfdData = new ZcanTransmitFdData
                {
                    frame = baseFrame,
                    transmit_type = 1
                };

                // ZCANDataObj
                dataObjs[i] = new ZCANDataObj
                {
                    dataType = 1,               // 1-CAN/CANFD
                    chnl = 0,                   // 通道
                    flag = 0,                   // 未使用
                    data = new byte[92]
                };

                // 结构体转字节数组
                IntPtr pCanFd = Marshal.AllocHGlobal(Marshal.SizeOf(canfdData));
                try
                {
                    Marshal.StructureToPtr(canfdData, pCanFd, false);
                    Marshal.Copy(pCanFd, dataObjs[i].data, 0, Marshal.SizeOf(canfdData));
                }
                finally
                {
                    Marshal.FreeHGlobal(pCanFd);
                }

                // 拷贝到非托管内存
                Marshal.StructureToPtr(dataObjs[i], IntPtr.Add(pDataObjs, i * Marshal.SizeOf(typeof(ZCANDataObj))), true);
            }

            // 发送
            uint sentCount = Method.ZCAN_Transmit(_channelHandle0, pDataObjs, (uint)FRAME_COUNT);
            Console.WriteLine("合并发送 {0} 帧报文", sentCount);

            Marshal.FreeHGlobal(pDataObjs);
        }

        private uint SendCan0Data(uint canId, string data, CanBus.CanType canType)
        {
            var frameTypeIndex = canType == CanBus.CanType.Standard ? 0 : 1;
            const int sendTypeIndex = 0;

            var canData = new ZcanTransmitData
            {
                frame =
                {
                    can_id = MakeCanId(canId, frameTypeIndex, 0, 0),
                    data = new byte[8]
                }
            };
            canData.frame.can_dlc = (byte)SplitData(data, ref canData.frame.data, 8);
            canData.transmit_type = sendTypeIndex;
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(canData));
            Marshal.StructureToPtr(canData, ptr, true);
            var result = Method.ZCAN_Transmit(_channelHandle0, ptr, 1);
            Marshal.FreeHGlobal(ptr);

            return result;
        }

        private uint SendCanFd0Data(uint canId, string data, CanBus.CanType canType, CanBus.CanProtocol canProtocol = CanBus.CanProtocol.CanFd)
        {
            var frameTypeIndex = canType == CanBus.CanType.Standard ? 0 : 1;
            const int sendTypeIndex = 0;

            var canfdData = new ZcanTransmitFdData
            {
                frame =
                {
                    can_id = MakeCanId(canId, frameTypeIndex, 0, 0),
                    data = new byte[64]
                }
            };
            canfdData.frame.len = (byte)SplitData(data, ref canfdData.frame.data, 64);
            canfdData.transmit_type = sendTypeIndex;
            canfdData.frame.flags = canProtocol == CanBus.CanProtocol.SpeedCanFd ? (byte)1 : (byte)0;
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(canfdData));
            Marshal.StructureToPtr(canfdData, ptr, true);
            var result = Method.ZCAN_TransmitFD(_channelHandle0, ptr, 1);
            Marshal.FreeHGlobal(ptr);

            // 1 = 发送成功
            // 0 = 发送失败
            return result;
        }

        private uint SendCan1Data(uint canId, string data, CanBus.CanType canType)
        {
            var frameTypeIndex = canType == CanBus.CanType.Standard ? 0 : 1;
            const int sendTypeIndex = 0;

            var canData = new ZcanTransmitData
            {
                frame =
                {
                    can_id = MakeCanId(canId, frameTypeIndex, 0, 0),
                    data = new byte[8]
                }
            };
            canData.frame.can_dlc = (byte)SplitData(data, ref canData.frame.data, 8);
            canData.transmit_type = sendTypeIndex;
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(canData));
            Marshal.StructureToPtr(canData, ptr, true);
            var result = Method.ZCAN_Transmit(_channelHandle1, ptr, 1);
            Marshal.FreeHGlobal(ptr);

            return result;
        }

        private uint SendCanFd1Data(uint canId, string data, CanBus.CanType canType, CanBus.CanProtocol canProtocol = CanBus.CanProtocol.CanFd)
        {
            var frameTypeIndex = canType == CanBus.CanType.Standard ? 0 : 1;
            const int sendTypeIndex = 0;

            var canfdData = new ZcanTransmitFdData
            {
                frame =
                {
                    can_id = MakeCanId(canId, frameTypeIndex, 0, 0),
                    data = new byte[64]
                }
            };
            canfdData.frame.len = (byte)SplitData(data, ref canfdData.frame.data, 64);
            canfdData.transmit_type = sendTypeIndex;
            canfdData.frame.flags = canProtocol == CanBus.CanProtocol.SpeedCanFd ? (byte)1 : (byte)0;
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(canfdData));
            Marshal.StructureToPtr(canfdData, ptr, true);
            var result = Method.ZCAN_TransmitFD(_channelHandle1, ptr, 1);
            Marshal.FreeHGlobal(ptr);

            return result;
        }

        /// <summary>
        /// 1:extend frame 0:standard frame
        /// </summary>
        /// <param name="id"></param>
        /// <param name="eff"></param>
        /// <param name="rtr"></param>
        /// <param name="err"></param>
        /// <returns>1:extend frame 0:standard frame</returns>
        private static uint MakeCanId(uint id, int eff, int rtr, int err)
        {
            var ueff = (uint)(Convert.ToBoolean(eff) ? 1 : 0);
            var urtr = (uint)(Convert.ToBoolean(rtr) ? 1 : 0);
            var uerr = (uint)(Convert.ToBoolean(err) ? 1 : 0);
            return id | ueff << 31 | urtr << 30 | uerr << 29;
        }

        /// <summary>
        /// 拆分text到发送data数组
        /// </summary>
        /// <param name="data"></param>
        /// <param name="transData"></param>
        /// <param name="maxLen"></param>
        /// <returns></returns>
        private static int SplitData(string data, ref byte[] transData, int maxLen)
        {
            var dataArray = data.Split(' ');
            for (var i = 0; (i < maxLen) && (i < dataArray.Length); i++)
                transData[i] = Convert.ToByte(dataArray[i].Substring(0, 2), 16);

            return dataArray.Length;
        }

        /// <summary>
        /// 1:extend frame 0:standard frame
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsEff(uint id)
        {
            return Convert.ToBoolean(id & CanEffFlag);
        }

        /// <summary>
        /// 1:remote frame 0:data frame
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsRtr(uint id)
        {
            return Convert.ToBoolean(id & CanRtrFlag);
        }

        /// <summary>
        /// 1:error frame 0:normal frame
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsErr(uint id)
        {
            return Convert.ToBoolean(id & CanErrFlag);
        }

        public uint GetId(uint id)
        {
            return id & CanIdFlag;
        }

        public class ZlgUsbCanChannel : CanBus
        {
            private readonly ZlgUsbCanFd200U _zlgUsbCanFd200U;
            private readonly int _canIndex;
            private const int MaxCanDataLen = 60;

            public ZlgUsbCanChannel(
                ZlgUsbCanFd200U zlgUsbCanFd200Ustring, string name, int canIndex)
                : base(name, MaxCanDataLen)
            {
                _canIndex = canIndex;
                _zlgUsbCanFd200U = zlgUsbCanFd200Ustring;
                RegisterSendAction(SendMultipleCans);
            }

            //public void ReceiveMsg()
            //{

            //}

            private void SendMultipleCans(CanDataPackage[] dataPackages)
            {
                if (_zlgUsbCanFd200U == null || dataPackages == null)
                    return;

                //_zlgUsbCanFd200U.SendCanDatas(_canIndex, dataPackages);
                //return;

                {
                    var sendAct = new Action<CanDataPackage[]>(dp =>
                    {
                        _zlgUsbCanFd200U.SendCanDatas(_canIndex, dp);
                    });

                    var maxCount = MaxCanDataLen;
                    if (_zlgUsbCanFd200U.CustomMaxLen > 0)
                        maxCount = _zlgUsbCanFd200U.CustomMaxLen;

                    if (dataPackages.Length <= maxCount)
                        sendAct(dataPackages);
                    else
                    {
                        var count = dataPackages.Length / maxCount;
                        var rest = dataPackages.Length % maxCount;

                        var temp = dataPackages.ToList();
                        for (var i = 0; i < count; i++)
                        {
                            var sendTemp = new CanDataPackage[maxCount];
                            Array.Copy(temp.ToArray(), sendTemp, maxCount);
                            temp.RemoveRange(0, maxCount);
                            sendAct(sendTemp);
                        }

                        if (temp.Any())
                            sendAct(temp.ToArray());
                    }

                    return;
                }

                if (dataPackages.ToList().FindAll(f => f.CanProtocol == CanProtocol.CanFd).Any())
                {
                    foreach (var p in dataPackages)
                    {
                        var canData =
                            p.CanData.Aggregate(string.Empty, (current, b) => current + ValueHelper.GetHextStr(b) + " ")
                                .TrimEnd(' ');

                        if (_canIndex == 0)
                        {
                            if (p.CanProtocol == CanProtocol.Can)
                                _zlgUsbCanFd200U.SendCan0Data(p.CanId, canData, p.CanType);
                            else if (p.CanProtocol == CanProtocol.CanFd || p.CanProtocol == CanProtocol.SpeedCanFd)
                                _zlgUsbCanFd200U.SendCanFd0Data(p.CanId, canData, p.CanType, p.CanProtocol);
                        }
                        else if (_canIndex == 1)
                        {
                            if (p.CanProtocol == CanProtocol.Can)
                                _zlgUsbCanFd200U.SendCan1Data(p.CanId, canData, p.CanType);
                            else if (p.CanProtocol == CanProtocol.CanFd || p.CanProtocol == CanProtocol.SpeedCanFd)
                                _zlgUsbCanFd200U.SendCanFd1Data(p.CanId, canData, p.CanType, p.CanProtocol);
                        }
                    }
                }
                else
                {
                    var sendAct = new Action<CanDataPackage[]>(dp =>
                    {
                        _zlgUsbCanFd200U.SendCanDatas(_canIndex, dp);
                    });

                    if (dataPackages.Length <= MaxCanDataLen)
                        sendAct(dataPackages);
                    else
                    {
                        var count = dataPackages.Length / MaxCanDataLen;
                        var rest = dataPackages.Length % MaxCanDataLen;

                        var temp = dataPackages.ToList();
                        for (var i = 0; i < count; i++)
                        {
                            var sendTemp = new CanDataPackage[MaxCanDataLen];
                            Array.Copy(temp.ToArray(), sendTemp, MaxCanDataLen);
                            temp.RemoveRange(0, MaxCanDataLen);
                            sendAct(sendTemp);
                        }

                        if (temp.Any())
                            sendAct(temp.ToArray());
                    }
                }
            }
        }
    }
}
