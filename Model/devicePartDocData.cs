using System;

namespace Model
{
	/// <summary>
	/// devicePartDocData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class devicePartDocData
	{
		public devicePartDocData()
		{}
		#region Model
		private int _id;
		private string _partno;
		private string _partdocno;
		private string _partdocname;
		private string _partdoclink;
		private string _creater;
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
		/// 部件编号
		/// </summary>
		public string partNo
		{
			set{ _partno=value;}
			get{return _partno;}
		}
		/// <summary>
		/// 部件文件编号
		/// </summary>
		public string partDocNo
		{
			set{ _partdocno=value;}
			get{return _partdocno;}
		}
		/// <summary>
		/// 文件名称
		/// </summary>
		public string partDocName
		{
			set{ _partdocname=value;}
			get{return _partdocname;}
		}
		/// <summary>
		/// 文件链接
		/// </summary>
		public string partDocLink
		{
			set{ _partdoclink=value;}
			get{return _partdoclink;}
		}
		/// <summary>
		/// 创建者
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @DateTimePicker:记录时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

