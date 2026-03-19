using System;

namespace Model
{
	/// <summary>
	/// Sheet2delete:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sheet2delete
	{
		public Sheet2delete()
		{}
		#region Model
		private string _materialno;
		private string _shelfno;
		private string _boxno;
		/// <summary>
		/// 
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string boxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
		}
		#endregion Model

	}
}

