using System;

namespace Model
{
	/// <summary>
	/// pdPFD:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdPFD
	{
		public pdPFD()
		{}
		#region Model
		private int _id;
		private string _pfdno;
		private string _productno;
		private string _productname;
		private string _partno;
		private string _partname;
		private string _status;
		private string _contactor;
		private string _tel;
		private string _fax;
		private string _supplierno;
		private string _suppliername;
		private string _supplieradd;
		private string _supplierconfirm;
		private string _filename;
		private byte[] _filecontent;
		private string _filestatus;
		private string _creater;
		private DateTime? _createtime;
		private DateTime? _updatetime;
		private int? _pdproductid;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:流程图编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pFDNO
		{
			set{ _pfdno=value;}
			get{return _pfdno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNO
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:零件代号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string partNO
		{
			set{ _partno=value;}
			get{return _partno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:零件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string partName
		{
			set{ _partname=value;}
			get{return _partname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:流程状态（生产、非生产）,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:联系人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string contactor
		{
			set{ _contactor=value;}
			get{return _contactor;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:联系人电话,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string tel
		{
			set{ _tel=value;}
			get{return _tel;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:传真,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string fax
		{
			set{ _fax=value;}
			get{return _fax;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:供应商编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplierNO
		{
			set{ _supplierno=value;}
			get{return _supplierno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:供应商名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplierName
		{
			set{ _suppliername=value;}
			get{return _suppliername;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:生产地址,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplierAdd
		{
			set{ _supplieradd=value;}
			get{return _supplieradd;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:供应商确认,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplierConfirm
		{
			set{ _supplierconfirm=value;}
			get{return _supplierconfirm;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string fileName
		{
			set{ _filename=value;}
			get{return _filename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文件内容,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public byte[] fileContent
		{
			set{ _filecontent=value;}
			get{return _filecontent;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文件状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string fileStatus
		{
			set{ _filestatus=value;}
			get{return _filestatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:更新时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? updateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品工序ID（pdProductProcess.ID）,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public int? pdProductID
		{
			set{ _pdproductid=value;}
			get{return _pdproductid;}
		}
		#endregion Model

	}
}

