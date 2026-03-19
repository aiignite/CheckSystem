using System;

namespace Model
{
	/// <summary>
	/// meBarcodeInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class meBarcodeInfo
	{
		public meBarcodeInfo()
		{}
		#region Model
		private string _id;
		private string _barcode;
		private string _materialcode;
		private string _inserttime;
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
		public string Barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string MaterialCode
		{
			set{ _materialcode=value;}
			get{return _materialcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string InsertTime
		{
			set{ _inserttime=value;}
			get{return _inserttime;}
		}
		#endregion Model

	}
}

