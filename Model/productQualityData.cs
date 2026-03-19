using System;

namespace Model
{
	/// <summary>
	/// productQualityData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class productQualityData
	{
		public productQualityData()
		{}
		#region Model
		private int _did;
		private int? _id;
		private string _productbarcode;
		private string _status;
		private DateTime? _updatedate;
		private string _updateman;
		/// <summary>
		/// 
		/// </summary>
		public int DID
		{
			set{ _did=value;}
			get{return _did;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ID
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string productBarcode
		{
			set{ _productbarcode=value;}
			get{return _productbarcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? UpdateDate
		{
			set{ _updatedate=value;}
			get{return _updatedate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string UpdateMan
		{
			set{ _updateman=value;}
			get{return _updateman;}
		}
		#endregion Model

	}
}

