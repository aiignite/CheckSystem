using System;

namespace Model
{
	/// <summary>
	/// manufactureEquipData:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class manufactureEquipData
	{
		public manufactureEquipData()
		{}
		#region Model
		private int _id;
		private string _equipid;
		private string _checkdata;
		private DateTime? _checktime;
		private string _creater;
		private DateTime? _createtime;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:01
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:设备编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:02
		/// </summary>
		public string equipid
		{
			set{ _equipid=value;}
			get{return _equipid;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:检测结果,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:03
		/// </summary>
		public string checkData
		{
			set{ _checkdata=value;}
			get{return _checkdata;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:检测时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:04
		/// </summary>
		public DateTime? checkTime
		{
			set{ _checktime=value;}
			get{return _checktime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:05
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:06
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		#endregion Model

	}
}

