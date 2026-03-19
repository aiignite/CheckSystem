using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using Go;
using HZH_Controls;
using HZH_Controls.IconFont;
using StateMachine;
using Sunny.UI;
using FontImages = HZH_Controls.IconFont.FontImages;

namespace CheckSystem.HelperForms.GeeleyCx1e
{
    public partial class FrmEnvBox : UIForm
    {
        private readonly Cx1EEmc _cx1EEmcDut1 = new Cx1EEmc("DUT1");
        private generator _action;
        private readonly Dictionary<string, string> _stateBinding = new Dictionary<string, string>();
        private readonly Dictionary<string, double> _currBinding = new Dictionary<string, double>();

        private SyControllerWith56Pin Controller { get; set; }
        //private BackgroundWorker CanBackgroundWorker { get; set; }
        private BackgroundWorker IoBackgroundWorker { get; set; }

        private readonly Dictionary<string, SyControllerWith56Pin> _listControllerWith56Pins = new Dictionary<string, SyControllerWith56Pin>();
        private readonly Dictionary<string, string> _listRlms = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _listVolts = new Dictionary<string, string>();

        public class DictionaryItem<TKey, TValue>
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }

            public DictionaryItem(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
        }

        public FrmEnvBox()
        {
            InitializeComponent();
            Load += FrmEnvBox_Load;
            Closed += FrmEnvBox_Closed;
            Icon = FontImages.GetIcon(
               FontIcons.A_fa_dot_circle_o, 32,
               Color.DodgerBlue);

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

        private void FrmEnvBox_Closed(object sender, EventArgs e)
        {
            _action.stop();

            //if (CanBackgroundWorker != null)
            //    CanBackgroundWorker.CancelAsync();

            if (IoBackgroundWorker != null)
                IoBackgroundWorker.CancelAsync();

            if (Controller != null)
                Controller.Dispose();

            foreach (var key in _listControllerWith56Pins.Keys.Where(key => _listControllerWith56Pins[key] != null))
                _listControllerWith56Pins[key].Dispose();

            Environment.Exit(0);
        }

        private void FrmEnvBox_Load(object sender, EventArgs e)
        {
            InitRelayButton();
            InitCurrDgv();
            InitStateDgv();
            InitDevice();
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

            _action = generator.tgo(FormSelection.MainStrand, async delegate
            {
                while (true)
                {
                    await generator.sleep(50);

                    lblSysState.Text = @"系统状态：" + _cx1EEmcDut1.Machine.State;

                    _cx1EEmcDut1.SwPowSupplyConPendalHall = swPowSupplyConPendalHall.Active;
                    _cx1EEmcDut1.SwPowSupplyCmdPos = swPowSupplyCmdPos.Active;
                    _cx1EEmcDut1.SwPowSupplyConHandleHall = swPowSupplyConHandleHall.Active;
                    _cx1EEmcDut1.SwPowSupplyConDoorHall = swPowSupplyConDoorHall.Active;
                    _cx1EEmcDut1.SwPowSupplyConDdSswitch = swPowSupplyConDDSswitch.Active;
                    _cx1EEmcDut1.MotorCmd = MotorCmd;
                    _cx1EEmcDut1.WindowLight = WindowLight;
                    _cx1EEmcDut1.FronHandleLight = FronHandleLight;
                    _cx1EEmcDut1.SwBsd = swBsd.Active;
                    _cx1EEmcDut1.SwPocketLight = swPocketLight.Active;
                    _cx1EEmcDut1.SwStepLight = swStepLight.Active;

                    txtMirrorXPos.DoubleValue = _cx1EEmcDut1.MirrPosnStsAtDrvrMirrPosnAdjCldLeRi;
                    txtMirrorYPos.DoubleValue = _cx1EEmcDut1.MirrPosnStsAtDrvrMirrPosnAdjCldUpDwn;

                    txtVbat1.DoubleValue = _cx1EEmcDut1.Vba1;
                    txtVbat2.DoubleValue = _cx1EEmcDut1.Vba2;
                    txtTemperature.DoubleValue = _cx1EEmcDut1.Temperature;
                    txtMotorPedalSpeed.DoubleValue = _cx1EEmcDut1.MotorPedalSpeed;
                    txtMotorDoorSpeed.DoubleValue = _cx1EEmcDut1.MotorDoorSpeed;

                    {
                        foreach (var f in _cx1EEmcDut1.GetType().GetFields())
                        {
                            var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                            if (!attrs.Any())
                                continue;
                            var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                            if (obj != null && obj.EmcType == Cx1EEmc.EmcType.State && _stateBinding.ContainsKey(obj.Name))
                                _stateBinding[obj.Name] = (string)f.GetValue(_cx1EEmcDut1);
                        }

                        var bindingList = new BindingList<DictionaryItem<string, string>>();
                        foreach (var kvp in _stateBinding)
                            bindingList.Add(new DictionaryItem<string, string>(kvp.Key, kvp.Value));
                        dgvState.DataSource = bindingList;
                        //dgvState.Refresh();
                    }

                    {
                        foreach (var f in _cx1EEmcDut1.GetType().GetFields())
                        {
                            var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                            if (!attrs.Any())
                                continue;
                            var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                            if (obj != null && obj.EmcType == Cx1EEmc.EmcType.Curr && _currBinding.ContainsKey(obj.Name))
                                _currBinding[obj.Name] = (double)f.GetValue(_cx1EEmcDut1);
                        }

                        var bindingList = new BindingList<DictionaryItem<string, double>>();
                        foreach (var kvp in _currBinding)
                            bindingList.Add(new DictionaryItem<string, double>(kvp.Key, kvp.Value));
                        dgvCurrent.DataSource = bindingList;
                        //dgvCurrent.Refresh();
                    }

                    Application.DoEvents();
                }
                // ReSharper disable once FunctionNeverReturns
            });
        }

        private void InitDevice()
        {
            Controller = new SyControllerWith56Pin("ContollerCan板IP31");
            Controller.InitRemoteIpAddress("192.168.1.31:8088");
            //Controller.InitRemoteIpAddress("127.0.0.1:502");
            _cx1EEmcDut1.AppCan = Controller.GatwayCan2;

            //Controller.InitRemoteIpAddress("192.168.1.28:8088");
            //_cx1EEmcDut1.AppCan = Controller.GatwayCan1; // for debug

            const string c1Name = "56pin板IP28";
            const string c2Name = "56pin板IP29";
            const string c3Name = "56pin板IP30";

            _listControllerWith56Pins.Add(c1Name, new SyControllerWith56Pin(c1Name));
            _listControllerWith56Pins[c1Name].InitRemoteIpAddress("192.168.1.28:8088");
            _listControllerWith56Pins.Add(c2Name, new SyControllerWith56Pin(c2Name));
            _listControllerWith56Pins[c2Name].InitRemoteIpAddress("192.168.1.29:8088");
            _listControllerWith56Pins.Add(c3Name, new SyControllerWith56Pin(c3Name));
            _listControllerWith56Pins[c3Name].InitRemoteIpAddress("192.168.1.30:8088");

            //_listControllerWith56Pins.Add(c1Name, new SyControllerWith56Pin(c1Name));
            //_listControllerWith56Pins[c1Name].InitRemoteIpAddress("192.168.1.18:8088"); // for debug
            //_listControllerWith56Pins.Add(c2Name, new SyControllerWith56Pin(c2Name));
            //_listControllerWith56Pins[c2Name].InitRemoteIpAddress("192.168.1.19:8088"); // for debug
            //_listControllerWith56Pins.Add(c3Name, new SyControllerWith56Pin(c3Name));
            //_listControllerWith56Pins[c3Name].InitRemoteIpAddress("192.168.1.10:8088"); // for debug

            _listRlms.Add("后遮阳帘电机霍尔信号1", string.Format("{0}.Field.Relay{1}", c1Name, 1));
            _listRlms.Add("后遮阳帘电机霍尔信号2", string.Format("{0}.Field.Relay{1}", c1Name, 2));
            _listRlms.Add("电动门电机霍尔信号1", string.Format("{0}.Field.Relay{1}", c1Name, 3));
            _listRlms.Add("电动门电机霍尔信号2", string.Format("{0}.Field.Relay{1}", c1Name, 4));

            _listRlms.Add("外把手开关", string.Format("{0}.Field.Relay{1}", c1Name, 5));
            _listRlms.Add("内把手开关2", string.Format("{0}.Field.Relay{1}", c1Name, 6));
            _listRlms.Add("内把手开关1", string.Format("{0}.Field.Relay{1}", c1Name, 7));
            _listRlms.Add("中控锁开关820Ω", string.Format("{0}.Field.Relay{1}", c1Name, 8));
            _listRlms.Add("中控锁开关2.2KΩ", string.Format("{0}.Field.Relay{1}", c1Name, 9));
            _listRlms.Add("车窗本地开关 402Ω", string.Format("{0}.Field.Relay{1}", c1Name, 10));
            _listRlms.Add("车窗本地开关 3.6kΩ", string.Format("{0}.Field.Relay{1}", c1Name, 11));
            _listRlms.Add("记忆开关1/2 1.2kΩ", string.Format("{0}.Field.Relay{1}", c1Name, 12));

            _listRlms.Add("记忆开关1/2 634Ω", string.Format("{0}.Field.Relay{1}", c2Name, 1));
            _listRlms.Add("记忆开关M/3 1.2KΩ", string.Format("{0}.Field.Relay{1}", c2Name, 2));
            _listRlms.Add("记忆开关M/3 634Ω", string.Format("{0}.Field.Relay{1}", c2Name, 3));
            _listRlms.Add("中控锁反馈开关", string.Format("{0}.Field.Relay{1}", c2Name, 4));
            _listRlms.Add("吸合锁复位开关", string.Format("{0}.Field.Relay{1}", c2Name, 5));
            _listRlms.Add("棘爪（半开）开关", string.Format("{0}.Field.Relay{1}", c2Name, 6));
            _listRlms.Add("门全开开关", string.Format("{0}.Field.Relay{1}", c2Name, 7));
            _listRlms.Add("门Ajar开关", string.Format("{0}.Field.Relay{1}", c2Name, 8));
            _listRlms.Add("隐藏门把手位置反馈1", string.Format("{0}.Field.Relay{1}", c2Name, 9));
            _listRlms.Add("隐藏门把手位置反馈2", string.Format("{0}.Field.Relay{1}", c2Name, 10));
            _listRlms.Add("后视镜左右(X轴)位置检测", string.Format("{0}.Field.Relay{1}", c2Name, 11));
            _listRlms.Add("后视镜上下(Y轴)位置检测", string.Format("{0}.Field.Relay{1}", c2Name, 12));

            _listRlms.Add("外温传感器", string.Format("{0}.Field.Relay{1}", c3Name, 1));

            _listVolts.Add("DDS开关供电", string.Format("{0}.Field.Voltage{1}", c1Name, 1));
            _listVolts.Add("门外把手灯", string.Format("{0}.Field.Voltage{1}", c1Name, 2));
            _listVolts.Add("门内把手灯背光", string.Format("{0}.Field.Voltage{1}", c1Name, 3));
            _listVolts.Add("锁开关指示灯", string.Format("{0}.Field.Voltage{1}", c1Name, 4)); // 中控锁反馈开关继电器
            _listVolts.Add("本地车窗开关背光", string.Format("{0}.Field.Voltage{1}", c1Name, 5));
            _listVolts.Add("记忆开关背光", string.Format("{0}.Field.Voltage{1}", c1Name, 6)); // 不用

            _listVolts.Add("电机霍尔供电-电动踏板", string.Format("{0}.Field.Voltage{1}", c2Name, 1)); // 电动踏板、隐藏本把手、电动门电机三个
            _listVolts.Add("电机霍尔供电-隐藏门把手", string.Format("{0}.Field.Voltage{1}", c2Name, 1)); // 电动踏板、隐藏本把手、电动门电机三个
            _listVolts.Add("电机霍尔供电-电动门电机", string.Format("{0}.Field.Voltage{1}", c2Name, 1)); // 电动踏板、隐藏本把手、电动门电机三个

            _listVolts.Add("记忆开关指示灯", string.Format("{0}.Field.Voltage{1}", c2Name, 2)); // 记忆开关1/2的继电器
            _listVolts.Add("门口袋灯", string.Format("{0}.Field.Voltage{1}", c2Name, 3));
            _listVolts.Add("BSD开关供电", string.Format("{0}.Field.Voltage{1}", c2Name, 4));
            _listVolts.Add("后视镜照地灯", string.Format("{0}.Field.Voltage{1}", c2Name, 5));
            _listVolts.Add("踏步灯/照地灯", string.Format("{0}.Field.Voltage{1}", c2Name, 6));

            //_listVolts.Add("门开警示灯", string.Format("{0}.Field.Voltage{1}", c3Name, 1)); // 不用
            _listVolts.Add("后视镜转向灯", string.Format("{0}.Field.Voltage{1}", c3Name, 2));
            _listVolts.Add("后视镜加热输出", string.Format("{0}.Field.Voltage{1}", c3Name, 3));
            _listVolts.Add("后视镜位置传感器(电位计供电)", string.Format("{0}.Field.Voltage{1}", c3Name, 4)); // 位置传感器供电控制
            _listVolts.Add("外后视镜防眩目", string.Format("{0}.Field.Voltage{1}", c3Name, 5)); // 后视镜防眩目调光信号100%，1.2V左右
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

            foreach (var f in _cx1EEmcDut1.GetType().GetFields())
            {
                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                if (!attrs.Any())
                    continue;
                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.Curr)
                    _currBinding.Add(obj.Name, (double)f.GetValue(_cx1EEmcDut1));
            }

            var bindingList = new BindingList<DictionaryItem<string, double>>();
            foreach (var kvp in _currBinding)
                bindingList.Add(new DictionaryItem<string, double>(kvp.Key, kvp.Value));
            dgvCurrent.DataSource = bindingList;
            dgvCurrent.Refresh();
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

            foreach (var f in _cx1EEmcDut1.GetType().GetFields())
            {
                var attrs = f.GetCustomAttributes(typeof(Cx1EEmc.EmcAttribute), false);

                if (!attrs.Any())
                    continue;
                var obj = attrs[0] as Cx1EEmc.EmcAttribute;

                if (obj != null && obj.EmcType == Cx1EEmc.EmcType.State)
                    _stateBinding.Add(obj.Name, (string)f.GetValue(_cx1EEmcDut1));
            }

            var bindingList = new BindingList<DictionaryItem<string, string>>();
            foreach (var kvp in _stateBinding)
                bindingList.Add(new DictionaryItem<string, string>(kvp.Key, kvp.Value));
            dgvState.DataSource = bindingList;
            dgvState.Refresh();
        }

        private void InitRelayButton()
        {
            foreach (var btn in groupRelayBtns.Controls.Cast<object>().Where(control => control.GetType() == typeof(UIButton)).OfType<UIButton>())
            {
                btn.Style = UIStyle.Gray;
                btn.Click += btnRelay_Click;
            }
        }

        private void InitThread()
        {
            //CanBackgroundWorker = new BackgroundWorker();
            //CanBackgroundWorker.DoWork += CanBackgroundWorker_DoWork;
            //CanBackgroundWorker.WorkerReportsProgress = true;
            //CanBackgroundWorker.WorkerSupportsCancellation = true;
            //CanBackgroundWorker.RunWorkerAsync();

            IoBackgroundWorker = new BackgroundWorker();
            IoBackgroundWorker.DoWork += IoBackgroundWorker_DoWork;
            IoBackgroundWorker.WorkerReportsProgress = true;
            IoBackgroundWorker.WorkerSupportsCancellation = true;
            IoBackgroundWorker.RunWorkerAsync();
        }

        private void swStartCan_ActiveChanged(object sender, EventArgs e)
        {
            if (swStartCan.Active)
            {
                _cx1EEmcDut1.StartCan();
                _cx1EEmcDut1.StartDiagnosis();
            }
            else
            {
                _cx1EEmcDut1.StopCan();
            }
        }

        #region 2F帧事件

        private void txtDcMirrorX_ValueChanged(object sender, int value)
        {
            _cx1EEmcDut1.List2FBytes[0x4003][0] = (byte)txtDcMirrorX.Value;
            this.ShowInfoTip(string.Format("后视镜展开/折叠占空比：{0}%", value));
        }

        private void txtDcMirrorY_ValueChanged(object sender, int value)
        {
            _cx1EEmcDut1.List2FBytes[0x4002][0] = (byte)txtDcMirrorY.Value;
            this.ShowInfoTip(string.Format("后视镜上/下占空比：{0}%", value));
        }

        private void txtDcWindow_ValueChanged(object sender, int value)
        {
            _cx1EEmcDut1.List2FBytes[0x400E][0] = (byte)txtDcWindow.Value;
            this.ShowInfoTip(string.Format("车窗电机占空比：{0}%", value));
        }

        private void txtDcChildrenLock_ValueChanged(object sender, int value)
        {
            _cx1EEmcDut1.List2FBytes[0x4010][0] = (byte)txtDcChildrenLock.Value;
            this.ShowInfoTip(string.Format("儿童锁电机占空比：{0}%", value));
        }

        private void txtDcElectricRelese_ValueChanged(object sender, int value)
        {
            _cx1EEmcDut1.List2FBytes[0x4051][0] = (byte)txtDcElectricRelese.Value;
            this.ShowInfoTip(string.Format("电释放电机占空比：{0}%", value));
        }

        private void txtDcLock2_ValueChanged(object sender, int value)
        {
            _cx1EEmcDut1.List2FBytes[0x4053][0] = (byte)txtDcLock2.Value;
            this.ShowInfoTip(string.Format("吸合锁电机占空比：{0}%", value));
        }

        private void txtDcHandle_ValueChanged(object sender, int value)
        {
            _cx1EEmcDut1.List2FBytes[0x4052][0] = (byte)txtDcHandle.Value;
            this.ShowInfoTip(string.Format("隐藏门把手电机占空比：{0}%", value));
        }

        private void txtDcPedal_ValueChanged(object sender, int value)
        {
            _cx1EEmcDut1.List2FBytes[0x4055][0] = (byte)txtDcPedal.Value;
            this.ShowInfoTip(string.Format("电动踏板电机占空比：{0}%", value));
        }

        private void txtDcDoor_ValueChanged(object sender, int value)
        {
            _cx1EEmcDut1.List2FBytes[0x40AE][0] = (byte)txtDcDoor.Value;
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
            _cx1EEmcDut1.MirrTintgCmd((byte)txtMirrTintgCmd.Value);
            this.ShowInfoTip(string.Format("调光信号：{0}%", value));
        }

        private void swMirrDefrstDrvrCmd_ActiveChanged(object sender, EventArgs e)
        {
            _cx1EEmcDut1.MirrDefrstAtDrvrCmd(swMirrDefrstDrvrCmd.Active);
        }

        private void swActvnOfIndcrOut_ActiveChanged(object sender, EventArgs e)
        {
            _cx1EEmcDut1.ActvnOfIndcrIndcrOut(swActvnOfIndcrOut.Active);
        }

        private void swActvnOfPudLi_ActiveChanged(object sender, EventArgs e)
        {
            _cx1EEmcDut1.ActvnOfPudLi(swActvnOfPudLi.Active);
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
            _cx1EEmcDut1.ActvnOfHndlDoorLi1HndlDoorLiDrvr((byte)cmbHndlDoorLiDrvr.SelectedIndex);
        }

        private void cmbDrvrMirrCmd5_SelectedIndexChanged(object sender, EventArgs e)
        {
            _cx1EEmcDut1.DrvrMirrCmd((byte)cmbDrvrMirrCmd5.SelectedIndex);
        }

        private void cmbDrvrAdjCmd_SelectedIndexChanged(object sender, EventArgs e)
        {
            _cx1EEmcDut1.DrvrAdjCmd((byte)cmbDrvrAdjCmd.SelectedIndex);
        }

        private void rbShortDropWin_CheckedChanged(object sender, EventArgs e)
        {
            if (rbShortDropWinIdle.Checked)
                _cx1EEmcDut1.ShortDropWin(0);
            else if (rbShortDropWinClose.Checked)
                _cx1EEmcDut1.ShortDropWin(1);
            else if (rbShortDropWinOpen.Checked)
                _cx1EEmcDut1.ShortDropWin(2);
        }

        private void rbChdLockReLeCtrlHmiReq_CheckedChanged(object sender, EventArgs e)
        {
            if (rbChdLockReLeCtrlHmiReqIdle.Checked)
            {
                _cx1EEmcDut1.ChdLockReLeCtrlHmiReq(0);
            }
            else if (rbChdLockReLeCtrlHmiReqLock.Checked)
            {
                _cx1EEmcDut1.ChdLockReLeCtrlHmiReq(2);
            }
            else if (rbChdLockReLeCtrlHmiReqUnlock.Checked)
            {
                _cx1EEmcDut1.ChdLockReLeCtrlHmiReq(1);
            }
        }

        private void rbDoor_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDoorIdle.Checked)
                _cx1EEmcDut1.Door(0);
            else if (rbDoorOpen.Checked)
                _cx1EEmcDut1.Door(1);
            else if (rbDoorClose.Checked)
                _cx1EEmcDut1.Door(2);
        }

        private void rbPedal_CheckedChanged(object sender, EventArgs e)
        {
            //rbPedalIdle.CheckedChanged += rbPedal_CheckedChanged;
            //rbPedalOpen.CheckedChanged += rbPedal_CheckedChanged;
            //rbPedalClose.CheckedChanged += rbPedal_CheckedChanged;

            if (rbPedalIdle.Checked)
                _cx1EEmcDut1.Pendal(0);
            else if (rbPedalOpen.Checked)
                _cx1EEmcDut1.Pendal(1);
            else if (rbPedalClose.Checked)
                _cx1EEmcDut1.Pendal(2);
        }

        private void rbDoorDrvrHndlCmd_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDoorDrvrHndlCmdIdle.Checked)
                _cx1EEmcDut1.DoorDrvrHndlCmd(0);
            else if (rbDoorDrvrHndlCmdClose.Checked)
                _cx1EEmcDut1.DoorDrvrHndlCmd(2);
            else if (rbDoorDrvrHndlCmdOpen.Checked)
                _cx1EEmcDut1.DoorDrvrHndlCmd(1);
        }

        #endregion

        #region IO控制

        private void IoBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var worker = (BackgroundWorker)sender;

            // Keep acquiring until we error or cancel
            while (!worker.CancellationPending)
            {
                try
                {
                    var tasks = new List<Task>();
                    foreach (var t in _listControllerWith56Pins.Keys.Select(key1 => new Task(() =>
                    {
                        _listControllerWith56Pins[key1].SetOutputs();
                        Thread.Sleep(15);
                        _listControllerWith56Pins[key1].GatheringFrequency = 8;
                        _listControllerWith56Pins[key1].GetCurrentVoltageDi();
                        Thread.Sleep(15);
                    })))
                    {
                        t.Start();

                        tasks.Add(t);
                    }
                    Task.WaitAll(tasks.ToArray());

                    Thread.Sleep(10);

                    ShowVolts();
                }
                catch (Exception exception)
                {
                    // If we have been stopped, ignore the exception since
                    // it is likely just an error indicating the acquisition has
                    // been stopped
                    if (!worker.CancellationPending)
                    {
                        e.Result = exception;
                    }
                    return;
                }
            }
        }

        private void ShowVolts()
        {
            UpdateVoltTxtValue(txtPowSupplyConDDSswitchVolt, "DDS开关供电");
            UpdateVoltTxtValue(txtHndlDoorLiDrvrVolt, "门外把手灯");
            UpdateVoltTxtValue(txtFronHandleLightVolt, "门内把手灯背光");
            UpdateVoltTxtValue(txtLockLightVolt, "锁开关指示灯");
            UpdateVoltTxtValue(txtWindowLightVolt, "本地车窗开关背光");
            UpdateVoltTxtValue(txtowSupplyConPendalHallVolt, "电机霍尔供电-电动踏板");
            UpdateVoltTxtValue(txtPowSupplyConHandleHallVolt, "电机霍尔供电-隐藏门把手");
            UpdateVoltTxtValue(txtPowSupplyConDoorHallVolt, "电机霍尔供电-电动门电机");
            UpdateVoltTxtValue(txtMemoryVolt, "记忆开关指示灯");
            UpdateVoltTxtValue(txtPocketLightVolt, "门口袋灯");
            UpdateVoltTxtValue(txtBsdVolt, "BSD开关供电");
            UpdateVoltTxtValue(txtActvnOfPudLiVolt, "后视镜照地灯");
            UpdateVoltTxtValue(txtStepLightVolt, "踏步灯/照地灯");
            UpdateVoltTxtValue(txtActvnOfIndcrOutVolt, "后视镜转向灯");
            UpdateVoltTxtValue(txtMirrorDefrstVolt, "后视镜加热输出");
            UpdateVoltTxtValue(txtPowSupplyCmdPosVolt, "后视镜位置传感器(电位计供电)");
            UpdateVoltTxtValue(txtMirrorTintgVolt, "外后视镜防眩目");

        }

        private delegate void UpdateVoltTxtValueDelegate(UITextBox control, string voltName);
        private void UpdateVoltTxtValue(UITextBox control, string voltName)
        {
            var updateTxtVoltValueDelegate = new UpdateVoltTxtValueDelegate(UpdateVoltTxtValue);

            if (control.InvokeRequired)
            {
                Invoke(updateTxtVoltValueDelegate, control, voltName);
            }
            else
            {
                if (string.IsNullOrEmpty(voltName) || !_listVolts.ContainsKey(voltName))
                    return;
                var binding = _listVolts[voltName];
                var controllerName = binding.GetStrsSplitByValue(".Field.")[0];
                var volt = binding.GetStrsSplitByValue(".Field.")[1];
                if (string.IsNullOrEmpty(controllerName) || !_listControllerWith56Pins.ContainsKey(controllerName) ||
                    string.IsNullOrEmpty(volt)) return;
                var field = _listControllerWith56Pins[controllerName].GetType().GetField(volt);
                if (field == null) return;
                var fieldValue = field.GetValue(_listControllerWith56Pins[controllerName]);
                if (fieldValue != null && !string.IsNullOrEmpty(fieldValue.ToString()))
                    control.DoubleValue = Math.Round(double.Parse(fieldValue.ToString()), 2,
                            MidpointRounding.AwayFromZero);
            }
        }

        private void btnRelay_Click(object sender, EventArgs e)
        {
            var btn = sender as UIButton;
            if (btn == null)
                return;
            bool isOn;

            if (btn.Style == UIStyle.Gray)
            {
                btn.Style = Style;
                isOn = true;
            }
            else
            {
                btn.Style = UIStyle.Gray;
                isOn = false;
            }

            if (!_listRlms.ContainsKey(btn.Text))
                return;
            var binding = _listRlms[btn.Text];
            var controllerName = binding.GetStrsSplitByValue(".Field.")[0];
            var relay = binding.GetStrsSplitByValue(".Field.")[1];
            if (!string.IsNullOrEmpty(controllerName) &&
                _listControllerWith56Pins.ContainsKey(controllerName) &&
                !string.IsNullOrEmpty(relay))
            {
                var field = _listControllerWith56Pins[controllerName].GetType().GetField(relay);
                if (field != null)
                {
                    field.SetValue(_listControllerWith56Pins[controllerName], isOn);
                    this.ShowInfoTip(string.Format("{0}: {1}", btn.Text, isOn ? "ON" : "OFF"));
                }
            }
        }

        #endregion
    }
}
