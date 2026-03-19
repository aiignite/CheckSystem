using CheckSystem.VisionDetection.Vision;
using StateMachine;
using Sunny.UI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CheckSystem.VisionDetection
{
    public partial class FrmBarcodeScan : UIForm
    {
        private BackgroundWorker DeviceBackgroundWorker { get; set; }

        private bool _isScanEnd;

        public FrmBarcodeScan(string barcode)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Closing += FrmBarcodeScan_Closing;

            txtBarcodeLogs.AppendText(barcode + "\r\n");

            barcodeDgv.Style = UIStyle.Gray;
            barcodeDgv.ReadOnly = true;
            barcodeDgv.RowHeadersVisible = false;
            barcodeDgv.AllowUserToAddRows = false;
            barcodeDgv.AllowUserToResizeRows = false;
            barcodeDgv.MultiSelect = true;
            barcodeDgv.RowHeadersVisible = false;
            barcodeDgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            barcodeDgv.ClearAll();
            barcodeDgv.AddColumn("名称", "名称");
            barcodeDgv.AddColumn("标准", "标准");
            barcodeDgv.AddColumn("结果", "结果");

            if (VisionCommon.VisionConfig.BarcodeInfo != null && VisionCommon.VisionConfig.BarcodeInfo.Any())
            {
                foreach (var code in VisionCommon.VisionConfig.BarcodeInfo)
                {
                    var name = code.Name;
                    var standardStrs = new List<string>();

                    for (var i = 0; i < int.Parse(code.Length); i++)
                        standardStrs.Add("?");

                    var keyValue = VisionCommon.IsLeft ? code.KeyWord.Value.L : code.KeyWord.Value.R;

                    var keyIndex = int.Parse(code.KeyWord.Index);
                    foreach (var t1 in keyValue)
                    {
                        standardStrs[keyIndex] = t1.ToString();
                        keyIndex++;
                    }

                    if (VisionCommon.SelectBarcodes.ContainsKey(name))
                    {
                        var v = VisionCommon.SelectBarcodes[name];

                        var g = code.Groups[v];

                        foreach (var gg in g)
                        {
                            var ggSp = gg.Split("，");
                            var ggIndex = int.Parse(ggSp[0]);
                            var ggValue = ggSp[1];

                            foreach (var t1 in ggValue)
                            {
                                standardStrs[ggIndex] = t1.ToString();
                                ggIndex++;
                            }
                        }
                    }

                    var standard = standardStrs.Aggregate(string.Empty, (current, ss) => current + ss);
                    barcodeDgv.AddRow(name, standard, "");
                }

                //foreach (var t in VisionCommon.SelectBrocdes.Keys)
                //{
                //    var v = VisionCommon.SelectBrocdes[t];

                //    var code = VisionCommon.VisionConfig.BarcodeInfo.ToList().Find(f => f.Name == t);
                //    if (code != null)
                //    {
                //        var name = code.Name;
                //        var standardStrs = new List<string>();

                //        for (var i = 0; i < int.Parse(code.Length); i++)
                //            standardStrs.Add("?");

                //        var keyValue = VisionCommon.IsLeft ? code.KeyWord.Value.L : code.KeyWord.Value.R;

                //        var keyIndex = int.Parse(code.KeyWord.Index);
                //        foreach (var t1 in keyValue)
                //        {
                //            standardStrs[keyIndex] = t1.ToString();
                //            keyIndex++;
                //        }

                //        var g = code.Groups[v];

                //        foreach (var gg in g)
                //        {
                //            var ggSp = gg.Split("，");
                //            var ggIndex = int.Parse(ggSp[0]);
                //            var ggValue = ggSp[1];

                //            foreach (var t1 in ggValue)
                //            {
                //                standardStrs[ggIndex] = t1.ToString();
                //                ggIndex++;
                //            }
                //        }

                //        var standard = standardStrs.Aggregate(string.Empty, (current, ss) => current + ss);
                //        barcodeDgv.AddRow(name, standard, "");
                //    }
                //}

                var row = barcodeDgv.Rows[0];

                if (row.Cells[2].Value == null || string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                {
                    var expecedStr = row.Cells[1].Value.ToString();

                    string analysisStr;
                    if (ConditionalCodeLine.GetStr(expecedStr, barcode, out analysisStr))
                    {
                        row.Cells[2].Value = analysisStr;
                        row.DefaultCellStyle.BackColor = IsRepeat(analysisStr) ? Color.Red : Color.Green;
                    }
                }
                //barcodeDgv.AutoResizeColumns();
            }
            else
            {
                _isScanEnd = true;
            }

            DeviceBackgroundWorker = new BackgroundWorker();
            DeviceBackgroundWorker.DoWork += DeviceBackgroundWorker_DoWork;
            DeviceBackgroundWorker.RunWorkerCompleted += DeviceBackgroundWorker_RunWorkerCompleted;
            DeviceBackgroundWorker.ProgressChanged += DeviceBackgroundWorker_ProgressChanged;
            DeviceBackgroundWorker.WorkerReportsProgress = true;
            DeviceBackgroundWorker.WorkerSupportsCancellation = true;

            DeviceBackgroundWorker.RunWorkerAsync();
        }

        private void FrmBarcodeScan_Closing(object sender, CancelEventArgs e)
        {
            if (VisionCommon.BarcodeScanReader != null)
            {
                VisionCommon.BarcodeScanReader.StopReadBarcode();
            }

            if (DeviceBackgroundWorker != null)
            {
                DeviceBackgroundWorker.CancelAsync();
            }
        }

        private void DeviceBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (var i = 0; i < barcodeDgv.RowCount; i++)
            {
                var row = barcodeDgv.Rows[i];

                if (row.DefaultCellStyle.BackColor != Color.Green)
                {
                    break;
                }

                //if (row.Cells[2].Value != null && !string.IsNullOrEmpty(row.Cells[2].Value.ToString()))
                //    continue;
                //var expecedStr = row.Cells[1].Value.ToString();

                //string analysisStr;
                //if (!ConditionalCodeLine.GetStr(expecedStr, actualStr, out analysisStr))
                //    break;
                //row.Cells[2].Value = analysisStr;
                //row.DefaultCellStyle.BackColor = Color.Green;
                if (i == barcodeDgv.RowCount - 1)
                    _isScanEnd = true;
                break;
            }

            while (!DeviceBackgroundWorker.CancellationPending)
            {
                if (_isScanEnd)
                {
                    DeviceBackgroundWorker.CancelAsync();
                    DialogResult = DialogResult.OK;
                    break;
                }

                if (VisionCommon.BarcodeScanReader != null)
                {
                    VisionCommon.BarcodeScanReader.ReadBarcode(1);
                    if (!string.IsNullOrEmpty(VisionCommon.BarcodeScanReader.GetBarcodeStr))
                    {
                        var actualStr = VisionCommon.BarcodeScanReader.GetBarcodeStr;
                        VisionCommon.BarcodeScanReader.GetBarcodeStr = string.Empty;

                        txtBarcodeLogs.AppendText(actualStr + "\r\n");

                        for (var i = 0; i < barcodeDgv.RowCount; i++)
                        {
                            var row = barcodeDgv.Rows[i];

                            if (row.Cells[2].Value != null && !string.IsNullOrEmpty(row.Cells[2].Value.ToString()) && row.DefaultCellStyle.BackColor == Color.Green)
                                continue;
                            var expecedStr = row.Cells[1].Value.ToString();

                            string analysisStr;
                            if (!ConditionalCodeLine.GetStr(expecedStr, actualStr, out analysisStr))
                                break;
                            row.Cells[2].Value = analysisStr;

                            if (IsRepeat(analysisStr))
                            {
                                row.DefaultCellStyle.BackColor = Color.Red;
                            }
                            else
                            {
                                row.DefaultCellStyle.BackColor = Color.Green;
                                if (i == barcodeDgv.RowCount - 1)
                                    _isScanEnd = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void DeviceBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void DeviceBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private static bool IsRepeat(string barcode)
        {
            var strSql = new StringBuilder();
            strSql.Append("select productBarcode ");
            strSql.Append(" FROM manufactureCheckData ");
            strSql.Append(" where " + string.Format("productBarcode = '{0}' and checkResult = '0001'", barcode));

            var sqlString = strSql.ToString();

            using (var connection = new SqlConnection("server=.;database=IPMS;uid=sa;pwd=123456"))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException)
                {
                    return true;
                }

                return ds.Tables[0].Rows.Count > 0;
            }
        }
    }
}
