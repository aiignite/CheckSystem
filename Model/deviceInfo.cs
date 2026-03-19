using System;

namespace Model
{
	/// <summary>
	/// deviceInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class deviceInfo
	{
		public deviceInfo()
		{}
		#region Model
		private int _id;
		private string _deviceno;
		private string _devicename;
		private string _devicelevel;
		private string _devicestatus;
		private string _devicetype;
		private string _devicemodel;
		private string _controllerno;
		private string _deptname;
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
		/// @controlType:LTextBox,@labelString:设备编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deviceNo
		{
			set{ _deviceno=value;}
			get{return _deviceno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deviceName
		{
			set{ _devicename=value;}
			get{return _devicename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备级别,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deviceLevel
		{
			set{ _devicelevel=value;}
			get{return _devicelevel;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:设备状态,@searchString:select value from commDic where name='设备状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deviceStatus
		{
			set{ _devicestatus=value;}
			get{return _devicestatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deviceType
		{
			set{ _devicetype=value;}
			get{return _devicetype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备型号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deviceModel
		{
			set{ _devicemodel=value;}
			get{return _devicemodel;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:控制器编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string controllerNo
		{
			set{ _controllerno=value;}
			get{return _controllerno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:部门名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deptName
		{
			set{ _deptname=value;}
			get{return _deptname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:11
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

