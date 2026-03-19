using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsProductInfoPrint
	{
		public wmsProductInfoPrint()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _materialname;
		private string _batchno;
		private string _minpackagenum;
		private string _customerno;
		private string _boxserialnostart;
		private string _boxserialnoend;
		private string _brightnessgroup;
		private string _special;
		private string _specialstatus;
		private DateTime? _createtime;
		private string _packagestyle;
		private string _status;
		private string _instorestatus;
		/// <summary>
		/// 编号
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @ComboBox:物料编号,productNo,productInfo,productNo,
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
		/// 追溯号
		/// </summary>
		public string batchNo
		{
			set{ _batchno=value;}
			get{return _batchno;}
		}
		/// <summary>
		/// 最小包装数量
		/// </summary>
		public string minPackageNum
		{
			set{ _minpackagenum=value;}
			get{return _minpackagenum;}
		}
		/// <summary>
		/// 客户编号
		/// </summary>
		public string customerNo
		{
			set{ _customerno=value;}
			get{return _customerno;}
		}
		/// <summary>
		/// 流水起始编号
		/// </summary>
		public string boxSerialNoStart
		{
			set{ _boxserialnostart=value;}
			get{return _boxserialnostart;}
		}
		/// <summary>
		/// 流水结束编号
		/// </summary>
		public string boxSerialNoEnd
		{
			set{ _boxserialnoend=value;}
			get{return _boxserialnoend;}
		}
		/// <summary>
		/// @ComboBox:成品LED档位,typeValue,commTypeCode,typeName,成品LED档位
		/// </summary>
		public string brightnessGroup
		{
			set{ _brightnessgroup=value;}
			get{return _brightnessgroup;}
		}
		/// <summary>
		/// @ComboBox:专用,typeValue,commTypeCode,typeName,成品专用备注
		/// </summary>
		public string special
		{
			set{ _special=value;}
			get{return _special;}
		}
		/// <summary>
		/// @ComboBox:特殊状态,typeValue,commTypeCode,typeName,成品特殊状态
		/// </summary>
		public string specialStatus
		{
			set{ _specialstatus=value;}
			get{return _specialstatus;}
		}
		/// <summary>
		/// @DateTimePicker:创建时间
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @CheckBox:包装方式,纸箱,非纸箱,PaperBox,Non-PaperBox
		/// </summary>
		public string packageStyle
		{
			set{ _packagestyle=value;}
			get{return _packagestyle;}
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
		/// 
		/// </summary>
		public string instoreStatus
		{
			set{ _instorestatus=value;}
			get{return _instorestatus;}
		}
		#endregion Model

	}
}

