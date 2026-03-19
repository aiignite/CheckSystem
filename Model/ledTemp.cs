using System;

namespace Model
{
	/// <summary>
	/// ledTemp:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ledTemp
	{
		public ledTemp()
		{}
		#region Model
		private int _id;
		private string _relationno;
		private string _materialno;
		private string _color1;
		private string _color2;
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
		/// @controlType:LTextBox,@labelString:关联字段,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string relationno
		{
			set{ _relationno=value;}
			get{return _relationno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:亮度1,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string color1
		{
			set{ _color1=value;}
			get{return _color1;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:亮度2,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string color2
		{
			set{ _color2=value;}
			get{return _color2;}
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

