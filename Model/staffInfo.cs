using System;

namespace Model
{
	/// <summary>
	/// staffInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class staffInfo
	{
		public staffInfo()
		{}
		#region Model
		private int _id;
		private string _staffno;
		private string _name;
		private string _sex;
		private string _ontime;
		private string _offtime;
		private string _studydegree;
		private string _birthplace;
		private string _birthdata;
		private string _deptname;
		private string _cardno;
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
		/// @controlType:LTextBox,@labelString:工号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string staffNo
		{
			set{ _staffno=value;}
			get{return _staffno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:姓名,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:性别,@searchString:select value from commDic where name = '性别',@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@inSearchPanel:False,@index:
		/// </summary>
		public string sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:入职时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string onTime
		{
			set{ _ontime=value;}
			get{return _ontime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:离职时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string offTime
		{
			set{ _offtime=value;}
			get{return _offtime;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:学历,@searchString:select value from commDic where name = '学历',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string studyDegree
		{
			set{ _studydegree=value;}
			get{return _studydegree;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:籍贯,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string birthPlace
		{
			set{ _birthplace=value;}
			get{return _birthplace;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:出生日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string birthData
		{
			set{ _birthdata=value;}
			get{return _birthdata;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:部门名称,@searchString:select value from commDic where name = '部门名称',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string deptName
		{
			set{ _deptname=value;}
			get{return _deptname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:卡号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string cardNo
		{
			set{ _cardno=value;}
			get{return _cardno;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:记录时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

