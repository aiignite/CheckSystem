using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class wmsNormalMaterialShelfNo
	{
		public wmsNormalMaterialShelfNo()
		{}
		#region Model
		private int _id;
		private string _shelftype;
		private string _channel;
		private string _type;
		private string _shelfno;
		private string _status;
		/// <summary>
		/// @controlType:LTextBox,@labelString:主键,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:库位类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string shelftype
		{
			set{ _shelftype=value;}
			get{return _shelftype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:通道号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string channel
		{
			set{ _channel=value;}
			get{return _channel;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:通道方向,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string type
		{
			set{ _type=value;}
			get{return _type;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:库位号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string shelfNo
		{
			set{ _shelfno=value;}
			get{return _shelfno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

