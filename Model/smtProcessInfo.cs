using System;

namespace Model
{
	/// <summary>
	/// smtProcessInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtProcessInfo
	{
		public smtProcessInfo()
		{}
		#region Model
		private int _id;
		private string _machinetypeno;
		private string _productno;
		private string _productnum;
		private string _materialno;
		private string _processno;
		private string _pbomno;
		private string _bomgroupno;
		private string _asslineno;
		private string _mounterno;
		private string _pmaterialno;
		private string _mounterposno;
		private string _feederno;
		private string _creater;
		private DateTime? _createtime;
		private string _pcompanybin;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:1
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:机种编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string machineTypeNo
		{
			set{ _machinetypeno=value;}
			get{return _machinetypeno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产品编号,@searchString:select productNo from productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:2
		/// </summary>
		public string productNo
		{
			set{ _productno=value;}
			get{return _productno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:产品数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string productNum
		{
			set{ _productnum=value;}
			get{return _productnum;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:物料编号,@searchString:select productNo from productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public string materialNo
		{
			set{ _materialno=value;}
			get{return _materialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:工艺编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:3
		/// </summary>
		public string processNo
		{
			set{ _processno=value;}
			get{return _processno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:组件编号,@searchString:select materialNo from smtBomInfo where productNo like '%%' and materialNo<'CMZ',@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBomNo
		{
			set{ _pbomno=value;}
			get{return _pbomno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:BOM组别,@searchString:select distinct pGroupNo+'--'+pBin from smtPBomInfo where pBomNo  like '%%' and pbin<>'',@tipString:,@inDataGrid:True,@inPanel:True,@index:4
		/// </summary>
		public string bomGroupNo
		{
			set{ _bomgroupno=value;}
			get{return _bomgroupno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:产线编号,@searchString:select assLineNo from manufactureAssLineInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:5
		/// </summary>
		public string assLineNo
		{
			set{ _asslineno=value;}
			get{return _asslineno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:贴片机编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:6
		/// </summary>
		public string mounterNo
		{
			set{ _mounterno=value;}
			get{return _mounterno;}
		}
		/// <summary>
		/// @controlType:LComboBox,@labelString:组件物料编号,@searchString:select productNo from productInfo,@tipString:,@inDataGrid:True,@inPanel:True,@index:7
		/// </summary>
		public string pMaterialNo
		{
			set{ _pmaterialno=value;}
			get{return _pmaterialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:贴片机栈号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:8
		/// </summary>
		public string mounterPosNo
		{
			set{ _mounterposno=value;}
			get{return _mounterposno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:feeder编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:9
		/// </summary>
		public string feederNo
		{
			set{ _feederno=value;}
			get{return _feederno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:创建人,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:10
		/// </summary>
		public string creater
		{
			set{ _creater=value;}
			get{return _creater;}
		}
		/// <summary>
		/// @controlType:LDateTimePicker,@labelString:创建时间,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:11
		/// </summary>
		public DateTime? createTime
		{
			set{ _createtime=value;}
			get{return _createtime;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:公司档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pCompanyBin
		{
			set{ _pcompanybin=value;}
			get{return _pcompanybin;}
		}
		#endregion Model

	}
}

