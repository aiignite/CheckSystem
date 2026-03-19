using System;

namespace CheckSystem.Yfas.Utility._3TierModel
{
    /// <summary>
    /// YfasPreProgramActCmd:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class YfasPreProgramActCmd
    {
        public YfasPreProgramActCmd()
        { }
        #region Model
        private int _id;
        private int _actionid;
        private int _type;
        private int _controllerid;
        private string _targetname;
        private string _targetparas;
        private int? _cmdtype;
        //private int _isdelete;
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
        public int ActionId
        {
            set { _actionid = value; }
            get { return _actionid; }
        }
        /// <summary>
        /// 1=SetField,2=InvokeMethod
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }
        /// <summary>
        /// -1=产品通讯控制器，1~5=56pin控制器
        /// </summary>
        public int ControllerId
        {
            set { _controllerid = value; }
            get { return _controllerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TargetName
        {
            set { _targetname = value; }
            get { return _targetname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TargetParas
        {
            set { _targetparas = value; }
            get { return _targetparas; }
        }
        /// <summary>
        /// 自定义：100000，AD采样：200000，CAN通信函数：300000，MasterLIN通信函数：400000，SlaveLIN通信函数：50000，延时：60000
        /// </summary>
        public int? CmdType
        {
            set { _cmdtype = value; }
            get { return _cmdtype; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int IsDelete;

        //{
        //    set { _isdelete = value; }
        //    get { return _isdelete; }
        //}

        #endregion Model

    }
}

