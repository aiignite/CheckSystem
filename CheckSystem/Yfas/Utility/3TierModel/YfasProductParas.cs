using System;

namespace CheckSystem.Yfas.Utility._3TierModel
{
    /// <summary>
    /// YfasProductParas:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class YfasProductParas
    {
        public YfasProductParas()
        { }
        #region Model
        private int _id;
        private int? _productid;
        private int? _detectionid;
        private string _datetype;
        private string _okforamt;
        private string _value;
        private string _min;
        private string _max;
        private string _uint;
        private int? _bindcontrollerid;
        private string _bindcontrollerfieldname;
        private string _offset;
        //private int? _filter;
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
        public int? ProductId
        {
            set { _productid = value; }
            get { return _productid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? DetectionId
        {
            set { _detectionid = value; }
            get { return _detectionid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DateType
        {
            set { _datetype = value; }
            get { return _datetype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string OkForamt
        {
            set { _okforamt = value; }
            get { return _okforamt; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Value
        {
            set { _value = value; }
            get { return _value; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Min
        {
            set { _min = value; }
            get { return _min; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Max
        {
            set { _max = value; }
            get { return _max; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Uint
        {
            set { _uint = value; }
            get { return _uint; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? BindControllerId
        {
            set { _bindcontrollerid = value; }
            get { return _bindcontrollerid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string BindControllerFieldName
        {
            set { _bindcontrollerfieldname = value; }
            get { return _bindcontrollerfieldname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Offset
        {
            set { _offset = value; }
            get { return _offset; }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? Filter;
        //{
        //    set { _filter = value; }
        //    get { return _filter; }
        //}
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

