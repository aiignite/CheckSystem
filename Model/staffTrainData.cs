using System;

namespace Model
{
	/// <summary>
	/// staffTrainData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class staffTrainData
	{
		public staffTrainData()
		{}
		#region Model
		private int _id;
		private string _trainno;
		private string _staffno;
		private string _trainresult;
		private string _traindata;
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
		/// @controlType:LComboBox,@labelString:培训编号,@searchString:select distinct trainNo from staffTrainInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string trainNo
		{
			set{ _trainno=value;}
			get{return _trainno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:人员编号,@searchString:select distinct staffNo from staffInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string staffNo
		{
			set{ _staffno=value;}
			get{return _staffno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:培训结果,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string trainResult
		{
			set{ _trainresult=value;}
			get{return _trainresult;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:培训数据,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string trainData
		{
			set{ _traindata=value;}
			get{return _traindata;}
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
		#endregion Model

	}
}

