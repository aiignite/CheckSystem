using System;
using System.Collections.Generic;
using System.Data;
using DBUtility;

namespace BLL
{
	/// <summary>
	/// meStorage
	/// </summary>
	public partial class meStorage
	{
		private readonly DAL.meStorage dal=new DAL.meStorage();
		public meStorage()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string StorageID)
		{
			return dal.Exists(StorageID);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(Model.meStorage model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(Model.meStorage model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string StorageID)
		{
			
			return dal.Delete(StorageID);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string StorageIDlist )
		{
			return dal.DeleteList(DBUtility.PageValidate.SafeLongFilter(StorageIDlist,0) );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public Model.meStorage GetModel(string StorageID)
		{
			
			return dal.GetModel(StorageID);
		}

		/// <summary>
		/// 得到一个对象实体，从缓存中
		/// </summary>
		public Model.meStorage GetModelByCache(string StorageID)
		{
			
			string CacheKey = "meStorageModel-" + StorageID;
			object objModel = DBUtility.DataCache.GetCache(CacheKey);
			if (objModel == null)
			{
				try
				{
					objModel = dal.GetModel(StorageID);
					if (objModel != null)
					{
						int ModelCache = DBUtility.ConfigHelper.GetConfigInt("ModelCache");
						DBUtility.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
					}
				}
				catch{}
			}
			return (Model.meStorage)objModel;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			return dal.GetList(Top,strWhere,filedOrder);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.meStorage> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<Model.meStorage> DataTableToList(DataTable dt)
		{
			List<Model.meStorage> modelList = new List<Model.meStorage>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				Model.meStorage model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

