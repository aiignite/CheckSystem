using System;

namespace Model
{
	/// <summary>
	/// mesDeviceCheckData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class mesDeviceCheckData
	{
		public mesDeviceCheckData()
		{}
		#region Model
		private int _id;
		private string _department;
		private string _deviceno;
		private string _assetno;
		private string _devicename;
		private string _address;
		private string _note;
		private string _status;
		private string _stockinoperator;
		private DateTime? _stockindate;
		private string _inventoryoperator;
		private DateTime? _inventorydate;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// ID
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
		/// @controlType:LTextBox,@labelString:资产编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
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
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入库人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string stockInOperator
		{
			set{ _stockinoperator=value;}
			get{return _stockinoperator;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:入库日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public DateTime? stockInDate
		{
			set{ _stockindate=value;}
			get{return _stockindate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:变更人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string inventoryOperator
		{
			set{ _inventoryoperator=value;}
			get{return _inventoryoperator;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:变更日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public DateTime? inventoryDate
		{
			set{ _inventorydate=value;}
			get{return _inventorydate;}
		}
		/// <summary>
		/// 创建人
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// 创建日期
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

