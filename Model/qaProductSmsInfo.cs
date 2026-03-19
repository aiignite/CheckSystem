using System;

namespace Model
{
	/// <summary>
	/// qaProductSmsInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class qaProductSmsInfo
	{
		public qaProductSmsInfo()
		{}
		#region Model
		private int _id;
		private string _productno;
		private string _processno;
		private string _statdate;
		private int? _statquantum;
		private string _senduser;
		private string _reason;
		private string _remark;
		private string _status;
		private int? _smsstatus;
		private string _dealer;
		private string _creater;
		private DateTime? _createtime;
		private int? _basicnum;
		private int? _equipqty=0;
		private int? _qualityqty=0;
		private int? _workshopqty=0;
		private int? _otherqty=0;
		private DateTime? _resettime;
		private DateTime? _finishtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品,@searchString:select distinct productNo from dbo.manufactureBasicData,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:工序,@searchString:select distinct processNo from dbo.manufactureBasicData,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:统计时间段,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string statDate
		{
			set{ _statdate=value;}
			get{return _statdate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? statQuantum
		{
			set{ _statquantum=value;}
			get{return _statquantum;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:发送人员,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sendUser
		{
			set{ _senduser=value;}
			get{return _senduser;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:原因,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string reason
		{
			set{ _reason=value;}
			get{return _reason;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:备注说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:处理状态,@searchString:select value from dbo.commDic where name ='处理状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string status
		{
			set{ _status=value;}
			get{return _status;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:信息状态,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public int? smsStatus
		{
			set{ _smsstatus=value;}
			get{return _smsstatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:复原人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public string dealer
		{
			set{ _dealer=value;}
			get{return _dealer;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:基数,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@index:
		/// </summary>
		public int? basicNum
		{
			set{ _basicnum=value;}
			get{return _basicnum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? EquipQTY
		{
			set{ _equipqty=value;}
			get{return _equipqty;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? QualityQTY
		{
			set{ _qualityqty=value;}
			get{return _qualityqty;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? WorkshopQTY
		{
			set{ _workshopqty=value;}
			get{return _workshopqty;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? OtherQTY
		{
			set{ _otherqty=value;}
			get{return _otherqty;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? ResetTime
		{
			set{ _resettime=value;}
			get{return _resettime;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? FinishTime
		{
			set{ _finishtime=value;}
			get{return _finishtime;}
		}
		#endregion Model

	}
}

