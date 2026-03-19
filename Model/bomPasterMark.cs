using System;

namespace Model
{
	/// <summary>
	/// bomPasterMark:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bomPasterMark
	{
		public bomPasterMark()
		{}
		#region Model
		private int _id;
		private string _dconcatmark;
		private string _sconcatmark;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dConcatMark
		{
			set{ _dconcatmark=value;}
			get{return _dconcatmark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sConcatMark
		{
			set{ _sconcatmark=value;}
			get{return _sconcatmark;}
		}
		#endregion Model

	}
}

