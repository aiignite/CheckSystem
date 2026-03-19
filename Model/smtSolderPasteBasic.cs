using System;

namespace Model
{
	/// <summary>
	/// smtSolderPasteBasic:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtSolderPasteBasic
	{
		public smtSolderPasteBasic()
		{}
		#region Model
		private int _id;
		private string _no;
		private string _validtime;
		private string _creater;
		private DateTime? _createtime;
		private string _name;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@Writeable:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:锡膏编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string no
		{
			set{ _no=value;}
			get{return _no;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:有效期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string validTime
		{
			set{ _validtime=value;}
			get{return _validtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			set{ _name=value;}
			get{return _name;}
		}
		#endregion Model

	}
}

