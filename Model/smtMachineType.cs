using System;

namespace Model
{
	/// <summary>
	/// smtMachineType:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtMachineType
	{
		public smtMachineType()
		{}
		#region Model
		private int _id;
		private string _machinetypeno;
		private string _machinename;
		private string _machinesubno;
		private string _machinesubname;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:机种编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string machineTypeNo
		{
			set{ _machinetypeno=value;}
			get{return _machinetypeno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:机种名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string machineName
		{
			set{ _machinename=value;}
			get{return _machinename;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:机种子编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string machineSubNo
		{
			set{ _machinesubno=value;}
			get{return _machinesubno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:子机种说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string machineSubName
		{
			set{ _machinesubname=value;}
			get{return _machinesubname;}
		}
		#endregion Model

	}
}

