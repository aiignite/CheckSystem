using System;

namespace CheckSystem.Yfas.Utility._3TierModel
{
    /// <summary>
    /// YfasProductList:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class YfasProductInfo
    {
        public YfasProductInfo() { }

        #region Model
        private int _id;
        private string _yfaspn;
        private string _gmpn;
        private string _name;
        private string _rowindex;
        private string _pos;
        private string _carMode;
        private string _barcode;
        private string _treeView;
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
        public string YfasPn
        {
            set { _yfaspn = value; }
            get { return _yfaspn; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string GmPn
        {
            set { _gmpn = value; }
            get { return _gmpn; }
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
        public string RowIndex
        {
            set { _rowindex = value; }
            get { return _rowindex; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Pos
        {
            set { _pos = value; }
            get { return _pos; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string CarMode
        {
            set { _carMode = value; }
            get { return _carMode; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Barcode
        {
            set { _barcode = value; }
            get { return _barcode; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string TreeView
        {
            set { _treeView = value; }
            get { return _treeView; }
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

