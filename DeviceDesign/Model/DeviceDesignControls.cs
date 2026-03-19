using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class DeviceDesignControls :modelBase
	{
		/// <summary>
		/// controlName
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:控件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string controlName { get; set; }

		/// <summary>
		/// controlType
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:控件类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string controlType { get; set; }

		/// <summary>
		/// col
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:起始行,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string col { get; set; }

		/// <summary>
		/// row
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:起始列,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string row { get; set; }

		/// <summary>
		/// colSpan
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:行数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string colSpan { get; set; }

		/// <summary>
		/// rowSpan
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:列数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string rowSpan { get; set; }

		/// <summary>
		/// note
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string note { get; set; }

	}
}
