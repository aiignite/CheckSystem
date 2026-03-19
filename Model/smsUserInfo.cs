using System;

namespace Model
{
	/// <summary>
	/// smsUserInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smsUserInfo
	{
		public smsUserInfo()
		{}
		#region Model
		private int _id;
		private string _userno;
		private string _username;
		private string _mobile;
		private string _userrank;
		private string _leaderuserno;
		private string _creater;
		private DateTime? _createtime;
		private string _smstype;
		private string _defaultsend;
		private string _smstypenew;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:员工号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userNo
		{
			set{ _userno=value;}
			get{return _userno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:员工姓名,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:员工手机号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string mobile
		{
			set{ _mobile=value;}
			get{return _mobile;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:员工职级,@searchString:select value from dbo.commDic where name='员工职级',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userRank
		{
			set{ _userrank=value;}
			get{return _userrank;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:员工领导,@searchString:select distinct userNo from smsUserInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string leaderUserNo
		{
			set{ _leaderuserno=value;}
			get{return _leaderuserno;}
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
		/// @controlType:LComboBox,@labelString:消息类型,@searchString: select value from dbo.commDic where name='消息类型',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string smsType
		{
			set{ _smstype=value;}
			get{return _smstype;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:默认发送,@searchString: select value from dbo.commDic where name='是否状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string defaultSend
		{
			set{ _defaultsend=value;}
			get{return _defaultsend;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string smsTypeNew
		{
			set{ _smstypenew=value;}
			get{return _smstypenew;}
		}
		#endregion Model

	}
}

