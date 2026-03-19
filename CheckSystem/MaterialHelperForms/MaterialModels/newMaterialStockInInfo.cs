using System;

namespace CheckSystem.MaterialHelperForms.MaterialModels
{
    /// <summary>
    /// newMaterialStockInInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class NewMaterialStockInInfo
    {
        public NewMaterialStockInInfo()
        { }
        #region Model
        private int _id;
        private string _stockinno;
        private string _materialid;
        private string _materialno;
        private string _materialname;
        private string _modelname;
        private string _supplyno;
        private string _qualevel;
        private string _issporadic;
        private string _earmarks;
        private int? _neednum;
        private int? _scannum;
        private int? _restnum;
        private string _status;
        private DateTime? _completiontime;
        private DateTime? _canceltime;
        private string _cancelresult;
        private string _note;
        private DateTime? _createtime;
        private string _creator;
        private string _editor;
        private DateTime? _edittime;
        private int? _isvalid;
        private int? _isdelete;
        private string _parentid;
        private int? _itemorder;
        private string _itemversionid;
        private string _itemversionno;
        private string _itemworkflowcode;
        private string _itemworkflowstatus;
        private string _itemworkflowid;
        /// <summary>
        /// 主键
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 订单号
        /// </summary>
        public string StockInNo
        {
            set { _stockinno = value; }
            get { return _stockinno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string MaterialId
        {
            set { _materialid = value; }
            get { return _materialid; }
        }
        /// <summary>
        /// 物料号
        /// </summary>
        public string MaterialNo
        {
            set { _materialno = value; }
            get { return _materialno; }
        }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MaterialName
        {
            set { _materialname = value; }
            get { return _materialname; }
        }
        /// <summary>
        /// 产品型号
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// 供应商
        /// </summary>
        public string SupplyNo
        {
            set { _supplyno = value; }
            get { return _supplyno; }
        }
        /// <summary>
        /// 质量等级
        /// </summary>
        public string Qualevel
        {
            set { _qualevel = value; }
            get { return _qualevel; }
        }
        /// <summary>
        /// 入库方式
        /// </summary>
        public string Issporadic
        {
            set { _issporadic = value; }
            get { return _issporadic; }
        }
        /// <summary>
        /// 特定用途
        /// </summary>
        public string Earmarks
        {
            set { _earmarks = value; }
            get { return _earmarks; }
        }
        /// <summary>
        /// 订单数
        /// </summary>
        public int? NeedNum
        {
            set { _neednum = value; }
            get { return _neednum; }
        }
        /// <summary>
        /// 扫描数
        /// </summary>
        public int? ScanNum
        {
            set { _scannum = value; }
            get { return _scannum; }
        }
        /// <summary>
        /// 差额
        /// </summary>
        public int? RestNum
        {
            set { _restnum = value; }
            get { return _restnum; }
        }
        /// <summary>
        /// 订单状态
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompletionTime
        {
            set { _completiontime = value; }
            get { return _completiontime; }
        }
        /// <summary>
        /// 取消时间
        /// </summary>
        public DateTime? CancelTime
        {
            set { _canceltime = value; }
            get { return _canceltime; }
        }
        /// <summary>
        /// 取消结果
        /// </summary>
        public string CancelResult
        {
            set { _cancelresult = value; }
            get { return _cancelresult; }
        }
        /// <summary>
        /// 备注说明
        /// </summary>
        public string Note
        {
            set { _note = value; }
            get { return _note; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public string Creator
        {
            set { _creator = value; }
            get { return _creator; }
        }
        /// <summary>
        /// 编辑人
        /// </summary>
        public string Editor
        {
            set { _editor = value; }
            get { return _editor; }
        }
        /// <summary>
        /// 编辑时间
        /// </summary>
        public DateTime? EditTime
        {
            set { _edittime = value; }
            get { return _edittime; }
        }
        /// <summary>
        /// 有效标识
        /// </summary>
        public int? IsValid
        {
            set { _isvalid = value; }
            get { return _isvalid; }
        }
        /// <summary>
        /// 删除标识
        /// </summary>
        public int? IsDelete
        {
            set { _isdelete = value; }
            get { return _isdelete; }
        }
        /// <summary>
        /// 父节点
        /// </summary>
        public string ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        public int? ItemOrder
        {
            set { _itemorder = value; }
            get { return _itemorder; }
        }
        /// <summary>
        /// 版本id
        /// </summary>
        public string ItemVersionId
        {
            set { _itemversionid = value; }
            get { return _itemversionid; }
        }
        /// <summary>
        /// 版本号
        /// </summary>
        public string ItemVersionNo
        {
            set { _itemversionno = value; }
            get { return _itemversionno; }
        }
        /// <summary>
        /// 流程编码
        /// </summary>
        public string ItemWorkflowCode
        {
            set { _itemworkflowcode = value; }
            get { return _itemworkflowcode; }
        }
        /// <summary>
        /// 流程状态
        /// </summary>
        public string ItemWorkflowStatus
        {
            set { _itemworkflowstatus = value; }
            get { return _itemworkflowstatus; }
        }
        /// <summary>
        /// 流程id
        /// </summary>
        public string ItemWorkflowId
        {
            set { _itemworkflowid = value; }
            get { return _itemworkflowid; }
        }
        #endregion Model

    }
}

