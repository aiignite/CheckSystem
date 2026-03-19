using System;

namespace Model
{
	/// <summary>
	/// smtLineProductInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtLineProductInfo
	{
		public smtLineProductInfo()
		{}
		#region Model
		private int _id;
		private string _processno;
		private string _smtlineno;
		private string _mounterno;
		private string _bomgroupno;
		private string _mounterposno;
		private string _materialno;
		private string _feederno;
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
		/// @controlType:LTextBox,@labelString:工艺编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:2
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产线编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:3
		/// </summary>
		public string smtLineNo
		{
			set{ _smtlineno=value;}
			get{return _smtlineno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:贴片机编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string mounterNo
		{
			set{ _mounterno=value;}
			get{return _mounterno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:BOM组别,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:5
		/// </summary>
		public string bomGroupNo
		{
			set{ _bomgroupno=value;}
			get{return _bomgroupno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:贴片机栈号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:6
		/// </summary>
		public string mounterPosNo
		{
			set{ _mounterposno=value;}
			get{return _mounterposno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:feeder编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:8
		/// </summary>
		public string feederNo
		{
			set{ _feederno=value;}
			get{return _feederno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:10
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

