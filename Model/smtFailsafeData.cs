using System;

namespace Model
{
	/// <summary>
	/// smtFailsafeData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtFailsafeData
	{
		public smtFailsafeData()
		{}
		#region Model
		private int _id;
		private string _receiptno;
		private string _materialno;
		private string _barcode;
		private string _mounterposno;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// 主键ID
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 出库单号
		/// </summary>
		public string ReceiptNo
		{
			set{ _receiptno=value;}
			get{return _receiptno;}
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
		/// 物料码
		/// </summary>
		public string Barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// 栈位号
		/// </summary>
		public string mounterPosNo
		{
			set{ _mounterposno=value;}
			get{return _mounterposno;}
		}
		/// <summary>
		/// 创建人
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

