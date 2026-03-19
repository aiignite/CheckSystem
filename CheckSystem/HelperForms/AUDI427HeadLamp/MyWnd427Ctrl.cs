using Controller;
using Sunny.UI;
using Sunny.UI.Win32;
using System;
using System.Windows.Forms;

namespace CheckSystem.HelperForms.AUDI427HeadLamp
{
    public partial class MyWnd427Ctrl : UIForm
    {
        private readonly Audi427CSuvFrontLamp _audi427CSuvFrontLamp = new Audi427CSuvFrontLamp("427前灯");
        private readonly ZlgUsbCanFd200U zlgUsbCanFd200U = new ZlgUsbCanFd200U("zlg");

        public MyWnd427Ctrl()
        {
            InitializeComponent();
            InitGgv();
            Load += MyWnd427Ctrl_Load;
        }

        private void InitGgv()
        {
            {
                dgvAdbPixel.Width = Width;
                dgvAdbPixel.Style = UIStyle.Gray;
                dgvAdbPixel.AllowUserToAddRows = false;
                dgvAdbPixel.AllowUserToResizeRows = true;
                dgvAdbPixel.AllowUserToDeleteRows = true;
                dgvAdbPixel.MultiSelect = true;
                dgvAdbPixel.RowHeadersVisible = true;
                dgvAdbPixel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvAdbPixel.AddColumn("ADB_L_Name", "ADB_L_Name", readOnly: true);
                dgvAdbPixel.AddColumn("ADB_L_PWM(0~63)", "ADB_L_PWM", readOnly: false);
                dgvAdbPixel.AddColumn("ADB_R_Name", "ADB_R_Name", readOnly: true);
                dgvAdbPixel.AddColumn("ADB_R_PWM(0~63)", "ADB_R_PWM", readOnly: false);
                dgvAdbPixel.AddColumn("MATRIX_R_Name", "MATRIX_R_Name", readOnly: true);
                dgvAdbPixel.AddColumn("MATRIX_R_PWM(0~63)", "MATRIX_R_PWM", readOnly: false);
                for (var i = 1; i < 13; i++)
                {
                    var name1 = string.Format("AdbCh{0}L", i);
                    var name2 = string.Format("AdbCh{0}R", i);
                    var name3 = string.Format("MatrixCh{0}R", i);

                    dgvAdbPixel.AddRow(new object[] { name1, 0, name2, 0, name3, 0 });
                }
            }

            {
                dgvSidePixel.Width = Width;
                dgvSidePixel.Style = UIStyle.Gray;
                dgvSidePixel.AllowUserToAddRows = false;
                dgvSidePixel.AllowUserToResizeRows = true;
                dgvSidePixel.AllowUserToDeleteRows = true;
                dgvSidePixel.MultiSelect = true;
                dgvSidePixel.RowHeadersVisible = true;
                dgvSidePixel.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                dgvSidePixel.AddColumn("Pixel_L_Name", "Pixel_L_Name", readOnly: true);
                dgvSidePixel.AddColumn("Pixel_L_PWM(0~100.8)", "Pixel_L_PWM", readOnly: false);
                dgvSidePixel.AddColumn("Pixel_R_Name", "Pixel_R_Name", readOnly: true);
                dgvSidePixel.AddColumn("Pixel_R_PWM(0~100.8)", "Pixel_R_PWM", readOnly: false);

                for (var i = 1; i < 19; i++)
                {
                    var name1 = string.Format("SideCh{0}L", i);
                    var name2 = string.Format("SideCh{0}R", i);

                    dgvSidePixel.AddRow(new object[] { name1, 0, name2, 0 });
                }
            }
        }

        private void MyWnd427Ctrl_Load(object sender, EventArgs e)
        {
            if (zlgUsbCanFd200U != null && zlgUsbCanFd200U.ZlgCanChannel0 != null)
            {
                _audi427CSuvFrontLamp.CanFd = zlgUsbCanFd200U.ZlgCanChannel0;
                swCan.ActiveChanged += SwCan_ActiveChanged;
                swDrlL.ActiveChanged += SwDrlL_ActiveChanged;
                swDrlR.ActiveChanged += SwDrlR_ActiveChanged;
                swPlL.ActiveChanged += SwPlL_ActiveChanged;
                swPlR.ActiveChanged += SwPlR_ActiveChanged;
                swLb.ActiveChanged += SwLb_ActiveChanged;
                swHbL.ActiveChanged += SwHbL_ActiveChanged;
                swHbR.ActiveChanged += SwHbR_ActiveChanged;
                swClL.ActiveChanged += SwClL_ActiveChanged;
                swTurnL.ActiveChanged += SwTurnL_ActiveChanged;
                swTurnR.ActiveChanged += SwTurnR_ActiveChanged;
                rbTurnLHolding.CheckedChanged += RbTurnLHolding_CheckedChanged;
                rbTurnLBlink.CheckedChanged += RbTurnLBlink_CheckedChanged;
                swTurnRunning.ActiveChanged += SwTurnRunning_ActiveChanged;

                swClL.ActiveChanged += SwClL_ActiveChanged;
                swClR.ActiveChanged += SwClR_ActiveChanged;

                swDlp.ActiveChanged += SwDlp_ActiveChanged;

                cmbAdbLine.SelectedIndex = 0;
                swAdbL.ActiveChanged += SwAdbL_ActiveChanged;
                swAdbR.ActiveChanged += SwAdbR_ActiveChanged;
                swMatrixR.ActiveChanged += SwMatrixR_ActiveChanged;
                cmbAdbLine.SelectedIndexChanged += CmbAdbLine_SelectedIndexChanged;
                dgvAdbPixel.CellValidating += DgvAdbPixel_CellValidating;

                swWelcome.ActiveChanged += SwWelcome_ActiveChanged;
                dgvSidePixel.CellValidating += DgvSidePixel_CellValidating;

                btnSidePixelAllOn.Click += BtnSidePixelAllOn_Click;
                btnSidePixelAllOff.Click += BtnSidePixelAllOff_Click;

                uiTabControl1.SelectedIndexChanged += UiTabControl1_SelectedIndexChanged;

                swCan.Active = true;
            }
            else
            {
                UIMessageBox.ShowError("ZLG打开失败！");
            }
        }

        private void SwDlp_ActiveChanged(object sender, EventArgs e)
        {
            if (swDlp.Active)
                _audi427CSuvFrontLamp.DlpOn();
            else
                _audi427CSuvFrontLamp.DlpOff();
        }

        private void SwPlR_ActiveChanged(object sender, EventArgs e)
        {
            if (swPlR.Active)
                _audi427CSuvFrontLamp.PlOnR();
            else
                _audi427CSuvFrontLamp.PlOffR();
        }

        private void SwPlL_ActiveChanged(object sender, EventArgs e)
        {
            if (swPlL.Active)
                _audi427CSuvFrontLamp.PlOnL();
            else
                _audi427CSuvFrontLamp.PlOffL();
        }

        private void UiTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (uiTabControl1.SelectedIndex == 0)
            {
                _audi427CSuvFrontLamp.NormalLampMode();

                swAdbL.Active = swAdbR.Active = swMatrixR.Active = false;
                for (var i = 0; i < dgvAdbPixel.Rows.Count; i++)
                {
                    dgvAdbPixel.Rows[i].Cells[1].Value = 0;
                    dgvAdbPixel.Rows[i].Cells[3].Value = 0;
                    dgvAdbPixel.Rows[i].Cells[5].Value = 0;
                }

                swWelcome.Active = false;
                for (int i = 0; i < dgvSidePixel.Rows.Count; i++)
                {
                    dgvSidePixel.Rows[i].Cells[1].Value = 0;
                    dgvSidePixel.Rows[i].Cells[3].Value = 0;
                }
                SetSidePixel();
            }
            else if (uiTabControl1.SelectedIndex == 1)
            {
                _audi427CSuvFrontLamp.AdbMode();

                swWelcome.Active = false;
                for (int i = 0; i < dgvSidePixel.Rows.Count; i++)
                {
                    dgvSidePixel.Rows[i].Cells[1].Value = 0;
                    dgvSidePixel.Rows[i].Cells[3].Value = 0;
                }
                SetSidePixel();
            }
            else if (uiTabControl1.SelectedIndex == 2)
            {
                _audi427CSuvFrontLamp.SidePixelMode();

                swAdbL.Active = swAdbR.Active = swMatrixR.Active = false;
                for (var i = 0; i < dgvAdbPixel.Rows.Count; i++)
                {
                    dgvAdbPixel.Rows[i].Cells[1].Value = 0;
                    dgvAdbPixel.Rows[i].Cells[3].Value = 0;
                    dgvAdbPixel.Rows[i].Cells[5].Value = 0;
                }
            }
        }

        private void SwTurnRunning_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnRunning.Active)
                _audi427CSuvFrontLamp.SwitchTurn400MsEnable();
            else
                _audi427CSuvFrontLamp.SwitchTurn400MsDisbale();
        }

        private void RbTurnLBlink_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTurnLBlink.Checked)
                _audi427CSuvFrontLamp.TurnRunningEnable();
        }

        private void RbTurnLHolding_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTurnLHolding.Checked)
                _audi427CSuvFrontLamp.TurnRunningDisable();
        }

        private void SwTurnR_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnR.Active)
                _audi427CSuvFrontLamp.RightTurnOn();
            else
                _audi427CSuvFrontLamp.RightTurnOff();
        }

        private void SwTurnL_ActiveChanged(object sender, EventArgs e)
        {
            if (swTurnL.Active)
                _audi427CSuvFrontLamp.LeftTurnOn();
            else
                _audi427CSuvFrontLamp.LeftTurnOff();
        }

        private void SwClL_ActiveChanged(object sender, EventArgs e)
        {
            if (swClL.Active)
                _audi427CSuvFrontLamp.LeftCL = 63;
            else
                _audi427CSuvFrontLamp.LeftCL = 0;
        }

        private void SwClR_ActiveChanged(object sender, EventArgs e)
        {
            if (swClR.Active)
                _audi427CSuvFrontLamp.RightCL = 63;
            else
                _audi427CSuvFrontLamp.RightCL = 0;
        }

        private void SwHbR_ActiveChanged(object sender, EventArgs e)
        {
            if (swHbR.Active)
                _audi427CSuvFrontLamp.HbOnR();
            else
                _audi427CSuvFrontLamp.HbOffR();
        }

        private void SwHbL_ActiveChanged(object sender, EventArgs e)
        {
            if (swHbL.Active)
                _audi427CSuvFrontLamp.HbOnL();
            else
                _audi427CSuvFrontLamp.HbOffL();
        }

        private void SwLb_ActiveChanged(object sender, EventArgs e)
        {
            if (swLb.Active)
                _audi427CSuvFrontLamp.LbOn();
            else
                _audi427CSuvFrontLamp.LbOff();
        }

        private void SwDrlR_ActiveChanged(object sender, EventArgs e)
        {
            if (swDrlR.Active)
                _audi427CSuvFrontLamp.DrlOnR();
            else
                _audi427CSuvFrontLamp.DrlOffR();
        }

        private void SwDrlL_ActiveChanged(object sender, EventArgs e)
        {
            if (swDrlL.Active)
                _audi427CSuvFrontLamp.DrlOnL();
            else
                _audi427CSuvFrontLamp.DrlOffL();
        }

        private void SwCan_ActiveChanged(object sender, EventArgs e)
        {
            if (swCan.Active)
                _audi427CSuvFrontLamp.StartCan();
            else
                _audi427CSuvFrontLamp.StopCan();
        }

        private void CmbAdbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            swAdbL.Active = swAdbR.Active = swMatrixR.Active = false;
            if (cmbAdbLine.SelectedIndex == 0)
            {
                _audi427CSuvFrontLamp.AllLeftAdbPixelOff();
                _audi427CSuvFrontLamp.AllRightAdbPixelOff();
                _audi427CSuvFrontLamp.AllRightMatrixPixelOff();

                try
                {
                    for (var i = 0; i < dgvAdbPixel.RowCount; i++)
                    {
                        for (var j = 0; j < dgvAdbPixel.ColumnCount; j = j + 2)
                        {
                            dgvAdbPixel.Rows[i].Cells[j + 1].Value = 0;

                        }
                    }
                }
                catch (Exception)
                {

                }

                SetAdbPixel();
                return;
            }
            _audi427CSuvFrontLamp.AdbLine(cmbAdbLine.SelectedIndex);

            for (var i = 0; i < dgvAdbPixel.RowCount; i++)
            {
                for (var j = 0; j < dgvAdbPixel.ColumnCount; j = j + 2)
                {
                    var name = dgvAdbPixel.Rows[i].Cells[j].Value.ToString();
                    var value = _audi427CSuvFrontLamp.GetType().GetField(name).GetValue(_audi427CSuvFrontLamp);
                    dgvAdbPixel.Rows[i].Cells[j + 1].Value = value;
                }
            }
        }

        private void SwAdbR_ActiveChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvAdbPixel.RowCount; i++)
                dgvAdbPixel.Rows[i].Cells[3].Value = swAdbR.Active ? 0x3F : 0;
            SetAdbPixel();
        }

        private void SwAdbL_ActiveChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvAdbPixel.RowCount; i++)
                dgvAdbPixel.Rows[i].Cells[1].Value = swAdbL.Active ? 0x3F : 0;
            SetAdbPixel();
        }

        private void SwMatrixR_ActiveChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvAdbPixel.RowCount; i++)
                dgvAdbPixel.Rows[i].Cells[5].Value = swMatrixR.Active ? 0x3F : 0;
            SetAdbPixel();
        }

        private void SetAdbPixel()
        {
            try
            {
                for (var i = 0; i < dgvAdbPixel.RowCount; i++)
                {
                    for (var j = 0; j < dgvAdbPixel.ColumnCount; j = j + 2)
                    {
                        var name = dgvAdbPixel.Rows[i].Cells[j].Value.ToString();
                        var value = byte.Parse(dgvAdbPixel.Rows[i].Cells[j + 1].Value.ToString());
                        _audi427CSuvFrontLamp.GetType().GetField(name).SetValue(_audi427CSuvFrontLamp, value);

                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void DgvAdbPixel_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var pointGgv = sender as UIDataGridView;

            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < pointGgv.RowCount)
                {
                    var cell = pointGgv[e.ColumnIndex, e.RowIndex];

                    if (cell == null)
                        return;

                    try
                    {
                        if (e.ColumnIndex == 1 || e.ColumnIndex == 3 || e.ColumnIndex == 5)
                        {
                            if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                            {
                                e.Cancel = true;
                                pointGgv.CurrentCell = cell;
                                pointGgv.Focus();
                                pointGgv.CancelEdit();
                            }
                            else
                            {
                                var newValue = byte.Parse(e.FormattedValue.ToString());
                                var name = pointGgv.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString();

                                if (newValue < 0 || newValue > 0x3F)
                                {
                                    e.Cancel = true;
                                    pointGgv.CurrentCell = cell;
                                    pointGgv.Focus();
                                    pointGgv.CancelEdit();
                                }
                                else
                                {
                                    cell.Value = newValue;
                                    pointGgv.CurrentCell = cell;
                                    pointGgv.Focus();
                                    _audi427CSuvFrontLamp.GetType().GetField(name).SetValue(_audi427CSuvFrontLamp, newValue);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        e.Cancel = true;
                        pointGgv.CurrentCell = cell;
                        pointGgv.Focus();
                        pointGgv.CancelEdit();
                    }
                }
            }
            catch (Exception)
            {
                e.Cancel = true;
                pointGgv.Focus();
                pointGgv.CancelEdit();
            }
        }

        private void SwWelcome_ActiveChanged(object sender, EventArgs e)
        {
            if (swWelcome.Active)
            {
                dgvSidePixel.Enabled = false;
                btnSidePixelAllOn.Enabled = false;
                btnSidePixelAllOff.Enabled = false;
                _audi427CSuvFrontLamp.ExecuteWelCome();
            }
            else
            {
                dgvSidePixel.Enabled = true;
                btnSidePixelAllOn.Enabled = true;
                btnSidePixelAllOff.Enabled = true;
                _audi427CSuvFrontLamp.AbortWelCome();
            }
        }

        private void DgvSidePixel_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            var pointGgv = sender as UIDataGridView;

            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < pointGgv.RowCount)
                {
                    var cell = pointGgv[e.ColumnIndex, e.RowIndex];

                    if (cell == null)
                        return;

                    if (swWelcome.Active)
                        return;

                    try
                    {
                        if (e.ColumnIndex == 1 || e.ColumnIndex == 3)
                        {
                            if (string.IsNullOrWhiteSpace(e.FormattedValue.ToString()))
                            {
                                e.Cancel = true;
                                pointGgv.CurrentCell = cell;
                                pointGgv.Focus();
                                pointGgv.CancelEdit();
                            }
                            else
                            {
                                var newValue = Math.Round(double.Parse(e.FormattedValue.ToString()), 2, MidpointRounding.AwayFromZero);
                                var name = pointGgv.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value.ToString();

                                if (newValue < 0 || newValue > 100.8)
                                {
                                    e.Cancel = true;
                                    pointGgv.CurrentCell = cell;
                                    pointGgv.Focus();
                                    pointGgv.CancelEdit();
                                }
                                else
                                {
                                    cell.Value = newValue;
                                    pointGgv.CurrentCell = cell;
                                    pointGgv.Focus();
                                    _audi427CSuvFrontLamp.GetType().GetField(name).SetValue(_audi427CSuvFrontLamp, newValue);
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                        e.Cancel = true;
                        pointGgv.CurrentCell = cell;
                        pointGgv.Focus();
                        pointGgv.CancelEdit();
                    }
                }
            }
            catch (Exception)
            {
                e.Cancel = true;
                pointGgv.Focus();
                pointGgv.CancelEdit();
            }
        }

        private void SetSidePixel()
        {
            try
            {
                for (var i = 0; i < dgvSidePixel.RowCount; i++)
                {
                    for (var j = 0; j < dgvSidePixel.ColumnCount; j = j + 2)
                    {
                        var name = dgvSidePixel.Rows[i].Cells[j].Value.ToString();
                        var value = double.Parse(dgvSidePixel.Rows[i].Cells[j + 1].Value.ToString());
                        _audi427CSuvFrontLamp.GetType().GetField(name).SetValue(_audi427CSuvFrontLamp, value);
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void BtnSidePixelAllOff_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < dgvSidePixel.RowCount; i++)
                for (var j = 0; j < dgvSidePixel.ColumnCount; j = j + 2)
                    dgvSidePixel.Rows[i].Cells[j + 1].Value = 0d;
            SetSidePixel();
        }

        private void BtnSidePixelAllOn_Click(object sender, EventArgs e)
        {
            for (var i = 0; i < dgvSidePixel.RowCount; i++)
                for (var j = 0; j < dgvSidePixel.ColumnCount; j = j + 2)
                    dgvSidePixel.Rows[i].Cells[j + 1].Value = 100.8d;
            SetSidePixel();
        }
    }
}
