using System;

namespace CheckSystem.Yfas.Utility._3TierModel
{
    /// <summary>
    /// YfasDetectionBase:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class YfasDetectionBase
    {
        public YfasDetectionBase()
        { }
        #region Model
        private int _id;
        private string _detectionname;
        private string _row;
        private string _carmodel;
        //private int? _isdelete;

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
        public string DetectionName
        {
            set { _detectionname = value; }
            get { return _detectionname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Row
        {
            set { _row = value; }
            get { return _row; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CarModel
        {
            set { _carmodel = value; }
            get { return _carmodel; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? IsDelete;

        //{
        //    set { _isdelete = value; }
        //    get { return _isdelete; }
        //}

        #endregion Model

    }
}

