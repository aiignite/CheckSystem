using System;

namespace Model
{
	/// <summary>
	/// manuCheckDataStat:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manuCheckDataStat
	{
		public manuCheckDataStat()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _processno;
		private string _checkresult;
		private string _checkyear;
		private string _checkmonth;
		private string _checkday;
		private string _checkhour;
		private int? _productnumber;
		private DateTime? _updatetime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:工艺编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检查结果,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkResult
		{
			set{ _checkresult=value;}
			get{return _checkresult;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:年份,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkYear
		{
			set{ _checkyear=value;}
			get{return _checkyear;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:月份,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkMonth
		{
			set{ _checkmonth=value;}
			get{return _checkmonth;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:天,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkDay
		{
			set{ _checkday=value;}
			get{return _checkday;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:时,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkHour
		{
			set{ _checkhour=value;}
			get{return _checkhour;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? productNumber
		{
			set{ _productnumber=value;}
			get{return _productnumber;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:更新时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? updateTime
		{
			set{ _updatetime=value;}
			get{return _updatetime;}
		}
		#endregion Model

	}
}

