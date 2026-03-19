using System;

namespace CheckSystem.Yfas.Utility._3TierModel
{
    /// <summary>
    /// YfasPreProgramAction:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class YfasPreProgramAction
    {
        public YfasPreProgramAction()
        { }
        #region Model
        private int _id;
        private string _name;
        //private string _row;
        //private string _carmodel;
        private int? _actiontype;
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
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Row;
        //{
        //    set { _row = value; }
        //    get { return _row; }
        //}
        /// <summary>
        /// 
        /// </summary>
        public string CarModel;
        //{
        //    set { _carmodel = value; }
        //    get { return _carmodel; }
        //}
        /// <summary>
        /// 自定义：100000，AD采样：200000，CAN通信函数：300000，MasterLIN通信函数：400000，SlaveLIN通信函数：50000，延时：60000
        /// </summary>
        public int? ActionType
        {
            set { _actiontype = value; }
            get { return _actiontype; }
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

