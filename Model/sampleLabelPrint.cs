using System;

namespace Model
{
	/// <summary>
	/// sampleLabelPrint:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class sampleLabelPrint
	{
		public sampleLabelPrint()
		{}
		#region Model
		private int _id;
		private string _materialname;
		private string _customerno;
		private int? _num;
		private string _producer;
		private DateTime? _producedate;
		private string _partno;
		private string _orderno;
		private string _mark;
		private int? _printnum;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:物料名称,@searchString:select distinct sampleName from productSampleInfo,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string materialName
		{
			set{ _materialname=value;}
			get{return _materialname;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:客户代码,@searchString:select distinct productNo2 from productSampleInfo,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string customerNo
		{
			set{ _customerno=value;}
			get{return _customerno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public int? num
		{
			set{ _num=value;}
			get{return _num;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:生准担当,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string producer
		{
			set{ _producer=value;}
			get{return _producer;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:生产日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public DateTime? produceDate
		{
			set{ _producedate=value;}
			get{return _producedate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:零件号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string partNo
		{
			set{ _partno=value;}
			get{return _partno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:订单号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string orderNo
		{
			set{ _orderno=value;}
			get{return _orderno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:零件状态标记,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string mark
		{
			set{ _mark=value;}
			get{return _mark;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:打印数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public int? printNum
		{
			set{ _printnum=value;}
			get{return _printnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

