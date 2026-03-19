using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class deviceDesignStatusUnits :modelBase
	{
		/// <summary>
		/// workstationName
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:工站名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string workstationName { get; set; }

		/// <summary>
		/// statusUnitName
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:状态名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string statusUnitName { get; set; }

		/// <summary>
		/// statusUnitNo
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:状态编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string statusUnitNo { get; set; }

		/// <summary>
		/// enterFunction
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:进入函数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string enterFunction { get; set; }

		/// <summary>
		/// duringFunction
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:执行函数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string duringFunction { get; set; }

		/// <summary>
		/// enterNote
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:进入函数说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string enterNote { get; set; }

		/// <summary>
		/// duringNote
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:执行函数说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string duringNote { get; set; }

		/// <summary>
		/// positionSize
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:状态图位置,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string positionSize { get; set; }

	}
}
