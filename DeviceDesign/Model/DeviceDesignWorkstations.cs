using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class DeviceDesignWorkstations :modelBase
	{
		/// <summary>
		/// workstationName
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:工站名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string workstationName { get; set; }

		/// <summary>
		/// initStatusUnit
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:初始化状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string initStatusUnit { get; set; }

		/// <summary>
		/// currentStatusUnit
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:当前状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string currentStatusUnit { get; set; }

	}
}
