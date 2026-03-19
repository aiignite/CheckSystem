using System;

namespace Model
{
	/// <summary>
	/// pdPFMEA:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdPFMEA
	{
		public pdPFMEA()
		{}
		#region Model
		private int _id;
		private string _pfmeano;
		private string _productno;
		private string _productname;
		private string _partno;
		private string _partname;
		private string _presponsibilty;
		private string _cartype;
		private DateTime? _keydate;
		private string _coremember;
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
		/// @controlType:LTextBox,@labelString:PFMEA编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pFMEANO
		{
			set{ _pfmeano=value;}
			get{return _pfmeano;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNO
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
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
		/// @controlType:LTextBox,@labelString:过程责任,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pResponsibilty
		{
			set{ _presponsibilty=value;}
			get{return _presponsibilty;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:车型年/类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string carType
		{
			set{ _cartype=value;}
			get{return _cartype;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:关键日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? keyDate
		{
			set{ _keydate=value;}
			get{return _keydate;}
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
		/// @controlType:LTextBox,@labelString:FMEA文件状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
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

