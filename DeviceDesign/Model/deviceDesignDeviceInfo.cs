using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class deviceDesignDeviceInfo :modelBase
	{
		/// <summary>
		/// deviceNo
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:设备编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string deviceNo { get; set; }

		/// <summary>
		/// deviceVersion
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:版本,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string deviceVersion { get; set; }

		/// <summary>
		/// deviceVersionTime
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:发布时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:")]
		public string deviceVersionTime { get; set; }

	}
}
