using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class deviceDesignPartsMapping :modelBase
	{
		/// <summary>
		/// partName
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:部件名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string partName { get; set; }

		/// <summary>
		/// dataType
		/// </summary>
		[DisplayName("@controlType:LComboBox,@labelString:数据类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string dataType { get; set; }

		/// <summary>
		/// controllerName
		/// </summary>
		[DisplayName("@controlType:LComboBox,@labelString:控制器名称,@searchString:select controllerName from deviceDesignControllers,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string controllerName { get; set; }

		/// <summary>
		/// controllerField
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:控制器变量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string controllerField { get; set; }

	}
}
