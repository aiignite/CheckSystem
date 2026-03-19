using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class DeviceDesignProcessParas :modelBase
	{
		/// <summary>
		/// paraName
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:参数名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string paraName { get; set; }

		/// <summary>
		/// paraType
		/// </summary>
		[DisplayName("@controlType:LComboBox,@labelString:参数类型,@searchString:select value from commDic where name = '参数类型',@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string paraType { get; set; }

		/// <summary>
		/// okFormate
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:匹配符,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string okFormate { get; set; }

		/// <summary>
		/// value
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string value { get; set; }

		/// <summary>
		/// min
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:最小值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string min { get; set; }

		/// <summary>
		/// max
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:最大值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string max { get; set; }

		/// <summary>
		/// unit
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:单位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string unit { get; set; }

		/// <summary>
		/// control
		/// </summary>
		[DisplayName("@controlType:LComboBox,@labelString:显示控件,@searchString:select controlName from deviceDesignControls,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string control { get; set; }

		/// <summary>
		/// controllerField
		/// </summary>
		[DisplayName("@controlType:LComboBox,@labelString:控制器变量,@searchString:select controllerField from deviceDesignControllers,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string controllerField { get; set; }

		/// <summary>
		/// offset
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:补偿参数,@searchString:,@tipString:k,b,@inDataGrid:True,@inPanel:True,@index:")]
		public string offset { get; set; }

	}
}
