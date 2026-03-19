using System;

namespace Model
{
	/// <summary>
	/// manufactureShopInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureShopInfo
	{
		public manufactureShopInfo()
		{}
		#region Model
		private int _id;
		private string _shopno;
		private string _shopname;
		private string _location;
		private int? _area;
		private string _plantname;
		private string _manager;
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
		/// @controlType:LTextBox,@labelString:车间编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string shopNo
		{
			set{ _shopno=value;}
			get{return _shopno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:车间名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string shopName
		{
			set{ _shopname=value;}
			get{return _shopname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:位置,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string location
		{
			set{ _location=value;}
			get{return _location;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:面积,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? area
		{
			set{ _area=value;}
			get{return _area;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:工厂名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string plantName
		{
			set{ _plantname=value;}
			get{return _plantname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:车间主管,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string manager
		{
			set{ _manager=value;}
			get{return _manager;}
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

