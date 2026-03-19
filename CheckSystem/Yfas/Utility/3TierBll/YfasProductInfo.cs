using System;
using System.Collections.Generic;
using System.Data;

namespace CheckSystem.Yfas.Utility._3TierBll
{
    /// <summary>
    /// YfasProductList
    /// </summary>
    public partial class YfasProductInfo
    {
        private readonly _3TierDal.YfasProductInfo _dal = new _3TierDal.YfasProductInfo();

        public YfasProductInfo()
        { }

        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int id)
        {
            return _dal.Exists(id);
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(_3TierModel.YfasProductInfo model)
        {
            return _dal.Add(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(_3TierModel.YfasProductInfo model)
        {
            return _dal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int id)
        {

            return _dal.Delete(id);
        }
        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteList(string idlist)
        {
            return _dal.DeleteList(idlist);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public _3TierModel.YfasProductInfo GetModel(int id)
        {

            return _dal.GetModel(id);
        }

        /// <summary>
        /// 得到一个对象实体，从缓存中
        /// </summary>
        public _3TierModel.YfasProductInfo GetModelByCache(int id)
        {
            string CacheKey = "YfasProductListModel-" + id;
            object objModel = DBUtility.DataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = _dal.GetModel(id);
                    if (objModel != null)
                    {
                        int ModelCache = DBUtility.ConfigHelper.GetConfigInt("ModelCache");
                        DBUtility.DataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            return (_3TierModel.YfasProductInfo)objModel;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            return _dal.GetList(strWhere);
        }
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int top, string strWhere, string filedOrder)
        {
            return _dal.GetList(top, strWhere, filedOrder);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<_3TierModel.YfasProductInfo> GetModelList(string strWhere)
        {
            DataSet ds = _dal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<_3TierModel.YfasProductInfo> DataTableToList(DataTable dt)
        {
            var modelList = new List<_3TierModel.YfasProductInfo>();
            var rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                for (var n = 0; n < rowsCount; n++)
                {
                    var model = _dal.DataRowToModel(dt.Rows[n]);
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
            return _dal.GetRecordCount(strWhere);
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
        {
            return _dal.GetListByPage(strWhere, orderby, startIndex, endIndex);
        }

        #endregion  BasicMethod
    }
}

