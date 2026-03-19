using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsMaterialLEDGroupCorrespond
	{
		public wmsMaterialLEDGroupCorrespond()
		{}
		#region Model
		private int _id;
		private string _materialno;
		private string _supplyledgroup;
		private string _companyledgroup;
		private string _productmodelno;
		private string _ordermodelno;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:,@labelString:公司物料编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:,@labelString:供应商档位编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string supplyLedgroup
		{
			set{ _supplyledgroup=value;}
			get{return _supplyledgroup;}
		}
		/// <summary>
		/// @controlType:,@labelString:公司档位编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string companyLedGroup
		{
			set{ _companyledgroup=value;}
			get{return _companyledgroup;}
		}
		/// <summary>
		/// @controlType:,@labelString:产品型号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productModelNo
		{
			set{ _productmodelno=value;}
			get{return _productmodelno;}
		}
		/// <summary>
		/// @controlType:,@labelString:订单型号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string orderModelNo
		{
			set{ _ordermodelno=value;}
			get{return _ordermodelno;}
		}
		/// <summary>
		/// @controlType:,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

