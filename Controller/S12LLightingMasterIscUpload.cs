using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Controller
{
    public sealed class S12LLightingMasterIscUpload : ControllerBase
    {
        [Description("R,上传结果")]
        public string UploadResult = string.Empty;

        public S12LLightingMasterIscUpload(string name)
            : base(name)
        {
        }

        ~S12LLightingMasterIscUpload()
        {
            Dispose();
        }

        [Description("R/W,主机名")]
        public string HostName = "172.31.3.97";

        [Description("R/W,端口号")]
        public int PortNo = 21;

        public string UploadFilePath = @"E:\Projects-2022\点灯&芯片相关\S12L\ECU\S12VAVE文件和软件安装包\S12L VAVE\2\APP.51";

        [Description("FTP上传")]
        public void FtpUpload()
        {
            UploadResult = string.Empty;

            var localFile = new FileInfo(UploadFilePath);

            if (!localFile.Exists)
            {
                UploadResult = "NG 未找到本地文件";
                return;
            }
            if (localFile.Length <= 0)
            {
                UploadResult = "NG 本地文件的长度不正确";
                return;
            }

            //var url = "ftp://" + HostName + "/" + localFile.Name;
            #region 先读取FTP文件路径下是否存在相同文件，如果有则先删除

            string[] currentFiles;
            if (!GetFileDetails(out currentFiles))
            {
                UploadResult = "NG FTP服务器连接失败";
                return;
            }

            if (currentFiles != null && currentFiles.Any() && currentFiles.Any(f => f.Contains(localFile.Name)))
            {
                if (!DeleteFile())
                {
                    UploadResult = "NG FTP服务器中存在相同文件，尝试删除但失败";
                    return;
                }
            }

            #endregion

            #region ftp上传文件

            if (!UploadFile(localFile))
            {
                UploadResult = "NG 上传文件到FTP服务器失败";
                return;
            }

            #endregion

            #region 上传完成后，读取ftp路径下的文件，判断是否存在相同文件以及长度与上传文件是否一致

            string[] filesAfterUpload;
            if (!GetFileDetails(out filesAfterUpload))
            {
                UploadResult = "NG 获取上传后FTP服务器上的文件列表失败";
                return;
            }

            if (filesAfterUpload == null || !filesAfterUpload.Any())
            {
                UploadResult = "NG 上传后未获取到FTP服务器上的任何文件";
                return;
            }

            var index = filesAfterUpload.ToList().FindIndex(f => f.Contains(localFile.Name));
            if (index == -1)
            {
                UploadResult = string.Format("NG 上传后未获取到FTP服务器上有'{0}'文件的信息", localFile.Name);
                return;
            }

            if (!filesAfterUpload[index].Contains(localFile.Length.ToString()))
            {
                UploadResult = string.Format("NG 上传后获取到FTP服务器上的'{0}'文件的长度不正确", localFile.Name);
                return;
            }

            #endregion

            UploadResult = "OK";
        }

        private static FtpWebRequest CreateRequest(string url, string method)
        {
            var request = (FtpWebRequest)WebRequest.Create(new Uri(url));
            request.Credentials = new NetworkCredential(string.Empty, string.Empty);
            request.Proxy = null;
            request.KeepAlive = true;
            request.UseBinary = true;
            request.UsePassive = true;
            request.EnableSsl = false;
            request.Method = method;
            return request;
        }

        private bool GetFileDetails(out string[] result)
        {
            //var localFile = new FileInfo(UploadFilePath);
            var temp = new List<string>();
            //建立连接
            var url = "ftp://" + HostName;
            try
            {
                var request = CreateRequest(url, WebRequestMethods.Ftp.ListDirectoryDetails);
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    var reader = new StreamReader(response.GetResponseStream(), Encoding.Default); //中文文件名
                    var line = reader.ReadLine();
                    while (line != null)
                    {
                        temp.Add(line);
                        line = reader.ReadLine();
                    }
                }

                result = temp.ToArray();
                temp.Clear();
                return true;
            }
            catch (Exception)
            {
                result = new string[0];
                return false;
            }
        }

        private bool UploadFile(FileInfo localFile)
        {
            try
            {
                //var localFile = new FileInfo(UploadFilePath);
                var url = "ftp://" + HostName + "/" + localFile.Name;

                var request = CreateRequest(url, WebRequestMethods.Ftp.UploadFile);

                //上传数据
                using (var rs = request.GetRequestStream())
                {
                    using (var fs = localFile.OpenRead())
                    {
                        var buffer = new byte[4096];//4K
                        var count = fs.Read(buffer, 0, buffer.Length);//每次从流中读4个字节再写入缓冲区
                        while (count > 0)
                        {
                            rs.Write(buffer, 0, count);
                            count = fs.Read(buffer, 0, buffer.Length);
                        }
                        fs.Close();
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private bool DeleteFile()
        {
            bool result;

            try
            {
                var localFile = new FileInfo(UploadFilePath);
                var url = "ftp://" + HostName + "/" + localFile.Name;

                var request = CreateRequest(url, WebRequestMethods.Ftp.DeleteFile);
                using (var response = (FtpWebResponse)request.GetResponse())
                {
                    result = true;
                }
            }
            catch (Exception)
            {

                result = false;
            }

            return result;
        }
    }
}
