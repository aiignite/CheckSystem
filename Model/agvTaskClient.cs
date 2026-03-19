using System;

namespace Model
{
	/// <summary>
	/// agvTaskClient:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class agvTaskClient
	{
		public agvTaskClient()
		{}
		#region Model
		private int _id;
		private string _taskclientno;
		private string _taskclientname;
		private string _taskclientnote;
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
		/// 发布端编号
		/// </summary>
		public string taskClientNo
		{
			set{ _taskclientno=value;}
			get{return _taskclientno;}
		}
		/// <summary>
		/// 发布端名称
		/// </summary>
		public string taskClientName
		{
			set{ _taskclientname=value;}
			get{return _taskclientname;}
		}
		/// <summary>
		/// 发布端说明
		/// </summary>
		public string taskClientNote
		{
			set{ _taskclientnote=value;}
			get{return _taskclientnote;}
		}
		/// <summary>
		/// @DateTimePicker:时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

