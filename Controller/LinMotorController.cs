using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("LIN-Product,LIN马达")]
    public sealed class LinMotorController : ControllerBase
    {
        public LinBus Lin;

        public LinMotorController(string name)
            : base(name)
        {
            _matrixValDefinitions.Add(_ldhlConfPos, new MatrixValDefinition(0, 10, 0));
            _matrixValDefinitions.Add(_dhlDirConf, new MatrixValDefinition(10, 2, 0));
            _matrixValDefinitions.Add(_ldhlRefCmd, new MatrixValDefinition(15, 1, 0));
            _matrixValDefinitions.Add(_laflConfPos, new MatrixValDefinition(16, 11, 0));
            _matrixValDefinitions.Add(_aflFeqCmd, new MatrixValDefinition(27, 2, 0));
            _matrixValDefinitions.Add(_aflFreqTab, new MatrixValDefinition(29, 2, 0));
            _matrixValDefinitions.Add(_laflRefCmd, new MatrixValDefinition(31, 1, 0));
            _matrixValDefinitions.Add(_raflConfPos, new MatrixValDefinition(32, 11, 0));
            _matrixValDefinitions.Add(_raflRefCmd, new MatrixValDefinition(47, 1, 0));
            _matrixValDefinitions.Add(_rdhlConfPos, new MatrixValDefinition(48, 10, 0));
            _matrixValDefinitions.Add(_rdhlRefCmd, new MatrixValDefinition(63, 1, 0));

            if (_mainWorkThread != null)
            {
                _mainWorkThread.Abort();
                _mainWorkThread.Join();
            }
            _mainWorkThread = new Thread(MainWork);
            _mainWorkThread.Start();
        }

        ~LinMotorController()
        {
            Dispose();
        }

        [Description("唤醒")]
        public void LampAwake()
        {
            _isAwake = true;
        }

        [Description("休眠")]
        public void LampSleep()
        {
            _isAwake = false;
        }

        [Description("初始化")]
        public void MotorInitStep()
        {
            if (Lin == null)
                return;

            MotorInit1();
            Thread.Sleep(1800);

            //MotorInit2();
            //Thread.Sleep(1500);

            //MotorInit3();
            //Thread.Sleep(2500);
        }

        private void MotorInit1()
        {
            _matrixValDefinitions[_ldhlRefCmd].Value = 0x01;
            _matrixValDefinitions[_rdhlRefCmd].Value = 0x01;
            _matrixValDefinitions[_laflRefCmd].Value = 0x01;
            _matrixValDefinitions[_raflRefCmd].Value = 0x01;
            _matrixValDefinitions[_aflFeqCmd].Value = 0x00;

            //foreach (var key in _matrixValDefinitions.Keys)
            //    _motorConf.UpdateData(_matrixValDefinitions[key]);

            //_motorConf.MatrixData[1] = 0xF1;

            //Lin.SendMasterLin(_motorConf.MasterLinId, _motorConf.MatrixData);
        }

        private void MotorInit2()
        {
        }

        private void MotorInit3()
        {
        }

        private void MainWork()
        {
            //foreach (var key in _matrixValDefinitions.Keys)
            //{
            //    _matrixValDefinitions[key].Value = 0x01;
            //}

            //_matrixValDefinitions[DISucsLghtEnb_l].Value = 0x00;
            //_matrixValDefinitions[BntClsLghtReq_l].Value = 0x01;

            while (_mainWorkThread.IsAlive)
            {
                if (!_mainWorkThread.IsAlive)
                    break;

                Thread.Sleep(25);

                if (Lin == null)
                    continue;

                if (!_isAwake)
                    continue;

                Lin.SendMasterLin(0x34, new byte[] {0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A, 0x5A});

                //foreach (var key in _matrixValDefinitions.Keys)
                //    _motorConf.UpdateData(_matrixValDefinitions[key]);
                //Lin.SendMasterLin(
                //    _motorConf.MasterLinId, _motorConf.MatrixData);
                //for (var i = 23; i <= 26; i++)
                //{


                //    byte[] echo;
                //    Lin.SendSlaveLin((byte)i, out echo);

                //    if (echo != null)
                //    {
                //        var str = string.Empty;
                //        foreach (var t in echo)
                //        {
                //            str += ValueHelper.GetHextStrWithOx(t) + " ";
                //        }

                //        Console.WriteLine("EP33" + str);
                //    }
                //}
            }
        }

        private readonly Thread _mainWorkThread;
        private bool _isAwake;
        private readonly LinCommunicationMatrix.IntelMatrix _motorConf =
            new LinCommunicationMatrix.IntelMatrix(0x30, 8);

        private readonly Dictionary<string, MatrixValDefinition> _matrixValDefinitions =
            new Dictionary<string, MatrixValDefinition>();

        private string _ldhlConfPos = "LDHLConfPos";
        private string _dhlDirConf = "DHLDirConf";
        private string _ldhlRefCmd = "LDHLRefCmd";
        private string _laflConfPos = "LAFLConfPos";
        private string _aflFeqCmd = "AFLFeqCmd";
        private string _aflFreqTab = "AFLFreqTab";
        private string _laflRefCmd = "LAFLRefCmd";
        private string _raflConfPos = "RAFLConfPos";
        private string _raflRefCmd = "RAFLRefCmd";
        private string _rdhlConfPos = "RDHLConfPos";
        private string _rdhlRefCmd = "RDHLRefCmd";

    }
}
