using System;

namespace Model
{
	/// <summary>
	/// ledFile:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class ledFile
	{
		public ledFile()
		{}
		#region Model
		private int _id;
		private string _filename;
		private string _filesrc;
		private string _commno;
		private string _version;
		private string _versionexplain;
		private string _productname;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// 
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string fileName
		{
			set{ _filename=value;}
			get{return _filename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:文件路径,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string fileSrc
		{
			set{ _filesrc=value;}
			get{return _filesrc;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:组件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string commNo
		{
			set{ _commno=value;}
			get{return _commno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:版本号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string version
		{
			set{ _version=value;}
			get{return _version;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:版本描述,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string versionExplain
		{
			set{ _versionexplain=value;}
			get{return _versionexplain;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:所属产品名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string productName
		{
			set{ _productname=value;}
			get{return _productname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

