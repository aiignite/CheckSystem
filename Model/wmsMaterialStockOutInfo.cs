using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialStockOutInfo
	{
		public wmsMaterialStockOutInfo()
		{}
		#region Model
		private int _id;
		private string _stockoutno;
		private string _materialno;
		private string _customerno;
		private int? _count;
		private int? _stockoutcount;
		private string _status;
		private string _note;
		private DateTime? _createtime;
		private string _bin;
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
		/// 物料编号
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// 供应商编号
		/// </summary>
		public string customerNo
		{
			set{ _customerno=value;}
			get{return _customerno;}
		}
		/// <summary>
		/// 数量
		/// </summary>
		public int? count
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 出库数量
		/// </summary>
		public int? stockOutCount
		{
			set{ _stockoutcount=value;}
			get{return _stockoutcount;}
		}
		/// <summary>
		/// @ComboBox:原材料出库单状态,typeName,commTypeCode,typeName,原材料出库单状态
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 说明
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// @DateTimePicker:日期
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 档位
		/// </summary>
		public string bin
		{
			set{ _bin=value;}
			get{return _bin;}
		}
		#endregion Model

	}
}

