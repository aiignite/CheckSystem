using CommonUtility;
using CommonUtility.FileOperator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace Controller
{
    public sealed class WriteNowApi : ControllerBase
    {
        [Description("R,通道1烧写结果")]
        public string Site1DownloadResult;
        [Description("R,通道2烧写结果")]
        public string Site2DownloadResult;
        [Description("R,通道3烧写结果")]
        public string Site3DownloadResult;
        [Description("R,通道4烧写结果")]
        public string Site4DownloadResult;
        [Description("R,通道5烧写结果")]
        public string Site5DownloadResult;
        [Description("R,通道6烧写结果")]
        public string Site6DownloadResult;
        [Description("R,通道7烧写结果")]
        public string Site7DownloadResult;
        [Description("R,通道8烧写结果")]
        public string Site8DownloadResult;

        [Description("R/W,生成Wni文件的保存路径")]
        public string GenerateWniSaveFilePath = string.Empty;
        //string.Format(@"G:\OsramEviyos\WriteNowOsramEviyosDebugUpload{0}.wni",
        //Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8));
        [Description("R/W,需要生成的BIN文件路径")]
        public string ToGenerateBinFilePath =
          string.Empty;//  @"G:\OsramEviyos\SAVE-OSRAM\KEWGBBMD1U_79952_10_10_7_0P8JNF0403.bin";
        [Description("R,判断生成的文件是否存在")]
        public string CheckFileExit = "NG";

        [Description("R/W,需要下载的文件目录")]
        public string DownLoadFileFolder = string.Empty;//@"G:\OsramEviyos\DownloadFile\";

        public WriteNowApi(string name)
            : base(name) { }

        private IntPtr Handle { get; set; }
        private string SelectedProjName { get; set; }

        [Description("连接烧写器网口")]
        public void ConnectLan(string ipPort)
        {
            Handle = WriteNowControlApi.ConnectWriteNowLan(ipPort);
        }

        [Description("断开烧写器网口")]
        public void DisConnectLan()
        {
            if (Handle != (IntPtr)0)
            {
                WriteNowControlApi.DisConnectWriteNow(Handle);
            }
        }

        /// <summary>
        /// 选项目名称
        /// </summary>
        /// <param name="projectName"></param>
        [Description("选项目名称")]
        public void SelectProjectName(string projectName)
        {
            SelectedProjName = projectName;
        }

        /// <summary>
        /// RUN单通道
        /// </summary>
        /// <param name="siteIndex">通道号</param>
        [Description("RUN单通道")]
        public void RunSingleSite(string siteIndex)
        {
            var i = int.Parse(siteIndex);
            var selectedIndex = string.Empty;

            if (i == 1)
                selectedIndex = "01";
            else if (i == 2)
                selectedIndex = "02";
            else if (i == 3)
                selectedIndex = "04";
            else if (i == 4)
                selectedIndex = "08";
            else if (i == 5)
                selectedIndex = "10";
            else if (i == 6)
                selectedIndex = "20";
            else if (i == 7)
                selectedIndex = "40";
            else if (i == 8)
                selectedIndex = "80";

            var resultStr = "NG";
            var resultField = GetType().GetField(string.Format("Site{0}DownloadResult", siteIndex));
            resultField.SetValue(this, resultStr);

            if (Handle == (IntPtr)0)
            {
                resultStr = "NG 烧写器未连接";
                resultField.SetValue(this, resultStr);
                return;
            }
            var str = string.Format(@"#exec -o prj -f \projects\{0}.wnp -s h{1}", SelectedProjName, selectedIndex);
            var result = WriteNowControlApi.ExeCommand(Handle, str, 120 * 1000);

            resultStr = result.Equals(">") ? "OK" : "NG";
            resultField.SetValue(this, resultStr);
        }

        /// <summary>
        /// RUN多通道
        /// </summary>
        /// <param name="startEnd">例1~4</param>
        public void RunRangeSite(string startEnd)
        {
            var sp = startEnd.Split('~');
            var start = int.Parse(sp[0]) - 1;
            var end = int.Parse(sp[1]) - 1;

            var siteListStr = new[]
            {
                0.ToString(), 0.ToString(), 0.ToString(), 0.ToString(),
                0.ToString(), 0.ToString(), 0.ToString(), 0.ToString()
            };

            for (var i = start; i <= end; i++)
            {
                siteListStr[i] = 1.ToString();

                var resultStr = "NG";
                var resultField = GetType().GetField(string.Format("Site{0}DownloadResult", i + 1));
                resultField.SetValue(this, resultStr);

                if (Handle != (IntPtr)0)
                    continue;
                resultStr += @" 串口或网口异常";
                resultField.SetValue(this, resultStr);
            }

            if (Handle != (IntPtr)0)
            {
                var str = string.Empty;
                for (var i = siteListStr.Length - 1; i >= 0; i--)
                    str += siteListStr[i];
                var siteHex = ValueHelper.GetHextStr(Convert.ToByte(str, 2));
                var sendStr = string.Format(@"#exec -o prj -f \projects\{0}.wnp -s h{1}", SelectedProjName, siteHex);
                var result = WriteNowControlApi.ExeCommand(Handle, str);

                var readBuffer = result.Select(t => t.ToString()).ToList();

                if (readBuffer.Any(f => !f.Equals("*") && !f.Equals(">") && !string.IsNullOrEmpty(f)))
                {
                    var find =
                        readBuffer.FindIndex(
                            f =>
                                !string.IsNullOrEmpty(f) && f.StartsWith("h", StringComparison.OrdinalIgnoreCase) &&
                                f.EndsWith("!") && f.Length == 8 + 2);

                    if (find != -1)
                    {
                        var hexStr = Convert.ToByte("0x" + readBuffer[find].Substring(7, 2), 16);
                        var bitStr = Convert.ToString(hexStr, 2).PadLeft(8, '0');

                        var tempBitList = new List<string>();
                        for (var i = bitStr.Length - 1; i >= 0; i--)
                            tempBitList.Add(bitStr[i].ToString());

                        for (var i = 0; i < tempBitList.Count; i++)
                        {
                            if (i < start || i > end)
                                continue;
                            var resultStr = tempBitList[i] == "1" ? "NG" : "OK";

                            var resultField = GetType().GetField(string.Format("Site{0}DownloadResult", i + 1));
                            resultField.SetValue(this, resultStr);
                        }
                    }
                }
            }
        }

        [Description("生成Wni文件")]
        public void GenerateBin2WniFile()
        {
            CheckFileExit = "NG";
            if (string.IsNullOrEmpty(GenerateWniSaveFilePath) || string.IsNullOrEmpty(ToGenerateBinFilePath))
                return;
            if (File.Exists(GenerateWniSaveFilePath))
                File.Delete(GenerateWniSaveFilePath);
            Thread.Sleep(3000);
            if (!File.Exists(ToGenerateBinFilePath))
                return;
            var okCount = 0;
            var ngCount = 0;
            for (var kk = 0; kk < 50; kk++)
            {
                using (var process = new Process())
                {
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.CreateNoWindow = true;
                    process.Start();
                    //process.StandardInput.AutoFlush = true;

                    var wnBin2WniExe = string.Format(@"{0}\{1}\{2}", Directory.GetCurrentDirectory(), "WriteNow!Developer",
                        "wn_bin2wni.exe");
                    var cmd = string.Format(
                        "\"{0}\" -load FILE -filein \"{1}\" -format BIN -generate -fileout \"{2}\" -format WNI -fillout HFF -fillmode disable -report off",
                        wnBin2WniExe, ToGenerateBinFilePath, GenerateWniSaveFilePath);

                    process.StandardInput.WriteLine(cmd);
                    //process.WaitForExit();
                    Thread.Sleep(1000);
                    process.Kill();            //var process = new Process { StartInfo = { FileName = "cmd.exe" } };

                };
                Thread.Sleep(100);
                if (File.Exists(GenerateWniSaveFilePath))
                {
                    break;
                    //OkCount++;
                    //File.Delete(GenerateWniSaveFilePath);
                    //Thread.Sleep(1000);
                }

                ngCount++;
                Thread.Sleep(1000);
                Console.WriteLine($"OKCount:{okCount}, NGCount:{ngCount} ");
            }

            Thread.Sleep(500);
            if (File.Exists(GenerateWniSaveFilePath))
                CheckFileExit = "OK";
        }

        public void DeleteWniFile()
        {
            if (string.IsNullOrEmpty(GenerateWniSaveFilePath) || string.IsNullOrEmpty(ToGenerateBinFilePath))
                return;
            if (File.Exists(GenerateWniSaveFilePath))
                File.Delete(GenerateWniSaveFilePath);
            Thread.Sleep(100);
        }

        public string DownloadFilePath = string.Empty;

        [Description("下载文件")]
        public void DownloadFile(string fileName)
        {
            DownloadFilePath = string.Empty;

            if (Handle == (IntPtr)0)
                return;

            DownloadFilePath = string.Format(@"{0}\{1}", DownLoadFileFolder, fileName);

            if (File.Exists(DownloadFilePath))
                File.Delete(DownloadFilePath);
            WriteNowControlApi.ReceiveFile(Handle, fileName, DownLoadFileFolder);

            //Thread.Sleep(3000);
        }

        [Description("删除文件")]
        public void DeleteFile(string fileName)
        {
            var DownloadFilePaths = string.Empty;

            if (Handle == (IntPtr)0)
                return;

            DownloadFilePaths = string.Format(@"{0}\{1}", DownLoadFileFolder, fileName);

            if (File.Exists(DownloadFilePaths))
                File.Delete(DownloadFilePaths);

            Thread.Sleep(100);
        }

        public string UploadFilePath = string.Empty;

        [Description("上传文件")]
        public void UploadFile()
        {
            if (Handle == (IntPtr)0)
                return;
            if (File.Exists(UploadFilePath))
            {
                WriteNowControlApi.SendFile(Handle, UploadFilePath);

                //WriteNowControlApi.SendFile(Handle, @"G:\OsramEviyos\dump.bin");
                //WriteNowControlApi.ExeCommand(Handle, @"#data -o set -c in -t file -f \images\WriteNowOsramEviyos.wni");
                //WriteNowControlApi.ExeCommand(Handle, @"#data -o set -c out -t file -f \images\dump.bin");

                //WriteNowControlApi.SendFile2(Handle, @"C:\Program Files (x86)\Algocraft\WriteNow! Software 2.24\projects\OsramEyios.wnp");
            }
        }

        #region 20250107 远程目录访问获取bin

        [Description("R/W,bin文件的远程目录")]
        public string RemoteBinFilePath = string.Empty;
        [Description("R/W,bin文件的远程目录用户名")]
        public string RemoteBinFileUserName = string.Empty;
        [Description("R/W,bin文件的远程目录密码")]
        public string RemoteBinFilePassword = string.Empty;

        [Description("生成Wni文件")]
        public void GenerateRemoteBin2WniFile()
        {
            CheckFileExit = "NG";
            if (string.IsNullOrEmpty(GenerateWniSaveFilePath) || string.IsNullOrEmpty(ToGenerateBinFilePath))
                return;
            if (File.Exists(GenerateWniSaveFilePath))
                File.Delete(GenerateWniSaveFilePath);
            Thread.Sleep(3000);

            RemoteFileHelper.ConnectToRemoteDirectory(RemoteBinFilePath, RemoteBinFileUserName, RemoteBinFilePassword);
            if (!File.Exists(ToGenerateBinFilePath))
            {
                RemoteFileHelper.DisconectFromRemoteDirectory(RemoteBinFilePath);
                return;
            }

            var okCount = 0;
            var ngCount = 0;

            for (var kk = 0; kk < 50; kk++)
            {
                try
                {
                    using (var process = new Process())
                    {
                        process.StartInfo.FileName = "cmd.exe";
                        process.StartInfo.UseShellExecute = false;
                        process.StartInfo.RedirectStandardInput = true;
                        process.StartInfo.RedirectStandardOutput = true;
                        process.StartInfo.RedirectStandardError = true;
                        process.StartInfo.CreateNoWindow = true;
                        process.Start();
                        //process.StandardInput.AutoFlush = true;

                        var wnBin2WniExe = string.Format(@"{0}\{1}\{2}", Directory.GetCurrentDirectory(),
                            "WriteNow!Developer",
                            "wn_bin2wni.exe");
                        var cmd = string.Format(
                            "\"{0}\" -load FILE -filein \"{1}\" -format BIN -generate -fileout \"{2}\" -format WNI -fillout HFF -fillmode disable -report off",
                            wnBin2WniExe, ToGenerateBinFilePath, GenerateWniSaveFilePath);

                        process.StandardInput.WriteLine(cmd);
                        //process.WaitForExit();
                        Thread.Sleep(1000);
                        process.Kill(); //var process = new Process { StartInfo = { FileName = "cmd.exe" } };
                    }

                    ;
                    Thread.Sleep(100);
                    if (File.Exists(GenerateWniSaveFilePath))
                    {
                        break;
                        //OkCount++;
                        //File.Delete(GenerateWniSaveFilePath);
                        //Thread.Sleep(1000);
                    }

                    ngCount++;
                    Thread.Sleep(1000);
                    Console.WriteLine($"OKCount:{okCount}, NGCount:{ngCount} ");
                }
                catch (Exception e)
                {
                    ngCount++;
                    Console.WriteLine(e);
                }
            }

            RemoteFileHelper.DisconectFromRemoteDirectory(RemoteBinFilePath);

            Thread.Sleep(500);
            if (File.Exists(GenerateWniSaveFilePath))
                CheckFileExit = "OK";
        }

        #endregion
    }
}
