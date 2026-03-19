using System;

namespace CheckSystem.MaterialHelperForms.MaterialModels
{
    /// <summary>
    /// newMaterialPrintCorrespond:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class NewMaterialPrintCorrespond
    {
        public NewMaterialPrintCorrespond()
        { }
        #region Model
        private int _id;
        private string _materialbarcode;
        private string _materialid;
        private string _materialno;
        private string _materialname;
        private string _modelname;
        private string _supplyledgroup;
        private string _supplyledno;
        private int? _count;
        private string _brandname;
        private string _barcodetype;
        private int? _barcodecount;
        private string _partnokey;
        private string _lotnokey;
        private string _dcnokey;
        private string _qtykey;
        private string _ledCatKey;
        private string _status;
        private string _remarks;
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
        /// @controlType:LTextBox,@labelString:主键ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@Writeable:False,@inSearchPanel:False,@index:
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:扫描标识,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string MaterialBarcode
        {
            set { _materialbarcode = value; }
            get { return _materialbarcode; }
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
        /// @controlType:LTextBox,@labelString:产品型号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string ModelName
        {
            set { _modelname = value; }
            get { return _modelname; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:特征值/档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string SupplyLedGroup
        {
            set { _supplyledgroup = value; }
            get { return _supplyledgroup; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:档位编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public string SupplyLedNo
        {
            set { _supplyledno = value; }
            get { return _supplyledno; }
        }
        /// <summary>
        /// @controlType:LTextBox,@labelString:包装规格,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
        /// </summary>
        public int? Count
        {
            set { _count = value; }
            get { return _count; }
        }
        /// <summary>
        /// 扫描品牌
        /// </summary>
        public string BrandName
        {
            set { _brandname = value; }
            get { return _brandname; }
        }
        /// <summary>
        /// 条码识别类型:关键字\指定位置\指定分隔符位置
        /// </summary>
        public string Barcodetype
        {
            set { _barcodetype = value; }
            get { return _barcodetype; }
        }
        /// <summary>
        /// 条码数
        /// </summary>
        public int? BarcodeCount
        {
            set { _barcodecount = value; }
            get { return _barcodecount; }
        }
        /// <summary>
        /// PartNo关键字
        /// </summary>
        public string PartNokey
        {
            set { _partnokey = value; }
            get { return _partnokey; }
        }
        /// <summary>
        /// LotNo关键字
        /// </summary>
        public string LotNokey
        {
            set { _lotnokey = value; }
            get { return _lotnokey; }
        }
        /// <summary>
        /// D/C关键字
        /// </summary>
        public string DcNokey
        {
            set { _dcnokey = value; }
            get { return _dcnokey; }
        }
        /// <summary>
        /// 数量关键字
        /// </summary>
        public string Qtykey
        {
            set { _qtykey = value; }
            get { return _qtykey; }
        }
        /// <summary>
        /// BIN关键字
        /// </summary>
        public string LedCatkey
        {
            set { _ledCatKey = value; }
            get { return _ledCatKey; }
        }
        /// <summary>
        /// @controlType:LComboBox,@labelString:状态,@searchString:select '有效' value union select '无效' value,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
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
        /// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@inSearchPanel:True,@index:
        /// </summary>
        public string Creator
        {
            set { _creator = value; }
            get { return _creator; }
        }
        /// <summary>
        /// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@inSearchPanel:True,@index:
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

