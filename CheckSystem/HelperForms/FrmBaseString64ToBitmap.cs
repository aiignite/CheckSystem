using CommonUtility;
using Newtonsoft.Json;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms
{
    public partial class FrmBaseString64ToBitmap : UIForm
    {
        public FrmBaseString64ToBitmap()
        {
            InitializeComponent();
            uiComboBox1.SelectedIndexChanged += UiComboBox1_SelectedIndexChanged;
            uiDatetimePicker1.Value = uiDatetimePicker2.Value = DateTime.Now;
        }

        private void UiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            uiRichTextBox1.Text = string.Empty;

            txtResult.Text = string.Empty;
            txtBarcode.Text = string.Empty;
            txtCreateTime.Text = string.Empty;

            if (uiComboBox1.Items.Count > 0)
            {
                var index = uiComboBox1.SelectedIndex;

                if (index <= _checkDataDetails.Count)
                {
                    var value = _checkDataDetails[index];
                    txtBarcode.Text = value.Barcode;
                    txtCreateTime.Text = value.CrateTime;
                    txtResult.Text = value.Result;

                    var findBitmapValue = value.CheckDataDetails.FindAll(f => f.Type.ToLower() == "bitmap");

                    for (var i = 0; i < findBitmapValue.Count; i++)
                    {
                        var checkValue = findBitmapValue[i].Value;
                        PictureBox pictureBox = null;

                        try
                        {
                            if (i == 0)
                            {
                                pictureBox = pictureBox1;

                                if (pictureBox1.Image != null)
                                {
                                    pictureBox1.Image.Dispose();
                                    pictureBox1.Image = null;
                                }
                            }
                            else if (i == 1)
                            {
                                pictureBox = pictureBox2;
                            }

                            if (pictureBox != null)
                            {
                                if (pictureBox.Image != null)
                                {
                                    pictureBox.Image.Dispose();
                                    pictureBox.Image = null;
                                }

                                // 从Base64字符串转换回字节数组
                                var convertedBytes = Convert.FromBase64String(checkValue);

                                // 从字节数组转换回Bitmap
                                using (var ms = new MemoryStream(convertedBytes))
                                {
                                    var convertedBitmap = new Bitmap(ms);
                                    // 保存或使用转换后的Bitmap
                                    //convertedBitmap.Save(@"E:\Projects\万级像素\自动线终检\增加图像检测结果\asdsad.jpg", ImageFormat.Jpeg);
                                    pictureBox.Image = convertedBitmap;
                                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                }
                            }
                        }
                        catch (Exception exception)
                        {
                            if (pictureBox != null)
                                pictureBox1.Image = null;
                            ShowErrorTip("转换异常：" + exception.Message);
                            //Console.WriteLine(exception);
                            //throw;
                        }
                    }


                    //if (findBitmapValue != null)
                    //    uiRichTextBox1.Text = findBitmapValue.Value;

                    //try
                    //{
                    //    if (pictureBox1.Image != null)
                    //    {
                    //        pictureBox1.Image.Dispose();
                    //        pictureBox1.Image = null;
                    //    }

                    //    var checkValue = uiRichTextBox1.Text;
                    //    uiRichTextBox1.Clear();

                    //    // 从Base64字符串转换回字节数组
                    //    var convertedBytes = Convert.FromBase64String(checkValue);

                    //    // 从字节数组转换回Bitmap
                    //    using (var ms = new MemoryStream(convertedBytes))
                    //    {
                    //        var convertedBitmap = new Bitmap(ms);
                    //        // 保存或使用转换后的Bitmap
                    //        //convertedBitmap.Save(@"E:\Projects\万级像素\自动线终检\增加图像检测结果\asdsad.jpg", ImageFormat.Jpeg);
                    //        pictureBox1.Image = convertedBitmap;
                    //        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //    pictureBox1.Image = null;
                    //}
                }
            }
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                }

                var checkValue = uiRichTextBox1.Text;
                uiRichTextBox1.Clear();

                // 从Base64字符串转换回字节数组
                var convertedBytes = Convert.FromBase64String(checkValue);

                // 从字节数组转换回Bitmap
                using (var ms = new MemoryStream(convertedBytes))
                {
                    var convertedBitmap = new Bitmap(ms);
                    // 保存或使用转换后的Bitmap
                    //convertedBitmap.Save(@"E:\Projects\万级像素\自动线终检\增加图像检测结果\asdsad.jpg", ImageFormat.Jpeg);
                    pictureBox1.Image = convertedBitmap;
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
            catch (Exception)
            {
                pictureBox1.Image = null;
            }
        }

        private readonly List<CheckDataDetail> _checkDataDetails = new List<CheckDataDetail>();

        private async void btnRefreshData_Click(object sender, EventArgs e)
        {
            txtResult.Text = string.Empty;
            if (!uiRadioButton2.Checked)
                txtBarcode.Text = string.Empty;
            txtCreateTime.Text = string.Empty;

            _checkDataDetails.Clear();
            uiComboBox1.Items.Clear();
            uiPanel1.Enabled = false;

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
                pictureBox1.Image = null;
            }

            var showWhichResultIndex = 0;

            if (uiRadioButton1.Checked)
            {
                var option = new UIEditOption();
                var showWhichResult = new string[] { "所有", "合格", "不合格" };
                option.AddCombobox("showWhichResult", "显示结果", showWhichResult, 0);

                var uiOption = new UIEditForm(option);
                uiOption.Render();
                uiOption.ShowDialog();
                var uiResult = uiOption.IsOK;
                if (!uiResult)
                {
                    ShowInfoTip("查询取消");
                    return;
                }

                showWhichResultIndex = (int)uiOption["showWhichResult"];
            }

            await Task.Run(() =>
            {
                var startDate = uiDatetimePicker1.Value.ToString("yyyy-MM-dd 00:00:00.00");
                var endDate = uiDatetimePicker2.Value.ToString("yyyy-MM-dd 23:59:59.999");

                var sql = string.Empty;
                if (uiRadioButton1.Checked)
                {
                    var tpResult1 = "0001";
                    var tpResult2 = "0002";
                    if (showWhichResultIndex == 1)
                    {
                        tpResult1 = "0001";
                        tpResult2 = "0001";
                    }
                    else if (showWhichResultIndex == 2)
                    {
                        tpResult1 = "0002";
                        tpResult2 = "0002";
                    }

                    sql = string.Format(
                        "SELECT ItemId, MaterialBarcode,DeviceNo,CheckData,CreateTime,Result FROM SyProductionCheckData WHERE DeviceNo = 'IN4304000306' and (CreateTime between  '{0}' and '{1}') and (Result = '{2}' or Result = '{3}') order by CreateTime desc",
                        startDate, endDate, tpResult1, tpResult2);
                }
                else if (uiRadioButton2.Checked)
                    sql = string.Format(
                        "SELECT ItemId, MaterialBarcode,DeviceNo,CheckData,CreateTime,Result FROM SyProductionCheckData WHERE DeviceNo = 'IN4304000306' and MaterialBarcode = '{0}' order by CreateTime desc", txtBarcode.Text);

                if (!string.IsNullOrEmpty(sql))
                {
                    var ds = Query(sql, "192.168.0.138");

                    for (var i = 0; i < ds.Tables[0].DefaultView.Count; i++)
                    {
                        var itemId = ds.Tables[0].Rows[i]["ItemId"];
                        var barcode = ds.Tables[0].Rows[i]["MaterialBarcode"];
                        var checkData = ds.Tables[0].Rows[i]["CheckData"];
                        var result = ds.Tables[0].Rows[i]["Result"];

                        var checkDataDetail = new CheckDataDetail
                        {
                            ItemId = itemId == null ? Guid.NewGuid().ToString() : itemId.ToString(),
                            Barcode = barcode == null ? Guid.NewGuid().ToString() : barcode.ToString(),
                            Result = result == null ? string.Empty : result.ToString()
                        };

                        if (checkData != null)
                        {
                            try
                            {
                                checkDataDetail.CheckDataDetails = JsonConvert.DeserializeObject<List<SyProductionSaveCheckData.CheckDataDetail>>(checkData.ToString());
                            }
                            catch (Exception)
                            {
                                checkDataDetail.CheckDataDetails = new List<SyProductionSaveCheckData.CheckDataDetail>();
                            }
                        }

                        _checkDataDetails.Add(checkDataDetail);
                    }
                }
            });

            foreach (var t in _checkDataDetails)
                uiComboBox1.Items.Add(t.Barcode);
            if (uiComboBox1.Items.Count > 0)
                uiComboBox1.SelectedIndex = 0;

            uiPanel1.Enabled = true;
        }

        private static DataSet Query(string sqlString, string serverIp)
        {
            var sqlConnectiong = string.Format(@"server={0};database={1};uid={2};pwd={3}", serverIp, "PLMS",
                "ipms", "Scae2020#");

            using (var connection = new SqlConnection(sqlConnectiong))
            {
                var ds = new DataSet();
                try
                {
                    connection.Open();
                    var command = new SqlDataAdapter(sqlString, connection);
                    command.Fill(ds, "ds");
                }
                catch (SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        internal class CheckDataDetail
        {
            public string ItemId { get; set; }
            public List<SyProductionSaveCheckData.CheckDataDetail> CheckDataDetails { get; set; }
            public string Barcode { get; set; }
            public string CrateTime { get; set; }
            public string Result { get; set; }
        }

        private void uiRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (uiRadioButton1.Checked)
            {
                uiDatetimePicker1.Enabled = uiDatetimePicker2.Enabled = true;
                txtBarcode.ReadOnly = true;
            }
        }

        private void uiRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (uiRadioButton2.Checked)
            {
                uiDatetimePicker1.Enabled = uiDatetimePicker2.Enabled = false;
                txtBarcode.ReadOnly = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //if (pictureBox1.Image != null)
            //{
            //    var path = string.Format(@"E:\Projects\万级像素\自动线终检\图像测试\20250218\{0}_image1.bmp", HighPrecisionTimer.GetTimestamp());
            //    pictureBox1.Image.Save(path);
            //}
            //if (pictureBox2.Image != null)
            //{
            //    var path = string.Format(@"E:\Projects\万级像素\自动线终检\图像测试\20250218\{0}_image2.bmp", HighPrecisionTimer.GetTimestamp());
            //    pictureBox2.Image.Save(@"E:\桌面\新建文件夹\万级像素NG件-20250212\20250212_ng_image2.bmp");
            //}

            var option = new UIEditOption();
            var showWhichResult = new string[] { "图像1", "图像2" };
            option.AddCombobox("showWhichResult", "显示结果", showWhichResult, 0);

            var uiOption = new UIEditForm(option);
            uiOption.Render();
            uiOption.ShowDialog();
            var uiResult = uiOption.IsOK;
            if (!uiResult)
            {
                ShowInfoTip("取消");
                return;
            }

            var index = (int)uiOption["showWhichResult"];

            if (index == 0 && pictureBox1.Image == null)
            {
                ShowInfoTip(@"图像1不存在");
                return;
            }
            else if (index == 1 && pictureBox2.Image == null)
            {
                ShowInfoTip(@"图像2不存在");
                return;
            }

            // 选择路径
            string path;
            using (var folder = new FolderBrowserDialog())
            {
                var result = folder.ShowDialog();
                if (result != DialogResult.OK)
                    return;
                path = folder.SelectedPath + string.Format(@"\MatrixImage-{1}_{0}.bmp",
                    DateTime.Now.ToString(CultureInfo.InvariantCulture)
                    .Replace(":", string.Empty)
                    .Replace("/", string.Empty)
                    .Replace(" ", string.Empty),
                    DateTime.Now.ToString("yyyyMMdd"));

                if (index == 0)
                    pictureBox1.Image.Save(path);
                else if (index == 1)
                    pictureBox2.Image.Save(path);
            }
        }

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            for (var index = 0; index < _checkDataDetails.Count; index++)
            {
                var value = _checkDataDetails[index];
                txtBarcode.Text = value.Barcode;
                txtCreateTime.Text = value.CrateTime;
                txtResult.Text = value.Result;

                var findBitmapValue = value.CheckDataDetails.FindAll(f => f.Type.ToLower() == "bitmap");

                for (var i = 0; i < findBitmapValue.Count; i++)
                {
                    var checkValue = findBitmapValue[i].Value;
                    PictureBox pictureBox = null;

                    try
                    {
                        if (i == 1 && !string.IsNullOrEmpty(checkValue))
                        {
                            // 从Base64字符串转换回字节数组
                            var convertedBytes = Convert.FromBase64String(checkValue);

                            // 从字节数组转换回Bitmap
                            using (var ms = new MemoryStream(convertedBytes))
                            {
                                var convertedBitmap = new Bitmap(ms);
                                // 保存或使用转换后的Bitmap
                                //convertedBitmap.Save(@"E:\Projects\万级像素\自动线终检\增加图像检测结果\asdsad.jpg", ImageFormat.Jpeg);
                                var path = string.Format(@"E:\Projects\万级像素\自动线终检\图像测试\20250218\OK_Image2_From_20250201\{0}_image2.bmp", HighPrecisionTimer.GetTimestamp());
                                convertedBitmap.Save(path);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        if (pictureBox != null)
                            pictureBox1.Image = null;
                        ShowErrorTip("转换异常：" + exception.Message);
                        //Console.WriteLine(exception);
                        //throw;
                    }
                }
            }
        }
    }
}
