using System;

namespace Model
{
	/// <summary>
	/// manufactureTaskData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureTaskData
	{
		public manufactureTaskData()
		{}
		#region Model
		private int _id;
		private string _taskno;
		private string _productno;
		private int? _quantity;
		private string _solutionno;
		private string _asslineno;
		private int? _rate;
		private string _ondate;
		private string _offdate;
		private string _taskstatus;
		private string _note;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:工单编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string taskNo
		{
			set{ _taskno=value;}
			get{return _taskno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? quantity
		{
			set{ _quantity=value;}
			get{return _quantity;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:生产方案编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string solutionNo
		{
			set{ _solutionno=value;}
			get{return _solutionno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产线编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string assLineNo
		{
			set{ _asslineno=value;}
			get{return _asslineno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:合格率,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public int? rate
		{
			set{ _rate=value;}
			get{return _rate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:开始时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string onDate
		{
			set{ _ondate=value;}
			get{return _ondate;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:结束时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string offDate
		{
			set{ _offdate=value;}
			get{return _offdate;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:工单状态,@searchString:select value from dbo.commDic (nolock) where name = '工单状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string taskStatus
		{
			set{ _taskstatus=value;}
			get{return _taskstatus;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string note
		{
			set{ _note=value;}
			get{return _note;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建者,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

