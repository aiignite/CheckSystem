using System;

namespace Model
{
	/// <summary>
	/// pdCPDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdCPDetail
	{
		public pdCPDetail()
		{}
		#region Model
		private int _id;
		private string _cpno;
		private string _pbno;
		private string _pbname;
		private string _pbdetailno;
		private string _custpbno;
		private string _custpdno;
		private string _pbdevice;
		private string _productkey;
		private string _processkey;
		private string _keycategory;
		private string _fdtolspec;
		private string _fduptol;
		private string _fddowntol;
		private string _intolspec;
		private string _inuptol;
		private string _indowntol;
		private string _emt;
		private string _samplecap;
		private string _samplefre;
		private string _conmethod;
		private string _respperson;
		private string _actplan;
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
		/// @controlType:LTextBox,@labelString:控制计划编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string cPNO
		{
			set{ _cpno=value;}
			get{return _cpno;}
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
		/// @controlType:LTextBox,@labelString:详细工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBDetailNo
		{
			set{ _pbdetailno=value;}
			get{return _pbdetailno;}
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
		/// @controlType:LTextBox,@labelString:自定义详细工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string custPDNO
		{
			set{ _custpdno=value;}
			get{return _custpdno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:生产设备,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBDevice
		{
			set{ _pbdevice=value;}
			get{return _pbdevice;}
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
		/// @controlType:LTextBox,@labelString:规范公差(外),@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string fdTolSpec
		{
			set{ _fdtolspec=value;}
			get{return _fdtolspec;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:上公差(外),@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public string fdUpTol
		{
			set{ _fduptol=value;}
			get{return _fduptol;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:下公差(外),@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public string fdDownTol
		{
			set{ _fddowntol=value;}
			get{return _fddowntol;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:规范公差(内),@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public string inTolSpec
		{
			set{ _intolspec=value;}
			get{return _intolspec;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:上公差(内),@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public string inUpTol
		{
			set{ _inuptol=value;}
			get{return _inuptol;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:下公差(内),@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public string inDownTol
		{
			set{ _indowntol=value;}
			get{return _indowntol;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:评价测量技术,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string eMT
		{
			set{ _emt=value;}
			get{return _emt;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:样本容量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sampleCap
		{
			set{ _samplecap=value;}
			get{return _samplecap;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:样本频率,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sampleFre
		{
			set{ _samplefre=value;}
			get{return _samplefre;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:控制方式,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string conMethod
		{
			set{ _conmethod=value;}
			get{return _conmethod;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:责任人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string respPerson
		{
			set{ _respperson=value;}
			get{return _respperson;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:反应计划,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string actPlan
		{
			set{ _actplan=value;}
			get{return _actplan;}
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

