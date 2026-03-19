using System;
using System.ComponentModel;

namespace DeviceDesign.Model
{
	[Serializable]
	public class modelBase
	{
		/// <summary>
		/// id
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1")]
		public int id { get; set; }

	
		/// <summary>
		/// creater
		/// </summary>
		[DisplayName("@controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9")]
		public string creater { get; set; }

	
		/// <summary>
		/// createTime
		/// </summary>
		[DisplayName("@controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:11")]
		public DateTime? createTime { get; set; }

	}
}
