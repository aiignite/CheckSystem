using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheckSystem.CAN;
using CheckSystem.LIN;
using Controller;
using HZH_Controls.Controls;
using HZH_Controls.Controls.Btn;
using HZH_Controls.Forms;
using Sunny.UI;

namespace CheckSystem.SyController
{
    public partial class ControllerWith56PinDataViewForm : UIForm
    {
        private Form CurrentOpenedForm { get; set; }
        private object ThisController { get; set; }
        private Thread RefreshFieldValueThread { get; set; }
        private readonly object _dataGridviewLocker = new object();

        public ControllerWith56PinDataViewForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            WindowState = FormWindowState.Maximized;
            Load += CanDataViewForm_Load;
            Closed += ControllerWith56PinDataViewForm_Closed;
            InitDataGridView();
        }

        private void ControllerWith56PinDataViewForm_Closed(object sender, EventArgs e)
        {
            if (ThisController != null)
                ((ControllerBase)ThisController).Dispose();
        }

        private void CanDataViewForm_Load(object sender, EventArgs e)
        {
            btnTablePanel.BackColor = Color.DarkGoldenrod;
            OpenCanDevice();
        }

        private void InitDataGridView()
        {
            //dgvInputFields.label.Height = 30;
            //dgvInputFields.label.Text = @"字段列表-R";
            dgvInputFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段名称" });
            dgvInputFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段备注" });
            dgvInputFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段类型" });
            dgvInputFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "当前值" });
            dgvInputFields.ReadOnly = true;
            dgvInputFields.RowHeadersVisible = false;
            dgvInputFields.AllowUserToAddRows = false;
            dgvInputFields.AllowUserToDeleteRows = false;
            dgvInputFields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvInputFields.AllowUserToResizeColumns = true;
            dgvInputFields.AllowUserToResizeRows = false;
            for (var i = 0; i < dgvInputFields.Columns.Count; i++)
                dgvInputFields.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;

            //dgvOutpuFields.label.Height = 30;
            //dgvOutpuFields.label.Text = @"字段列表-R/W";
            dgvOutpuFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段名称" });
            dgvOutpuFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段备注" });
            dgvOutpuFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "字段类型" });
            dgvOutpuFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "当前值" });
            dgvOutpuFields.Columns.Add(new DataGridViewTextBoxColumn { Name = "修改值" });
            dgvOutpuFields.Columns.Add(new DataGridViewButtonColumn { Name = "设置按钮", DefaultCellStyle = { NullValue = "设置" } });
            dgvOutpuFields.RowHeadersVisible = false;
            dgvOutpuFields.AllowUserToAddRows = false;
            dgvOutpuFields.AllowUserToDeleteRows = false;
            dgvOutpuFields.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvOutpuFields.AllowUserToResizeColumns = true;
            dgvOutpuFields.AllowUserToResizeRows = false;
            for (var i = 0; i < dgvOutpuFields.Columns.Count; i++)
            {
                dgvOutpuFields.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dgvOutpuFields.Columns[i].ReadOnly = true;
            }
            dgvOutpuFields.Columns[4].ReadOnly = false;

            //dgvMethods.label.Height = 30;
            //dgvMethods.label.Text = @"方法列表";
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "方法名称" });
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "方法备注" });
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "是否需要填入参数" });
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "参数" });
            //dgvMethods.dataGridView.Columns.Add(new DataGridViewButtonColumn { Name = "执行按钮", DefaultCellStyle = { NullValue = "执行" } });
            //dgvMethods.dataGridView.RowHeadersVisible = false;
            //dgvMethods.dataGridView.AllowUserToAddRows = false;
            //dgvMethods.dataGridView.AllowUserToDeleteRows = false;
            //dgvMethods.dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dgvMethods.dataGridView.AllowUserToResizeColumns = true;
            //dgvMethods.dataGridView.AllowUserToResizeRows = false;
            //for (var i = 0; i < dgvMethods.dataGridView.Columns.Count; i++)
            //{
            //    dgvMethods.dataGridView.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            //    dgvMethods.dataGridView.Columns[i].ReadOnly = true;
            //}
            //dgvMethods.dataGridView.Columns[3].ReadOnly = false;
        }

        private void OpenCanDevice()
        {
            if (ThisController == null)
            {
                using (var mgrForm = new SyControllerDeviceMgrForm(typeof(SyControllerWith56Pin).Name))
                {
                    if (mgrForm.ShowDialog() == DialogResult.OK &&
                        mgrForm.InitController != null &&
                        ((SyControllerWith56Pin)mgrForm.InitController).IsConnected)
                    {
                        MessageBox.Show(@"打开设备成功！");
                        ThisController = mgrForm.InitController;
                        btnTablePanel.BackColor = Color.Green;
                        InitProduct();
                    }
                    else
                        MessageBox.Show(@"打开设备失败！");
                }
            }
            else
            {
                MessageBox.Show(@"设备已经打开！");
            }
        }

        private void InitProduct()
        {
            const string controllerName = "SyControllerWith56Pin";

            var asmb = Assembly.LoadFrom("Controller.dll");
            var controllerType = asmb.GetType("Controller." + controllerName);

            var controller = Activator.CreateInstance(controllerType, controllerName);

            dgvInputFields.Rows.Clear();
            dgvOutpuFields.Rows.Clear();
            //dgvMethods.dataGridView.Rows.Clear();
            //listMethods.Items.Clear();

            #region 字段/Fields

            var fi = controller.GetType().GetFields();

            foreach (var fieldInfo in fi)
            {
                if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                    continue;

                var des =
                    ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                        .Description;

                var sp = des.Split(',');

                var name = fieldInfo.Name;
                var type = fieldInfo.FieldType.Name;
                var inputOutputType = sp[0];
                var fieldNote = sp[1];

                if (inputOutputType == "R")
                {
                    var row = dgvInputFields.Rows[dgvInputFields.Rows.Add()];

                    row.Cells[0].Value = name;
                    row.Cells[1].Value = fieldNote;
                    row.Cells[2].Value = type;
                    row.Cells[3].Value = string.Empty;
                }
                else if (inputOutputType == "R/W")
                {
                    var row = dgvOutpuFields.Rows[dgvOutpuFields.Rows.Add()];

                    row.Cells[0].Value = name;
                    row.Cells[1].Value = fieldNote;
                    row.Cells[2].Value = type;
                    row.Cells[3].Value = "null";
                }
            }

            #endregion

            #region 方法/Methods

            var me = controller.GetType().GetMethods();

            for (var i = 0; i < me.Length; i++)
            {
                var method = me[i];

                if (Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute)) == null)
                    continue;

                var methodNote =
                   ((DescriptionAttribute)Attribute.GetCustomAttribute(method, typeof(DescriptionAttribute)))
                       .Description;

                var name = method.Name;
                var isNeedPara = method.GetParameters().Any();

                //var row = dgvMethods.dataGridView.Rows[dgvMethods.dataGridView.Rows.Add()];

                //var paras = method.GetParameters().Select(t => t.ParameterType.Name).Cast<object>().ToList();

                //row.Cells[0].Value = name;
                //row.Cells[1].Value = methodNote;
                //row.Cells[2].Value = isNeedPara ? string.Join(",", paras) : "否";
                //row.Cells[3].Value = isNeedPara ? string.Empty : "不需要填";

                //listMethods.Items.Add(name);
                var newBtn = new UCBtnExt
                {
                    BtnText = methodNote,
                    Name = name,
                    ConerRadius = 34,
                    Cursor = Cursors.Hand,
                    EnabledMouseEffect = true,
                    BtnFont = new Font("微软雅黑", 12f, FontStyle.Regular, GraphicsUnit.Point, 134),
                    IsRadius = true,
                    FillColor = i % 2 == 0 ? Color.CadetBlue : Color.DarkCyan,
                    RectColor = Color.Gray,
                    TipsText = name,
                    Margin = new Padding(3)
                };
                newBtn.BtnClick += newBtn_Click;
                listMethods.Controls.Add(newBtn);
            }
            #endregion

            dgvOutpuFields.CellContentClick += dgvOutpuFields_CellContentClick;
            //dgvMethods.dataGridView.CellContentClick += dgvMethods_CellContentClick; ;


            if (RefreshFieldValueThread != null)
            {
                RefreshFieldValueThread.Abort();
                RefreshFieldValueThread.Join();
            }

            RefreshFieldValueThread =
                new Thread(RefreshFieldValue) { IsBackground = true };
            RefreshFieldValueThread.Start();
        }

        private void RefreshFieldValue()
        {
            while (RefreshFieldValueThread.IsAlive)
            {
                Thread.Sleep(50);

                if (!RefreshFieldValueThread.IsAlive)
                    break;

                try
                {
                    lock (_dataGridviewLocker)
                    {
                        var controller = ThisController;
                        if (controller == null)
                            continue;

                        #region 字段/Fields

                        var fi = controller.GetType().GetFields();

                        foreach (var fieldInfo in fi)
                        {
                            if (Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) == null)
                                continue;

                            var des =
                                ((DescriptionAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)))
                                    .Description;

                            var sp = des.Split(',');

                            var name = fieldInfo.Name;
                            var type = fieldInfo.FieldType.ToString();
                            var inputOutputType = sp[0];
                            var fieldNote = sp[1];

                            if (inputOutputType == "R")
                            {
                                for (var i = 0; i < dgvInputFields.RowCount; i++)
                                {
                                    var row = dgvInputFields.Rows[i];

                                    if (row.Cells[0].Value.ToString() != fieldInfo.Name)
                                        continue;

                                    row.Cells[3].Value = fieldInfo.GetValue(controller) == null
                                        ? string.Empty
                                        : fieldInfo.FieldType == typeof(bool)
                                            ? fieldInfo.GetValue(controller).ToString() == bool.TrueString ? "1" : "0"
                                            : fieldInfo.GetValue(controller).ToString();

                                    break;
                                }
                            }
                            else if (inputOutputType == "R/W")
                            {
                                for (var i = 0; i < dgvOutpuFields.RowCount; i++)
                                {
                                    var row = dgvOutpuFields.Rows[i];

                                    if (row.Cells[0].Value.ToString() != fieldInfo.Name)
                                        continue;

                                    row.Cells[3].Value = fieldInfo.GetValue(controller) == null
                                        ? string.Empty
                                        : fieldInfo.FieldType == typeof(bool)
                                            ? fieldInfo.GetValue(controller).ToString() == bool.TrueString ? "1" : "0"
                                            : fieldInfo.GetValue(controller).ToString();

                                    break;
                                }
                            }
                        }

                        #endregion
                    }
                }
                catch (Exception)
                {
                    // ignored
                }
            }
        }

        private void dgvOutpuFields_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            lock (_dataGridviewLocker)
            {
                var dgv = sender as DataGridView;
                if (dgv == null)
                    return;

                if (dgv.Columns[e.ColumnIndex].Name != "设置按钮")
                    return;

                try
                {
                    if (dgv.Rows[e.RowIndex].Cells[4].Value == null)
                        return;

                    var row = dgv.Rows[e.RowIndex];
                    var toSetValue = row.Cells[4].Value.ToString();
                    if (string.IsNullOrEmpty(toSetValue))
                        return;

                    var type = row.Cells[2].Value.ToString();

                    if (type == typeof(bool).Name)
                    {
                        if (toSetValue == 1.ToString())
                            toSetValue = bool.TrueString;
                        else if (toSetValue == 0.ToString())
                            toSetValue = bool.FalseString;
                    }

                    var controller = ThisController;

                    if (controller == null)
                        return;
                    object value = null;

                    var fieldName = row.Cells[0].Value.ToString();
                    var fieldType = controller.GetType().GetField(fieldName).FieldType;

                    if (fieldType == typeof(bool))
                        value = string.Equals(toSetValue, bool.TrueString, StringComparison.CurrentCultureIgnoreCase);
                    else if (fieldType == typeof(ushort))
                        value = Convert.ToUInt16(toSetValue);
                    else if (fieldType == typeof(float))
                        value = Convert.ToSingle(toSetValue);
                    else if (fieldType == typeof(double))
                        value = Convert.ToDouble(toSetValue);
                    else if (fieldType == typeof(int))
                        value = Convert.ToInt32(toSetValue);
                    else if (fieldType == typeof(byte))
                        value = Convert.ToByte(toSetValue);
                    else if (fieldType == typeof(string))
                        value = toSetValue;

                    controller.GetType()
                        .GetField(fieldName)
                        .SetValue(controller, Convert.ChangeType(value, fieldType));
                }
                catch (Exception)
                {
                    // ignored
                }
                finally
                {
                    dgv.Rows[e.RowIndex].Cells[4].Value = string.Empty;
                }
            }
        }

        private async void newBtn_Click(object sender, EventArgs e)
        {
            Enabled = false;

            await Task.Run(() =>
            {
                var btn = sender as UCBtnExt;
                if (btn == null)
                    return;

                var methodName = btn.Name;

                var method = ThisController.GetType().GetMethod(methodName);
                if (method == null)
                    return;

                var isNeedPara = method.GetParameters().Any();
                if (!isNeedPara)
                {
                    method.Invoke(ThisController, null);
                }
                else
                {
                    var paras = method.GetParameters().Select(t => t.Name).ToList();
                    using (var frm = new FrmInputs(@"请输入参数", paras.ToArray()))
                    {
                        if (frm.ShowDialog(this) != DialogResult.OK) return;
                        var inputParas = frm.Values;

                        if (inputParas == null || inputParas.Length != paras.Count)
                            return;

                        try
                        {
                            var lstParaObjs = new List<object>();
                            var lstParas = inputParas.ToList();

                            for (var i = 0; i < lstParas.Count; i++)
                            {
                                var m = method.GetParameters()[i];
                                lstParaObjs.Add(Convert.ChangeType(lstParas[i], m.ParameterType));
                            }

                            method.Invoke(ThisController, lstParaObjs.ToArray());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            });

            Enabled = true;
        }

        private void ucBtnFillet1_BtnClick(object sender, EventArgs e)
        {
            OpenCanDevice();
        }

        private void ucBtnFlashConfig_BtnClick(object sender, EventArgs e)
        {
            if (ThisController == null)
            {
                MessageBox.Show(@"请先打开设备！");
            }
            else
            {
                const string formName = "56PIN-Flash-Config";

                if (CurrentOpenedForm == null)
                {
                    CurrentOpenedForm = new ControllerWith56PinFlashConfigForm((SyControllerWith56Pin)ThisController)
                    {
                        Name = formName
                    };
                    CurrentOpenedForm.Show();
                }
                else
                {
                    if (CurrentOpenedForm.Name == formName)
                    {
                        if (CurrentOpenedForm.IsDisposed)
                        {
                            CurrentOpenedForm = new ControllerWith56PinFlashConfigForm((SyControllerWith56Pin)ThisController)
                            {
                                Name = formName
                            };
                            CurrentOpenedForm.Show();
                        }
                        CurrentOpenedForm.WindowState = FormWindowState.Normal;
                        CurrentOpenedForm.Focus();
                    }
                    else
                    {
                        CurrentOpenedForm.Close();
                        CurrentOpenedForm.Dispose();

                        CurrentOpenedForm = new ControllerWith56PinFlashConfigForm((SyControllerWith56Pin)ThisController)
                        {
                            Name = formName
                        };
                        CurrentOpenedForm.Show();
                    }
                }
            }
        }

        private void ucBtnucBtnCan1Test_BtnClick(object sender, EventArgs e)
        {
            if (ThisController == null)
            {
                MessageBox.Show(@"请先打开设备！");
            }
            else
            {
                const string formName = "56PIN-CAN1-TEST";

                if (CurrentOpenedForm == null)
                {
                    CurrentOpenedForm = new CanDataViewForm(((SyControllerWith56Pin)ThisController).GatwayCan1, formName)
                    {
                        Name = formName
                    };
                    CurrentOpenedForm.Show();
                }
                else
                {
                    if (CurrentOpenedForm.Name == formName)
                    {
                        if (CurrentOpenedForm.IsDisposed)
                        {
                            CurrentOpenedForm = new CanDataViewForm(((SyControllerWith56Pin)ThisController).GatwayCan1, formName)
                            {
                                Name = formName
                            };
                            CurrentOpenedForm.Show();
                        }
                        CurrentOpenedForm.WindowState = FormWindowState.Normal;
                        CurrentOpenedForm.Focus();
                    }
                    else
                    {
                        CurrentOpenedForm.Close();
                        CurrentOpenedForm.Dispose();

                        CurrentOpenedForm = new CanDataViewForm(((SyControllerWith56Pin)ThisController).GatwayCan1, formName)
                        {
                            Name = formName
                        };
                        CurrentOpenedForm.Show();
                    }
                }
            }
        }

        private void ucBtnucBtnCan2Test_BtnClick(object sender, EventArgs e)
        {
            if (ThisController == null)
            {
                MessageBox.Show(@"请先打开设备！");
            }
            else
            {
                const string formName = "56PIN-CAN2-TEST";

                if (CurrentOpenedForm == null)
                {
                    CurrentOpenedForm = new CanDataViewForm(((SyControllerWith56Pin)ThisController).GatwayCan2, formName)
                    {
                        Name = formName
                    };
                    CurrentOpenedForm.Show();
                }
                else
                {
                    if (CurrentOpenedForm.Name == formName)
                    {
                        if (CurrentOpenedForm.IsDisposed)
                        {
                            CurrentOpenedForm = new CanDataViewForm(((SyControllerWith56Pin)ThisController).GatwayCan2, formName)
                            {
                                Name = formName
                            };
                            CurrentOpenedForm.Show();
                        }
                        CurrentOpenedForm.WindowState = FormWindowState.Normal;
                        CurrentOpenedForm.Focus();
                    }
                    else
                    {
                        CurrentOpenedForm.Close();
                        CurrentOpenedForm.Dispose();

                        CurrentOpenedForm = new CanDataViewForm(((SyControllerWith56Pin)ThisController).GatwayCan2, formName)
                        {
                            Name = formName
                        };
                        CurrentOpenedForm.Show();
                    }
                }
            }
        }

        private void ucBtnucBtnLinTest_BtnClick(object sender, EventArgs e)
        {
            if (ThisController == null)
            {
                MessageBox.Show(@"请先打开设备！");
            }
            else
            {
                const string formName = "56PIN-LIN-TEST";

                if (CurrentOpenedForm == null)
                {
                    CurrentOpenedForm = new LinDataViewForm(((SyControllerWith56Pin)ThisController).GatewayLin, formName)
                    {
                        Name = formName
                    };
                    CurrentOpenedForm.Show();
                }
                else
                {
                    if (CurrentOpenedForm.Name == formName)
                    {
                        if (CurrentOpenedForm.IsDisposed)
                        {
                            CurrentOpenedForm = new LinDataViewForm(((SyControllerWith56Pin)ThisController).GatewayLin, formName)
                            {
                                Name = formName
                            };
                            CurrentOpenedForm.Show();
                        }
                        CurrentOpenedForm.WindowState = FormWindowState.Normal;
                        CurrentOpenedForm.Focus();
                    }
                    else
                    {
                        CurrentOpenedForm.Close();
                        CurrentOpenedForm.Dispose();

                        CurrentOpenedForm = new LinDataViewForm(((SyControllerWith56Pin)ThisController).GatewayLin, formName)
                        {
                            Name = formName
                        };
                        CurrentOpenedForm.Show();
                    }
                }
            }
        }

        private void ucBtnProduct_BtnClick(object sender, EventArgs e)
        {
            //if (Can == null)
            //{
            //    MessageBox.Show(@"请先打开设备！");
            //}
            //else
            //{
            //    const string formName = "ProductSendForm";

            //    if (CurrentOpenedForm == null)
            //    {
            //        CurrentOpenedForm = new CanDataProductFrom(Can) { Name = formName };
            //        CurrentOpenedForm.Show();
            //    }
            //    else
            //    {
            //        if (CurrentOpenedForm.Name == formName)
            //        {
            //            if (CurrentOpenedForm.IsDisposed)
            //            {
            //                CurrentOpenedForm = new CanDataProductFrom(Can) { Name = formName };
            //                CurrentOpenedForm.Show();
            //            }
            //            CurrentOpenedForm.WindowState = FormWindowState.Normal;
            //            CurrentOpenedForm.Focus();
            //        }
            //        else
            //        {
            //            CurrentOpenedForm.Close();
            //            CurrentOpenedForm.Dispose();

            //            CurrentOpenedForm = new CanDataProductFrom(Can) { Name = formName };
            //            CurrentOpenedForm.Show();
            //        }
            //    }
            //}
        }
    }
}
