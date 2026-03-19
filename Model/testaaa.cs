using System;

namespace Model
{
	/// <summary>
	/// 1
	/// </summary>
	[Serializable]
	public partial class testaaa
	{
		public testaaa()
		{}
		#region Model
		private int _id;
		private string _name;
		private string _sex;
		private string _brithday;
		private string _aaa;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:姓名,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:性别,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string sex
		{
			set{ _sex=value;}
			get{return _sex;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:生日,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@index:
		/// </summary>
		public string brithday
		{
			set{ _brithday=value;}
			get{return _brithday;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:试验,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string aaa
		{
			set{ _aaa=value;}
			get{return _aaa;}
		}
		#endregion Model

	}
}

