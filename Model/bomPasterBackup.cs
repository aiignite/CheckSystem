using System;

namespace Model
{
	/// <summary>
	/// bomPasterBackup:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bomPasterBackup
	{
		public bomPasterBackup()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _type;
		private string _stype;
		private string _light;
		private string _color;
		private string _voltage;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sType
		{
			set{ _stype=value;}
			get{return _stype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string light
		{
			set{ _light=value;}
			get{return _light;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string color
		{
			set{ _color=value;}
			get{return _color;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string voltage
		{
			set{ _voltage=value;}
			get{return _voltage;}
		}
		#endregion Model

	}
}

