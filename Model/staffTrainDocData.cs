using System;

namespace Model
{
	/// <summary>
	/// staffTrainDocData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class staffTrainDocData
	{
		public staffTrainDocData()
		{}
		#region Model
		private int _id;
		private string _trainno;
		private string _docno;
		private string _traindocname;
		private string _traindoclink;
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
		/// @controlType:LTextBox,@labelString:培训编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string trainNo
		{
			set{ _trainno=value;}
			get{return _trainno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string DocNo
		{
			set{ _docno=value;}
			get{return _docno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string trainDocName
		{
			set{ _traindocname=value;}
			get{return _traindocname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文档链接,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string trainDocLink
		{
			set{ _traindoclink=value;}
			get{return _traindoclink;}
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

