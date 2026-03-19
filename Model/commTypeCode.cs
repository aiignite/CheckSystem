using System;

namespace Model
{
	/// <summary>
	/// commTypeCode:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class commTypeCode
	{
		public commTypeCode()
		{}
		#region Model
		private int _id;
		private string _typename;
		private int? _typeindex;
		private string _typevalue;
		/// <summary>
		/// ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 类型名称
		/// </summary>
		public string typeName
		{
			set{ _typename=value;}
			get{return _typename;}
		}
		/// <summary>
		/// 编号
		/// </summary>
		public int? typeIndex
		{
			set{ _typeindex=value;}
			get{return _typeindex;}
		}
		/// <summary>
		/// 类型值
		/// </summary>
		public string typeValue
		{
			set{ _typevalue=value;}
			get{return _typevalue;}
		}
		#endregion Model

	}
}

