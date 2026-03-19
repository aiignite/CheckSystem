using System;

namespace Model
{
	/// <summary>
	/// bomPaster:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class bomPaster
	{
		public bomPaster()
		{}
		#region Model
		private int _id;
		private string _componentno;
		private string _usage;
		private string _materialserialno;
		private string _materialno;
		private int? _amount;
		private string _lightone;
		private string _colorone;
		private string _vdropone;
		private string _lighttwo;
		private string _colortwo;
		private string _vdroptwo;
		private string _paratype;
		private string _dconcatmark;
		private string _sconcatmark;
		private string _materialname;
		private string _tagnumber;
		private string _creater;
		private DateTime? _createtime;
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
		public string componentNo
		{
			set{ _componentno=value;}
			get{return _componentno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string usage
		{
			set{ _usage=value;}
			get{return _usage;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string materialSerialNo
		{
			set{ _materialserialno=value;}
			get{return _materialserialno;}
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
		public int? amount
		{
			set{ _amount=value;}
			get{return _amount;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string lightOne
		{
			set{ _lightone=value;}
			get{return _lightone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string colorOne
		{
			set{ _colorone=value;}
			get{return _colorone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string vDropOne
		{
			set{ _vdropone=value;}
			get{return _vdropone;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string lightTwo
		{
			set{ _lighttwo=value;}
			get{return _lighttwo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string colorTwo
		{
			set{ _colortwo=value;}
			get{return _colortwo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string vDropTwo
		{
			set{ _vdroptwo=value;}
			get{return _vdroptwo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string paraType
		{
			set{ _paratype=value;}
			get{return _paratype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string dConcatMark
		{
			set{ _dconcatmark=value;}
			get{return _dconcatmark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sConcatMark
		{
			set{ _sconcatmark=value;}
			get{return _sconcatmark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string materialName
		{
			set{ _materialname=value;}
			get{return _materialname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string tagNumber
		{
			set{ _tagnumber=value;}
			get{return _tagnumber;}
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
		#endregion Model

	}
}

