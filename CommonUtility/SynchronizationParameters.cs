using RestSharp;
using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace CommonUtility
{
    public static class SynchronizationParameters
    {
        public static void SendDeviceNoByApi(string deviceNo)
        {
            // 调用api发送当前设备号
        }

        public static bool GetWorkOrderNoByApi(string deviceNo, out string workOrderNo, out string pnNo, out string paraFilePath, out string originFileName, ref string errorCode)
        {
            workOrderNo = string.Empty;
            pnNo = string.Empty;
            paraFilePath = string.Empty;
            originFileName = string.Empty;

            var postUrl = "http://192.168.0.136:8021/api/Other/DownLoadFileByDevice?" + string.Format("DeviceNo={0}", deviceNo);
            //var postUrl = "http://192.168.0.136:8021/api/Other/DownLoadFileByDevice";

            var client = new RestClient(postUrl) { Timeout = 500 };
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            //request.AddParameter("application/json", string.Format("DeviceNo={0}", deviceNo), ParameterType.RequestBody);
            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var info = JsonConvert.DeserializeObject<WorkOrderInfo>(response.Content);

                if (info.Code == "200")
                {
                    if (info.Data != null && info.Data.Files.Any())
                    {
                        var data = info.Data;
                        var files = data.Files;
                        var fileName = files[0].FileName;
                        var remotePath = files[0].DownLoadUrl;

                        if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(remotePath) ||
                            string.IsNullOrEmpty(data.WorkOrderNo) ||
                            string.IsNullOrEmpty(data.ProcessInfoNo)) 
                            return false;

                        workOrderNo = data.WorkOrderNo;
                        pnNo = data.ProcessInfoNo;
                        paraFilePath = remotePath;//.@"E:\桌面\新建文件夹\WorkOrder\工单测试.xml";
                        originFileName = fileName;
                        return true;
                    }
                }
                else
                {
                    errorCode = info.Code + ":" + info.Info;
                }
            }
            else
            {
                errorCode = response.StatusCode.ToString();
            }

            return false;
        }

        public static void DownloadParameters(string paraFilePath)
        {

        }

        public static bool CompareParameters(string fileNameFromServer, string localName)
        {
            return string.Equals(fileNameFromServer, localName, StringComparison.CurrentCultureIgnoreCase);
        }

        public class WorkOrderInfo
        {
            public string Code { get; set; }
            public string Info { get; set; }

            public WorkOrderInfoData Data { get; set; }
        }

        public class WorkOrderInfoData
        {
            public string WorkOrderNo { get; set; }
            public string ProcessInfoNo { get; set; }
            public string ErrorMessage { get; set; }
            public WorkOrderFile[] Files { get; set; }
        }

        public class WorkOrderFile
        {
            public string FileName { get; set; }
            public string DownLoadUrl { get; set; }
        }
    }
}
