using CommonUtility;
using MiniExcelLibs;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static CommonUtility.SyProductionSaveCheckData;

namespace CheckSystem.HelperForms
{
    public partial class MyWndExcelCheckDataJsonSplit : UIForm
    {
        public MyWndExcelCheckDataJsonSplit()
        {
            InitializeComponent();
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            uiTextBox1.Text = string.Empty;

            var openFi = new OpenFileDialog
            {
                Site = null,
                Tag = null,
                AddExtension = false,
                CheckPathExists = false,
                DefaultExt = null,
                DereferenceLinks = false,
                FileName = null,
                Filter = null,
                FilterIndex = 0,
                InitialDirectory = null,
                RestoreDirectory = false,
                ShowHelp = false,
                SupportMultiDottedExtensions = false,
                Title = null,
                ValidateNames = false,
                AutoUpgradeEnabled = false,
                CheckFileExists = false,
                Multiselect = false,
                ReadOnlyChecked = false,
                ShowReadOnly = false
            };
            openFi.Filter = "文件(xlsx)|*.xlsx;";
            if (openFi.ShowDialog() == DialogResult.OK)
            {
                uiTextBox1.Text = openFi.FileName;
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(uiTextBox1.Text))
            {
                UIMessageTip.ShowError("请选择文件");
                return;
            }

            if (!File.Exists(uiTextBox1.Text))
            {
                UIMessageTip.ShowError("文件不存在");
                return;
            }


            try
            {
                var rows = MiniExcel.Query(uiTextBox1.Text, excelType: ExcelType.XLSX).ToList();
                var listCheckData = new List<MyData>();

                for (int i = 0; i < rows.Count; i++)
                {
                    var row = rows[i];
                    var name = row.A.ToString().Split(new string[] { "/", "S" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    var barcode = row.B.ToString();
                    var checkResult = row.C.ToString();
                    if (checkResult == "0001" || checkResult == "9001")
                        checkResult = "OK";
                    else
                        checkResult = "NG";
                    var data = row.D.ToString();
                    var checkDate = row.E.ToString();

                    if (string.IsNullOrEmpty(barcode))
                        continue;

                    var MyData = new MyData();

                    try
                    {
                        MyData.Barcode = barcode;
                        MyData.Result = checkResult;

                        var listDetail = new List<MyDetail>();

                        var checkDetail = JsonConvert.DeserializeObject<CheckDataDetail[]>(data);
                        foreach (var item in checkDetail)
                            listDetail.Add(new MyDetail { Data = item.Value, Name = item.ParaName.Split(new string[] { "/", "S" }, StringSplitOptions.RemoveEmptyEntries)[0] });

                        MyData.CheckData = listDetail.ToArray();

                        var createTime = checkDate;
                        MyData.CreateTime = createTime;
                        listCheckData.Add(MyData);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }

                var dic = new List<string>();

                var maxLen = double.MinValue;
                var maxIndex = -1;

                for (var i = 0; i < listCheckData.Count; i++)
                {
                    var item = listCheckData[i];

                    if (item.CheckData.Length > maxLen)
                    {
                        maxLen = item.CheckData.Length;
                        maxIndex = i;
                    }
                }

                if (maxIndex > -1)
                {
                    var max = listCheckData[maxIndex];

                    foreach (var item in max.CheckData)
                    {
                        dic.Add(item.Name);
                    }

                    var values = new List<Dictionary<string, object>>();
                    foreach (var item in listCheckData)
                    {
                        var value = new Dictionary<string, object>();
                        value.Add("barcode", item.Barcode);
                        value.Add("CreateTime", item.CreateTime);
                        value.Add("Result", item.Result);

                        foreach (var t in dic)
                            value.Add(t, string.Empty);

                        foreach (var detail in item.CheckData)
                            value[detail.Name] = detail.Data;

                        values.Add(value);
                    }

                    var fileInfo = new FileInfo(uiTextBox1.Text);
                    var folder = fileInfo.DirectoryName;
                    var fileName = fileInfo.NameWithoutExt();
                    var newFile = string.Format(@"{0}\{1}_{2}.xlsx", folder, fileName, HighPrecisionTimer.GetTimestamp());
                    MiniExcel.SaveAs(newFile, values, overwriteFile: true);
                    UIMessageTip.Show("导出文件：" + newFile);
                }
            }
            catch (Exception ex)
            {
                UIMessageBox.ShowError("导出失败：" + ex.Message);
            }
        }
    }

    public class MyData
    {
        public string Barcode { get; set; }
        public string CreateTime { get; set; }
        public MyDetail[] CheckData { get; set; }
        public string Result { get; set; }

        public MyData()
        {
            //CheckData = new List<MyDetail>();
        }
    }

    public class MyDetail
    {
        public string Name { get; set; }
        public string Data { get; set; }
    }
}
