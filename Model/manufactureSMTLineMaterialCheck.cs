using System;

namespace Model
{
	/// <summary>
	/// manufactureSMTLineMaterialCheck:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureSMTLineMaterialCheck
	{
		public manufactureSMTLineMaterialCheck()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _deviceno;
		private string _scannoin;
		private string _scannoout;
		private string _status;
		private string _smtoperator;
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
		/// 产品编号
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// 设备编号
		/// </summary>
		public string deviceNo
		{
			set{ _deviceno=value;}
			get{return _deviceno;}
		}
		/// <summary>
		/// 换入料盘扫描
		/// </summary>
		public string scanNoIn
		{
			set{ _scannoin=value;}
			get{return _scannoin;}
		}
		/// <summary>
		/// 换出料盘扫描
		/// </summary>
		public string scanNoOut
		{
			set{ _scannoout=value;}
			get{return _scannoout;}
		}
		/// <summary>
		/// 状态
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// 人员
		/// </summary>
		public string smtOperator
		{
			set{ _smtoperator=value;}
			get{return _smtoperator;}
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

