using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialSupplyInfo
	{
		public wmsMaterialSupplyInfo()
		{}
		#region Model
		private int _id;
		private string _supplyno;
		/// <summary>
		/// 编号
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// 供应商编号
		/// </summary>
		public string supplyNo
		{
			set{ _supplyno=value;}
			get{return _supplyno;}
		}
		#endregion Model

	}
}

