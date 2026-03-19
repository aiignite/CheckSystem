using System;

namespace Model
{
	/// <summary>
	/// smtTaskComputer:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtTaskComputer
	{
		public smtTaskComputer()
		{}
		#region Model
		private int _id;
		private string _taskno;
		private string _machinetypeno;
		private string _productno;
		private string _count;
		private string _bomgroupno;
		private string _materialno;
		private int? _materialcount;
		private int? _linewarecount;
		private int? _instorecount;
		private int? _diffcount;
		private string _creater;
		private DateTime? _createtime;
		private string _bin;
		private string _status;
		private string _shelfno;
		private string _islineware;
		private int? _linewareoutcount;
		private int? _stockoutcount;
		private int? _stockextraoutcout;
		private string _ishavereplacematerial;
		private int? _replacematerialtempgroup;
		private int? _priority;
		private string _supplybin;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:工单编号,@searchString:select taskNo from smtTaskData,@tipString:,@inDataGrid:True,@inPanel:True,@index:2
		/// </summary>
		public string taskNo
		{
			set{ _taskno=value;}
			get{return _taskno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:机种编号,@searchString:select machineTypeNo from smtTaskData where taskNo like '%%',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string machineTypeNo
		{
			set{ _machinetypeno=value;}
			get{return _machinetypeno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:select productNo from smtTaskData where taskNo like '%%',@tipString:,@inDataGrid:True,@inPanel:True,@index:3
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:数量,@searchString:select count from smtTaskData where taskNo like '%%',@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:BOM组别,@searchString:select distinct bomGroupNo from smtBomInfo where productNo like '%%',@tipString:,@inDataGrid:True,@inPanel:True,@index:5
		/// </summary>
		public string bomGroupNo
		{
			set{ _bomgroupno=value;}
			get{return _bomgroupno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:6
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public int? materialCount
		{
			set{ _materialcount=value;}
			get{return _materialcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:线边仓数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? lineWareCount
		{
			set{ _linewarecount=value;}
			get{return _linewarecount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:库存数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:8
		/// </summary>
		public int? instoreCount
		{
			set{ _instorecount=value;}
			get{return _instorecount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:差异数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9
		/// </summary>
		public int? diffCount
		{
			set{ _diffcount=value;}
			get{return _diffcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:10
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:11
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string bin
		{
			set{ _bin=value;}
			get{return _bin;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:货架编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:线边仓,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isLineWare
		{
			set{ _islineware=value;}
			get{return _islineware;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:线边仓出库数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? lineWareOutCount
		{
			set{ _linewareoutcount=value;}
			get{return _linewareoutcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:库存出库数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? stockOutCount
		{
			set{ _stockoutcount=value;}
			get{return _stockoutcount;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:库存多出数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? stockExtraOutCout
		{
			set{ _stockextraoutcout=value;}
			get{return _stockextraoutcout;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:是否包含替代料,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isHaveReplaceMaterial
		{
			set{ _ishavereplacematerial=value;}
			get{return _ishavereplacematerial;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:替代料临时组,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? replaceMaterialTempGroup
		{
			set{ _replacematerialtempgroup=value;}
			get{return _replacematerialtempgroup;}
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
		/// @controlType:LTextBox,@labelString:供应商档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplyBin
		{
			set{ _supplybin=value;}
			get{return _supplybin;}
		}
		#endregion Model

	}
}

