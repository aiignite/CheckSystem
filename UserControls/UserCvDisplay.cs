using CommonUtility;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace UserControls
{
    public partial class UserCvDisplay : UserControl
    {
        private readonly BindingList<ResultDataSource> _resultList = new BindingList<ResultDataSource>();

        public UserCvDisplay(string title)
        {
            InitializeComponent();
            InitDataGridView(uiDataGridView1);
            lblTitle.Text = title;
        }

        private void InitDataGridView(Sunny.UI.UIDataGridView dgv)
        {
            var dataGridView = dgv;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.AllowUserToResizeColumns = true;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //userDataGridGrayList.dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridView.Margin = new Padding(3, 4, 3, 4);
            dataGridView.RowTemplate.Height = 30;
            dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 7.5f, FontStyle.Regular);
            dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
            dataGridView.EnableHeadersVisualStyles = false;
            dataGridView.RowHeadersVisible = false;
            dataGridView.AllowUserToDeleteRows = false;
            dataGridView.AllowUserToAddRows = false;
            dataGridView.AllowUserToResizeRows = false;
            dataGridView.ReadOnly = true;

            dataGridView.Style = Sunny.UI.UIStyle.Gray;

            //获取控件的Type,设置双缓存
            var dgvType = dataGridView.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(dataGridView, true, null);

            {
                //dataGridView.VirtualMode = true;

                dataGridView.DataSource = _resultList;

                // 绑定虚拟模式事件                
                dataGridView.CellValueNeeded += DataGridView_CellValueNeeded;
                dataGridView.CellFormatting += DataGridView_CellFormatting;
            }

            //dataGridView.AddColumn("name", "name", readOnly: true);
            //dataGridView.AddColumn("range", "range", readOnly: true);
            //dataGridView.AddColumn("value", "value", readOnly: true);
            //dataGridView.AddColumn("result", "result", readOnly: true);
        }

        /// <summary>
        /// 虚拟模式：动态提供单元格数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            return;
            //if (e.RowIndex >= 0 && e.RowIndex < _dataSource.Count)
            //{
            //    try
            //    {
            //        var p = _dataSource[e.RowIndex].GetType().GetProperties()[e.ColumnIndex];
            //        var pVlaue = p.GetValue(_dataSource[e.RowIndex]);
            //        e.Value = pVlaue.ToString();

            //        //if (e.ColumnIndex == 0 && (e.Value == null || e.Value.ToString() != string.Format("DUT{0}", e.RowIndex + 1)))
            //        //    e.Value = string.Format("DUT{0}", e.RowIndex + 1);
            //        //else if (e.ColumnIndex > 0 && e.ColumnIndex <= 15)
            //        //{
            //        //    //e.Value = _dataSource[e.RowIndex].Values[e.ColumnIndex - 1].ToString("F2");
            //        //    //e.Value = _dataSource[e.RowIndex].GetType().GetProperty()

            //        //    var p = _dataSource[e.RowIndex].GetType().GetProperties()[e.ColumnIndex];
            //        //    var pVlaue = p.GetValue(_dataSource[e.RowIndex]);
            //        //    //e.Value = p.GetValue(_dataSource[e.RowIndex]);

            //        //    if (p.PropertyType == typeof(double))
            //        //    {
            //        //        e.Value = (pVlaue) is double.MinValue ? "NA" : pVlaue;
            //        //    }
            //        //    else if (p.PropertyType == typeof(string))
            //        //    {
            //        //        e.Value = string.IsNullOrEmpty(pVlaue as string) ? "NA" : pVlaue;
            //        //    }
            //        //}
            //    }
            //    catch (Exception)
            //    {

            //    }
            //}
        }

        /// <summary>
        /// 单元格样式：奇偶行背景色+数据范围警示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            return;
            var dgv = sender as DataGridView;

            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 3)
                {
                    // 检查数据是否超范围（仅对数据列生效）
                    if (e.ColumnIndex == 3 && dgv.ColumnCount > 0 && e.ColumnIndex < dgv.ColumnCount)
                    {
                        var isNg = true;
                        if (e.Value != null && !string.IsNullOrEmpty(e.Value.ToString()) && e.Value.ToString().ToLower() == "ok")
                        {
                            isNg = false;
                        }

                        if (isNg && e.CellStyle.BackColor != Color.Red)
                        {
                            e.CellStyle.BackColor = Color.Red;
                            e.CellStyle.ForeColor = Color.White;
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public void ReleaseResult()
        {
            if (cvDisplay1.Image != null)
            {
                cvDisplay1.Image?.Dispose();
                cvDisplay1.Image = null;
                _isHaveImg = false;
            }

            _resultList.Clear();
            _ngRowCount = 0;
            uiDataGridView1.Style = Sunny.UI.UIStyle.Gray;

        }

        private bool _isHaveImg;
        private int _ngRowCount;

        public void AppendImage(string baseString64)
        {
            if (_isHaveImg)
                return;
            if (string.IsNullOrEmpty(baseString64))
                return;

            cvDisplay1.Invoke(new Action(() =>
            {
                try
                {
                    var t1 = HighPrecisionTimer.GetTimestamp();

                    var bitmap = CommonUtility.HikSdk.MyCamera.Base64StringToBitmap(baseString64);
                    if (bitmap == null)
                        return;

                    var toShow = BitmapConverter.ToMat(bitmap);
                    //Cv2.Resize(toShow, toShow, new OpenCvSharp.Size(1024, 768));
                    cvDisplay1.Image = toShow.Clone();
                    toShow?.Dispose();
                    toShow = null;
                    cvDisplay1.Fit();
                    bitmap.Dispose();
                    bitmap = null;
                    _isHaveImg = true;

                    var t2 = HighPrecisionTimer.GetTimestamp();
                    var ts = HighPrecisionTimer.GetTimestampIntervalMs(t1, t2);
                    Console.WriteLine("cv display cost: {0}/ms", ts);                   
                }
                catch (Exception ex)
                {
                    if (cvDisplay1.Image != null)
                    {
                        cvDisplay1.Image.Dispose();
                        cvDisplay1.Image = null;
                        _isHaveImg = false;
                    }
                }
            }));
        }

        public void AppendRow(string name, string range, string checkValue, string checkResult)
        {
            var action = new Action(() =>
            {
                if (!_resultList.Any(f => f.Name.ToLower() == name.ToLower()))
                {
                    _resultList.Add(new ResultDataSource
                    {
                        Name = name,
                        Range = range,
                        CheckValue = checkValue,
                        CheckResult = checkResult.ToLower() == "false" || checkResult == "0" ? "NG" : "OK"
                    });

                    if (checkResult.ToLower() == "false" || checkResult == "0")
                    {
                        _ngRowCount++;
                    }
                }

                var dataGridView = uiDataGridView1;

                if (_ngRowCount > 0)
                {
                    if (dataGridView.Style != Sunny.UI.UIStyle.Red)
                        dataGridView.Style = Sunny.UI.UIStyle.Red;
                }
                else
                {
                    if (dataGridView.Style != Sunny.UI.UIStyle.LayuiGreen)
                        dataGridView.Style = Sunny.UI.UIStyle.LayuiGreen;
                }
            });

            action.Invoke();

            //if (InvokeRequired)
            //{
            //    Invoke(action);
            //}
            //else
            //{
            //    action.Invoke();
            //}


            //var dataGridView = uiDataGridView1;
            //checkResult = checkResult.ToLower();

            //dataGridView.Invoke(new Action(() =>
            //{
            //    var isHave = -1;
            //    for (var i = 0; i < dataGridView.RowCount; i++)
            //    {
            //        if (dataGridView.Rows[i].Cells[0].Value.ToString() != name)
            //            continue;

            //        isHave = i;
            //        break;
            //    }

            //    if (isHave > -1)
            //    {
            //        dataGridView.Rows[isHave].Cells[1].Value = range;
            //        dataGridView.Rows[isHave].Cells[2].Value = checkValue;
            //        dataGridView.Rows[isHave].Cells[3].Value =
            //            checkResult == "false" || checkResult == "0"
            //                ? "NG"
            //                : "OK";
            //        dataGridView.Rows[isHave].DefaultCellStyle.BackColor =
            //            checkResult == "false" || checkResult == "0"
            //                ? Color.Red
            //                : Color.White;
            //        _ngRowCount = checkResult == "false" || checkResult == "0" ? _ngRowCount + 1 : _ngRowCount;
            //    }
            //    else
            //    {
            //        var index = dataGridView.Rows.Add();
            //        dataGridView.Rows[index].Cells[0].Value = name;
            //        dataGridView.Rows[index].Cells[1].Value = range;
            //        dataGridView.Rows[index].Cells[2].Value = checkValue;
            //        dataGridView.Rows[index].Cells[3].Value =
            //            checkResult == "false" || checkResult == "0"
            //                ? "NG"
            //                : "OK";
            //        dataGridView.Rows[index].DefaultCellStyle.BackColor =
            //            checkResult == "false" || checkResult == "0"
            //                ? Color.Red
            //                : Color.White;
            //        _ngRowCount = checkResult == "false" || checkResult == "0" ? _ngRowCount + 1 : _ngRowCount;
            //    }

            //    dataGridView.FirstDisplayedScrollingRowIndex = dataGridView.RowCount - 1;
            //    if (_ngRowCount > 0)
            //    {
            //        if (dataGridView.Style != Sunny.UI.UIStyle.Red)
            //            dataGridView.Style = Sunny.UI.UIStyle.Red;
            //    }
            //    else
            //    {
            //        if (dataGridView.Style != Sunny.UI.UIStyle.LayuiGreen)
            //            dataGridView.Style = Sunny.UI.UIStyle.LayuiGreen;
            //    }
            //}));
        }

        private class ResultDataSource
        {
            public string Name { get; set; }
            public string Range { get; set; }
            public string CheckValue { get; set; }
            public string CheckResult { get; set; }
        }
    }
}
