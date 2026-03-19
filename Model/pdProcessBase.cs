using System;

namespace Model
{
	/// <summary>
	/// pdProcessBase:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdProcessBase
	{
		public pdProcessBase()
		{}
		#region Model
		private int _id;
		private string _pbno;
		private string _pbname;
		private string _pbdevice;
		private string _pbtype;
		private string _acttype;
		private string _pbnote;
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
		/// @controlType:LTextBox,@labelString:工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:2
		/// </summary>
		public string pBNO
		{
			set{ _pbno=value;}
			get{return _pbno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:工序名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:3
		/// </summary>
		public string pBName
		{
			set{ _pbname=value;}
			get{return _pbname;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:生产设备,@searchString:select deviceName from dbo.deviceInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string pBDevice
		{
			set{ _pbdevice=value;}
			get{return _pbdevice;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:工序类别,@searchString:select value from commDic where name = '基础工序类别',@tipString:,@inDataGrid:True,@inPanel:True,@index:5
		/// </summary>
		public string pBType
		{
			set{ _pbtype=value;}
			get{return _pbtype;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:操作类型,@searchString:select value from commDic where name = '基础工序操作类型',@tipString:,@inDataGrid:True,@inPanel:True,@index:6
		/// </summary>
		public string actType
		{
			set{ _acttype=value;}
			get{return _acttype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public string pBNote
		{
			set{ _pbnote=value;}
			get{return _pbnote;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:8
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

