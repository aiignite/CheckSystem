using System;

namespace Model
{
	/// <summary>
	/// manufactureFinishedProductTableLabelPrint:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureFinishedProductTableLabelPrint
	{
		public manufactureFinishedProductTableLabelPrint()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _materialname;
		private string _batchno;
		private string _minpackagenum;
		private string _customerno;
		private string _boxserialnostart;
		private string _boxserialnoend;
		private string _brightnessgroup;
		private string _special;
		private string _specialstatus;
		private DateTime? _createtime;
		private string _packagestyle;
		private string _status;
		private string _instorestatus;
		private string _creater;
		private string _productcount;
		private int? _boxcount;
		private string _instorereceiptcount;
		private string _pcno;
		private string _instorereceiptstart;
		/// <summary>
		/// 编号
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:物料编号,@searchString:select productNo from productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string materialName
		{
			set{ _materialname=value;}
			get{return _materialname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:追溯号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string batchNo
		{
			set{ _batchno=value;}
			get{return _batchno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:最小包装数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string minPackageNum
		{
			set{ _minpackagenum=value;}
			get{return _minpackagenum;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:客户编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string customerNo
		{
			set{ _customerno=value;}
			get{return _customerno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:流水起始编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@inSearchPanel:True,@index:
		/// </summary>
		public string boxSerialNoStart
		{
			set{ _boxserialnostart=value;}
			get{return _boxserialnostart;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:流水结束编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@inSearchPanel:True,@index:
		/// </summary>
		public string boxSerialNoEnd
		{
			set{ _boxserialnoend=value;}
			get{return _boxserialnoend;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:成品LED档位,@searchString:select value from commDic where ,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string brightnessGroup
		{
			set{ _brightnessgroup=value;}
			get{return _brightnessgroup;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:专用,@searchString:select value from commDic where name,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string special
		{
			set{ _special=value;}
			get{return _special;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:特殊状态,@searchString:select value from commDic where name,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string specialStatus
		{
			set{ _specialstatus=value;}
			get{return _specialstatus;}
		}
		/// <summary>
		/// @DateTimePicker:创建时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:包装方式,@searchString:select value from commDic where,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string packageStyle
		{
			set{ _packagestyle=value;}
			get{return _packagestyle;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@inSearchPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库单状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@inSearchPanel:True,@index:
		/// </summary>
		public string instoreStatus
		{
			set{ _instorestatus=value;}
			get{return _instorestatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string productCount
		{
			set{ _productcount=value;}
			get{return _productcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:料箱数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public int? boxCount
		{
			set{ _boxcount=value;}
			get{return _boxcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库单状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@inSearchPanel:True,@index:
		/// </summary>
		public string instoreReceiptCount
		{
			set{ _instorereceiptcount=value;}
			get{return _instorereceiptcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:批次号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string pcNo
		{
			set{ _pcno=value;}
			get{return _pcno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库单起始编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string instoreReceiptStart
		{
			set{ _instorereceiptstart=value;}
			get{return _instorereceiptstart;}
		}
		#endregion Model

	}
}

