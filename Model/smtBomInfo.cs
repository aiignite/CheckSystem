using System;

namespace Model
{
	/// <summary>
	/// smtBomInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtBomInfo
	{
		public smtBomInfo()
		{}
		#region Model
		private int _id;
		private string _machinetypeno;
		private string _productno;
		private string _bomgroupno;
		private string _materialno;
		private string _materialname;
		private string _materialsubno;
		private int? _count;
		private int? _replacegroup;
		private int? _priority;
		private int? _advancepriority;
		private string _creater;
		private DateTime? _createtime;
		private string _materialsubname;
		private string _iscm;
		private string _solderpastetype;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:机种编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string machineTypeNo
		{
			set{ _machinetypeno=value;}
			get{return _machinetypeno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:select productNo from productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:2
		/// </summary>
		public string productNO
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:组别,@searchString:,@tipString:PR2100000001G01,@inDataGrid:True,@inPanel:True,@index:3
		/// </summary>
		public string bomGroupNo
		{
			set{ _bomgroupno=value;}
			get{return _bomgroupno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:物料编号,@searchString:select productNo from productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string materialName
		{
			set{ _materialname=value;}
			get{return _materialname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料子编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:5
		/// </summary>
		public string materialSubNo
		{
			set{ _materialsubno=value;}
			get{return _materialsubno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:6
		/// </summary>
		public int? count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:替代组,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? replaceGroup
		{
			set{ _replacegroup=value;}
			get{return _replacegroup;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:优先级,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? priority
		{
			set{ _priority=value;}
			get{return _priority;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:优先,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? advancePriority
		{
			set{ _advancepriority=value;}
			get{return _advancepriority;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:8
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料子名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string materialSubName
		{
			set{ _materialsubname=value;}
			get{return _materialsubname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:是否组件BOM,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isCM
		{
			set{ _iscm=value;}
			get{return _iscm;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:锡膏编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string solderpasteType
		{
			set{ _solderpastetype=value;}
			get{return _solderpastetype;}
		}
		#endregion Model

	}
}

