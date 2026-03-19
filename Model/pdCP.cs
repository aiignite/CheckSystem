using System;

namespace Model
{
	/// <summary>
	/// pdCP:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdCP
	{
		public pdCP()
		{}
		#region Model
		private int _id;
		private string _cpno;
		private string _productno;
		private string _productname;
		private string _partno;
		private string _partname;
		private string _status;
		private string _contactor;
		private string _tel;
		private string _coremember;
		private string _custengconfirm;
		private string _custquaconfirm;
		private string _supplierno;
		private string _suppliername;
		private string _supplierconfirm;
		private string _otherconfirm;
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
		/// @controlType:LTextBox,@labelString:控制计划编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string cPNO
		{
			set{ _cpno=value;}
			get{return _cpno;}
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
		/// @controlType:LTextBox,@labelString:样件,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
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
		/// @controlType:LTextBox,@labelString:核心小组,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string coreMember
		{
			set{ _coremember=value;}
			get{return _coremember;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:顾客工程批准/日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string custEngConfirm
		{
			set{ _custengconfirm=value;}
			get{return _custengconfirm;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:顾客质量批准/日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string custQuaConfirm
		{
			set{ _custquaconfirm=value;}
			get{return _custquaconfirm;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:供方代号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplierNO
		{
			set{ _supplierno=value;}
			get{return _supplierno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:供方名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplierName
		{
			set{ _suppliername=value;}
			get{return _suppliername;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:供方批准日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplierConfirm
		{
			set{ _supplierconfirm=value;}
			get{return _supplierconfirm;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:其他批准/日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string otherConfirm
		{
			set{ _otherconfirm=value;}
			get{return _otherconfirm;}
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

