using System;

namespace CheckSystem.Yfas.Utility._3TierModel
{
    /// <summary>
    /// YfasPreProgramFuncCmd:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class YfasPreProgramFuncCmd
    {
        public YfasPreProgramFuncCmd()
        { }
        #region Model
        private int _id;
        private int? _funcid;
        private int? _type;
        private int? _controllerid;
        private string _tocomparename;
        private string _tocomparevalue;
        //private int? _isdelete;
        private int? _cmdtype;
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
        public int? FuncId
        {
            set { _funcid = value; }
            get { return _funcid; }
        }
        /// <summary>
        /// 1是，2是>，3是>=，4是<，5是<=
        /// </summary>
        public int? Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? ControllerId
        {
            set { _controllerid = value; }
            get { return _controllerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ToCompareName
        {
            set { _tocomparename = value; }
            get { return _tocomparename; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ToCompareValue
        {
            set { _tocomparevalue = value; }
            get { return _tocomparevalue; }
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
        /// 
        /// </summary>
        public int? CmdType
        {
            set { _cmdtype = value; }
            get { return _cmdtype; }
        }
        #endregion Model

    }
}

