using System;

namespace Model
{
	/// <summary>
	/// productPcbaInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class productPcbaInfo
	{
		public productPcbaInfo()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _pcbano;
		private string _pcbaname;
		private string _pcbamodel;
		private int? _productnum;
		private string _pcbabarcode;
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
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PCBA编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pcbaNo
		{
			set{ _pcbano=value;}
			get{return _pcbano;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PCBA名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pcbaName
		{
			set{ _pcbaname=value;}
			get{return _pcbaname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PCBA型号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pcbaModel
		{
			set{ _pcbamodel=value;}
			get{return _pcbamodel;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:拼板数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? productNum
		{
			set{ _productnum=value;}
			get{return _productnum;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PCBA条形码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pcbaBarcode
		{
			set{ _pcbabarcode=value;}
			get{return _pcbabarcode;}
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

