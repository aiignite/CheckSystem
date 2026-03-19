using System;

namespace Model
{
	/// <summary>
	/// smtToolLoader:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtToolLoader
	{
		public smtToolLoader()
		{}
		#region Model
		private int _id;
		private string _no;
		private string _position;
		private string _productno;
		private string _barcode;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@Writeable:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:载具编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string no
		{
			set{ _no=value;}
			get{return _no;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:载具位置,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string position
		{
			set{ _position=value;}
			get{return _position;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:编码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		#endregion Model

	}
}

