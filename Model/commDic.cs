using System;

namespace Model
{
	/// <summary>
	/// commDic:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class commDic
	{
		public commDic()
		{}
		#region Model
		private int _id;
		private string _name;
		private string _value;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:2
		/// </summary>
		public string name
		{
			set{ _name=value;}
			get{return _name;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:3
		/// </summary>
		public string value
		{
			set{ _value=value;}
			get{return _value;}
		}
		#endregion Model

	}
}

