using System;

namespace Model
{
	/// <summary>
	/// ledPbom:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ledPbom
	{
		public ledPbom()
		{}
		#region Model
		private int _id;
		private string _commno;
		private string _usage;
		private string _groupindex;
		private string _spec;
		private int? _materialindex;
		private string _materialno;
		private int? _num;
		private string _materialname;
		private string _positionno;
		private string _stype;
		private string _remark;
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
		/// @controlType:LTextBox,@labelString:组件编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:1
		/// </summary>
		public string commNo
		{
			set{ _commno=value;}
			get{return _commno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:用途,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:2
		/// </summary>
		public string usage
		{
			set{ _usage=value;}
			get{return _usage;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:组别,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:3
		/// </summary>
		public string groupIndex
		{
			set{ _groupindex=value;}
			get{return _groupindex;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:特征值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:4
		/// </summary>
		public string spec
		{
			set{ _spec=value;}
			get{return _spec;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:料序号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:5
		/// </summary>
		public int? materialIndex
		{
			set{ _materialindex=value;}
			get{return _materialindex;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:6
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:7
		/// </summary>
		public int? num
		{
			set{ _num=value;}
			get{return _num;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:8
		/// </summary>
		public string materialName
		{
			set{ _materialname=value;}
			get{return _materialname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:位号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:9
		/// </summary>
		public string positionNo
		{
			set{ _positionno=value;}
			get{return _positionno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:10
		/// </summary>
		public string sType
		{
			set{ _stype=value;}
			get{return _stype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:备注,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:11
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
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

