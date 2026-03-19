using System;

namespace Model
{
	/// <summary>
	/// labelCheckinfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class labelCheckinfo
	{
		public labelCheckinfo()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _remark;
		private string _lablerule;
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
		/// 产品号
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// 规则分类
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 规则标签
		/// </summary>
		public string lablerule
		{
			set{ _lablerule=value;}
			get{return _lablerule;}
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

