using System;

namespace Model
{
	/// <summary>
	/// pdPFMEADetail:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdPFMEADetail
	{
		public pdPFMEADetail()
		{}
		#region Model
		private int _id;
		private string _pfmeano;
		private string _pbno;
		private string _pbname;
		private string _pbdetailno;
		private string _custpbno;
		private string _custpdno;
		private string _requiment;
		private string _pfm;
		private string _pef;
		private int? _severity;
		private string _keycategory;
		private string _pcmf;
		private string _prevention;
		private int? _occr;
		private string _detect;
		private int? _det;
		private string _rpn;
		private string _recmeasure;
		private int? _sz;
		private int? _dz;
		private int? _pl;
		private string _respandtargetdate;
		private string _mrmeasure;
		private int? _mrsev;
		private int? _mroccr;
		private int? _mrdec;
		private string _mrrpn;
		private string _checkos;
		private string _checkds;
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
		/// @controlType:LTextBox,@labelString:PFMEA编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pFMEANO
		{
			set{ _pfmeano=value;}
			get{return _pfmeano;}
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
		/// @controlType:LTextBox,@labelString:要求,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string requiment
		{
			set{ _requiment=value;}
			get{return _requiment;}
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
		/// @controlType:LTextBox,@labelString:特性类别,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string keyCategory
		{
			set{ _keycategory=value;}
			get{return _keycategory;}
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
		/// @controlType:LTextBox,@labelString:风险顺序号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string rPN
		{
			set{ _rpn=value;}
			get{return _rpn;}
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
		/// @controlType:LTextBox,@labelString:严重区域,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? sZ
		{
			set{ _sz=value;}
			get{return _sz;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:探测区域,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? dZ
		{
			set{ _dz=value;}
			get{return _dz;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:优先等级,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? pL
		{
			set{ _pl=value;}
			get{return _pl;}
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
		/// @controlType:LTextBox,@labelString:措施结果风险顺序数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string mRRPN
		{
			set{ _mrrpn=value;}
			get{return _mrrpn;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:O+S判定,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkOS
		{
			set{ _checkos=value;}
			get{return _checkos;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:D+S判定,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string checkDS
		{
			set{ _checkds=value;}
			get{return _checkds;}
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

