using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialBarcodePrintCorrespond
	{
		public wmsMaterialBarcodePrintCorrespond()
		{}
		#region Model
		private int _id;
		private string _materialbarcode;
		private string _materialno;
		private string _ledgroup;
		private int? _packagenum;
		private string _basicunit;
		private string _supplycode;
		private string _companyledgroup;
		private string _productmodelno;
		private string _ordermodelno;
		private string _status;
		/// <summary>
		/// 编号
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 物料条形码
		/// </summary>
		public string materialBarcode
		{
			set{ _materialbarcode=value;}
			get{return _materialbarcode;}
		}
		/// <summary>
		/// 物料
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// LED档位
		/// </summary>
		public string ledGroup
		{
			set{ _ledgroup=value;}
			get{return _ledgroup;}
		}
		/// <summary>
		/// 包装数量
		/// </summary>
		public int? packageNum
		{
			set{ _packagenum=value;}
			get{return _packagenum;}
		}
		/// <summary>
		/// 基本计量单位
		/// </summary>
		public string basicUnit
		{
			set{ _basicunit=value;}
			get{return _basicunit;}
		}
		/// <summary>
		/// 供应商编号
		/// </summary>
		public string supplyCode
		{
			set{ _supplycode=value;}
			get{return _supplycode;}
		}
		/// <summary>
		/// 公司LED档位
		/// </summary>
		public string companyLEDGroup
		{
			set{ _companyledgroup=value;}
			get{return _companyledgroup;}
		}
		/// <summary>
		/// 产品型号
		/// </summary>
		public string productModelNo
		{
			set{ _productmodelno=value;}
			get{return _productmodelno;}
		}
		/// <summary>
		/// 订单型号
		/// </summary>
		public string orderModelNo
		{
			set{ _ordermodelno=value;}
			get{return _ordermodelno;}
		}
		/// <summary>
		/// @ComboBox:状态,typeValue,commTypeCode,typeName,原材料LED打印状态
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

