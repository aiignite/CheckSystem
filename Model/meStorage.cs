using System;

namespace Model
{
	/// <summary>
	/// meStorage:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class meStorage
	{
		public meStorage()
		{}
		#region Model
		private string _storageid;
		private int? _materialtype;
		private string _storageno;
		private string _barcode;
		private decimal? _num;
		private string _noteno;
		/// <summary>
		/// 
		/// </summary>
		public string StorageID
		{
			set{ _storageid=value;}
			get{return _storageid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? MaterialType
		{
			set{ _materialtype=value;}
			get{return _materialtype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string StorageNo
		{
			set{ _storageno=value;}
			get{return _storageno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BarCode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? Num
		{
			set{ _num=value;}
			get{return _num;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NoteNo
		{
			set{ _noteno=value;}
			get{return _noteno;}
		}
		#endregion Model

	}
}

