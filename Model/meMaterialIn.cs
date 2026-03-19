using System;

namespace Model
{
	/// <summary>
	/// meMaterialIn:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class meMaterialIn
	{
		public meMaterialIn()
		{}
		#region Model
		private int _id;
		private int? _materialid;
		private int? _materialnum;
		private DateTime? _indate;
		private DateTime? _expiredate;
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
		/// @controlType:LTextBox,@labelString:原材料ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public int? materialId
		{
			set{ _materialid=value;}
			get{return _materialid;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public int? materialNum
		{
			set{ _materialnum=value;}
			get{return _materialnum;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:入库日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public DateTime? inDate
		{
			set{ _indate=value;}
			get{return _indate;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:材料过期日期,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@Writeable:True,@inSearchPanel:True,@index:
		/// </summary>
		public DateTime? expireDate
		{
			set{ _expiredate=value;}
			get{return _expiredate;}
		}
		/// <summary>
		/// @controlType:,@labelString:,@searchString:,@tipString:,@inDataGrid:False,@inPanel:False,@Writeable:False,@inSearchPanel:False,@index:
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

