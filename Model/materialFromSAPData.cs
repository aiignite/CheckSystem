using System;

namespace Model
{
	/// <summary>
	/// materialFromSAPData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class materialFromSAPData
	{
		public materialFromSAPData()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _materialname;
		private string _sapdate;
		private string _materialtype;
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
		/// 物料编号
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// 物料名称
		/// </summary>
		public string materialName
		{
			set{ _materialname=value;}
			get{return _materialname;}
		}
		/// <summary>
		/// SAP入库时间
		/// </summary>
		public string sapDate
		{
			set{ _sapdate=value;}
			get{return _sapdate;}
		}
		/// <summary>
		/// 物料类型
		/// </summary>
		public string materialType
		{
			set{ _materialtype=value;}
			get{return _materialtype;}
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

