using System;

namespace Model
{
	/// <summary>
	/// processInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class processInfo
	{
		public processInfo()
		{}
		#region Model
		private int _id;
		private string _processno;
		private string _processname;
		private string _productno;
		private string _deviceno;
		private string _note;
		private string _preprocessno;
		private string _pretestprocessno;
		private string _ispreprocesscheck;
		private int? _continuealarmnum;
		private int? _totalalarmnum;
		private int? _manualtime;
		private int? _devicetime;
		private int? _walktime;
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
		/// @controlType:LTextBox,@labelString:工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:工序名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processName
		{
			set{ _processname=value;}
			get{return _processname;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:select productNo from productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:设备编号,@searchString:select deviceNo from deviceInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deviceNo
		{
			set{ _deviceno=value;}
			get{return _deviceno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:前道工序编号,@searchString:select processNo from processInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string preProcessNo
		{
			set{ _preprocessno=value;}
			get{return _preprocessno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:前道测试工序,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string preTestProcessNo
		{
			set{ _pretestprocessno=value;}
			get{return _pretestprocessno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:是否有效,@searchString:select value from commDic where name='是否有效',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isPreProcessCheck
		{
			set{ _ispreprocesscheck=value;}
			get{return _ispreprocesscheck;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:连续报警数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? continueAlarmNum
		{
			set{ _continuealarmnum=value;}
			get{return _continuealarmnum;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:累积报警数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? totalAlarmNum
		{
			set{ _totalalarmnum=value;}
			get{return _totalalarmnum;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:人工标准时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? manualTime
		{
			set{ _manualtime=value;}
			get{return _manualtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备标准时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? deviceTime
		{
			set{ _devicetime=value;}
			get{return _devicetime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:行走时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? walkTime
		{
			set{ _walktime=value;}
			get{return _walktime;}
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

