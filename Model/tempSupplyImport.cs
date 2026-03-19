using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class tempSupplyImport
	{
		public tempSupplyImport()
		{}
		#region Model
		private string _zz;
		private string _typetext;
		private string _cggroup;
		private string _supplyno;
		private string _material;
		private string _materialgroup;
		/// <summary>
		/// 采购组织
		/// </summary>
		public string ZZ
		{
			set{ _zz=value;}
			get{return _zz;}
		}
		/// <summary>
		/// 项目类别文本
		/// </summary>
		public string typeText
		{
			set{ _typetext=value;}
			get{return _typetext;}
		}
		/// <summary>
		/// 采购组
		/// </summary>
		public string cgGroup
		{
			set{ _cggroup=value;}
			get{return _cggroup;}
		}
		/// <summary>
		/// 供应商
		/// </summary>
		public string supplyNo
		{
			set{ _supplyno=value;}
			get{return _supplyno;}
		}
		/// <summary>
		/// 物料
		/// </summary>
		public string material
		{
			set{ _material=value;}
			get{return _material;}
		}
		/// <summary>
		/// 物料组
		/// </summary>
		public string materialGroup
		{
			set{ _materialgroup=value;}
			get{return _materialgroup;}
		}
		#endregion Model

	}
}

