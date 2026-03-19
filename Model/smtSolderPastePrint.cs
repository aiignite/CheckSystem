using System;

namespace Model
{
	/// <summary>
	/// smtSolderPastePrint:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtSolderPastePrint
	{
		public smtSolderPastePrint()
		{}
		#region Model
		private int _id;
		private string _stno;
		private string _startdate;
		private string _enddate;
		private int? _serialnumber;
		private string _boxserialnumber;
		private DateTime? _createtime;
		private string _creater;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string stNo
		{
			set{ _stno=value;}
			get{return _stno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:生成日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string startDate
		{
			set{ _startdate=value;}
			get{return _startdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:有效日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string endDate
		{
			set{ _enddate=value;}
			get{return _enddate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:序列号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public int? serialNumber
		{
			set{ _serialnumber=value;}
			get{return _serialnumber;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:瓶身系列号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string BoxserialNumber
		{
			set{ _boxserialnumber=value;}
			get{return _boxserialnumber;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		#endregion Model

	}
}

