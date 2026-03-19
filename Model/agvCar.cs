using System;

namespace Model
{
	/// <summary>
	/// agvCar:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvCar
	{
		public agvCar()
		{}
		#region Model
		private int _id;
		private int? _agvlabel;
		private int? _agvbattery;
		private int? _agvportnum;
		private int? _agvrundist;
		private int? _agvcargolabel;
		private decimal? _agvspeed;
		private int? _isused;
		private int? _hascargo;
		private string _agvtask;
		private string _agvcurrunstatus;
		private string _agvalarminfo;
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
		/// @controlType:LTextBox,@labelString:agvLabel,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public int? agvLabel
		{
			set{ _agvlabel=value;}
			get{return _agvlabel;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:agvBattery,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? agvBattery
		{
			set{ _agvbattery=value;}
			get{return _agvbattery;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:agvPortNum,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? agvPortNum
		{
			set{ _agvportnum=value;}
			get{return _agvportnum;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:agvRunDist,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? agvRunDist
		{
			set{ _agvrundist=value;}
			get{return _agvrundist;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:agvCargoLabel,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? agvCargoLabel
		{
			set{ _agvcargolabel=value;}
			get{return _agvcargolabel;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:agvSpeed,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public decimal? agvSpeed
		{
			set{ _agvspeed=value;}
			get{return _agvspeed;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:isUsed,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? isUsed
		{
			set{ _isused=value;}
			get{return _isused;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:hasCargo,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public int? hasCargo
		{
			set{ _hascargo=value;}
			get{return _hascargo;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:agvTask,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string agvTask
		{
			set{ _agvtask=value;}
			get{return _agvtask;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:agvCurRunStatus,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string agvCurRunStatus
		{
			set{ _agvcurrunstatus=value;}
			get{return _agvcurrunstatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:agvAlarmInfo,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string agvAlarmInfo
		{
			set{ _agvalarminfo=value;}
			get{return _agvalarminfo;}
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

