using CommonUtility;
using CommonUtility.BusLoader;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    [Description("LIN-Product,充电门指示灯")]
    public sealed class ChargePortDoorLight : ControllerBase
    {
        public LinBus Lin;

        [Description("R,读取软件版本号Byte2的高位")]
        public string VerByte2 = string.Empty;

        public ChargePortDoorLight(string name) : base(name)
        {
            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();
        }

        ~ChargePortDoorLight()
        {
            Dispose();
        }

        private readonly Thread _mainWorkThread;
        private bool _isAwake;
        private readonly object _lock = new object();
        private byte ChrgStsIndColrReq;
        private byte ChrgStsIndDCReq = 0x40;
        private byte ChrgStsIndFlshRateReq = 0x21;
        private byte ChrgStsIndStyReq;
        private byte ChrgStsIndPtrnChgReqd;
        private byte ChrgPortDrAmbtLgtReqdOn;

        private void MainWork()
        {
            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(20);

                if (Lin == null)
                    continue;

                if (!_isAwake)
                    continue;

                lock (_lock)
                {
                    var _motorolaMatrix = new LinCommunicationMatrix.IntelMatrix(0x20, 8);
                    _motorolaMatrix.UpdateData(new MatrixValDefinition(0, 2, ChrgStsIndColrReq));
                    _motorolaMatrix.UpdateData(new MatrixValDefinition(2, 2, ChrgStsIndStyReq));
                    _motorolaMatrix.UpdateData(new MatrixValDefinition(8, 8, ChrgStsIndFlshRateReq));
                    _motorolaMatrix.UpdateData(new MatrixValDefinition(16, 7, ChrgStsIndDCReq));
                    _motorolaMatrix.UpdateData(new MatrixValDefinition(40, 1, ChrgStsIndPtrnChgReqd));
                    _motorolaMatrix.UpdateData(new MatrixValDefinition(41, 1, ChrgPortDrAmbtLgtReqdOn));
                    Lin.SendMasterLin(_motorolaMatrix.MasterLinId, _motorolaMatrix.MatrixData);
                }
            }
        }

        [Description("打开LIN")]
        public void StartLin()
        {
            _isAwake = true;
        }

        [Description("关闭LIN")]
        public void StopLin()
        {
            _isAwake = false;
        }

        [Description("读取软件版本号Byte2的高位")]
        public void ReadVerByte2Hi()
        {
            VerByte2 = string.Empty;

            if (Lin is null)
                return;
            lock (_lock)
            {
                Lin.SendMasterLinAndRecvSingleSlaveLin(0x3C, 0x3D, new byte[] { 0x68, 0x02, 0xBA, 0x0F, 0xFF, 0xFF, 0xFF, 0xFF }, out byte[] echo);
                if (echo != null &&
                    echo.Length == 8 &&
                    echo[0] == 0x68 &&
                    echo[1] == 0x10 &&
                    echo[2] == 0x0D &&
                    echo[3] == 0xFA &&
                    echo[4] == 0x0F)
                {
                    VerByte2 = echo[7].GetByteHighOrder().ToString();
                }
            }
        }

        [Description("绿色G")]
        public void SetColorGreen()
        {
            ChrgStsIndColrReq = 0x03;
        }

        [Description("蓝色B")]
        public void SetColorBlue()
        {
            ChrgStsIndColrReq = 0x02;
        }

        [Description("红色R")]
        public void SetColorRed()
        {
            ChrgStsIndColrReq = 0x01;
        }

        [Description("无色NONE")]
        public void SetColorNone()
        {
            ChrgStsIndColrReq = 0x00;
        }

        [Description("设置闪烁周期")]
        public void SetDutyCycle(int duty)
        {
            if (duty < 0 || duty > 100)
                return;

            ChrgStsIndDCReq = (byte)(duty / 0.7874);
        }

        [Description("设置闪烁频率")]
        public void SetFlashRate(int flashRate)
        {
            if (flashRate < 0 || flashRate > 25500)
                return;

            ChrgStsIndFlshRateReq = (byte)(flashRate / 0.01);
        }

        [Description("设置闪烁样式-不亮")]
        public void SetStyleOff()
        {
            ChrgStsIndStyReq = 0x00;
        }

        [Description("设置闪烁样式-常亮")]
        public void SetStyleSolid()
        {
            ChrgStsIndStyReq = 0x01;
        }

        [Description("设置闪烁样式-闪烁")]
        public void SetStyleBlink()
        {
            ChrgStsIndStyReq = 0x02;
        }

        [Description("设置闪烁样式-呼吸")]
        public void SetStylePulse()
        {
            ChrgStsIndStyReq = 0x03;
        }

        [Description("小照明灯ON")]
        public void AmbientLightOn()
        {
            ChrgPortDrAmbtLgtReqdOn = 0x01;
        }

        [Description("小照明灯OFF")]
        public void AmbientLightOff()
        {
            ChrgPortDrAmbtLgtReqdOn = 0x00;
        }
    }
}
