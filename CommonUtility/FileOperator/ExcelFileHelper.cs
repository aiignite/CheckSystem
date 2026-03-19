using MiniExcelLibs;
using System.Collections.Generic;
using System.Linq;

namespace CommonUtility.FileOperator
{
    public class ExcelFileHelper
    {
        public List<dynamic> ReadExcelRows(string filePath)
        {
            var rows = MiniExcel.Query(filePath).ToList();
            return rows;
        }
    }
}
