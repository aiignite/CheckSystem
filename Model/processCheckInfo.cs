using System;

namespace Model
{
	/// <summary>
	/// processCheckInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class processCheckInfo
	{
		public processCheckInfo()
		{}
		#region Model
		private int _id;
		private string _checkno;
		private string _processno;
		private string _checkname;
		private string _checkcontent;
		private int? _checkindex;
		private string _checkstardant;
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
		/// @controlType:LTextBox,@labelString:点检编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkNo
		{
			set{ _checkno=value;}
			get{return _checkno;}
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
		/// @controlType:LTextBox,@labelString:点检名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkName
		{
			set{ _checkname=value;}
			get{return _checkname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:点检内容,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkContent
		{
			set{ _checkcontent=value;}
			get{return _checkcontent;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:点检序号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? checkIndex
		{
			set{ _checkindex=value;}
			get{return _checkindex;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:合格标准,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkStardant
		{
			set{ _checkstardant=value;}
			get{return _checkstardant;}
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

