using System;

namespace Model
{
	/// <summary>
	/// commRightsInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class commRightsInfo
	{
		public commRightsInfo()
		{}
		#region Model
		private int _id;
		private string _userno;
		private string _tablename;
		private string _userrole;
		private string _note;
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
		/// @controlType:LComboBox,@labelString:用户编号,@searchString:select userNo from commUserInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userNo
		{
			set{ _userno=value;}
			get{return _userno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:窗体名称,@searchString:select  nodeName  from systemTreeInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string tableName
		{
			set{ _tablename=value;}
			get{return _tablename;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:角色编号,@searchString:select  roleNo  from commRoleInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string userRole
		{
			set{ _userrole=value;}
			get{return _userrole;}
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

