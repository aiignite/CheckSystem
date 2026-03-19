using System;

namespace CheckSystem.Yfas.Utility._3TierModel
{
    /// <summary>
    /// YfasCheckData:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class YfasCheckData
    {
        public YfasCheckData()
        { }
        #region Model
        private int _id;
        //private string _taskno;
        private string _productno;
        private string _productuid;
        //private string _pcbano;
        //private string _pcbabarcode;
        //private string _productbarcode;
        //private string _packagebarcode;
        //private string _processno;
        private string _checkdata;
        //private DateTime? _checkdate;
        private string _checkstaffno;
        private string _checkresult;
        private string _creater;
        private DateTime _createtime;
        //private string __checkdate;
        /// <summary>
        /// 
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string taskNo;
        /// <summary>
        /// 
        /// </summary>
        public string productNo
        {
            set { _productno = value; }
            get { return _productno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string productUid
        {
            set { _productuid = value; }
            get { return _productuid; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string pcbaNo;

        /// <summary>
        /// 
        /// </summary>
        public string pcbaBarcode;

        /// <summary>
        /// 
        /// </summary>
        public string productBarcode;

        /// <summary>
        /// 
        /// </summary>
        public string packageBarcode;

        /// <summary>
        /// 
        /// </summary>
        public string processNo;
        /// <summary>
        /// 
        /// </summary>
        public string checkData
        {
            set { _checkdata = value; }
            get { return _checkdata; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? checkDate;
        /// <summary>
        /// 
        /// </summary>
        public string checkStaffNo
        {
            set { _checkstaffno = value; }
            get { return _checkstaffno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string checkResult
        {
            set { _checkresult = value; }
            get { return _checkresult; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string creater
        {
            set { _creater = value; }
            get { return _creater; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime createTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string _checkDate;

        #endregion Model

    }
}

