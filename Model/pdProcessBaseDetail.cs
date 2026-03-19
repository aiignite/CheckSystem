using System;

namespace Model
{
	/// <summary>
	/// pdProcessBaseDetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdProcessBaseDetail
	{
		public pdProcessBaseDetail()
		{}
		#region Model
		private int _id;
		private string _pbno;
		private string _pbname;
		private string _pbdetailno;
		private string _requiment;
		private string _productkey;
		private string _processkey;
		private string _keycategory;
		private string _pfm;
		private string _pef;
		private int? _severity;
		private string _pcmf;
		private string _prevention;
		private int? _occr;
		private string _detect;
		private int? _det;
		private string _recmeasure;
		private string _respandtargetdate;
		private string _mrmeasure;
		private int? _mrsev;
		private int? _mroccr;
		private int? _mrdec;
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
		private DateTime _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:基础工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBNO
		{
			set{ _pbno=value;}
			get{return _pbno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:基础工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
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
		/// @controlType:LTextBox,@labelString:要求,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string requiment
		{
			set{ _requiment=value;}
			get{return _requiment;}
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
		/// @controlType:LTextBox,@labelString:潜在失效模式,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pFM
		{
			set{ _pfm=value;}
			get{return _pfm;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:潜在失效后果,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pEF
		{
			set{ _pef=value;}
			get{return _pef;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:严重度(S),@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? severity
		{
			set{ _severity=value;}
			get{return _severity;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:潜在失效起因机理,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pCMF
		{
			set{ _pcmf=value;}
			get{return _pcmf;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:现行预防控制,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string prevention
		{
			set{ _prevention=value;}
			get{return _prevention;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:频度(O),@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? occr
		{
			set{ _occr=value;}
			get{return _occr;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:现行探测控制,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string detect
		{
			set{ _detect=value;}
			get{return _detect;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:不可探测度(D),@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? det
		{
			set{ _det=value;}
			get{return _det;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:建议措施,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string recMeasure
		{
			set{ _recmeasure=value;}
			get{return _recmeasure;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:责任及目标完成日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string respAndTargetDate
		{
			set{ _respandtargetdate=value;}
			get{return _respandtargetdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:采取措施,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string mRMeasure
		{
			set{ _mrmeasure=value;}
			get{return _mrmeasure;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:措施结果严重度(S),@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? mRSev
		{
			set{ _mrsev=value;}
			get{return _mrsev;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:措施结果频度(O),@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? mROccr
		{
			set{ _mroccr=value;}
			get{return _mroccr;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:措施结果探测度(D),@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? mRDec
		{
			set{ _mrdec=value;}
			get{return _mrdec;}
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
		/// @controlType:LTextBox,@labelString:上公差(外),@searchString:,@tipString:,@inDataGrid:,@inPanel:,@index:
		/// </summary>
		public string fdUpTol
		{
			set{ _fduptol=value;}
			get{return _fduptol;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:下公差(外),@searchString:,@tipString:,@inDataGrid:,@inPanel:,@index:
		/// </summary>
		public string fdDownTol
		{
			set{ _fddowntol=value;}
			get{return _fddowntol;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:规范公差(内),@searchString:,@tipString:,@inDataGrid:,@inPanel:,@index:
		/// </summary>
		public string inTolSpec
		{
			set{ _intolspec=value;}
			get{return _intolspec;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:上公差(内),@searchString:,@tipString:,@inDataGrid:,@inPanel:,@index:
		/// </summary>
		public string inUpTol
		{
			set{ _inuptol=value;}
			get{return _inuptol;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:下公差(内),@searchString:,@tipString:,@inDataGrid:,@inPanel:,@index:
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
		/// @controlType:LTextBox,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

