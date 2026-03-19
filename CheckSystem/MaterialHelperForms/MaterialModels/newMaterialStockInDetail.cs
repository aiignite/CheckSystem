using System;

namespace CheckSystem.MaterialHelperForms.MaterialModels
{
    /// <summary>
    /// newMaterialStockInDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class NewMaterialStockInDetail
    {
        public NewMaterialStockInDetail()
        { }
        #region Model
        private int _id;
        private string _stockinno;
        private string _materialid;
        private string _materialno;
        private string _materialname;
        private string _materialbarcode;
        private string _materialprintno;
        private string _modelname;
        private string _supplyno;
        private string _trayno;
        private int? _count;
        private string _lotno;
        private string _dclotno;
        private string _stockindate;
        private string _supplyledgroup;
        private string _supplyledno;
        private string _expdate;
        private string _qualevel;
        private string _issporadic;
        private string _earmarks;
        private string _equipno;
        private string _orderno;
        private string _other;
        private string _other2;
        private string _other3;
        private string _status;
        private string _remarks;
        private DateTime? _sendtime;
        private DateTime? _backtime;
        private string _creator;
        private DateTime? _createtime;
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
        /// @controlType:LTextBox,@labelString:主键,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@inSearchPanel:False,@index:
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:入库单号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string StockInNo
        {
            set { _stockinno = value; }
            get { return _stockinno; }
        }
        /// <summary>
        /// 物料Id
        /// </summary>
        public string MaterialId
        {
            set { _materialid = value; }
            get { return _materialid; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:物料号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string MaterialNo
        {
            set { _materialno = value; }
            get { return _materialno; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:物料描述,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string MaterialName
        {
            set { _materialname = value; }
            get { return _materialname; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:扫描条码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string MaterialBarcode
        {
            set { _materialbarcode = value; }
            get { return _materialbarcode; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:打印条码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string MaterialPrintNo
        {
            set { _materialprintno = value; }
            get { return _materialprintno; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:产品型号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:供应商代码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string SupplyNo
        {
            set { _supplyno = value; }
            get { return _supplyno; }
        }
        /// <summary>
        /// 托盘号
        /// </summary>
        public string TrayNo
        {
            set { _trayno = value; }
            get { return _trayno; }
        }
        /// <summary>
        /// 规格数量
        /// </summary>
        public int? Count
        {
            set { _count = value; }
            get { return _count; }
        }
        /// <summary>
        /// 生产批号
        /// </summary>
        public string LotNo
        {
            set { _lotno = value; }
            get { return _lotno; }
        }
        /// <summary>
        /// 生产批次
        /// </summary>
        public string DclotNo
        {
            set { _dclotno = value; }
            get { return _dclotno; }
        }
        /// <summary>
        /// 入库日期
        /// </summary>
        public string StockInDate
        {
            set { _stockindate = value; }
            get { return _stockindate; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:特征值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string SupplyLedGroup
        {
            set { _supplyledgroup = value; }
            get { return _supplyledgroup; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:特征值编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string SupplyLedNo
        {
            set { _supplyledno = value; }
            get { return _supplyledno; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:到期日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string Expdate
        {
            set { _expdate = value; }
            get { return _expdate; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:质量等级,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string Qualevel
        {
            set { _qualevel = value; }
            get { return _qualevel; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:入库方式,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string Issporadic
        {
            set { _issporadic = value; }
            get { return _issporadic; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:特定用途,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string Earmarks
        {
            set { _earmarks = value; }
            get { return _earmarks; }
        }
        /// <summary>
        /// 扫描设备号
        /// </summary>
        public string EquipNo
        {
            set { _equipno = value; }
            get { return _equipno; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:序列号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string OrderNo
        {
            set { _orderno = value; }
            get { return _orderno; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:其他参数1,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@Writeable:False,@inSearchPanel:False,@index:
        /// </summary>
        public string Other
        {
            set { _other = value; }
            get { return _other; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:其他参数2,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@Writeable:False,@inSearchPanel:False,@index:
        /// </summary>
        public string Other2
        {
            set { _other2 = value; }
            get { return _other2; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:其他参数3,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@Writeable:False,@inSearchPanel:False,@index:
        /// </summary>
        public string Other3
        {
            set { _other3 = value; }
            get { return _other3; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string Status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:备注,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string Remarks
        {
            set { _remarks = value; }
            get { return _remarks; }
        }
        /// <summary>
        /// 下发时间
        /// </summary>
        public DateTime? SendTime
        {
            set { _sendtime = value; }
            get { return _sendtime; }
        }
        /// <summary>
        /// 回传时间
        /// </summary>
        public DateTime? BackTime
        {
            set { _backtime = value; }
            get { return _backtime; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string Creator
        {
            set { _creator = value; }
            get { return _creator; }
        }
        /// <summary>
        /// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
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

