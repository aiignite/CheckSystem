using System;

namespace Model
{
	/// <summary>
	/// staffSkillData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class staffSkillData
	{
		public staffSkillData()
		{}
		#region Model
		private int _id;
		private string _staffno;
		private string _postno;
		private string _skilllevel;
		private DateTime? _assessdate;
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
		/// @controlType:LComboBox,@labelString:人员编号,@searchString:select staffNo from staffInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string staffNo
		{
			set{ _staffno=value;}
			get{return _staffno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:岗位编号,@searchString:select postNo from manufacturePostInfo ,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string postNo
		{
			set{ _postno=value;}
			get{return _postno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:技能等级,@searchString:select value from commDic where name='技能等级',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string skillLevel
		{
			set{ _skilllevel=value;}
			get{return _skilllevel;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:通过日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? assessDate
		{
			set{ _assessdate=value;}
			get{return _assessdate;}
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

