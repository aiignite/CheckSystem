using System;

namespace Model
{
	/// <summary>
	/// productSampleInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class productSampleInfo
	{
		public productSampleInfo()
		{}
		#region Model
		private int _id;
		private string _sampleno;
		private string _samplename;
		private string _sampletype;
		private string _productno;
		private string _processno;
		private string _isvalid;
		private string _versionno;
		private string _samplestatus;
		private string _note;
		private string _creater;
		private DateTime? _createtime;
		private string _productno2;
		private int? _productminpackagenum;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:样件编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string sampleNo
		{
			set{ _sampleno=value;}
			get{return _sampleno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:样件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string sampleName
		{
			set{ _samplename=value;}
			get{return _samplename;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:样件类型,@searchString:select distinct value from dbo.commDic where name='样件类型',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sampleType
		{
			set{ _sampletype=value;}
			get{return _sampletype;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:工序编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:是否有效,@searchString:select distinct value from dbo.commDic where name='是否有效',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isValid
		{
			set{ _isvalid=value;}
			get{return _isvalid;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:版本号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string versionNo
		{
			set{ _versionno=value;}
			get{return _versionno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:样件状态,@searchString:select distinct value from dbo.commDic where name='样件状态',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sampleStatus
		{
			set{ _samplestatus=value;}
			get{return _samplestatus;}
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
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:客户编号,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public string productNo2
		{
			set{ _productno2=value;}
			get{return _productno2;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:最小包装数,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public int? productMinPackageNum
		{
			set{ _productminpackagenum=value;}
			get{return _productminpackagenum;}
		}
		#endregion Model

	}
}

