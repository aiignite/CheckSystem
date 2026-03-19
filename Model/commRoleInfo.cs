using System;

namespace Model
{
	/// <summary>
	/// commRoleInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class commRoleInfo
	{
		public commRoleInfo()
		{}
		#region Model
		private int _id;
		private string _roleno;
		private string _rolename;
		private string _rolecontent;
		private string _rolecontentnote;
		private string _rolecontentbutton;
		private string _rolecontentmenu;
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
		/// @controlType:LTextBox,@labelString:角色编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string roleNo
		{
			set{ _roleno=value;}
			get{return _roleno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:角色名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string roleName
		{
			set{ _rolename=value;}
			get{return _rolename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:角色内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string roleContent
		{
			set{ _rolecontent=value;}
			get{return _rolecontent;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:角色内容说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string roleContentNote
		{
			set{ _rolecontentnote=value;}
			get{return _rolecontentnote;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:按钮功能,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string roleContentButton
		{
			set{ _rolecontentbutton=value;}
			get{return _rolecontentbutton;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:菜单功能,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string roleContentMenu
		{
			set{ _rolecontentmenu=value;}
			get{return _rolecontentmenu;}
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

