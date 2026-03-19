using System;

namespace Model
{
	/// <summary>
	/// processParaInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class processParaInfo
	{
		public processParaInfo()
		{}
		#region Model
		private int _id;
		private string _processparano;
		private string _processno;
		private string _processpara;
		private string _version;
		private string _isvalid;
		private string _note;
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
		/// @controlType:LTextBox,@labelString:工序参数编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processParaNo
		{
			set{ _processparano=value;}
			get{return _processparano;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:工序编号,@searchString:select processNo from processinfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:参数(自动生成),@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string processPara
		{
			set{ _processpara=value;}
			get{return _processpara;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:版本,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string version
		{
			set{ _version=value;}
			get{return _version;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:是否有效,@searchString:select value from commDic where name='是否有效',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string isValid
		{
			set{ _isvalid=value;}
			get{return _isvalid;}
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
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public DateTime? createTIme
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

