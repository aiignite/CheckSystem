using System;

namespace Model
{
	/// <summary>
	/// pdSOPDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdSOPDetail
	{
		public pdSOPDetail()
		{}
		#region Model
		private int _id;
		private string _sopno;
		private string _pbno;
		private string _pbname;
		private string _pbdevice;
		private string _pbdetailno;
		private int? _symbol;
		private string _serialnumber;
		private string _operkey;
		private int? _manualworktime;
		private int? _devicetime;
		private int? _walktime;
		private string _operdiagname;
		private byte[] _operdiagcontent;
		private string _operdesc;
		private string _creater;
		private DateTime? _createtime;
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
		/// @controlType:LTextBox,@labelString:基础工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBNO
		{
			set{ _pbno=value;}
			get{return _pbno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:基础工序名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBName
		{
			set{ _pbname=value;}
			get{return _pbname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBDevice
		{
			set{ _pbdevice=value;}
			get{return _pbdevice;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:详细工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBDetailNo
		{
			set{ _pbdetailno=value;}
			get{return _pbdetailno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:代号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? symbol
		{
			set{ _symbol=value;}
			get{return _symbol;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string serialNumber
		{
			set{ _serialnumber=value;}
			get{return _serialnumber;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:操作要素,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string operKey
		{
			set{ _operkey=value;}
			get{return _operkey;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:人工,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? manualWorkTime
		{
			set{ _manualworktime=value;}
			get{return _manualworktime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? deviceTime
		{
			set{ _devicetime=value;}
			get{return _devicetime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:行走,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? walkTime
		{
			set{ _walktime=value;}
			get{return _walktime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:操作图名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string operDiagName
		{
			set{ _operdiagname=value;}
			get{return _operdiagname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:操作图内容,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public byte[] operDiagContent
		{
			set{ _operdiagcontent=value;}
			get{return _operdiagcontent;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:操作说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string operDesc
		{
			set{ _operdesc=value;}
			get{return _operdesc;}
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
		/// @controlType:LTextBox,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

