using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialStockInInfo
	{
		public wmsMaterialStockInInfo()
		{}
		#region Model
		private int _id;
		private string _stockinno;
		private string _materialno;
		private string _supplierno;
		private int? _count;
		private int? _stockincount;
		private string _status;
		private string _note;
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
		/// 入库单编号
		/// </summary>
		public string stockInNo
		{
			set{ _stockinno=value;}
			get{return _stockinno;}
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
		public string supplierNo
		{
			set{ _supplierno=value;}
			get{return _supplierno;}
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
		/// 入库数量
		/// </summary>
		public int? stockInCount
		{
			set{ _stockincount=value;}
			get{return _stockincount;}
		}
		/// <summary>
		/// @ComboBox:原材料入库单状态,typeName,commTypeCode,typeName,原材料入库单状态
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
		#endregion Model

	}
}

