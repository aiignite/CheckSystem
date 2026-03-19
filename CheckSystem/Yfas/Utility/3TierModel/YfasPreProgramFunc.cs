using System;

namespace CheckSystem.Yfas.Utility._3TierModel
{
    /// <summary>
    /// YfasPreProgramFunc:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class YfasPreProgramFunc
    {
        public YfasPreProgramFunc()
        { }
        #region Model
        private int _id;
        private string _name;
        //private int? _isdelete;
        //private int? _timeoutms;
        private string _row;
        private string _carmodel;
        private int? _functype;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? IsDelete;
        //{
        //    set { _isdelete = value; }
        //    get { return _isdelete; }
        //}

        /// <summary>
        /// <-1则为无超时时间设置，毫秒级别
        /// </summary>
        public int? TimeOutMs;
        //{
        //    set { _timeoutms = value; }
        //    get { return _timeoutms; }
        //}
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
        public int? FuncType
        {
            set { _functype = value; }
            get { return _functype; }
        }
        #endregion Model

    }
}

