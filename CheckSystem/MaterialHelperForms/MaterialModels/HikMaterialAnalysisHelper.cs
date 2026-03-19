using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtility;

namespace CheckSystem.MaterialHelperForms.MaterialModels
{
    /// <summary>
    /// 二维码解析方法
    /// </summary>
    public static class HikMaterialAnalysisHelper
    {
        public static List<TransDate> TransDates = new List<TransDate>();

        public static bool FormatDcode(string toFormatDcode, out string formattedDcode, ref string errorMsg)
        {
            formattedDcode = string.Empty;

            #region 根据[newTransDate]表，生成批次号
            // 根据[newTransDate]表，生成批次号
            var toSearchDcNo = toFormatDcode.Replace(".", "").Replace("/", "");
            if (toSearchDcNo.Length == 8)
            {
                var findAll =
                    TransDates.FindAll(f => f.DateNo == toSearchDcNo)
                        .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                if (findAll.Any())
                {
                    formattedDcode = findAll[0].DateNo;
                }
                else
                {
                    findAll =
                        TransDates.FindAll(f => f.Yymmdd == toSearchDcNo.Substring(0, 6))
                            .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                    if (findAll.Any())
                    {
                        formattedDcode = findAll[0].DateNo;
                    }
                    else
                    {
                        findAll = TransDates.FindAll(f => f.Week == toSearchDcNo.Substring(0, 4))
                           .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                        if (findAll.Any())
                        {
                            formattedDcode = findAll[0].DateNo;
                        }
                        else
                        {
                            errorMsg = string.Format("当前DateCode：{0}解析失败", toFormatDcode);
                            return false;
                        }
                    }
                }
            }
            else if (toSearchDcNo.Length == 3)
            {
                var findAll =
                    TransDates.FindAll(f => f.LotNo == toSearchDcNo)
                        .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                if (findAll.Any())
                {
                    formattedDcode = findAll[0].DateNo;
                }
                else
                {
                    errorMsg = string.Format("当前DateCode：{0}解析失败", toFormatDcode);
                    return false;
                }
            }
            else if (toSearchDcNo.Length == 10)
            {
                var findAll =
                    TransDates.FindAll(f => f.Ymd == toSearchDcNo)
                        .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                if (findAll.Any())
                {
                    formattedDcode = findAll[0].DateNo;
                }
                else
                {
                    findAll =
                        TransDates.FindAll(f => f.Yymmdd == toSearchDcNo.Substring(0, 6))
                            .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                    if (findAll.Any())
                    {
                        formattedDcode = findAll[0].DateNo;
                    }
                    else
                    {

                        findAll = TransDates.FindAll(f => f.Week == toSearchDcNo.Substring(0, 4))
                           .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                        if (findAll.Any())
                        {
                            formattedDcode = findAll[0].DateNo;
                        }
                        else
                        {
                            errorMsg = string.Format("当前DateCode：{0}解析失败", toFormatDcode);
                            return false;
                        }
                    }
                }
            }
            else if (toSearchDcNo.Length == 4)
            {
                var findAll =
                    TransDates.FindAll(f => f.Week == toSearchDcNo)
                        .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                if (findAll.Any())
                {
                    formattedDcode = findAll[0].DateNo;
                }
                else
                {
                    findAll =
                        TransDates.FindAll(f => f.Week == toSearchDcNo.Substring(0, 4))
                            .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                    if (findAll.Any())
                    {
                        formattedDcode = findAll[0].DateNo;
                    }
                    else
                    {
                        errorMsg = string.Format("当前DateCode：{0}解析失败", toFormatDcode);
                        return false;
                    }
                }
            }
            else if (toSearchDcNo.Length == 6)
            {
                var findAll =
                    TransDates.FindAll(f => f.Yymmdd == toSearchDcNo)
                        .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                if (findAll.Any())
                {
                    formattedDcode = findAll[0].DateNo;
                }
                else
                {
                    findAll =
                        TransDates.FindAll(f => f.Week == toSearchDcNo.Substring(0, 4))
                            .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                    if (findAll.Any())
                    {
                        formattedDcode = findAll[0].DateNo;
                    }
                    else
                    {
                        errorMsg = string.Format("当前DateCode：{0}解析失败", toFormatDcode);
                        return false;
                    }
                }
            }
            else
            {
                if (toSearchDcNo.Length >= 4)
                {
                    var findAll =
                        TransDates.FindAll(f => f.Yymmdd == toSearchDcNo.Substring(0, 6))
                            .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                    if (findAll.Any())
                    {
                        formattedDcode = findAll[0].DateNo;
                    }
                    else
                    {

                        findAll = TransDates.FindAll(f => f.Week == toSearchDcNo.Substring(0, 4))
                           .OrderBy(o => DateTime.Parse(o.Ymd)).ToList();
                        if (findAll.Any())
                        {
                            formattedDcode = findAll[0].DateNo;
                        }
                        else
                        {
                            errorMsg = string.Format("当前DateCode：{0}解析失败", toFormatDcode);
                            return false;
                        }
                    }
                }
                else
                {
                    errorMsg = string.Format("当前DateCode：{0}解析失败", toFormatDcode);
                    return false;
                }
            }
            #endregion

            return true;
        }

        /// <summary>
        /// “[)>@06”格式开头的DM码
        /// 做特殊处理
        /// </summary>
        /// <param name="strBarcode"></param>
        /// <param name="newMaterialPrintCorresponds"></param>
        /// <param name="materialStockInInfo"></param>
        /// <param name="stockInfo"></param>
        /// <param name="errorMsg"></param>
        /// <returns></returns>
        public static bool LedOsramBarcodeAnalysis(
            string strBarcode, List<NewMaterialPrintCorrespond> newMaterialPrintCorresponds, List<NewMaterialStockInInfo> materialStockInInfo, out StockInfo stockInfo, ref string errorMsg)
        {
            try
            {
                stockInfo = new StockInfo();

                var listBarcodePart = new List<string>(strBarcode.Split(new[] { '@' }, StringSplitOptions.RemoveEmptyEntries));
                var strBin = string.Empty;
                var qty = -1;

                var strPartNum = listBarcodePart.Find(e => e.StartsWith("1P")).Substring(2);
                strPartNum = strPartNum.ToUpper();

                strBin += listBarcodePart.Find(e => e.StartsWith("1L")).Substring(2);
                strBin += "-";
                strBin += listBarcodePart.Find(e => e.StartsWith("1C")).Substring(2);
                strBin += "-";
                strBin += listBarcodePart.Find(e => e.StartsWith("1U")).Substring(2);

                if (listBarcodePart.Find(e => e.StartsWith("2L")).Length > 2)
                {
                    strBin += ":";
                    strBin += listBarcodePart.Find(e => e.StartsWith("2L")).Substring(2);
                    strBin += "-";
                    strBin += listBarcodePart.Find(e => e.StartsWith("2C")).Substring(2);
                    strBin += "-";
                    strBin += listBarcodePart.Find(e => e.StartsWith("2U")).Substring(2);
                }

                if (listBarcodePart.Find(e => e.StartsWith("3L")).Length > 2)
                {
                    strBin += ":";
                    strBin += listBarcodePart.Find(e => e.StartsWith("3L")).Substring(2);
                    strBin += "-";
                    strBin += listBarcodePart.Find(e => e.StartsWith("3C")).Substring(2);
                    strBin += "-";
                    strBin += listBarcodePart.Find(e => e.StartsWith("3U")).Substring(2);
                }

                if (listBarcodePart.Find(e => e.StartsWith("Q")).Length > 2)
                {
                    var qIndex = listBarcodePart.FindIndex(e => e.StartsWith("Q"));
                    var qtyStr = listBarcodePart[qIndex].Substring(1, listBarcodePart[qIndex].Length - 1);
                    qty = int.Parse(qtyStr);
                }

                strBin = strBin.ToUpper();

                var models = HikDbHelperSql.GetNewMaterialPrintCorrespondModels("IsDelete = '0' and Status = '有效' and SupplyLedGroup = '" + strBin + "' and ModelName = '" + strPartNum.Replace(" ", "") + "' and Count = '" + qty + "'");
                if (!models.Any())
                {
                    errorMsg += string.Format(
                        "SupplyLedGroup={0},ModelName={1},Count={2}在维护信息表中未找到数据",
                        strBin, strPartNum.Replace(" ", ""), qty);
                    return false;
                }

                var temp = new List<NewMaterialPrintCorrespond>();
                foreach (var find in materialStockInInfo.Select(item => models.FindAll(f => f.MaterialNo == item.MaterialNo)))
                    temp.AddRange(find);

                if (temp.Count == 0)
                {
                    errorMsg += string.Format(
                       "SupplyLedGroup={0},ModelName={1},Count={2}在维护信息表中未找到数据",
                       strBin, strPartNum.Replace(" ", ""), qty);
                    return false;
                }

                if (temp.Count > 1)
                {
                    var materialNos = temp.Aggregate(string.Empty, (current, t) => current + (t.MaterialNo + ',')).TrimEnd(',');

                    errorMsg += string.Format(
                        "SupplyLedGroup={0},ModelName={1},Count={2}在维护信息表中找到多条数据数据，相关物料号：{3}",
                        strBin, strPartNum.Replace(" ", ""), qty, materialNos);
                    return false;
                }

                var id = temp[0].id;
                var detail = newMaterialPrintCorresponds.Find(f => f.id == id);

                if (detail == null)
                {
                    errorMsg += string.Format(
                        "SupplyLedGroup={0},ModelName={1},Count={2}在当前选择的订单号和供应商中找到的相关物料号中未找到数据",
                        strBin, strPartNum.Replace(" ", ""), qty);
                    return false;
                }

                stockInfo = GetStockInfo(
                    strBarcode, detail.Barcodetype,
                    detail.MaterialNo, detail.SupplyLedGroup, detail.SupplyLedNo, detail.ModelName,
                    detail.PartNokey, detail.LotNokey, detail.DcNokey, detail.Qtykey);

                return true;
            }
            catch (Exception)
            {
                stockInfo = new StockInfo();
                return false;
            }
        }

        /// <summary>
        /// 矩阵码解析方法
        /// </summary>
        /// <param name="matrixCode">扫描得到的矩形码</param>
        /// <param name="newMaterialPrintCorresponds">匹配规则集合</param>
        /// <param name="stockInfo">生成的入库信息</param>
        /// <param name="errorMsg">解析错误信息</param>
        /// <returns>是否解析成功</returns>
        public static bool MatrixCodeAnalysis(
            string matrixCode, List<NewMaterialPrintCorrespond> newMaterialPrintCorresponds, out StockInfo stockInfo, ref string errorMsg)
        {
            stockInfo = new StockInfo();
            matrixCode = ReplaceSpecialCharacter(matrixCode);

            #region 符合条件的匹配规则
            var matchedList = new List<NewMaterialPrintCorrespond>(); // 符合条件的匹配规则

            foreach (var t in newMaterialPrintCorresponds.Where(t => ReplaceSpecialCharacter(matrixCode).Replace(" ", "").Contains(t.MaterialBarcode.Replace(" ", "")) && t.Barcodetype.StartsWith("分隔符")))
            {
                //if (!t.Barcodetype.StartsWith("分隔符"))
                //{
                //    errorMsg = string.Format("NewMaterialPrintCorrespond表中id={0}的数据中Barcodetype缺失;", t.id);
                //    return false;
                //}

                if (string.IsNullOrEmpty(t.PartNokey))
                {
                    errorMsg = string.Format("NewMaterialPrintCorrespond表中id={0}的数据中PartNokey缺失;", t.id);
                    return false;
                }

                if (string.IsNullOrEmpty(t.LotNokey))
                {
                    errorMsg = string.Format("NewMaterialPrintCorrespond表中id={0}的数据中LotNokey缺失;", t.id);
                    return false;
                }

                if (string.IsNullOrEmpty(t.Qtykey))
                {
                    errorMsg = string.Format("NewMaterialPrintCorrespond表中id={0}的数据中Qtykey缺失;", t.id);
                    return false;
                }

                if (string.IsNullOrEmpty(t.MaterialNo))
                {
                    errorMsg = string.Format("NewMaterialPrintCorrespond表中id={0}的数据中MaterialNo缺失;", t.id);
                    return false;
                }

                if (t.Count == null || string.IsNullOrEmpty(t.Count.ToString()))
                {
                    errorMsg = string.Format("NewMaterialPrintCorrespond表中id={0}的数据中Count缺失;", t.id);
                    return false;
                }

                matchedList.Add(t);
            }
            #endregion

            if (matchedList.Any())
            {
                #region  Qtykey不为“无”，即有规则可以解析出count
                if (matchedList.FindAll(f => f.Qtykey.Contains("无")).Count == 0) //  Qtykey不为“无”，即有规则可以解析出count
                {
                    var getStockInfoWithQtyAndLedGroup = new List<StockInfo>();

                    foreach (var eachMatchedDetail in matchedList)
                    {
                        var temp = new StockInfo
                        {
                            QtyNo = GetValueBySeparator(
                                eachMatchedDetail.Barcodetype, eachMatchedDetail.Qtykey, ReplaceSpecialCharacter(matrixCode), true)
                        };

                        if (string.IsNullOrEmpty(eachMatchedDetail.LedCatkey) || eachMatchedDetail.LedCatkey == "无") // 不用解析LED BIN
                        {
                            temp.SupplyLedGroup = string.Empty;
                        }
                        else
                        {
                            if (string.Equals(eachMatchedDetail.LedCatkey, "CREE", StringComparison.CurrentCultureIgnoreCase))
                                temp.SupplyLedGroup = FindCreeBin(matrixCode).ToUpper();
                            else if (string.Equals(eachMatchedDetail.LedCatkey, "NICHIA", StringComparison.CurrentCultureIgnoreCase))
                                temp.SupplyLedGroup = FindNichiaBin(matrixCode).ToUpper();
                            else if (string.Equals(eachMatchedDetail.LedCatkey, "EVERLIGHT", StringComparison.CurrentCultureIgnoreCase))
                                temp.SupplyLedGroup = FindEverLightBin(matrixCode).ToUpper();
                            else if (eachMatchedDetail.Barcodetype.StartsWith("分隔符"))
                                temp.SupplyLedGroup = GetValueBySeparator(eachMatchedDetail.Barcodetype,
                                       eachMatchedDetail.LedCatkey, ReplaceSpecialCharacter(matrixCode)).ToUpper();
                            else
                            {
                                errorMsg =
                                    string.Format("NewMaterialPrintCorrespond表中id={0}的数据中Barcodetype={1}或LEDcatkey={2}有误;",
                                    eachMatchedDetail.id, eachMatchedDetail.Barcodetype, eachMatchedDetail.LedCatkey);
                                return false;
                            }
                        }

                        getStockInfoWithQtyAndLedGroup.Add(temp);
                    }

                    foreach (var t in getStockInfoWithQtyAndLedGroup)
                    {
                        var findQtyAndLedGroupMatched =
                            matchedList.FindAll(
                                f => f.Count.ToString() == t.QtyNo && f.SupplyLedGroup.ToUpper() == t.SupplyLedGroup.ToUpper());

                        if (findQtyAndLedGroupMatched.Count > 1)
                        {
                            errorMsg =
                                string.Format("NewMaterialPrintCorrespond表中Count={0}和SupplyLedGroup{1}找到多条数据;", t.QtyNo, t.SupplyLedGroup.ToUpper());
                            return false;
                        }

                        if (findQtyAndLedGroupMatched.Count != 1)
                            continue;

                        var matched = findQtyAndLedGroupMatched[0];
                        stockInfo = GetStockInfo(
                            ReplaceSpecialCharacter(matrixCode), matched.Barcodetype,
                            matched.MaterialNo, matched.SupplyLedGroup, matched.SupplyLedNo, matched.ModelName,
                            matched.PartNokey, matched.LotNokey, matched.DcNokey, matched.Qtykey);
                        //stockInfo.MatchedId = matched.id;
                        return true;
                    }

                    errorMsg = "NewMaterialPrintCorrespond表中找不到符合当前扫描规格和档位的数据";
                    return false;
                }
                #endregion

                #region Qtykey为“无”，但矩阵码中是通过分隔符查询，不可能出现qtykey为"无"的情况
                errorMsg = "Qtykey为“无”，但矩阵码中是通过分隔符查询，不可能出现qtykey为“无”的情况";
                return false;
                //if (matchedList.FindAll(f => f.Qtykey == "无").Count == matchedList.Count)
                //{
                //    var getStockInfoLedGroup = new List<StockInfo>();

                //    foreach (var eachMatchedDetail in matchedList)
                //    {
                //        var temp = new StockInfo();

                //        if (string.IsNullOrEmpty(eachMatchedDetail.LedCatkey) || eachMatchedDetail.LedCatkey == "无") // 不用解析LED BIN
                //        {
                //            temp.SupplyLedGroup = string.Empty;
                //        }
                //        else
                //        {
                //            if (string.Equals(eachMatchedDetail.LedCatkey, "CREE", StringComparison.CurrentCultureIgnoreCase))
                //                temp.SupplyLedGroup = FindCreeBin(matrixCode);
                //            else if (string.Equals(eachMatchedDetail.LedCatkey, "NICHIA", StringComparison.CurrentCultureIgnoreCase))
                //                temp.SupplyLedGroup = FindNichiaBin(matrixCode);
                //            else if (string.Equals(eachMatchedDetail.LedCatkey, "EVERLIGHT", StringComparison.CurrentCultureIgnoreCase))
                //                temp.SupplyLedGroup = FindEverLightBin(matrixCode);
                //            else if (eachMatchedDetail.Barcodetype.StartsWith("分隔符"))
                //                temp.SupplyLedGroup = GetValueBySeparator(eachMatchedDetail.Barcodetype,
                //                       eachMatchedDetail.LedCatkey, matrixCode);
                //            else
                //            {
                //                errorMsg =
                //                     string.Format("NewMaterialPrintCorrespond表中id={0}的数据中Barcodetype={1}或LEDcatkey={2}有误;",
                //                     eachMatchedDetail.id, eachMatchedDetail.Barcodetype, eachMatchedDetail.LedCatkey);
                //                return false;
                //            }
                //        }

                //        getStockInfoLedGroup.Add(temp);
                //    }

                //    foreach (var t in getStockInfoLedGroup)
                //    {
                //        var findQtyAndLedGroupMatched = matchedList.FindAll(f => f.SupplyLedGroup == t.SupplyLedGroup);

                //        if (findQtyAndLedGroupMatched.Count > 1)
                //        {
                //            errorMsg =
                //                string.Format("NewMaterialPrintCorrespond表中SupplyLedGroup{0}找到多条数据;", t.SupplyLedGroup);
                //            return false;
                //        }

                //        if (findQtyAndLedGroupMatched.Count != 1)
                //            continue;

                //        var matched = matchedList[0];
                //        stockInfo = GetStockInfo(
                //            matrixCode, matched.Barcodetype,
                //            matched.MaterialNo, matched.SupplyLedGroup, matched.SupplyLedNo, matched.ModelName,
                //            matched.PartNokey, matched.LotNokey, matched.DcNokey, matched.Qtykey);
                //        //stockInfo.MatchedId = matched.id;
                //        return true;
                //    }
                //}
                #endregion
            }

            errorMsg = "没有找到矩阵码的匹配关系数据";
            return false;
        }

        /// <summary>
        /// 条形码解析方法
        /// </summary>
        /// <param name="scanResults">扫描得到的条码码集合</param>
        /// <param name="newMaterialPrintCorresponds">匹配规则集合</param>
        /// <param name="stockInfo">生成的入库信息</param>
        /// <param name="errorMsg">解析错误信息</param>
        /// <returns>是否解析成功</returns>
        public static bool BarCodeAnalysis(
            List<HikScanerClass.BarcodeStruct> scanResults, List<NewMaterialPrintCorrespond> newMaterialPrintCorresponds, out StockInfo stockInfo, ref string errorMsg)
        {
            stockInfo = new StockInfo();

            #region 符合条件的匹配规则
            var expectedMatchedList = new List<NewMaterialPrintCorrespond>();
            foreach (var t in scanResults.FindAll(f => !IsStringAllInt(ReplaceSpecialCharacter(f.Barcode))))
            {
                foreach (var r in newMaterialPrintCorresponds.FindAll(f => f.Barcodetype.StartsWith("关键字") || f.Barcodetype.Contains("位置")))
                {
                    if (ReplaceSpecialCharacter(t.Barcode.Replace(" ", "")).Contains(r.MaterialBarcode.Replace(" ", "")))
                    {
                        if (string.IsNullOrEmpty(r.PartNokey))
                        {
                            errorMsg = string.Format("NewMaterialPrintCorrespond中id={0}的数据中PartNokey缺失;", r.id);
                            return false;
                        }

                        if (string.IsNullOrEmpty(r.LotNokey))
                        {
                            errorMsg = string.Format("NewMaterialPrintCorrespond中id={0}的数据中LotNokey缺失;", r.id);
                            return false;
                        }

                        if (string.IsNullOrEmpty(r.Qtykey))
                        {
                            errorMsg = string.Format("NewMaterialPrintCorrespond中id={0}的数据中Qtykey缺失;", r.id);
                            return false;
                        }

                        if (string.IsNullOrEmpty(r.MaterialNo))
                        {
                            errorMsg = string.Format("NewMaterialPrintCorrespond中id={0}的数据中MaterialNo缺失;", r.id);
                            return false;
                        }

                        if (r.Count == null || string.IsNullOrEmpty(r.Count.ToString()))
                        {
                            errorMsg = string.Format("NewMaterialPrintCorrespond中id={0}的数据中Count缺失;", r.id);
                            return false;
                        }

                        expectedMatchedList.Add(r);
                    }
                }
            }

            if (!expectedMatchedList.Any())
            {
                errorMsg = "没有找到二维码的匹配关系数据";
                return false;
            }
            #endregion

            var chars = string.Empty;
            for (var i = 0x20; i <= 0x2F; i++)
                chars += Encoding.ASCII.GetString(new byte[] { (byte)i });
            for (var i = 0x3A; i <= 0x7F; i++)
                chars += Encoding.ASCII.GetString(new byte[] { (byte)i });

            try
            {
                #region  Qtykey不为“无”，即有规则可以解析出count
                if (expectedMatchedList.FindAll(f => f.Qtykey.Contains("无")).Count == 0)
                {
                    var matchedByCountList = new List<NewMaterialPrintCorrespond>();

                    for (var i = 0; i < expectedMatchedList.Count; i++)
                    {
                        var item = expectedMatchedList[i];
                        var qtyNoKey = item.Qtykey;

                        if (item.Barcodetype.StartsWith("关键字"))
                        {
                            // 找Qty即count
                            foreach (var t in scanResults)
                            {
                                if (ReplaceSpecialCharacter(t.Barcode).StartsWith(qtyNoKey) || ReplaceSpecialCharacter(t.Barcode).EndsWith(qtyNoKey))
                                {
                                    // 以qtyNoKey开头或结尾
                                    var getQty = string.Empty;
                                    if (ReplaceSpecialCharacter(t.Barcode).StartsWith(qtyNoKey))
                                        getQty = GetCount(ReplaceSpecialCharacter(t.Barcode).Substring(
                                            qtyNoKey.Length, ReplaceSpecialCharacter(t.Barcode).Length - qtyNoKey.Length).Trim());
                                    else if (ReplaceSpecialCharacter(t.Barcode).EndsWith(qtyNoKey))
                                        getQty = GetCount(ReplaceSpecialCharacter(t.Barcode).Substring(
                                            0, ReplaceSpecialCharacter(t.Barcode).Length - qtyNoKey.Length).Trim());

                                    int getQtyInt;
                                    if (!int.TryParse(getQty, out getQtyInt) ||
                                        getQtyInt.ToString() != item.Count.ToString())
                                        continue;

                                    stockInfo.QtyNo = getQtyInt.ToString();
                                    matchedByCountList.Add(expectedMatchedList[i]);
                                    break;
                                }
                            }
                        }
                        else if (item.Barcodetype.Contains("位置"))
                        {
                            if (qtyNoKey.Contains(','))
                            {
                                var qtyKeySp = qtyNoKey.Split(',');
                                var qtyStartWith = qtyKeySp[0];
                                var qtyIndex = int.Parse(qtyKeySp[1]);
                                var qtyLen = int.Parse(qtyKeySp[2]);

                                foreach (var t in scanResults)
                                {
                                    var rpBarcode = ReplaceSpecialCharacter(t.Barcode);

                                    if (item.Barcodetype.Contains("起始位置"))
                                    {
                                        if (rpBarcode.StartsWith(qtyStartWith) && rpBarcode.Length >= qtyLen)
                                        {
                                            var getQty = rpBarcode.Substring(qtyIndex, qtyLen);

                                            int getQtyInt;
                                            if (!int.TryParse(getQty, out getQtyInt) ||
                                                getQtyInt.ToString() != item.Count.ToString())
                                                continue;

                                            stockInfo.QtyNo = getQtyInt.ToString();
                                            matchedByCountList.Add(expectedMatchedList[i]);
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        if (rpBarcode.Contains(qtyStartWith) && rpBarcode.Length >= qtyLen)
                                        {
                                            var getQty = rpBarcode.Substring(qtyIndex, qtyLen);

                                            int getQtyInt;
                                            if (!int.TryParse(getQty, out getQtyInt) ||
                                                getQtyInt.ToString() != item.Count.ToString())
                                                continue;

                                            stockInfo.QtyNo = getQtyInt.ToString();
                                            matchedByCountList.Add(expectedMatchedList[i]);
                                            break;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                // 找Qty即count
                                foreach (var t in scanResults)
                                {
                                    if (ReplaceSpecialCharacter(t.Barcode).StartsWith(qtyNoKey) || ReplaceSpecialCharacter(t.Barcode).EndsWith(qtyNoKey))
                                    {
                                        // 以qtyNoKey开头或结尾
                                        var getQty = string.Empty;
                                        if (ReplaceSpecialCharacter(t.Barcode).StartsWith(qtyNoKey))
                                            getQty = GetCount(ReplaceSpecialCharacter(t.Barcode).Substring(
                                                qtyNoKey.Length, ReplaceSpecialCharacter(t.Barcode).Length - qtyNoKey.Length).Trim());
                                        else if (ReplaceSpecialCharacter(t.Barcode).EndsWith(qtyNoKey))
                                            getQty = GetCount(ReplaceSpecialCharacter(t.Barcode).Substring(
                                                0, ReplaceSpecialCharacter(t.Barcode).Length - qtyNoKey.Length).Trim());

                                        int getQtyInt;
                                        if (!int.TryParse(getQty, out getQtyInt) ||
                                            getQtyInt.ToString() != item.Count.ToString())
                                            continue;

                                        stockInfo.QtyNo = getQtyInt.ToString();
                                        matchedByCountList.Add(expectedMatchedList[i]);
                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            errorMsg = "二维码解析失败，无法定位到当前扫描得到的规则和档位的数据";
                            return false;
                        }
                    }

                    if (!matchedByCountList.Any())
                    {
                        errorMsg = "没有找到二维码的匹配关系数据";
                        return false;
                    }

                    // 再根据bin值查找
                    foreach (var eachMatchedDetail in matchedByCountList)
                    {
                        var temp = new StockInfo
                        {
                            QtyNo = eachMatchedDetail.Count.ToString(),
                            MaterialNo = eachMatchedDetail.MaterialNo,
                            ModelName = eachMatchedDetail.ModelName,
                            //SupplyLedGroup = eachMatchedDetail.SupplyLedGroup,
                        };

                        string bin;
                        var findBinError = string.Empty;
                        if (TryFindBin(eachMatchedDetail, scanResults, out bin, ref findBinError))
                        {
                            temp.SupplyLedGroup = bin;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(findBinError))
                            {
                                errorMsg = findBinError;
                                return false;
                            }
                        }

                        if (temp.QtyNo == eachMatchedDetail.Count.ToString() && string.Equals(temp.SupplyLedGroup, eachMatchedDetail.SupplyLedGroup, StringComparison.CurrentCultureIgnoreCase))
                        {
                            string partNo;
                            string lotNo;
                            string dcNo;
                            var findValueErrorMsg = string.Empty;
                            FindValue(eachMatchedDetail, scanResults, out partNo, out lotNo, out dcNo, ref findValueErrorMsg);

                            if (string.IsNullOrEmpty(findValueErrorMsg))
                            {
                                temp.PartNo = partNo;
                                temp.LotNo = lotNo;
                                temp.DcNo = dcNo;

                                temp.SupplyLedNo = eachMatchedDetail.SupplyLedNo;

                                stockInfo = temp;
                                return true;
                            }

                            errorMsg = findValueErrorMsg;
                            return false;
                        }
                    }

                    errorMsg = "二维码解析失败，无法定位到当前扫描得到的规则和档位的数据";
                    return false;
                }
                #endregion

                #region Qtykey为“无定位”，没有关键字可输入，需要识别二维码内容中全为数字的码
                if (expectedMatchedList.FindAll(f => f.Qtykey == "无定位").Count == expectedMatchedList.Count)
                {
                    // 先根据BIN值查找
                    var matchedByBinList = new List<NewMaterialPrintCorrespond>();
                    foreach (var eachMatchedDetail in expectedMatchedList)
                    {
                        string bin;
                        var findBinError = string.Empty;
                        if (TryFindBin(eachMatchedDetail, scanResults, out bin, ref findBinError))
                        {
                            matchedByBinList.Add(eachMatchedDetail);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(findBinError))
                            {
                                errorMsg = findBinError;
                                return false;
                            }
                        }
                    }

                    if (!matchedByBinList.Any())
                    {
                        errorMsg = "找不到相关档位的数据";
                        return false;
                    }

                    foreach (var eachMatchedDetail in matchedByBinList)
                    {
                        // 再解析条码中长度<6且全部为数字的
                        foreach (var mt in scanResults)
                        {
                            int tempCount;
                            var toCount = ReplaceSpecialCharacter(mt.Barcode).Trim(chars.ToCharArray());

                            if (ReplaceSpecialCharacter(mt.Barcode).Length < 6 && int.TryParse(toCount, out tempCount))
                            {
                                if (tempCount > 0 && tempCount <= HikSetup.MaxCount)
                                {
                                    if (eachMatchedDetail.Count.ToString() == tempCount.ToString())
                                    {
                                        var temp = new StockInfo
                                        {
                                            QtyNo = eachMatchedDetail.Count.ToString(),
                                            MaterialNo = eachMatchedDetail.MaterialNo,
                                            ModelName = eachMatchedDetail.ModelName,
                                            SupplyLedGroup = eachMatchedDetail.SupplyLedGroup
                                        };

                                        string partNo;
                                        string lotNo;
                                        string dcNo;
                                        var findValueErrorMsg = string.Empty;
                                        FindValue(eachMatchedDetail, scanResults, out partNo, out lotNo, out dcNo, ref findValueErrorMsg);

                                        if (string.IsNullOrEmpty(findValueErrorMsg))
                                        {
                                            temp.PartNo = partNo;
                                            temp.LotNo = lotNo;
                                            temp.DcNo = dcNo;

                                            temp.SupplyLedNo = eachMatchedDetail.SupplyLedNo;

                                            stockInfo = temp;
                                            return true;
                                        }

                                        errorMsg = findValueErrorMsg;
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    errorMsg = "Qtykey为无，但无法从条形码中找到匹配的规格";
                    return false;
                }
                #endregion

                #region Qtykey为“无”，没有Count二维码，取表中数据

                if (expectedMatchedList.FindAll(f => f.Qtykey == "无").Count == expectedMatchedList.Count)
                {
                    // 先根据BIN值查找
                    var matchedByBinList = new List<NewMaterialPrintCorrespond>();
                    foreach (var eachMatchedDetail in expectedMatchedList)
                    {
                        string bin;
                        var findBinError = string.Empty;
                        if (TryFindBin(eachMatchedDetail, scanResults, out bin, ref findBinError))
                        {
                            matchedByBinList.Add(eachMatchedDetail);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(findBinError))
                            {
                                errorMsg = findBinError;
                                return false;
                            }
                        }
                    }

                    if (!matchedByBinList.Any())
                    {
                        errorMsg = "找不到相关档位的数据";
                        return false;
                    }

                    if (matchedByBinList.Count == 0)
                    {
                        errorMsg = "QtyKey类型为无，需要取系统内的Count，但未找到任何匹配数据";
                        return false;
                    }
                    if (matchedByBinList.Count == 1)
                    {
                        var eachMatchedDetail = matchedByBinList[0];

                        var temp = new StockInfo
                        {
                            QtyNo = eachMatchedDetail.Count.ToString(),
                            MaterialNo = eachMatchedDetail.MaterialNo,
                            ModelName = eachMatchedDetail.ModelName,
                            SupplyLedGroup = eachMatchedDetail.SupplyLedGroup,
                        };

                        string partNo;
                        string lotNo;
                        string dcNo;
                        var findValueErrorMsg = string.Empty;
                        FindValue(eachMatchedDetail, scanResults, out partNo, out lotNo, out dcNo, ref findValueErrorMsg);

                        if (string.IsNullOrEmpty(findValueErrorMsg))
                        {
                            temp.PartNo = partNo;
                            temp.LotNo = lotNo;
                            temp.DcNo = dcNo;

                            temp.SupplyLedNo = eachMatchedDetail.SupplyLedNo;

                            stockInfo = temp;
                            return true;
                        }

                        errorMsg = findValueErrorMsg;
                        return false;
                    }

                    errorMsg = "QtyKey类型为无，需要取系统内的Count，但找到多条匹配数据，无法定位";
                    return false;
                }

                #endregion

                errorMsg = "找到的Qtykey格式有无";
                return false;
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 生成入库信息
        /// </summary>
        /// <param name="code">矩阵码内容</param>
        /// <param name="barcodeType">解析类型</param>
        /// <param name="materialNo">物料号</param>
        /// <param name="supplyLedGroup">BIN</param>
        /// <param name="supplyLedNo">LedNo</param>
        /// <param name="modelName">物料型号</param>
        /// <param name="partNoKey">PartNo关键字</param>
        /// <param name="lotNoKey">LotNo关键字</param>
        /// <param name="dcNoKey">DateCode关键字</param>
        /// <param name="qtykey">Count关键字</param>
        /// <returns></returns>
        private static StockInfo GetStockInfo(
            string code, string barcodeType,
            string materialNo, string supplyLedGroup, string supplyLedNo, string modelName,
            string partNoKey, string lotNoKey, string dcNoKey, string qtykey = "无")
        {
            var stockInfo = new StockInfo
            {
                PartNo = partNoKey == "无" ? string.Empty : GetValueBySeparator(barcodeType, partNoKey, code).Trim(),
                LotNo = lotNoKey == "无" ? string.Empty : GetValueBySeparator(barcodeType, lotNoKey, code).Trim(),
                DcNo = dcNoKey == "无" ? DateTime.Now.ToString("yyyyMMdd") : GetValueBySeparator(barcodeType, dcNoKey, code),
                QtyNo = GetValueBySeparator(barcodeType, qtykey, code, true),
                MaterialNo = materialNo.Trim(),
                SupplyLedGroup = supplyLedGroup.Trim(),
                SupplyLedNo = supplyLedNo.Trim(),
                ModelName = modelName.Trim()
            };

            return stockInfo;
        }

        private static bool IsStringAllInt(string value)
        {
            return value.Select(t => Encoding.ASCII.GetBytes(t.ToString())[0]).All(b => b >= 0x30 && b <= 0x39);
        }

        /// <summary>
        /// 将不可见字符用@符号代替
        /// 0x20~0x7E为不用代替的可见字符
        /// </summary>
        /// <param name="toReplace"></param>
        /// <returns></returns>
        public static string ReplaceSpecialCharacter(string toReplace)
        {
            if (string.IsNullOrEmpty(toReplace))
                return string.Empty;
            var bytes = Encoding.ASCII.GetBytes(toReplace);
            for (var i = 0; i < bytes.Length; i++)
            {
                var val = bytes[i];
                if (val >= 0x20 && val <= 0x7E)
                    bytes[i] = val;
                else
                    bytes[i] = 0x40;
            }

            return Encoding.ASCII.GetString(bytes);
        }

        /// <summary>
        /// 分隔符处理方法
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="key"></param>
        /// <param name="codeValue"></param>
        /// <param name="isQty"></param>
        /// <returns></returns>
        private static string GetValueBySeparator(string keyType, string key, string codeValue, bool isQty = false)
        {
            try
            {
                if (key == "无")
                    return "无";

                if (keyType == "分隔符")
                {
                    var keySp = key.Split(',');
                    var separatorValue = keySp[0];
                    var keyIndex = int.Parse(keySp[1].ToLower());
                    var keyValue = keySp[2];

                    if (!isQty)
                    {
                        var str = codeValue.Split(separatorValue.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[keyIndex].Substring(keyValue.Length,
                            codeValue.Split(separatorValue.ToCharArray())[keyIndex].Length - keyValue.Length).Trim();
                        return str;
                    }

                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        var str = codeValue.Split(separatorValue.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[keyIndex].Replace(keyValue, string.Empty).Trim();
                        return GetCount(str);

                    }
                    else
                    {
                        var str = codeValue.Split(separatorValue.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[keyIndex].Trim();
                        return GetCount(str);
                    }
                }
                if (keyType != "分隔符" && keyType.StartsWith("分隔符") && keyType.Length == 4)
                {
                    var separatorValue = keyType.Substring(3, 1);
                    var keySp = key.Split(new[] { separatorValue }, StringSplitOptions.None);
                    var keyIndex = int.Parse(keySp[0].ToLower());
                    var keyValue = keySp[1];

                    if (!isQty)
                    {
                        var str = codeValue.Split(separatorValue.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[keyIndex].Substring(keyValue.Length,
                           codeValue.Split(separatorValue.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[keyIndex].Length - keyValue.Length).Trim();
                        return str;
                    }

                    if (!string.IsNullOrEmpty(keyValue))
                    {
                        var str = codeValue.Split(separatorValue.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[keyIndex].Replace(keyValue, string.Empty).Trim();
                        return GetCount(str);
                    }
                    else
                    {
                        var str = codeValue.Split(separatorValue.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[keyIndex].Trim();
                        return GetCount(str);
                    }
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        private static string GetCount(string strValue)
        {
            return Encoding.ASCII.GetBytes(strValue).Where(t => t >= 0x30 && t <= 0x39)
                .Aggregate(string.Empty, (current, t) => current + Encoding.ASCII.GetString(new[] { t }));
        }

        /// <summary>
        /// CREE品牌解析BIN值方法
        /// 用@分隔，第4位-第5位
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string FindCreeBin(string code)
        {
            try
            {
                return code.Split('@')[4] + "-" + code.Split('@')[5];
            }
            catch (Exception ex)
            {
                return "未读取出BIN值:" + ex.Message;
            }
        }

        /// <summary>
        /// EVERLIGHT品牌解析BIN值方法
        /// 规则是用;分隔，获取第5，6，7位用空格连接
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string FindEverLightBin(string code)
        {
            try
            {
                return
                    string.Format("{0}-{1}-{2}", code.Split(';')[5], code.Split(';')[6], code.Split(';')[7]).Replace("CAT:", "").Replace("HUE:", "").Replace("REF:", "");
            }
            catch (Exception ex)
            {
                return "未读取出BIN值:" + ex.Message;
            }
        }

        /// <summary>
        /// NICHIA品牌解析BIN值方法
        /// 规则是用，分隔 取第3位
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static string FindNichiaBin(string code)
        {
            try
            {
                return code.Split(',')[3];
            }
            catch (Exception ex)
            {
                return "未读取出BIN值:" + ex.Message;
            }
        }

        private static bool TryFindBin(
            NewMaterialPrintCorrespond eachMatchedDetail, IEnumerable<HikScanerClass.BarcodeStruct> scanResults, out string bin, ref string errorMsg)
        {
            bin = string.Empty;

            if (string.IsNullOrEmpty(eachMatchedDetail.LedCatkey) || eachMatchedDetail.LedCatkey == "无") // 不用解析LED BIN
            {
                bin = string.Empty;
                return true;
            }

            if (eachMatchedDetail.LedCatkey == "无定位")
            {
                if (string.IsNullOrEmpty(eachMatchedDetail.SupplyLedGroup))
                {
                    errorMsg =
                        string.Format(
                            "NewMaterialPrintCorrespond表中id={0}的数据中LEDcatkey={1}，应取对照表中的SupplyLedGroup做定位，但对照表中SupplyLedGroup为空;",
                            eachMatchedDetail.id, eachMatchedDetail.Barcodetype, eachMatchedDetail.LedCatkey);
                    return false;
                }

                if (scanResults.Select(t => ReplaceSpecialCharacter(t.Barcode)).Any(rp => rp.Contains(eachMatchedDetail.SupplyLedGroup)))
                {
                    bin = eachMatchedDetail.SupplyLedGroup;
                    return true;
                }
            }
            else
            {
                if (eachMatchedDetail.Barcodetype.StartsWith("关键字"))
                {
                    var detail = eachMatchedDetail;
                    foreach (var t in scanResults.Where(t => ReplaceSpecialCharacter(t.Barcode).StartsWith(detail.LedCatkey)))
                    {
                        bin = ReplaceSpecialCharacter(t.Barcode)
                            .Substring(eachMatchedDetail.LedCatkey.Length,
                                ReplaceSpecialCharacter(t.Barcode).Length - eachMatchedDetail.LedCatkey.Length).ToUpper();

                        if (bin == eachMatchedDetail.SupplyLedGroup)
                        {
                            return true;
                        }
                        else
                        {
                            bin = string.Empty;
                        }

                    }
                }
                else if (eachMatchedDetail.Barcodetype.Contains("位置"))
                {
                    var detail = eachMatchedDetail;

                    if (detail.LedCatkey.Contains(','))
                    {
                        var ledKeySp = detail.LedCatkey.Split(',');
                        var ledStartWith = ledKeySp[0];
                        var ledIndex = int.Parse(ledKeySp[1]);
                        var ledLen = int.Parse(ledKeySp[2]);

                        foreach (var t in scanResults)
                        {
                            var rpBarcode = ReplaceSpecialCharacter(t.Barcode);

                            if (eachMatchedDetail.Barcodetype.Contains("起始位置"))
                            {
                                if (rpBarcode.StartsWith(ledStartWith) && rpBarcode.Length >= ledLen)
                                {
                                    bin = rpBarcode.Substring(ledIndex, ledLen);
                                    if (bin == eachMatchedDetail.SupplyLedGroup)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        bin = string.Empty;
                                    }
                                }
                            }
                            else
                            {
                                if (rpBarcode.Contains(ledStartWith) && rpBarcode.Length >= ledLen)
                                {
                                    bin = rpBarcode.Substring(ledIndex, ledLen);
                                    if (bin == eachMatchedDetail.SupplyLedGroup)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        bin = string.Empty;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        foreach (var t in scanResults.Where(t => ReplaceSpecialCharacter(t.Barcode).StartsWith(detail.LedCatkey)))
                        {
                            bin = ReplaceSpecialCharacter(t.Barcode)
                                .Substring(eachMatchedDetail.LedCatkey.Length,
                                    ReplaceSpecialCharacter(t.Barcode).Length - eachMatchedDetail.LedCatkey.Length).ToUpper();
                            if (bin == eachMatchedDetail.SupplyLedGroup)
                            {
                                return true;
                            }
                            else
                            {
                                bin = string.Empty;
                            }
                        }
                    }
                }
                else
                {
                    errorMsg =
                        string.Format("NewMaterialPrintCorrespond表中id={0}的数据中Barcodetype={1}或LEDcatkey={2}有误;",
                            eachMatchedDetail.id, eachMatchedDetail.Barcodetype, eachMatchedDetail.LedCatkey);
                    return false;
                }
            }

            return false;
        }

        private static void FindValue(NewMaterialPrintCorrespond eachMatchedDetail, List<HikScanerClass.BarcodeStruct> scanResults, out string partNo, out string lotNo, out string dcNo, ref string errorMsg)
        {
            partNo = string.Empty;
            lotNo = string.Empty;
            dcNo = string.Empty;

            // 再找剩余的
            if (eachMatchedDetail.Barcodetype.StartsWith("关键字"))
            {
                foreach (var t in scanResults)
                {
                    if (!string.IsNullOrEmpty(eachMatchedDetail.PartNokey) &&
                        string.IsNullOrEmpty(partNo) &&
                        ReplaceSpecialCharacter(t.Barcode).StartsWith(eachMatchedDetail.PartNokey))
                    {
                        partNo = ReplaceSpecialCharacter(t.Barcode).Substring(eachMatchedDetail.PartNokey.Length, ReplaceSpecialCharacter(t.Barcode).Length - eachMatchedDetail.PartNokey.Length).Trim();
                    }

                    if (!string.IsNullOrEmpty(eachMatchedDetail.LotNokey) &&
                        string.IsNullOrEmpty(lotNo) &&
                        ReplaceSpecialCharacter(t.Barcode).StartsWith(eachMatchedDetail.LotNokey))
                    {
                        lotNo = ReplaceSpecialCharacter(t.Barcode).Substring(eachMatchedDetail.LotNokey.Length, ReplaceSpecialCharacter(t.Barcode).Length - eachMatchedDetail.LotNokey.Length).Trim();
                    }

                    if (!string.IsNullOrEmpty(eachMatchedDetail.DcNokey) && string.IsNullOrEmpty(dcNo))
                    {
                        if (eachMatchedDetail.DcNokey == "无")
                        {
                            dcNo = DateTime.Now.ToString("yyyyMMdd"); //"无";
                            //break;
                        }

                        if (ReplaceSpecialCharacter(t.Barcode).StartsWith(eachMatchedDetail.DcNokey))
                        {
                            dcNo = ReplaceSpecialCharacter(t.Barcode).Substring(eachMatchedDetail.DcNokey.Length, ReplaceSpecialCharacter(t.Barcode).Length - eachMatchedDetail.DcNokey.Length).Trim();
                            //break;
                        }
                    }
                }
            }
            else if (eachMatchedDetail.Barcodetype == "位置")
            {
                if (eachMatchedDetail.PartNokey.Contains(','))
                {
                    if (eachMatchedDetail.PartNokey == "无")
                    {
                        partNo = string.Empty;
                    }
                    else
                    {
                        var sp = eachMatchedDetail.PartNokey.Split(',');
                        var startWith = sp[0];
                        var index = int.Parse(sp[1]);
                        var len = int.Parse(sp[2]);
                        foreach (var t in scanResults)
                        {
                            var rp = ReplaceSpecialCharacter(t.Barcode);
                            if (rp.Contains(startWith) && rp.Length >= len)
                            {
                                partNo = rp.Substring(index, len);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var t in scanResults)
                    {
                        if (!string.IsNullOrEmpty(eachMatchedDetail.PartNokey) &&
                            string.IsNullOrEmpty(partNo) &&
                            ReplaceSpecialCharacter(t.Barcode).StartsWith(eachMatchedDetail.PartNokey))
                        {
                            partNo = ReplaceSpecialCharacter(t.Barcode).Substring(eachMatchedDetail.PartNokey.Length, ReplaceSpecialCharacter(t.Barcode).Length - eachMatchedDetail.PartNokey.Length).Trim();
                            break;
                        }
                    }
                }

                if (eachMatchedDetail.LotNokey.Contains(','))
                {
                    if (eachMatchedDetail.LotNokey == "无")
                    {
                        lotNo = string.Empty;
                    }
                    else
                    {
                        var sp = eachMatchedDetail.LotNokey.Split(',');
                        var startWith = sp[0];
                        var index = int.Parse(sp[1]);
                        var len = int.Parse(sp[2]);
                        foreach (var t in scanResults)
                        {
                            var rp = ReplaceSpecialCharacter(t.Barcode);
                            if (rp.Contains(startWith) && rp.Length >= len)
                            {
                                lotNo = rp.Substring(index, len);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var t in scanResults)
                    {
                        if (!string.IsNullOrEmpty(eachMatchedDetail.LotNokey) &&
                            string.IsNullOrEmpty(lotNo) &&
                            ReplaceSpecialCharacter(t.Barcode).StartsWith(eachMatchedDetail.LotNokey))
                        {
                            lotNo =
                                ReplaceSpecialCharacter(t.Barcode)
                                    .Substring(eachMatchedDetail.LotNokey.Length,
                                        ReplaceSpecialCharacter(t.Barcode).Length -
                                        eachMatchedDetail.LotNokey.Length)
                                    .Trim();
                            break;
                        }
                    }
                }

                if (eachMatchedDetail.DcNokey.Contains(','))
                {
                    if (eachMatchedDetail.DcNokey == "无")
                    {
                        dcNo = DateTime.Now.ToString("yyyyMMdd"); //"无";
                    }
                    else
                    {
                        var sp = eachMatchedDetail.DcNokey.Split(',');
                        var startWith = sp[0];
                        var index = int.Parse(sp[1]);
                        var len = int.Parse(sp[2]);
                        foreach (var t in scanResults)
                        {
                            var rp = ReplaceSpecialCharacter(t.Barcode);
                            if (rp.Contains(startWith) && rp.Length >= len)
                            {
                                dcNo = rp.Substring(index, len);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var t in scanResults)
                    {
                        if (!string.IsNullOrEmpty(eachMatchedDetail.DcNokey) && string.IsNullOrEmpty(dcNo))
                        {
                            if (eachMatchedDetail.DcNokey == "无")
                            {
                                dcNo = DateTime.Now.ToString("yyyyMMdd"); //"无";
                                break;
                            }

                            if (ReplaceSpecialCharacter(t.Barcode).StartsWith(eachMatchedDetail.DcNokey))
                            {
                                dcNo = ReplaceSpecialCharacter(t.Barcode).Substring(eachMatchedDetail.DcNokey.Length, ReplaceSpecialCharacter(t.Barcode).Length - eachMatchedDetail.DcNokey.Length).Trim();
                                break;
                            }
                        }
                    }
                }
            }
            else if (eachMatchedDetail.Barcodetype == "起始位置")
            {
                if (eachMatchedDetail.PartNokey.Contains(','))
                {
                    if (eachMatchedDetail.PartNokey == "无")
                    {
                        partNo = string.Empty;
                    }
                    else
                    {
                        var sp = eachMatchedDetail.PartNokey.Split(',');
                        var startWith = sp[0];
                        var index = int.Parse(sp[1]);
                        var len = int.Parse(sp[2]);
                        foreach (var t in scanResults)
                        {
                            var rp = ReplaceSpecialCharacter(t.Barcode);
                            if (rp.StartsWith(startWith) && rp.Length >= len)
                            {
                                partNo = rp.Substring(index, len);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var t in scanResults)
                    {
                        if (!string.IsNullOrEmpty(eachMatchedDetail.PartNokey) &&
                            string.IsNullOrEmpty(partNo) &&
                            ReplaceSpecialCharacter(t.Barcode).StartsWith(eachMatchedDetail.PartNokey))
                        {
                            partNo = ReplaceSpecialCharacter(t.Barcode).Substring(eachMatchedDetail.PartNokey.Length, ReplaceSpecialCharacter(t.Barcode).Length - eachMatchedDetail.PartNokey.Length).Trim();
                            break;
                        }
                    }
                }

                if (eachMatchedDetail.LotNokey.Contains(','))
                {
                    if (eachMatchedDetail.LotNokey == "无")
                    {
                        lotNo = string.Empty;
                    }
                    else
                    {
                        var sp = eachMatchedDetail.LotNokey.Split(',');
                        var startWith = sp[0];
                        var index = int.Parse(sp[1]);
                        var len = int.Parse(sp[2]);
                        foreach (var t in scanResults)
                        {
                            var rp = ReplaceSpecialCharacter(t.Barcode);
                            if (rp.StartsWith(startWith) && rp.Length >= len)
                            {
                                lotNo = rp.Substring(index, len);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var t in scanResults)
                    {
                        if (!string.IsNullOrEmpty(eachMatchedDetail.LotNokey) &&
                            string.IsNullOrEmpty(lotNo) &&
                            ReplaceSpecialCharacter(t.Barcode).StartsWith(eachMatchedDetail.LotNokey))
                        {
                            lotNo =
                                ReplaceSpecialCharacter(t.Barcode)
                                    .Substring(eachMatchedDetail.LotNokey.Length,
                                        ReplaceSpecialCharacter(t.Barcode).Length -
                                        eachMatchedDetail.LotNokey.Length)
                                    .Trim();
                            break;
                        }
                    }
                }

                if (eachMatchedDetail.DcNokey.Contains(','))
                {
                    if (eachMatchedDetail.DcNokey == "无")
                    {
                        dcNo = DateTime.Now.ToString("yyyyMMdd"); //"无";
                    }
                    else
                    {
                        var sp = eachMatchedDetail.DcNokey.Split(',');
                        var startWith = sp[0];
                        var index = int.Parse(sp[1]);
                        var len = int.Parse(sp[2]);
                        foreach (var t in scanResults)
                        {
                            var rp = ReplaceSpecialCharacter(t.Barcode);
                            if (rp.StartsWith(startWith) && rp.Length >= len)
                            {
                                dcNo = rp.Substring(index, len);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    foreach (var t in scanResults)
                    {
                        if (!string.IsNullOrEmpty(eachMatchedDetail.DcNokey) && string.IsNullOrEmpty(dcNo))
                        {
                            if (eachMatchedDetail.DcNokey == "无")
                            {
                                dcNo = DateTime.Now.ToString("yyyyMMdd"); //"无";
                                break;
                            }

                            if (ReplaceSpecialCharacter(t.Barcode).StartsWith(eachMatchedDetail.DcNokey))
                            {
                                dcNo = ReplaceSpecialCharacter(t.Barcode).Substring(eachMatchedDetail.DcNokey.Length, ReplaceSpecialCharacter(t.Barcode).Length - eachMatchedDetail.DcNokey.Length).Trim();
                                break;
                            }
                        }
                    }
                }
            }
            else
            {
                errorMsg = "二维码解析失败，无法定位到当前扫描得到的规则和档位的数据";
            }
        }

        public struct TransDate
        {
            public string Id;
            public string DateNo;
            public string LotNo;
            public string Year;
            public string Month;
            public string Date;
            public string Ymd;
            public string Week;
            public string Yymmdd;
        }

        /// <summary>
        /// 入库信息
        /// </summary>
        public class StockInfo
        {
            //public int MatchedId = -1;
            //public string MaterialBarcode;
            //public string ScanCodes;
            //public string SupplyNo;

            public string PartNo = string.Empty;
            public string LotNo = string.Empty;
            public string DcNo = string.Empty;
            public string QtyNo = string.Empty;
            public string MaterialNo = string.Empty;
            public string SupplyLedGroup = string.Empty;
            public string SupplyLedNo = string.Empty;
            public string ModelName = string.Empty;

            public string Qualevel = string.Empty;
            public string Issporadic = string.Empty;
            public string Earmarks = string.Empty;
        }
    }
}
