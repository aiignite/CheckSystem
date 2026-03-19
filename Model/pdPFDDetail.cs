using System;

namespace Model
{
	/// <summary>
	/// pdPFDDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdPFDDetail
	{
		public pdPFDDetail()
		{}
		#region Model
		private int _id;
		private string _pfdno;
		private string _pbno;
		private string _pbname;
		private string _custpbno;
		private string _acttype;
		private string _pbdetailno;
		private string _productkey;
		private string _processkey;
		private string _keycategory;
		private string _fdtolspec;
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
		/// @controlType:LTextBox,@labelString:过程流程图编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pFDNO
		{
			set{ _pfdno=value;}
			get{return _pfdno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:基础工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBNO
		{
			set{ _pbno=value;}
			get{return _pbno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:基础工序名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBName
		{
			set{ _pbname=value;}
			get{return _pbname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:自定义基础工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string custPBNO
		{
			set{ _custpbno=value;}
			get{return _custpbno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:操作类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string actType
		{
			set{ _acttype=value;}
			get{return _acttype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:详细工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBDetailNo
		{
			set{ _pbdetailno=value;}
			get{return _pbdetailno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品特性,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productKey
		{
			set{ _productkey=value;}
			get{return _productkey;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:过程特性,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processKey
		{
			set{ _processkey=value;}
			get{return _processkey;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:特性类别,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string keyCategory
		{
			set{ _keycategory=value;}
			get{return _keycategory;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:规范公差,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string fdTolSpec
		{
			set{ _fdtolspec=value;}
			get{return _fdtolspec;}
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

