using System;

namespace Model
{
	/// <summary>
	/// ledBase:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ledBase
	{
		public ledBase()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _materialname;
		private string _type;
		private string _stype;
		private string _light1;
		private string _color1;
		private string _voltage1;
		private string _light2;
		private string _color2;
		private string _voltage2;
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
		/// @controlType:LTextBox,@labelString:物料号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:1
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:2
		/// </summary>
		public string materialName
		{
			set{ _materialname=value;}
			get{return _materialname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:型号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:3
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:小型号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:False,@index:4
		/// </summary>
		public string sType
		{
			set{ _stype=value;}
			get{return _stype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:亮度1,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:False,@index:5
		/// </summary>
		public string light1
		{
			set{ _light1=value;}
			get{return _light1;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:颜色1,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:False,@index:6
		/// </summary>
		public string color1
		{
			set{ _color1=value;}
			get{return _color1;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:压强1,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:False,@index:7
		/// </summary>
		public string voltage1
		{
			set{ _voltage1=value;}
			get{return _voltage1;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:亮度2,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:False,@index:8
		/// </summary>
		public string light2
		{
			set{ _light2=value;}
			get{return _light2;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:颜色2,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:False,@index:9
		/// </summary>
		public string color2
		{
			set{ _color2=value;}
			get{return _color2;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:压强2,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:False,@index:10
		/// </summary>
		public string voltage2
		{
			set{ _voltage2=value;}
			get{return _voltage2;}
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

