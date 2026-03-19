using System;

namespace Model
{
	/// <summary>
	/// smtPBomInfo:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class smtPBomInfo
	{
		public smtPBomInfo()
		{}
		#region Model
		private int _id;
		private string _pbomno;
		private string _puse;
		private string _pgroupno;
		private string _pfeature;
		private string _pmaterialorder;
		private string _pmaterialno;
		private string _pmaterialnum;
		private string _pmaterialname;
		private string _ppositionno;
		private string _pbin;
		private string _pcompanybin;
		private string _pmemo;
		private string _pspecialmemo1;
		/// <summary>
		/// @controlType:LTextBox,@labelString:ID,@searchString:,@tipString:,@inDataGrid:True,@inPanel:False,@index:
		/// </summary>
		public int id
		{
			set{ _id=value;}
			get{return _id;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:组件编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBomNo
		{
			set{ _pbomno=value;}
			get{return _pbomno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:用途,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pUse
		{
			set{ _puse=value;}
			get{return _puse;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:组别,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pGroupNo
		{
			set{ _pgroupno=value;}
			get{return _pgroupno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:特征值,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pFeature
		{
			set{ _pfeature=value;}
			get{return _pfeature;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:料序号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pMaterialOrder
		{
			set{ _pmaterialorder=value;}
			get{return _pmaterialorder;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料编号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pMaterialNo
		{
			set{ _pmaterialno=value;}
			get{return _pmaterialno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料数量,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pMaterialNum
		{
			set{ _pmaterialnum=value;}
			get{return _pmaterialnum;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:物料名称,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pMaterialName
		{
			set{ _pmaterialname=value;}
			get{return _pmaterialname;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:位号,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pPositionNo
		{
			set{ _ppositionno=value;}
			get{return _ppositionno;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pBin
		{
			set{ _pbin=value;}
			get{return _pbin;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:公司档位,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pCompanyBin
		{
			set{ _pcompanybin=value;}
			get{return _pcompanybin;}
		}
		/// <summary>
		/// @controlType:LTextBox,@labelString:备注,@searchString:,@tipString:,@inDataGrid:True,@inPanel:True,@index:
		/// </summary>
		public string pMemo
		{
			set{ _pmemo=value;}
			get{return _pmemo;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string pSpecialMemo1
		{
			set{ _pspecialmemo1=value;}
			get{return _pspecialmemo1;}
		}
		#endregion Model

	}
}

