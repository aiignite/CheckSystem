using System;

namespace Model
{
	/// <summary>
	/// meInventoryFlow:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class meInventoryFlow
	{
		public meInventoryFlow()
		{}
		#region Model
		private string _id;
		private int? _inout;
		private int? _materialtype;
		private string _barcode;
		private decimal? _num;
		private string _storageno;
		private string _createby;
		private DateTime? _createdt;
		private string _noteno;
		/// <summary>
		/// 
		/// </summary>
		public string ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? InOut
		{
			set{ _inout=value;}
			get{return _inout;}
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
		public string Barcode
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
		public string StorageNo
		{
			set{ _storageno=value;}
			get{return _storageno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Createby
		{
			set{ _createby=value;}
			get{return _createby;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? Createdt
		{
			set{ _createdt=value;}
			get{return _createdt;}
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

