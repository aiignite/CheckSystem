using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialStockOutPosition
	{
		public wmsMaterialStockOutPosition()
		{}
		#region Model
		private int _id;
		private string _stockoutno;
		private string _shelfno;
		private string _boxno;
		private string _materialno;
		private int? _count;
		private DateTime? _createtime;
		/// <summary>
		/// ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 出库单编号
		/// </summary>
		public string stockOutNo
		{
			set{ _stockoutno=value;}
			get{return _stockoutno;}
		}
		/// <summary>
		/// 货架编号
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// 料箱编号
		/// </summary>
		public string boxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
		}
		/// <summary>
		/// 物料编号
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// 最小包装数量
		/// </summary>
		public int? count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// @DateTimePicker:日期
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

