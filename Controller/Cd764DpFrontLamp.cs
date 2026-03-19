using CommonUtility.BusLoader;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    [Description("LIN-Product,CD764低配前灯-波特率10.417")]
    public sealed class Cd764DpFrontLamp : ControllerBase
    {
        public LinBus Lin;

        private readonly Thread _mainWorkThread;
        private bool _isAwake;
        private readonly LinCommunicationMatrix.MotorolaMatrix _motorolaMatrix =
            new LinCommunicationMatrix.MotorolaMatrix(0x01, 8);
        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
           new Dictionary<string, MatrixValDefinition>();

        public Cd764DpFrontLamp(string name)
            : base(name)
        {
            _matrixValDefinitions.Add(IgnitionStatus, new MatrixValDefinition(0, 4, 0));
            _matrixValDefinitions.Add(WfSuperstate, new MatrixValDefinition(4, 3, 0));
            _matrixValDefinitions.Add(RemoteStartStatus, new MatrixValDefinition(7, 1, 0));
            _matrixValDefinitions.Add(WfSubstate, new MatrixValDefinition(8, 3, 0));
            _matrixValDefinitions.Add(TurnSignalLightCmd, new MatrixValDefinition(11, 3, 0));
            _matrixValDefinitions.Add(HighBeamStatus, new MatrixValDefinition(14, 1, 0));
            _matrixValDefinitions.Add(FtpStatus, new MatrixValDefinition(15, 1, 0));
            _matrixValDefinitions.Add(HeadlightStatus, new MatrixValDefinition(16, 3, 0));
            _matrixValDefinitions.Add(DayNightStatus, new MatrixValDefinition(19, 2, 0));
            _matrixValDefinitions.Add(DrlRqst, new MatrixValDefinition(22, 1, 0));
            _matrixValDefinitions.Add(AutolampRqst, new MatrixValDefinition(23, 1, 0));
            _matrixValDefinitions.Add(VehSpeed, new MatrixValDefinition(24, 8, 0));
            _matrixValDefinitions.Add(PositionParkRightCmd, new MatrixValDefinition(32, 1, 0));
            _matrixValDefinitions.Add(PositionParkLeftCmd, new MatrixValDefinition(33, 1, 0));
            _matrixValDefinitions.Add(HeadlampLowBeamsOut, new MatrixValDefinition(34, 2, 0));
            _matrixValDefinitions.Add(VehicleLdmCfg, new MatrixValDefinition(36, 4, 0));
            _matrixValDefinitions.Add(LdmHbRampingSpeed, new MatrixValDefinition(40, 2, 0));
            _matrixValDefinitions.Add(CorneringCmd, new MatrixValDefinition(42, 2, 0));
            _matrixValDefinitions.Add(DelayAccy, new MatrixValDefinition(45, 1, 0));
            _matrixValDefinitions.Add(MultimediaSystem, new MatrixValDefinition(45, 1, 0));

            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();
        }

        ~Cd764DpFrontLamp()
        {
            Dispose();
        }


        [Description("模块唤醒")]
        public void ModuleAwake()
        {
            _isAwake = true;
        }

        [Description("模块休眠")]
        public void ModuleSleep()
        {
            _isAwake = false;
        }

        [Description("远光打开")]
        public void HightBeamOn()
        {
            _matrixValDefinitions[HighBeamStatus].Value = 1;
            _cmd = new byte[] { 0x34, 0x88, 0xab, 0x00, 0x1f, 0x90, 0x80, 0x3e };
            //_cmd = new byte[] { 0x31, 0x08, 0x34, 0x00, 0x14, 0x90, 0x80, 0x3e };
            //_cmd = new byte[] { 0x34, 0xc8, 0xAC, 0x00, 0x1c, 0x90, 0x80, 0x3e };
        }

        [Description("远光关闭")]
        public void HighBeamOff()
        {
            _matrixValDefinitions[HighBeamStatus].Value = 0;
            _cmd = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        [Description("动画开")]
        public async void AnimationOn()
        {
            await Task.Run(() =>
            {
                _cmd = new byte[] { 0x00, 0x08, 0x20, 0x00, 0x00, 0x90, 0x00, 0x00 };
                Thread.Sleep(2555);
                _cmd = new byte[] { 0x10, 0x0D, 0x20, 0x00, 0x00, 0x90, 0x00, 0x00 };
            });
        }

        [Description("动画关")]
        public void AnimationOff()
        {
            _cmd = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        }

        private byte[] _cmd = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

        private void MainWork()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(25);

                if (Lin == null)
                    continue;

                if (!_isAwake)
                    continue;

                foreach (var key in _matrixValDefinitions.Keys)
                    _motorolaMatrix.UpdateData(_matrixValDefinitions[key]);

                //Lin.SendMasterLin(
                //    _motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);

                Lin.SendMasterLin(0x01, _cmd);
            }
        }

        private const string IgnitionStatus = "Ignition_Status";
        private const string WfSuperstate = "WFSuperstate";
        private const string RemoteStartStatus = "Remote_Start_Status";
        private const string WfSubstate = "WFSubstate";
        private const string TurnSignalLightCmd = "Turn_Signal_Light_Cmd";
        private const string HighBeamStatus = "High_Beam_Status";
        private const string FtpStatus = "FTP_Status";
        private const string HeadlightStatus = "Headlight_Status";
        private const string DayNightStatus = "Day_Night_Status";
        private const string DrlRqst = "DRL_Rqst";
        private const string VehSpeed = "Veh_Speed";
        private const string PositionParkRightCmd = "Position_Park_Right_Cmd";
        private const string PositionParkLeftCmd = "Position_Park_Left_Cmd";
        private const string HeadlampLowBeamsOut = "Headlamp_Low_Beams_Out";
        private const string VehicleLdmCfg = "vehicle_ldm_cfg";
        private const string LdmHbRampingSpeed = "LDM_HB_RampingSpeed";
        private const string CorneringCmd = "Cornering_Cmd";
        private const string DelayAccy = "Delay_Accy";
        private const string MultimediaSystem = "Multimedia_System";
        private const string AutolampRqst = "Autolamp_Rqst";
    }
}
