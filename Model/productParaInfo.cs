using System;

namespace Model
{
	/// <summary>
	/// productParaInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class productParaInfo
	{
		public productParaInfo()
		{}
		#region Model
		private int _id;
		private string _parano;
		private string _paraname;
		private string _paranamecn;
		private string _paratype;
		private string _paraunit;
		private string _producttype;
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
		/// @controlType:LTextBox,@labelString:参数编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string paraNo
		{
			set{ _parano=value;}
			get{return _parano;}
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
		/// @controlType:LTextBox,@labelString:参数中文名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string paraNameCN
		{
			set{ _paranamecn=value;}
			get{return _paranamecn;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:参数类型,@searchString:select distinct value from dbo.commDic where name='产品参数类型',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string paraType
		{
			set{ _paratype=value;}
			get{return _paratype;}
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
		/// @controlType:LComboBox,@labelString:产品类型,@searchString:select distinct value from dbo.commDic where name='产品类型',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productType
		{
			set{ _producttype=value;}
			get{return _producttype;}
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
		public DateTime? createTIme
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

