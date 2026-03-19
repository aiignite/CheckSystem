using System;

namespace Model
{
	/// <summary>
	/// pdProductProcess:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class pdProductProcess
	{
		public pdProductProcess()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _productname;
		private string _partno;
		private string _partname;
		private string _pbnos;
		private string _pbinfos;
		private int? _filetype;
		private string _iscomputesdp;
		private string _ischecksev;
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
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNO
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:零件代号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string partNO
		{
			set{ _partno=value;}
			get{return _partno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:零件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string partName
		{
			set{ _partname=value;}
			get{return _partname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:基础工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBNOs
		{
			set{ _pbnos=value;}
			get{return _pbnos;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:基础工序名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBInfos
		{
			set{ _pbinfos=value;}
			get{return _pbinfos;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:已生成文件（&1=1：PFMEA;,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? fileType
		{
			set{ _filetype=value;}
			get{return _filetype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:是否需要计算SZ，DZ，PL（F：否；T:是）,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string IsComputeSDP
		{
			set{ _iscomputesdp=value;}
			get{return _iscomputesdp;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:是否需要严重度等级判定（F：否；T:是）,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string IsCheckSev
		{
			set{ _ischecksev=value;}
			get{return _ischecksev;}
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
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

