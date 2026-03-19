using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class DeviceDesignUiSplit :modelBase
	{
		/// <summary>
		/// deviceNo
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:设备编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string deviceNo { get; set; }

		/// <summary>
		/// col
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:列数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string col { get; set; }

		/// <summary>
		/// row
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:行数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string row { get; set; }

		/// <summary>
		/// colPercent
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:列百分比,@searchString:,@tipString:25,,25,25,25,@inDataGrid:True,@inPanel:True,@index:")]
		public string colPercent { get; set; }

		/// <summary>
		/// rowPix
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:行高,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string rowPix { get; set; }

	}
}
