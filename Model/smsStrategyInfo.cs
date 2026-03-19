using System;

namespace Model
{
	/// <summary>
	/// smsStrategyInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smsStrategyInfo
	{
		public smsStrategyInfo()
		{}
		#region Model
		private int _id;
		private string _stragegyno;
		private string _stragegyname;
		private string _stragegyunit;
		private int? _value;
		private string _userrank;
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
		/// @controlType:LTextBox,@labelString:策略编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string stragegyNo
		{
			set{ _stragegyno=value;}
			get{return _stragegyno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:策略名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string stragegyName
		{
			set{ _stragegyname=value;}
			get{return _stragegyname;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:策略单位,@searchString:select value from dbo.commDic where name='消息策略单位',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string stragegyUnit
		{
			set{ _stragegyunit=value;}
			get{return _stragegyunit;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? value
		{
			set{ _value=value;}
			get{return _value;}
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

