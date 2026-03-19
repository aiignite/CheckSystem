using System;

namespace Model
{
	/// <summary>
	/// pdSOP:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdSOP
	{
		public pdSOP()
		{}
		#region Model
		private int _id;
		private string _sopno;
		private string _productno;
		private string _productname;
		private string _partno;
		private string _partname;
		private string _remark;
		private string _excelfilename;
		private byte[] _excelfilecontent;
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
		/// @controlType:LTextBox,@labelString:作业指导书编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sOPNO
		{
			set{ _sopno=value;}
			get{return _sopno;}
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
		/// @controlType:LTextBox,@labelString:备注,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:excel文件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string excelFileName
		{
			set{ _excelfilename=value;}
			get{return _excelfilename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:excel文件内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public byte[] excelFileContent
		{
			set{ _excelfilecontent=value;}
			get{return _excelfilecontent;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PDF文件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string fileName
		{
			set{ _filename=value;}
			get{return _filename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PDF文件内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
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

