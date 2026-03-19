using System;

namespace Model
{
	/// <summary>
	/// processParaData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class processParaData
	{
		public processParaData()
		{}
		#region Model
		private int _id;
		private string _processparano;
		private string _paraname;
		private string _paravalue;
		private string _paramin;
		private string _paramax;
		private string _paraunit;
		private string _isused;
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
		/// @controlType:LComboBox,@labelString:参数编号,@searchString:select processParaNo from processParaInfo ,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processParaNo
		{
			set{ _processparano=value;}
			get{return _processparano;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:参数名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string paraName
		{
			set{ _paraname=value;}
			get{return _paraname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:参数值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string paraValue
		{
			set{ _paravalue=value;}
			get{return _paravalue;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:最小值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string paraMin
		{
			set{ _paramin=value;}
			get{return _paramin;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:最大值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string paraMax
		{
			set{ _paramax=value;}
			get{return _paramax;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:单位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string paraUnit
		{
			set{ _paraunit=value;}
			get{return _paraunit;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:是否使用,@searchString:select value from commDic where name='是否使用',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isUsed
		{
			set{ _isused=value;}
			get{return _isused;}
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
		/// @controlType:LDateTimePicker,@labelString:创建日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

