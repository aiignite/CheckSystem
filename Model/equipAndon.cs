using System;

namespace Model
{
	/// <summary>
	/// equipAndon:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class equipAndon
	{
		public equipAndon()
		{}
		#region Model
		private int _id;
		private string _equipid;
		private string _checkdata;
		private DateTime? _checktime;
		private string _creater;
		private DateTime? _createtime;
		private string _sendcontent;
		private string _smsstatus;
		private string _deal;
		private DateTime? _dealtime;
		private int? _sendlevel;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:1
		/// </summary>
		public string equipid
		{
			set{ _equipid=value;}
			get{return _equipid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string checkData
		{
			set{ _checkdata=value;}
			get{return _checkdata;}
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
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sendContent
		{
			set{ _sendcontent=value;}
			get{return _sendcontent;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string smsStatus
		{
			set{ _smsstatus=value;}
			get{return _smsstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string deal
		{
			set{ _deal=value;}
			get{return _deal;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? dealTime
		{
			set{ _dealtime=value;}
			get{return _dealtime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? sendlevel
		{
			set{ _sendlevel=value;}
			get{return _sendlevel;}
		}
		#endregion Model

	}
}

