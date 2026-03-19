using System;

namespace Model
{
	/// <summary>
	/// commUserInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class commUserInfo
	{
		public commUserInfo()
		{}
		#region Model
		private int _id;
		private string _userno;
		private string _userpassword;
		private string _username;
		private string _userrole;
		private string _isvalid;
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
		/// @controlType:LTextBox,@labelString:用户编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userNo
		{
			set{ _userno=value;}
			get{return _userno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:密码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userPassword
		{
			set{ _userpassword=value;}
			get{return _userpassword;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:用户名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userName
		{
			set{ _username=value;}
			get{return _username;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:用户角色,@searchString:select roleName from commRoleInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userRole
		{
			set{ _userrole=value;}
			get{return _userrole;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:是否有效,@searchString:select typeValue from commDic where typeName='是否有效',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isValid
		{
			set{ _isvalid=value;}
			get{return _isvalid;}
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

