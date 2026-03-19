using System;

namespace Model
{
	/// <summary>
	/// deviceSMSInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class deviceSMSInfo
	{
		public deviceSMSInfo()
		{}
		#region Model
		private int _id;
		private string _repairno;
		private string _deviceno;
		private string _failuredetail;
		private string _repairdetail;
		private DateTime? _ontime;
		private DateTime? _offtime;
		private string _repairer;
		private string _lessonnote;
		private string _status;
		private int? _smsstatus;
		private string _dealer;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:维修编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string repairNo
		{
			set{ _repairno=value;}
			get{return _repairno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:设备编号,@searchString:select distinct deviceNo from dbo.deviceInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deviceNo
		{
			set{ _deviceno=value;}
			get{return _deviceno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:故障内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string failureDetail
		{
			set{ _failuredetail=value;}
			get{return _failuredetail;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:维修内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string repairDetail
		{
			set{ _repairdetail=value;}
			get{return _repairdetail;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:开始时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? onTime
		{
			set{ _ontime=value;}
			get{return _ontime;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:结束时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? offTime
		{
			set{ _offtime=value;}
			get{return _offtime;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:维修人员,@searchString:select distinct (userNo+':'+userName)  as userInfo from smsUserInfo ,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string repairer
		{
			set{ _repairer=value;}
			get{return _repairer;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:经验教训,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string lessonNote
		{
			set{ _lessonnote=value;}
			get{return _lessonnote;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:维修状态,@searchString:select value from dbo.commDic where name ='处理状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:信息状态,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public int? smsStatus
		{
			set{ _smsstatus=value;}
			get{return _smsstatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:复原人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public string dealer
		{
			set{ _dealer=value;}
			get{return _dealer;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建者,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
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
		#endregion Model

	}
}

