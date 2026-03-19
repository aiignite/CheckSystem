using System;

namespace Model
{
	/// <summary>
	/// smtStockDayShow:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtStockDayShow
	{
		public smtStockDayShow()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _productname;
		private DateTime? _plandate;
		private int? _plancount;
		private int? _productcount;
		private int? _unqualifycount;
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
		/// 产品编号
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// 产品名称
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// 计划日期
		/// </summary>
		public DateTime? planDate
		{
			set{ _plandate=value;}
			get{return _plandate;}
		}
		/// <summary>
		/// 计划产量
		/// </summary>
		public int? planCount
		{
			set{ _plancount=value;}
			get{return _plancount;}
		}
		/// <summary>
		/// 实时产量
		/// </summary>
		public int? productCount
		{
			set{ _productcount=value;}
			get{return _productcount;}
		}
		/// <summary>
		/// 不良品数
		/// </summary>
		public int? unqualifyCount
		{
			set{ _unqualifycount=value;}
			get{return _unqualifycount;}
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

