using System;

namespace Model
{
	/// <summary>
	/// devicePartInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class devicePartInfo
	{
		public devicePartInfo()
		{}
		#region Model
		private int _id;
		private string _partno;
		private string _partname;
		private string _partmodel;
		private string _manufacturer;
		private string _contact;
		private string _contacttel;
		private int? _stockmin;
		private int? _stockmax;
		private int? _purchasecycle;
		private string _isvalid;
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
		/// 部件名称
		/// </summary>
		public string partName
		{
			set{ _partname=value;}
			get{return _partname;}
		}
		/// <summary>
		/// 部件型号
		/// </summary>
		public string partModel
		{
			set{ _partmodel=value;}
			get{return _partmodel;}
		}
		/// <summary>
		/// 生产厂家
		/// </summary>
		public string manufacturer
		{
			set{ _manufacturer=value;}
			get{return _manufacturer;}
		}
		/// <summary>
		/// 联系方式
		/// </summary>
		public string contact
		{
			set{ _contact=value;}
			get{return _contact;}
		}
		/// <summary>
		/// 联系电话
		/// </summary>
		public string contactTel
		{
			set{ _contacttel=value;}
			get{return _contacttel;}
		}
		/// <summary>
		/// 最小库存
		/// </summary>
		public int? stockMin
		{
			set{ _stockmin=value;}
			get{return _stockmin;}
		}
		/// <summary>
		/// 最大库存
		/// </summary>
		public int? stockMax
		{
			set{ _stockmax=value;}
			get{return _stockmax;}
		}
		/// <summary>
		/// 采购周期
		/// </summary>
		public int? purchaseCycle
		{
			set{ _purchasecycle=value;}
			get{return _purchasecycle;}
		}
		/// <summary>
		/// @ComboBox:是否有效,typeValue,commTypeCode,typeName,是否有效
		/// </summary>
		public string isValid
		{
			set{ _isvalid=value;}
			get{return _isvalid;}
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

