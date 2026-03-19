using DBUtility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckSystem.SmtForms.DataUploader
{
    public static class DataUploaderSqlHelper
    {
        private static readonly object SqlLocker = new object();
        private const string Dbpath = @"SMT配置文件\SmtDataUploadRecord";
        private static readonly Sqlite Sqlite = new Sqlite(Dbpath);

        public static List<UploadConfigModel> GetConfigModels(
            string sql = "select * from DataConfig order by id asc")
        {
            var getData = Sqlite.GetRows(sql);

            return getData.Select(t => new UploadConfigModel
            {
                Id = int.Parse(t.ItemArray[0].ToString()),
                LineName = t.ItemArray[1].ToString(),
                Folder = t.ItemArray[2].ToString(),
                CreateTime = DateTime.Parse(t.ItemArray[3].ToString()),
                Type = t.ItemArray[4].ToString(),
                DeviceNo = t.ItemArray[5].ToString(),
            }).ToList();
        }
    }

    public class UploadConfigModel
    {
        private int _id;
        private string _lineName;
        private string _folder;
        private DateTime _createTime;
        private string _type;
        private string _deviceNo;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string LineName
        {
            get { return _lineName; }
            set { _lineName = value; }
        }

        public string Folder
        {
            get { return _folder; }
            set { _folder = value; }
        }

        public DateTime CreateTime
        {
            get { return _createTime; }
            set { _createTime = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }

        public string DeviceNo
        {
            get { return _deviceNo; }
            set { _deviceNo = value; }
        }
    }
}
