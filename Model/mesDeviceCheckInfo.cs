using System;

namespace Model
{
	/// <summary>
	/// mesDeviceCheckInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class mesDeviceCheckInfo
	{
		public mesDeviceCheckInfo()
		{}
		#region Model
		private int _id;
		private string _department;
		private string _deviceno;
		private string _assetno;
		private string _devicename;
		private string _address;
		private string _note;
		private string _checkoperator;
		private DateTime? _checkdate;
		private string _status;
		private string _remark;
		private string _flag;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:所属部门,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string department
		{
			set{ _department=value;}
			get{return _department;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:SAP编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string deviceNo
		{
			set{ _deviceno=value;}
			get{return _deviceno;}
		}
		/// <summary>
		/// @controlType:,@labelString:资产编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string assetNo
		{
			set{ _assetno=value;}
			get{return _assetno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string deviceName
		{
			set{ _devicename=value;}
			get{return _devicename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:存放地点,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string address
		{
			set{ _address=value;}
			get{return _address;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:所属人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:盘点人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string checkoperator
		{
			set{ _checkoperator=value;}
			get{return _checkoperator;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:盘点时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public DateTime? checkdate
		{
			set{ _checkdate=value;}
			get{return _checkdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:当前状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:备注,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:状态标识,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string flag
		{
			set{ _flag=value;}
			get{return _flag;}
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
		/// 
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

