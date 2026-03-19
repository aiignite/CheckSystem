using System;

namespace Model
{
	/// <summary>
	/// meMaterialBasic:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class meMaterialBasic
	{
		public meMaterialBasic()
		{}
		#region Model
		private string _materialname;
		private string _no;
		/// <summary>
		/// 
		/// </summary>
		public string MaterialName
		{
			set{ _materialname=value;}
			get{return _materialname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string No
		{
			set{ _no=value;}
			get{return _no;}
		}
		#endregion Model

	}
}

