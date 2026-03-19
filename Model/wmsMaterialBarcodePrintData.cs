using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialBarcodePrintData
	{
		public wmsMaterialBarcodePrintData()
		{}
		#region Model
		private int _id;
		private string _materialscanno;
		private string _materialprintno;
		private string _boxno;
		private string _creator;
		private DateTime? _createtime;
		/// <summary>
		/// 编号
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 物料扫描编号
		/// </summary>
		public string materialScanNo
		{
			set{ _materialscanno=value;}
			get{return _materialscanno;}
		}
		/// <summary>
		/// 物料打印编号
		/// </summary>
		public string materialPrintNo
		{
			set{ _materialprintno=value;}
			get{return _materialprintno;}
		}
		/// <summary>
		/// 料箱编号
		/// </summary>
		public string boxNo
		{
			set{ _boxno=value;}
			get{return _boxno;}
		}
		/// <summary>
		/// 扫描入库人员
		/// </summary>
		public string creator
		{
			set{ _creator=value;}
			get{return _creator;}
		}
		/// <summary>
		/// 扫描入库时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

