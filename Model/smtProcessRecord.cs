using System;

namespace Model
{
	/// <summary>
	/// smtProcessRecord:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtProcessRecord
	{
		public smtProcessRecord()
		{}
		#region Model
		private int _id;
		private string _zno;
		private string _position;
		private string _productno;
		private string _barcode;
		private DateTime? _createtime;
		private string _linenumber;
		private string _status;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@Writeable:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:载具编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string zno
		{
			set{ _zno=value;}
			get{return _zno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:载具位置,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string position
		{
			set{ _position=value;}
			get{return _position;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:编码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string barcode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:生成时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产线编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string lineNumber
		{
			set{ _linenumber=value;}
			get{return _linenumber;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:False,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		#endregion Model

	}
}

