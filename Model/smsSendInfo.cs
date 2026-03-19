using System;

namespace Model
{
	/// <summary>
	/// smsSendInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smsSendInfo
	{
		public smsSendInfo()
		{}
		#region Model
		private int _id;
		private string _phonenumber;
		private string _userno;
		private string _sendcontent;
		private string _sendstatus;
		private int? _sendedtimes;
		private DateTime? _sendtime;
		private string _source;
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
		/// @controlType:LTextBox,@labelString:手机号码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string phoneNumber
		{
			set{ _phonenumber=value;}
			get{return _phonenumber;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:用户号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userNo
		{
			set{ _userno=value;}
			get{return _userno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:发送内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sendContent
		{
			set{ _sendcontent=value;}
			get{return _sendcontent;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:发送状态,@searchString:select value from dbo.commDic where name = '消息状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sendStatus
		{
			set{ _sendstatus=value;}
			get{return _sendstatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:已发送次数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int? sendedTimes
		{
			set{ _sendedtimes=value;}
			get{return _sendedtimes;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:发送时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:,@index:
		/// </summary>
		public DateTime? sendTime
		{
			set{ _sendtime=value;}
			get{return _sendtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:信息来源,@searchString:,@tipString:,@inDataGrid:True,@inPanel:,@index:
		/// </summary>
		public string source
		{
			set{ _source=value;}
			get{return _source;}
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
		#endregion Model

	}
}

