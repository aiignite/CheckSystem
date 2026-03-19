using System;

namespace Model
{
	/// <summary>
	/// smtReplaceMaterial:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtReplaceMaterial
	{
		public smtReplaceMaterial()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _productname;
		private string _componentsno;
		private string _oldcomponentsno;
		private string _specification;
		private string _skpictureno;
		private string _brand;
		private string _count;
		private string _unit;
		private string _supplier;
		private string _positionno;
		private string _memo;
		private string _priority;
		private string _mainmaterial;
		private string _iscm;
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string componentsNo
		{
			set{ _componentsno=value;}
			get{return _componentsno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string oldComponentsNo
		{
			set{ _oldcomponentsno=value;}
			get{return _oldcomponentsno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string specification
		{
			set{ _specification=value;}
			get{return _specification;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string skPictureNo
		{
			set{ _skpictureno=value;}
			get{return _skpictureno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string brand
		{
			set{ _brand=value;}
			get{return _brand;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string unit
		{
			set{ _unit=value;}
			get{return _unit;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplier
		{
			set{ _supplier=value;}
			get{return _supplier;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string positionNo
		{
			set{ _positionno=value;}
			get{return _positionno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string memo
		{
			set{ _memo=value;}
			get{return _memo;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:优先级,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string priority
		{
			set{ _priority=value;}
			get{return _priority;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:主BOM物料编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string mainMaterial
		{
			set{ _mainmaterial=value;}
			get{return _mainmaterial;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:是否组件,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isCM
		{
			set{ _iscm=value;}
			get{return _iscm;}
		}
		#endregion Model

	}
}

