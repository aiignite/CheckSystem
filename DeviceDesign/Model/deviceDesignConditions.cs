using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class deviceDesignConditions :modelBase
	{
		/// <summary>
		/// workstationName
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:工站名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string workstationName { get; set; }

		/// <summary>
		/// conditionName
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:条件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string conditionName { get; set; }

		/// <summary>
		/// sourceStatusUnit
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:源状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string sourceStatusUnit { get; set; }

		/// <summary>
		/// targetStatusUnit
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:目标状态,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string targetStatusUnit { get; set; }

		/// <summary>
		/// conditionFunction
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:条件函数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string conditionFunction { get; set; }

		/// <summary>
		/// exitFunction
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:退出函数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string exitFunction { get; set; }

		/// <summary>
		/// conditionNote
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:条件函数说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string conditionNote { get; set; }

		/// <summary>
		/// exitNote
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:退出函数说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string exitNote { get; set; }

		/// <summary>
		/// middlePosition
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:状态图中点,@searchString:,@tipString:x,y,@inDataGrid:True,@inPanel:True,@index:")]
		public string middlePosition { get; set; }

	}
}
