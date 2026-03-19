using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using CommonUtility;
using Controller;
using Go;
using StateMachine;
using Sunny.UI;

namespace CheckSystem.HelperForms.GeeleyCx1e
{
    public partial class FrmSingleDutPage : UIPage
    {
        private generator _action;
        private readonly Dictionary<string, string> _stateBinding = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _currBinding = new Dictionary<string, string>();

        private State State { get; set; }
        private Cx1EEmc Cx1EEmcDut { get; set; }
        private BackgroundWorker BackgroundWorker { get; set; }
        private bool _isRefresh;

        private float _x;//当前窗体的宽度
        private float _y;//当前窗体的高度

        public FrmSingleDutPage(State state)
        {
            InitializeComponent();
            State = state;
            Text = state.DeviceConfig.DeviceInfo.DeviceName;

            SetStyle(ControlStyles.DoubleBuffer |
                     ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint, true);
            UpdateStyles();

            Load += FrmEnvBox_Load;
            Closed += FrmEnvBox_Closed;

            swStartCan.ActiveChanged += swStartCan_ActiveChanged;

            cmbDrvrMirrCmd5.SelectedIndex = 0;
            cmbDrvrAdjCmd.SelectedIndex = 0;
            cmbFronHandleLight.SelectedIndex = 0;
            cmbHndlDoorLiDrvr.SelectedIndex = 0;
            cmbWindowLight.SelectedIndex = 0;
            cmbMotorCmd.SelectedIndex = 0;

            #region 2F帧事件
            txtDcMirrorX.ValueChanged += txtDcMirrorX_ValueChanged;
            txtDcMirrorY.ValueChanged += txtDcMirrorY_ValueChanged;
            txtDcWindow.ValueChanged += txtDcWindow_ValueChanged;
            txtDcChildrenLock.ValueChanged += txtDcChildrenLock_ValueChanged;
            txtDcElectricRelese.ValueChanged += txtDcElectricRelese_ValueChanged;
            txtDcLock2.ValueChanged += txtDcLock2_ValueChanged;
            txtDcHandle.ValueChanged += txtDcHandle_ValueChanged;
            txtDcPedal.ValueChanged += txtDcPedal_ValueChanged;
            txtDcDoor.ValueChanged += txtDcDoor_ValueChanged;

            cmbMotorCmd.SelectedIndexChanged += cmbMotorCmd_SelectedIndexChanged;
            #endregion

            #region 周期帧事件
            txtMirrTintgCmd.ValueChanged += txtMirrTintgCmd_ValueChanged;
            swMirrDefrstDrvrCmd.ActiveChanged += swMirrDefrstDrvrCmd_ActiveChanged;
            swActvnOfIndcrOut.ActiveChanged += swActvnOfIndcrOut_ActiveChanged;
            swActvnOfPudLi.ActiveChanged += swActvnOfPudLi_ActiveChanged;

            cmbHndlDoorLiDrvr.SelectedIndexChanged += cmbHndlDoorLiDrvr_SelectedIndexChanged;
            cmbWindowLight.SelectedIndexChanged += cmbWindowLight_SelectedIndexChanged;
            cmbDrvrMirrCmd5.SelectedIndexChanged += cmbDrvrMirrCmd5_SelectedIndexChanged;
            cmbDrvrAdjCmd.SelectedIndexChanged += cmbDrvrAdjCmd_SelectedIndexChanged;
            cmbFronHandleLight.SelectedIndexChanged += cmbFronHandleLight_SelectedIndexChanged;

            rbShortDropWinIdle.CheckedChanged += rbShortDropWin_CheckedChanged;
            rbShortDropWinClose.CheckedChanged += rbShortDropWin_CheckedChanged;
            rbShortDropWinOpen.CheckedChanged += rbShortDropWin_CheckedChanged;

            rbDoorDrvrHndlCmdIdle.CheckedChanged += rbDoorDrvrHndlCmd_CheckedChanged;
            rbDoorDrvrHndlCmdOpen.CheckedChanged += rbDoorDrvrHndlCmd_CheckedChanged;
            rbDoorDrvrHndlCmdClose.CheckedChanged += rbDoorDrvrHndlCmd_CheckedChanged;

            rbChdLockReLeCtrlHmiReqIdle.CheckedChanged += rbChdLockReLeCtrlHmiReq_CheckedChanged;
            rbChdLockReLeCtrlHmiReqLock.CheckedChanged += rbChdLockReLeCtrlHmiReq_CheckedChanged;
            rbChdLockReLeCtrlHmiReqUnlock.CheckedChanged += rbChdLockReLeCtrlHmiReq_CheckedChanged;

            rbDoorIdle.CheckedChanged += rbDoor_CheckedChanged;
            rbDoorOpen.CheckedChanged += rbDoor_CheckedChanged;
            rbDoorClose.CheckedChanged += rbDoor_CheckedChanged;

            rbPedalIdle.CheckedChanged += rbPedal_CheckedChanged;
            rbPedalOpen.CheckedChanged += rbPedal_CheckedChanged;
            rbPedalClose.CheckedChanged += rbPedal_CheckedChanged;

            #endregion

            Style = UIStyle.LayuiGreen;
            //uiTextBox3.FillReadOnlyColor = Color.Firebrick;
        }

        public sealed override string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        /// <summary>
        /// 将控件的宽，高，左边距，顶边距和字体大小暂存到tag属性中
        /// </summary>
        /// <param name="cons">递归控件中的控件</param>
        private static void SetTag(System.Windows.Forms.Control cons)
        {
            foreach (System.Windows.Forms.Control con in cons.Controls)//循环窗体中的控件
            {
                if (con.GetType() != typeof(UIButton))
                {
                    con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                    if (con.Controls.Count > 0)
                        SetTag(con);
                }
            }
        }

        /// <summary>
        /// 根据窗体大小调整控件大小
        /// </summary>
        /// <param name="newx">窗体宽度缩放比例</param>
        /// <param name="newy">窗体高度缩放比例</param>
        /// <param name="cons">随窗体改变控件大小</param>
        private static void SetControls(float newx, float newy, System.Windows.Forms.Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (System.Windows.Forms.Control con in cons.Controls)
            {
                if (con.GetType() != typeof(UIButton))
                {
                    var mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组
                    var a = System.Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度
                    con.Width = (int)a;//宽度
                    a = System.Convert.ToSingle(mytag[1]) * newy;//高度
                    con.Height = (int)(a);
                    a = System.Convert.ToSingle(mytag[2]) * newx;//左边距离
                    con.Left = (int)(a);
                    a = System.Convert.ToSingle(mytag[3]) * newy;//上边缘距离
                    con.Top = (int)(a);
                    var currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                        SetControls(newx, newy, con);
                }
            }
        }

        private void FrmSingleDutPage_Resize(object sender, EventArgs e)
        {
            var newX = (this.Width) / _x; //窗体宽度缩放比例
            var newY = (this.Height) / _y;//窗体高度缩放比例
            SetControls(newX, newY, this);//随窗体改变控件大小
        }

        private void FrmEnvBox_Closed(object sender, EventArgs e)
        {
            _action.stop();

            if (BackgroundWorker != null)
                BackgroundWorker.CancelAsync();

            Environment.Exit(0);
        }

        private void FrmEnvBox_Load(object sender, EventArgs e)
        {
            _x = this.Width;//获取窗体的宽度
            _y = this.Height;//获取窗体的高度
            SetTag(this);//调用方法

            Resize += FrmSingleDutPage_Resize;

            Cx1EEmcDut = !(State.LstControllers.Find(f => f is Cx1EEmc) is Cx1EEmc)
                ? new Cx1EEmc("CX1E_CAN_NULL")
                : State.LstControllers.Find(f => f is Cx1EEmc) as Cx1EEmc;

            InitRelayButton();
            InitCurrDgv();
            InitStateDgv();
            InitThread();

            txtDcMirrorX.Value = 100;
            txtDcMirrorY.Value = 100;
            txtDcChildrenLock.Value = 100;
            txtDcDoor.Value = 100;
            txtDcWindow.Value = 100;
            txtDcHandle.Value = 100;
            txtDcPedal.Value = 100;
            txtDcLock2.Value = 100;
            txtDcElectricRelese.Value = 100;
            //txtMirrTintgCmd.Value = 100;

            swStartCan.Active = true;

            //return;

            var awakeCount = 0;

            _action = generator.tgo(FormSelection.MainStrand, async delegate
            {
                while (true)
                {
                    try
                    {
                        await generator.sleep(50);

                        if (Cx1EEmcDut == null)
                            continue;

                        lblSysState.Text = @"系统状态：" + Cx1EEmcDut.Machine.State;

                        Cx1EEmcDut.SwPowSupplyConPendalHall = swPowSupplyConPendalHall.Active;
                        Cx1EEmcDut.SwPowSupplyCmdPos = swPowSupplyCmdPos.Active;
                        Cx1EEmcDut.SwPowSupplyConHandleHall = swPowSupplyConHandleHall.Active;
                        Cx1EEmcDut.SwPowSupplyConDoorHall = swPowSupplyConDoorHall.Active;
                        Cx1EEmcDut.SwPowSupplyConDdSswitch = swPowSupplyConDDSswitch.Active;
                        Cx1EEmcDut.MotorCmd = MotorCmd;
                        Cx1EEmcDut.WindowLight = WindowLight;
                        Cx1EEmcDut.FronHandleLight = FronHandleLight;
                        Cx1EEmcDut.SwBsd = swBsd.Active;
                        Cx1EEmcDut.SwPocketLight = swPocketLight.Active;
                        Cx1EEmcDut.SwStepLight = swStepLight.Active;

                        var isAwake = ValueHelper.GetTimeSpanMs(Cx1EEmcDut.LastAppCanRecvDate, DateTime.Now) <= 800;

                        if (isAwake)
                        {
                            canfdState.Color = Color.Green;

                            awakeCount++;

                            if (awakeCount == 50)
                                _isRefresh = true;
                        }
                        else
                        {
                            canfdState.Color = Color.FromArgb(255, 192, 192);

                            _isRefresh = false;
                            awakeCount = 0;
                        }

                        //canfdState.Color = ValueHelper.GetTimeSpanMs(Cx1EEmcDut.LastAppCanRecvDate, DateTime.Now) > 2000
                        //    ? Color.FromArgb(255, 192, 192)
                        //    : Color.Green;
                        canState.Color = ValueHelper.GetTimeSpanMs(Cx1EEmcDut.LastCan2RecvDate, DateTime.Now) > 800
                            ? Color.FromArgb(255, 192, 192)
                            : Color.Green;
                        linState.Color = ValueHelper.GetTimeSpanMs(Cx1EEmcDut.LastLinRecvDate, DateTime.Now) > 800
                            ? Color.FromArgb(255, 192, 192)
                            : Color.Green;

                        if (_isRefresh)
                        {
                            txtMirrorXPos.DoubleValue = Cx1EEmcDut.MirrPosnStsAtDrvrMirrPosnAdjCldLeRi;
                            txtMirrorYPos.DoubleValue = Cx1EEmcDut.MirrPosnStsAtDrvrMirrPosnAdjCldUpDwn;

                            if (Cx1EEmcDut.MirrPosnStsAtDrvrMirrPosnAdjCldLeRi >= 60 && Cx1EEmcDut.MirrPosnStsAtDrvrMirrPosnAdjCldLeRi <= 80)
                                txtMirrorXPos.FillReadOnlyColor = Color.LightGreen;
                            else
                                txtMirrorXPos.FillReadOnlyColor = Color.FromArgb(244, 244, 244);

                            if (Cx1EEmcDut.MirrPosnStsAtDrvrMirrPosnAdjCldUpDwn >= 60 && Cx1EEmcDut.MirrPosnStsAtDrvrMirrPosnAdjCldUpDwn <= 80)
                                txtMirrorYPos.FillReadOnlyColor = Color.LightGreen;
                            else
                                txtMirrorYPos.FillReadOnlyColor = Color.FromArgb(244, 244, 244);

                            txtVbat1.DoubleValue = Cx1EEmcDut.Vba1;
                            txtVbat2.DoubleValue = Cx1EEmcDut.Vba2;
                            txtTemperature.DoubleValue = Cx1EEmcDut.Temperature;
                            txtMotorPedalSpeed.DoubleValue = Cx1EEmcDut.MotorPedalSpeed;
                            txtMotorDoorSpeed.DoubleValue = Cx1EEmcDut.MotorDoorSpeed;

                            if (Cx1EEmcDut.MotorPedalSpeed >= 900 && Cx1EEmcDut.MotorPedalSpeed <= 1100)
                                txtMotorPedalSpeed.FillReadOnlyColor = Color.LightGreen;
                            else
                                txtMotorPedalSpeed.FillReadOnlyColor = Color.FromArgb(244, 244, 244);

                            if (Cx1EEmcDut.MotorDoorSpeed >= 900 && Cx1EEmcDut.MotorDoorSpeed <= 1100)
                                txtMotorDoorSpeed.FillReadOnlyColor = Color.LightGreen;
                            else
                                txtMotorDoorSpeed.FillReadOnlyColor = Color.FromArgb(244, 244, 244);

                            if (Cx1EEmcDut.Vba1 >= 8000 && Cx1EEmcDut.Vba1 <= 20000)
                                txtVbat1.FillReadOnlyColor = Color.LightGreen;
                            else
                                txtVbat1.FillReadOnlyColor = Color.FromArgb(244, 244, 244);

                            if (Cx1EEmcDut.Vba2 >= 8000 && Cx1EEmcDut.Vba2 <= 20000)
                                txtVbat2.FillReadOnlyColor = Color.LightGreen;
                            else
                                txtVbat2.FillReadOnlyColor = Color.FromArgb(244, 244, 244);

                            if (Cx1EEmcDut.Temperature >= 2000 && Cx1EEmcDut.Temperature <= 3000)
                                txtTemperature.FillReadOnlyColor = Color.LightGreen;
                            else
                                txtTemperature.FillReadOnlyColor = Color.FromArgb(244, 244, 244);

                            foreach (var f in Cx1EEmcDut.GetType().GetFields())
                            {
                                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                                if (!attrs.Any())
                                    continue;
                                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.State && _stateBinding.ContainsKey(obj.Name))
                                    _stateBinding[obj.Name] = (string)f.GetValue(Cx1EEmcDut);
                            }

                            foreach (var f in Cx1EEmcDut.GetType().GetFields())
                            {
                                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                                if (!attrs.Any())
                                    continue;
                                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.Curr && _currBinding.ContainsKey(obj.Name))
                                    _currBinding[obj.Name] = f.GetValue(Cx1EEmcDut).ToString();
                            }
                        }
                        else
                        {
                            txtMirrorXPos.DoubleValue = 0;
                            txtMirrorYPos.DoubleValue = 0;
                            txtMirrorXPos.FillReadOnlyColor = Color.FromArgb(244, 244, 244);
                            txtMirrorYPos.FillReadOnlyColor = Color.FromArgb(244, 244, 244);

                            txtVbat1.DoubleValue = 0;
                            txtVbat2.DoubleValue = 0;
                            txtTemperature.DoubleValue = 0;
                            txtMotorPedalSpeed.DoubleValue = 0;
                            txtMotorDoorSpeed.DoubleValue = 0;

                            txtVbat1.FillReadOnlyColor = Color.FromArgb(244, 244, 244);
                            txtVbat2.FillReadOnlyColor = Color.FromArgb(244, 244, 244);
                            txtTemperature.FillReadOnlyColor = Color.FromArgb(244, 244, 244);
                            txtMotorPedalSpeed.FillReadOnlyColor = Color.FromArgb(244, 244, 244);
                            txtMotorDoorSpeed.FillReadOnlyColor = Color.FromArgb(244, 244, 244);

                            foreach (var f in Cx1EEmcDut.GetType().GetFields())
                            {
                                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                                if (!attrs.Any())
                                    continue;
                                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.State &&
                                    _stateBinding.ContainsKey(obj.Name))
                                    _stateBinding[obj.Name] = string.Empty;
                            }

                            foreach (var f in Cx1EEmcDut.GetType().GetFields())
                            {
                                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                                if (!attrs.Any())
                                    continue;
                                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.Curr && _currBinding.ContainsKey(obj.Name))
                                    _currBinding[obj.Name] = string.Empty;
                            }
                        }

                        if (dgvState.InvokeRequired)
                        {
                            dgvState.Invoke(new Action(() =>
                            {
                                dgvState.RowCount = _stateBinding.Count;
                                dgvState.Refresh();
                            }));
                        }
                        else
                        {
                            dgvState.RowCount = _stateBinding.Count;
                            dgvState.Refresh();
                        }

                        if (dgvCurrent.InvokeRequired)
                        {
                            dgvCurrent.Invoke(new Action(() =>
                            {
                                dgvCurrent.RowCount = _currBinding.Count;
                                dgvCurrent.Refresh();
                            }));
                        }
                        else
                        {
                            dgvCurrent.RowCount = _currBinding.Count;
                            dgvCurrent.Refresh();
                        }

                        ShowVolts(!_isRefresh);
                    }
                    catch (AccessViolationException exception)
                    {
                        Console.WriteLine(@"0xC0000005 exception:" + exception.ToString());
                    }
                }
                // ReSharper disable once FunctionNeverReturns
            });
        }

        private void InitCurrDgv()
        {
            //dgvCurrent.Style = UIStyle.Green;
            dgvCurrent.ReadOnly = true;
            dgvCurrent.RowHeadersVisible = false;
            dgvCurrent.AllowUserToAddRows = false;
            dgvCurrent.AllowUserToResizeRows = false;
            dgvCurrent.MultiSelect = true;
            dgvCurrent.RowHeadersVisible = false;
            dgvCurrent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvCurrent.ClearRows();
            dgvCurrent.ClearColumns();

            foreach (var f in Cx1EEmcDut.GetType().GetFields())
            {
                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                if (!attrs.Any())
                    continue;
                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.Curr)
                    _currBinding.Add(obj.Name, f.GetValue(Cx1EEmcDut).ToString());
            }

            dgvCurrent.VirtualMode = true;
            dgvCurrent.AddColumn("KeyColumn", "Key");
            dgvCurrent.AddColumn("ValueColumn", "Value");
            dgvCurrent.RowCount = _currBinding.Count;

            //注册事件处理器
            dgvCurrent.CellValueNeeded += DgvCurrent_CellValueNeeded;
        }

        private void DgvCurrent_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // 获取键的列表
            var keys = new List<string>(_currBinding.Keys);

            // 如果我们在第 0 列（显示键），则设置单元格的值为键
            if (e.ColumnIndex == 0)
            {
                e.Value = keys[e.RowIndex];
            }
            // 如果我们在第 1 列（显示值），则设置单元格的值为字典中键对应的值
            else if (e.ColumnIndex == 1)
            {
                var key1 = keys[e.RowIndex];
                e.Value = _currBinding[key1]; // 在这里使用正确的语法获取字典中的值

                if (e.Value != null)
                {
                    var value = e.Value.ToString();
                    if (string.IsNullOrEmpty(value))
                    {
                        dgvCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                            dgvCurrent.Rows[e.RowIndex].Cells[0].Style.BackColor;
                    }
                    else
                    {
                        var key = keys[e.RowIndex];
                        double dValue;
                        if (double.TryParse(value, out dValue))
                        {
                            var isInRange = false;

                            if (key.ToLower().Contains("X电机".ToLower()))
                            {
                                if (dValue >= 300 && dValue <= 1000)
                                {
                                    isInRange = true;
                                }
                            }
                            else if (key.ToLower().Contains("Y电机".ToLower()))
                            {
                                if (dValue >= 150 && dValue <= 600)
                                {
                                    isInRange = true;
                                }
                            }
                            else if (key.ToLower().Contains("车窗".ToLower()) || key.ToLower().Contains("电动脚踏电机".ToLower()) || key.ToLower().Contains("电动门".ToLower()))
                            {
                                if (dValue >= 1300 && dValue <= 3000)
                                {
                                    isInRange = true;
                                }
                            }
                            else
                            {
                                if (dValue >= 0 && dValue <= 100)
                                {
                                    isInRange = true;
                                }
                            }

                            dgvCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = isInRange ? Color.LightGreen : dgvCurrent.Rows[e.RowIndex].Cells[0].Style.BackColor;
                        }
                        else
                        {
                            dgvCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                                dgvCurrent.Rows[e.RowIndex].Cells[0].Style.BackColor;
                        }
                    }
                }
                else
                {
                    dgvCurrent.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                        dgvCurrent.Rows[e.RowIndex].Cells[0].Style.BackColor;
                }
            }
        }

        private void InitStateDgv()
        {
            //dgvState.Style = UIStyle.Green;
            dgvState.ReadOnly = true;
            dgvState.RowHeadersVisible = false;
            dgvState.AllowUserToAddRows = false;
            dgvState.AllowUserToResizeRows = false;
            dgvState.MultiSelect = true;
            dgvState.RowHeadersVisible = false;
            dgvState.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvState.ClearRows();
            dgvState.ClearColumns();

            foreach (var f in Cx1EEmcDut.GetType().GetFields())
            {
                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                if (!attrs.Any())
                    continue;
                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.State)
                    _stateBinding.Add(obj.Name, (string)f.GetValue(Cx1EEmcDut));
            }

            dgvState.VirtualMode = true;
            dgvState.AddColumn("KeyColumn", "Key");
            dgvState.AddColumn("ValueColumn", "Value");
            dgvState.RowCount = _stateBinding.Count;

            //注册事件处理器
            dgvState.CellValueNeeded += DgvState_CellValueNeeded;
        }

        private void DgvState_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            // 获取键的列表
            var keys = new List<string>(_stateBinding.Keys);

            // 如果我们在第 0 列（显示键），则设置单元格的值为键
            if (e.ColumnIndex == 0)
            {
                e.Value = keys[e.RowIndex];
            }
            // 如果我们在第 1 列（显示值），则设置单元格的值为字典中键对应的值
            else if (e.ColumnIndex == 1)
            {
                var key1 = keys[e.RowIndex];
                e.Value = _stateBinding[key1]; // 在这里使用正确的语法获取字典中的值

                if (e.Value != null)
                {
                    var value = e.Value.ToString();
                    if (string.IsNullOrEmpty(value))
                    {
                        dgvState.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                            dgvState.Rows[e.RowIndex].Cells[0].Style.BackColor;
                    }
                    else
                    {
                        var key = keys[e.RowIndex];

                        var para = State.DeviceConfig.Paras.ToList().Find(f => f.Name == key);
                        if (para == null)
                        {
                            dgvState.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                                dgvState.Rows[e.RowIndex].Cells[0].Style.BackColor;
                            return;
                        }

                        if (string.IsNullOrEmpty(para.Value))
                        {
                            dgvState.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                                dgvState.Rows[e.RowIndex].Cells[0].Style.BackColor;
                            return;
                        }

                        var binding = para.ControllerField;
                        var controllerName = binding.GetStrsSplitByValue(".Field.")[0];
                        var state = binding.GetStrsSplitByValue(".Field.")[1];
                        var controller = State.LstControllers.Find(f => (f as ControllerBase) != null && ((ControllerBase)f).Name == controllerName);

                        if (string.IsNullOrEmpty(controllerName) ||
                            controller == null ||
                            string.IsNullOrEmpty(state))
                            return;

                        var field = controller.GetType().GetField(state);
                        if (field == null)
                        {
                            dgvState.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                                dgvState.Rows[e.RowIndex].Cells[0].Style.BackColor;
                            return;
                        }

                        var fieldValue = field.GetValue(controller);
                        if (fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString()))
                        {
                            if (string.Equals(para.Value, fieldValue.ToString(), StringComparison.CurrentCultureIgnoreCase))
                            {
                                dgvState.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor = Color.LightGreen;
                            }
                            else
                            {
                                dgvState.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                                    dgvState.Rows[e.RowIndex].Cells[0].Style.BackColor;
                            }
                        }
                        else
                        {
                            dgvState.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                                dgvState.Rows[e.RowIndex].Cells[0].Style.BackColor;
                        }
                    }
                }
                else
                {
                    dgvState.Rows[e.RowIndex].Cells[e.ColumnIndex].Style.BackColor =
                        dgvState.Rows[e.RowIndex].Cells[0].Style.BackColor;
                }
            }
        }

        private void InitRelayButton()
        {
            foreach (var btn in
                     groupRelayBtns.Controls.Cast<object>().Where(
                         control => control.GetType() == typeof(UIButton)).OfType<UIButton>())
            {
                btn.Tag = false;
                btn.Style = UIStyle.Gray;
                btn.Click += btnRelay_Click;
            }
        }

        private void InitThread()
        {
            BackgroundWorker = new BackgroundWorker();
            BackgroundWorker.DoWork += BackgroundWorker_DoWork;
            BackgroundWorker.WorkerReportsProgress = true;
            BackgroundWorker.WorkerSupportsCancellation = true;
            BackgroundWorker.RunWorkerAsync();
        }

        private void swStartCan_ActiveChanged(object sender, EventArgs e)
        {
            if (swStartCan.Active)
            {
                Cx1EEmcDut.StartCan();
                Cx1EEmcDut.StartDiagnosis();
            }
            else
            {
                Cx1EEmcDut.StopCan();
            }
        }

        #region 2F帧事件

        private void txtDcMirrorX_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.List2FBytes[0x4003][0] = (byte)txtDcMirrorX.Value;
            this.ShowInfoTip(string.Format("后视镜展开/折叠占空比：{0}%", value));
        }

        private void txtDcMirrorY_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.List2FBytes[0x4002][0] = (byte)txtDcMirrorY.Value;
            this.ShowInfoTip(string.Format("后视镜上/下占空比：{0}%", value));
        }

        private void txtDcWindow_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.List2FBytes[0x400E][0] = (byte)txtDcWindow.Value;
            this.ShowInfoTip(string.Format("车窗电机占空比：{0}%", value));
        }

        private void txtDcChildrenLock_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.List2FBytes[0x4010][0] = (byte)txtDcChildrenLock.Value;
            this.ShowInfoTip(string.Format("儿童锁电机占空比：{0}%", value));
        }

        private void txtDcElectricRelese_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.List2FBytes[0x4051][0] = (byte)txtDcElectricRelese.Value;
            this.ShowInfoTip(string.Format("电释放电机占空比：{0}%", value));
        }

        private void txtDcLock2_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.List2FBytes[0x4053][0] = (byte)txtDcLock2.Value;
            this.ShowInfoTip(string.Format("吸合锁电机占空比：{0}%", value));
        }

        private void txtDcHandle_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.List2FBytes[0x4052][0] = (byte)txtDcHandle.Value;
            this.ShowInfoTip(string.Format("隐藏门把手电机占空比：{0}%", value));
        }

        private void txtDcPedal_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.List2FBytes[0x4055][0] = (byte)txtDcPedal.Value;
            this.ShowInfoTip(string.Format("电动踏板电机占空比：{0}%", value));
        }

        private void txtDcDoor_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.List2FBytes[0x40AE][0] = (byte)txtDcDoor.Value;
            this.ShowInfoTip(string.Format("电动门电机占空比：{0}%", value));
        }

        private byte MotorCmd { get; set; }

        private void cmbMotorCmd_SelectedIndexChanged(object sender, EventArgs e)
        {
            MotorCmd = (byte)cmbMotorCmd.SelectedIndex;
        }

        #endregion

        #region 周期帧事件

        private void txtMirrTintgCmd_ValueChanged(object sender, int value)
        {
            Cx1EEmcDut.MirrTintgCmd((byte)txtMirrTintgCmd.Value);
            this.ShowInfoTip(string.Format("调光信号：{0}%", value));
        }

        private void swMirrDefrstDrvrCmd_ActiveChanged(object sender, EventArgs e)
        {
            Cx1EEmcDut.MirrDefrstAtDrvrCmd(swMirrDefrstDrvrCmd.Active);
        }

        private void swActvnOfIndcrOut_ActiveChanged(object sender, EventArgs e)
        {
            Cx1EEmcDut.ActvnOfIndcrIndcrOut(swActvnOfIndcrOut.Active);
        }

        private void swActvnOfPudLi_ActiveChanged(object sender, EventArgs e)
        {
            Cx1EEmcDut.ActvnOfPudLi(swActvnOfPudLi.Active);
        }

        private byte FronHandleLight { get; set; }
        private byte WindowLight { get; set; }

        private void cmbFronHandleLight_SelectedIndexChanged(object sender, EventArgs e)
        {
            FronHandleLight = (byte)cmbFronHandleLight.SelectedIndex;
        }

        private void cmbWindowLight_SelectedIndexChanged(object sender, EventArgs e)
        {
            WindowLight = (byte)cmbWindowLight.SelectedIndex;
        }

        private void cmbHndlDoorLiDrvr_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cx1EEmcDut.ActvnOfHndlDoorLi1HndlDoorLiDrvr((byte)cmbHndlDoorLiDrvr.SelectedIndex);
        }

        private void cmbDrvrMirrCmd5_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cx1EEmcDut.DrvrMirrCmd((byte)cmbDrvrMirrCmd5.SelectedIndex);
        }

        private void cmbDrvrAdjCmd_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cx1EEmcDut.DrvrAdjCmd((byte)cmbDrvrAdjCmd.SelectedIndex);
        }

        private void rbShortDropWin_CheckedChanged(object sender, EventArgs e)
        {
            if (rbShortDropWinIdle.Checked)
                Cx1EEmcDut.ShortDropWin(0);
            else if (rbShortDropWinClose.Checked)
                Cx1EEmcDut.ShortDropWin(1);
            else if (rbShortDropWinOpen.Checked)
                Cx1EEmcDut.ShortDropWin(2);
        }

        private void rbChdLockReLeCtrlHmiReq_CheckedChanged(object sender, EventArgs e)
        {
            if (rbChdLockReLeCtrlHmiReqIdle.Checked)
            {
                Cx1EEmcDut.ChdLockReLeCtrlHmiReq(0);
            }
            else if (rbChdLockReLeCtrlHmiReqLock.Checked)
            {
                Cx1EEmcDut.ChdLockReLeCtrlHmiReq(2);
            }
            else if (rbChdLockReLeCtrlHmiReqUnlock.Checked)
            {
                Cx1EEmcDut.ChdLockReLeCtrlHmiReq(1);
            }
        }

        private void rbDoor_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDoorIdle.Checked)
                Cx1EEmcDut.Door(0);
            else if (rbDoorOpen.Checked)
                Cx1EEmcDut.Door(1);
            else if (rbDoorClose.Checked)
                Cx1EEmcDut.Door(2);
        }

        private void rbPedal_CheckedChanged(object sender, EventArgs e)
        {
            //rbPedalIdle.CheckedChanged += rbPedal_CheckedChanged;
            //rbPedalOpen.CheckedChanged += rbPedal_CheckedChanged;
            //rbPedalClose.CheckedChanged += rbPedal_CheckedChanged;

            if (rbPedalIdle.Checked)
                Cx1EEmcDut.Pendal(0);
            else if (rbPedalOpen.Checked)
                Cx1EEmcDut.Pendal(1);
            else if (rbPedalClose.Checked)
                Cx1EEmcDut.Pendal(2);
        }

        private void rbDoorDrvrHndlCmd_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDoorDrvrHndlCmdIdle.Checked)
                Cx1EEmcDut.DoorDrvrHndlCmd(0);
            else if (rbDoorDrvrHndlCmdClose.Checked)
                Cx1EEmcDut.DoorDrvrHndlCmd(2);
            else if (rbDoorDrvrHndlCmdOpen.Checked)
                Cx1EEmcDut.DoorDrvrHndlCmd(1);
        }

        #endregion

        #region 控制

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
            {
                //try
                //{
                //    //if (Cx1EEmcDut == null)
                //    //    continue;

                //    //BeginInvoke(new Action(() =>
                //    //{
                //    //    lblSysState.Text = @"系统状态：" + Cx1EEmcDut.Machine.State;

                //    //    txtMirrorXPos.DoubleValue = Cx1EEmcDut.MirrPosnStsAtDrvrMirrPosnAdjCldLeRi;
                //    //    txtMirrorYPos.DoubleValue = Cx1EEmcDut.MirrPosnStsAtDrvrMirrPosnAdjCldUpDwn;

                //    //    txtVbat1.DoubleValue = Cx1EEmcDut.Vba1;
                //    //    txtVbat2.DoubleValue = Cx1EEmcDut.Vba2;
                //    //    txtTemperature.DoubleValue = Cx1EEmcDut.Temperature;
                //    //    txtMotorPedalSpeed.DoubleValue = Cx1EEmcDut.MotorPedalSpeed;
                //    //    txtMotorDoorSpeed.DoubleValue = Cx1EEmcDut.MotorDoorSpeed;

                //    //    canfdState.Color = ValueHelper.GetTimeSpanMs(Cx1EEmcDut.LastAppCanRecvDate, DateTime.Now) > 2000
                //    //        ? Color.FromArgb(255, 192, 192)
                //    //        : Color.Green;
                //    //    canState.Color = ValueHelper.GetTimeSpanMs(Cx1EEmcDut.LastCan2RecvDate, DateTime.Now) > 2000
                //    //        ? Color.FromArgb(255, 192, 192)
                //    //        : Color.Green;
                //    //    linState.Color = ValueHelper.GetTimeSpanMs(Cx1EEmcDut.LastLinRecvDate, DateTime.Now) > 2000
                //    //        ? Color.FromArgb(255, 192, 192)
                //    //        : Color.Green;
                //    //}));

                //    Cx1EEmcDut.SwPowSupplyConPendalHall = swPowSupplyConPendalHall.Active;
                //    Cx1EEmcDut.SwPowSupplyCmdPos = swPowSupplyCmdPos.Active;
                //    Cx1EEmcDut.SwPowSupplyConHandleHall = swPowSupplyConHandleHall.Active;
                //    Cx1EEmcDut.SwPowSupplyConDoorHall = swPowSupplyConDoorHall.Active;
                //    Cx1EEmcDut.SwPowSupplyConDdSswitch = swPowSupplyConDDSswitch.Active;
                //    Cx1EEmcDut.MotorCmd = MotorCmd;
                //    Cx1EEmcDut.WindowLight = WindowLight;
                //    Cx1EEmcDut.FronHandleLight = FronHandleLight;
                //    Cx1EEmcDut.SwBsd = swBsd.Active;
                //    Cx1EEmcDut.SwPocketLight = swPocketLight.Active;
                //    Cx1EEmcDut.SwStepLight = swStepLight.Active;

                //    //if (Cx1EEmcDut.Machine.State == Cx1EEmc.EmcState.Run)
                //    {
                //        {
                //            foreach (var f in Cx1EEmcDut.GetType().GetFields())
                //            {
                //                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                //                if (!attrs.Any())
                //                    continue;
                //                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                //                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.State && _stateBinding.ContainsKey(obj.Name))
                //                    _stateBinding[obj.Name] = (string)f.GetValue(Cx1EEmcDut);
                //            }
                //        }

                //        {
                //            foreach (var f in Cx1EEmcDut.GetType().GetFields())
                //            {
                //                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                //                if (!attrs.Any())
                //                    continue;
                //                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                //                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.Curr && _currBinding.ContainsKey(obj.Name))
                //                    _currBinding[obj.Name] = (double)f.GetValue(Cx1EEmcDut);
                //            }
                //        }

                //        if (dgvState.InvokeRequired)
                //        {
                //            dgvState.Invoke(new Action(() =>
                //            {
                //                dgvState.RowCount = _stateBinding.Count;
                //                dgvState.Refresh();
                //            }));
                //        }
                //        else
                //        {
                //            dgvState.RowCount = _stateBinding.Count;
                //            dgvState.Refresh();
                //        }

                //        if (dgvCurrent.InvokeRequired)
                //        {
                //            dgvCurrent.Invoke(new Action(() =>
                //            {
                //                dgvCurrent.RowCount = _currBinding.Count;
                //                dgvCurrent.Refresh();
                //            }));
                //        }
                //        else
                //        {
                //            dgvCurrent.RowCount = _currBinding.Count;
                //            dgvCurrent.Refresh();
                //        }

                //        ShowVolts();
                //    }
                //}
                //catch (Exception exception)
                //{
                //    if (exception.GetType() != typeof(AccessViolationException))
                //    {
                //        // If we have been stopped, ignore the exception since
                //        // it is likely just an error indicating the acquisition has
                //        // been stopped
                //        if (!worker.CancellationPending)
                //        {
                //            e.Result = exception;
                //        }
                //        return;
                //    }
                //}
            }
        }

        private void ShowVolts(bool isReset)
        {
            UpdateVoltTxtValue(txtPowSupplyConDDSswitchVolt, "DDS开关供电", isReset);
            UpdateVoltTxtValue(txtHndlDoorLiDrvrVolt, "门外把手灯", isReset);
            UpdateVoltTxtValue(txtFronHandleLightVolt, "门内把手灯背光", isReset);
            UpdateVoltTxtValue(txtLockLightVolt, "锁开关指示灯", isReset);
            UpdateVoltTxtValue(txtWindowLightVolt, "本地车窗开关背光", isReset);
            UpdateVoltTxtValue(txtowSupplyConPendalHallVolt, "电机霍尔供电-电动踏板", isReset);
            UpdateVoltTxtValue(txtPowSupplyConHandleHallVolt, "电机霍尔供电-隐藏门把手", isReset);
            UpdateVoltTxtValue(txtPowSupplyConDoorHallVolt, "电机霍尔供电-电动门电机", isReset);
            UpdateVoltTxtValue(txtMemoryVolt, "记忆开关指示灯", isReset);
            UpdateVoltTxtValue(txtPocketLightVolt, "门口袋灯", isReset);
            UpdateVoltTxtValue(txtBsdVolt, "BSD开关供电", isReset);
            UpdateVoltTxtValue(txtActvnOfPudLiVolt, "后视镜照地灯", isReset);
            UpdateVoltTxtValue(txtStepLightVolt, "踏步灯/照地灯", isReset);
            UpdateVoltTxtValue(txtActvnOfIndcrOutVolt, "后视镜转向灯", isReset);
            UpdateVoltTxtValue(txtMirrorDefrstVolt, "后视镜加热输出", isReset);
            UpdateVoltTxtValue(txtPowSupplyCmdPosVolt, "后视镜位置传感器(电位计供电)", isReset);
            UpdateVoltTxtValue(txtMirrorTintgVolt, "外后视镜防眩目", isReset);
        }

        private delegate void UpdateVoltTxtValueDelegate(UITextBox control, string voltName, bool isReset);
        private void UpdateVoltTxtValue(UITextBox control, string voltName, bool isReset)
        {
            var updateTxtVoltValueDelegate = new UpdateVoltTxtValueDelegate(UpdateVoltTxtValue);

            if (control.InvokeRequired)
            {
                Invoke(updateTxtVoltValueDelegate, control, voltName);
            }
            else
            {
                if (State.DeviceConfig.Paras == null)
                    return;

                var para = State.DeviceConfig.Paras.ToList().Find(f => f.Name == voltName);
                if (para == null)
                    return;

                var binding = para.ControllerField;
                var controllerName = binding.GetStrsSplitByValue(".Field.")[0];
                var volt = binding.GetStrsSplitByValue(".Field.")[1];
                var controller = State.LstControllers.Find(f => (f as ControllerBase) != null && ((ControllerBase)f).Name == controllerName);

                if (string.IsNullOrEmpty(controllerName) ||
                    controller == null ||
                    string.IsNullOrEmpty(volt))
                    return;

                var field = controller.GetType().GetField(volt);
                if (field == null)
                    return;

                var fieldValue = field.GetValue(controller);
                if (fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString()))
                {
                    if (voltName.Contains("电机霍尔供电") && double.Parse(fieldValue.ToString()) > 10)
                    {
                        var textToWrite = string.Format("{0}: {1}", voltName, (fieldValue.ToString()));
                        var logFilePath = string.Format(@"{0}\电机霍尔供电_ErrorVolt_{1}.txt", Directory.GetCurrentDirectory(), Guid.NewGuid().ToString().Replace("-", ""));

                        Task.Run(() =>
                        {
                            // 使用StreamWriter类写入文本到文件
                            // 第二个参数为 false 表示若文件存在，则覆写；为 true 表示追加内容
                            //using (var writer = new StreamWriter(logFilePath, false))
                            //    writer.WriteLine(textToWrite);
                        });
                    }

                    if (isReset)
                    {
                        control.DoubleValue = 0;
                        control.FillReadOnlyColor = Color.FromArgb(244, 244, 244);
                    }
                    else
                    {
                        control.DoubleValue = Math.Round(double.Parse(fieldValue.ToString()), 2,
                            MidpointRounding.AwayFromZero);

                        if (!string.IsNullOrEmpty(para.Min) && !string.IsNullOrEmpty(para.Max))
                        {
                            double min;
                            double max;

                            if (double.TryParse(para.Min, out min) && double.TryParse(para.Max, out max))
                            {
                                if (Math.Round(double.Parse(fieldValue.ToString()), 2,
                                        MidpointRounding.AwayFromZero) >= min &&
                                    Math.Round(double.Parse(fieldValue.ToString()), 2,
                                        MidpointRounding.AwayFromZero) <= max)
                                {
                                    control.FillReadOnlyColor = Color.LightGreen; // 244, 244, 244
                                }
                                else
                                {
                                    control.FillReadOnlyColor = Color.FromArgb(244, 244, 244);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void btnRelay_Click(object sender, EventArgs e)
        {
            if (State.DeviceConfig.Parts == null)
                return;

            var btn = sender as UIButton;
            if (btn == null)
                return;

            var part = State.DeviceConfig.Parts.ToList().Find(f =>
                f.Name.Replace(" ", "").Replace("_", "").Replace("（", "").Replace("）", "").Replace("(", "")
                    .Replace(")", "") == btn.Text.Replace(" ", "").Replace("_", "").Replace("（", "").Replace("）", "")
                    .Replace("(", "").Replace(")", ""));
            if (part == null)
                return;

            bool isOn;

            if ((bool)btn.Tag == false)
            {
                btn.Tag = true;
                btn.Style = UIStyle.Blue;
                isOn = true;
            }
            else
            {
                btn.Tag = false;
                btn.Style = UIStyle.Gray;
                isOn = false;
            }

            var binding = part.ControllerField;
            var controllerName = binding.GetStrsSplitByValue(".Field.")[0];
            var relay = binding.GetStrsSplitByValue(".Field.")[1];
            var controller = State.LstControllers.Find(f => (f as ControllerBase) != null && ((ControllerBase)f).Name == controllerName);

            if (!string.IsNullOrEmpty(controllerName) &&
                controller != null &&
                !string.IsNullOrEmpty(relay))
            {
                var field = controller.GetType().GetField(relay);
                if (field != null)
                {
                    field.SetValue(controller, isOn);
                    this.ShowInfoTip(string.Format("{0}: {1}", btn.Text, isOn ? "ON" : "OFF"));
                }
            }
        }

        #endregion

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                using (var form = new FormStateMonitor(State))
                    form.ShowDialog();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!File.Exists(State.XmlFilePath))
                return;

            try
            {
                var dirPath = Program.SysDir;
                var xmlPath = State.XmlFilePath;
                var controllerPath = dirPath + @"\Controller.dll";
                var userControlPath = dirPath + @"\UserControls.dll";
                using (var form = new DeviceDesign.FormMain(xmlPath, controllerPath, userControlPath))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"打开失败：" + ex.Message);
            }
        }
    }
}
