using System;

namespace Model
{
	/// <summary>
	/// manufactureShowData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureShowData
	{
		public manufactureShowData()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _productname;
		private string _status;
		private string _shopname;
		private string _responselevel;
		private string _responseman;
		private DateTime? _checktime;
		private string _listurl;
		private string _listvalue;
		private DateTime? _createtime;
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
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
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
		public string shopName
		{
			set{ _shopname=value;}
			get{return _shopname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string responselevel
		{
			set{ _responselevel=value;}
			get{return _responselevel;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string responseman
		{
			set{ _responseman=value;}
			get{return _responseman;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? checkTime
		{
			set{ _checktime=value;}
			get{return _checktime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string listurl
		{
			set{ _listurl=value;}
			get{return _listurl;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string listvalue
		{
			set{ _listvalue=value;}
			get{return _listvalue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

