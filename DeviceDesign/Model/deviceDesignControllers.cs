using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class deviceDesignControllers :modelBase
	{
		/// <summary>
		/// controllerName
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:控制器名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string controllerName { get; set; }

		/// <summary>
		/// controllerType
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:控制器类型,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string controllerType { get; set; }

		/// <summary>
		/// initPara
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:初始化参数,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string initPara { get; set; }

		/// <summary>
		/// note
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:说明,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string note { get; set; }

	}
}
