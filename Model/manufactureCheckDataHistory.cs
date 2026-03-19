using System;

namespace Model
{
	/// <summary>
	/// manufactureCheckDataHistory:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureCheckDataHistory
	{
		public manufactureCheckDataHistory()
		{}
		#region Model
		private int _id;
		private string _taskno;
		private string _productno;
		private string _productuid;
		private string _pcbano;
		private string _pcbabarcode;
		private string _productbarcode;
		private string _packagebarcode;
		private string _processno;
		private string _checkdata;
		private DateTime? _checkdate;
		private string _checkstaffno;
		private string _checkresult;
		private string _creater;
		private DateTime _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:01
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:生产工单号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:02
		/// </summary>
		public string taskNo
		{
			set{ _taskno=value;}
			get{return _taskno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:03
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品唯一号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:04
		/// </summary>
		public string productUid
		{
			set{ _productuid=value;}
			get{return _productuid;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PCBA编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:05
		/// </summary>
		public string pcbaNo
		{
			set{ _pcbano=value;}
			get{return _pcbano;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:PCBA条形码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:06
		/// </summary>
		public string pcbaBarcode
		{
			set{ _pcbabarcode=value;}
			get{return _pcbabarcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品条形码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:07
		/// </summary>
		public string productBarcode
		{
			set{ _productbarcode=value;}
			get{return _productbarcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:包装条形码,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:08
		/// </summary>
		public string packageBarcode
		{
			set{ _packagebarcode=value;}
			get{return _packagebarcode;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:09
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检测数据,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:10
		/// </summary>
		public string checkData
		{
			set{ _checkdata=value;}
			get{return _checkdata;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:检测时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:11
		/// </summary>
		public DateTime? checkDate
		{
			set{ _checkdate=value;}
			get{return _checkdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:测试人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:12
		/// </summary>
		public string checkStaffNo
		{
			set{ _checkstaffno=value;}
			get{return _checkstaffno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检测结果,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:13
		/// </summary>
		public string checkResult
		{
			set{ _checkresult=value;}
			get{return _checkresult;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建者,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:14
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:15
		/// </summary>
		public DateTime createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

