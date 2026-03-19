using CommonUtility;
using CommonUtility.BusLoader;
using MiniExcelLibs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace Controller
{
    [Description("CAN-Product,427C-SUV前组合灯")]
    public sealed class Audi427CSuvFrontLamp : ControllerBase
    {
        [Description("R,当前模式")]
        public Mode NowMode = Mode.FrontLamp;

        public CanBus CanFd;

        public Audi427CSuvFrontLamp(string name) : base(name)
        {
            Addsignal();
            AddSidePixelSignal();
            AddAdbPixelSignal();
            MainWork();
            SchedulerAsync();
        }

        ~Audi427CSuvFrontLamp() => Dispose();

        private object _lockCanSend = new object();
        private bool _isSleep = true;
        private CanCommunicationMatrix.MotorolaMatrix _ledRequestMsg0x2B0 = new CanCommunicationMatrix.MotorolaMatrix(0x2B0, 32);
        private CanCommunicationMatrix.MotorolaMatrix _ledRequestMsg0x294 = new CanCommunicationMatrix.MotorolaMatrix(0x294, 48);
        private CanCommunicationMatrix.MotorolaMatrix _ledRequestMsg0x295 = new CanCommunicationMatrix.MotorolaMatrix(0x295, 48);
        private CanCommunicationMatrix.MotorolaMatrix _ledRequestMsg0x2A6 = new CanCommunicationMatrix.MotorolaMatrix(0x2A6, 64);

        private void Addsignal()
        {
            _baseLampSignal.Add("IMainBeamLghtOnReq_RHA", new MatrixValDefinition(106, 2, 0));
            _baseLampSignal.Add("IMainBeamLghtOnReq_LHA", new MatrixValDefinition(108, 2, 0));

            _baseLampSignal.Add("IDipdBeamLghtOnReq_RHA", new MatrixValDefinition(116, 2, 0));
            _baseLampSignal.Add("IDipdBeamLghtOnReq_LHA", new MatrixValDefinition(118, 2, 0));

            _baseLampSignal.Add("IFRDircnLghtOnReq_RHA", new MatrixValDefinition(122, 2, 0));
            _baseLampSignal.Add("IFLDircnLghtOnReq_LHA", new MatrixValDefinition(124, 2, 0));

            _baseLampSignal.Add("IDISucsLghtEnb", new MatrixValDefinition(59, 1, 0));

            _baseLampSignal.Add("IDayTimeRunningLghtOnReq_LHA", new MatrixValDefinition(16, 2, 0));
            _baseLampSignal.Add("IDayTimeRunningLghtOnReq_RHA", new MatrixValDefinition(104, 2, 0));

            _baseLampSignal.Add("ISideLghtOnReq_RHA", new MatrixValDefinition(52, 2, 0));
            _baseLampSignal.Add("ISideLghtOnReq_LHA", new MatrixValDefinition(54, 2, 0));

            //_baseLampSignal.Add("ISideLghtOnReq_TailLampAsmblB_RH", new MatrixValDefinition(60, 2, 2));
            //_baseLampSignal.Add("ISideLghtOnReq_TailLampAsmblB_LH", new MatrixValDefinition(62, 2, 2));
            //_baseLampSignal.Add("IBrkLghtOnReq_TailLampAsmblA_LH", new MatrixValDefinition(12, 2, 3));

            _baseLampSignal.Add("IMainBeamLghtOnReq_DLPCM", new MatrixValDefinition(110, 2, 0));
        }

        private void AddSidePixelSignal()
        {
            // L
            {
                _sidePixelSignal.Add("SideCh1L", new MatrixValDefinition(2, 6, 0));
                _sidePixelSignal.Add("SideCh2L", new MatrixValDefinition(12, 6, 0));
                _sidePixelSignal.Add("SideCh3L", new MatrixValDefinition(22, 6, 0));
                _sidePixelSignal.Add("SideCh4L", new MatrixValDefinition(16, 6, 0));
                _sidePixelSignal.Add("SideCh5L", new MatrixValDefinition(26, 6, 0));
                _sidePixelSignal.Add("SideCh6L", new MatrixValDefinition(36, 6, 0));
                _sidePixelSignal.Add("SideCh7L", new MatrixValDefinition(46, 6, 0));
                _sidePixelSignal.Add("SideCh8L", new MatrixValDefinition(40, 6, 0));
                _sidePixelSignal.Add("SideCh9L", new MatrixValDefinition(50, 6, 0));
                _sidePixelSignal.Add("SideCh10L", new MatrixValDefinition(60, 6, 0));
                _sidePixelSignal.Add("SideCh11L", new MatrixValDefinition(70, 6, 0));
                _sidePixelSignal.Add("SideCh12L", new MatrixValDefinition(64, 6, 0));
                _sidePixelSignal.Add("SideCh13L", new MatrixValDefinition(74, 6, 0));
                _sidePixelSignal.Add("SideCh14L", new MatrixValDefinition(84, 6, 0));
                _sidePixelSignal.Add("SideCh15L", new MatrixValDefinition(94, 6, 0));
                _sidePixelSignal.Add("SideCh16L", new MatrixValDefinition(88, 6, 0));
                _sidePixelSignal.Add("SideCh17L", new MatrixValDefinition(98, 6, 0));
                _sidePixelSignal.Add("SideCh18L", new MatrixValDefinition(108, 6, 0));
            }

            // R
            {
                _sidePixelSignal.Add("SideCh1R", new MatrixValDefinition(194, 6, 0));
                _sidePixelSignal.Add("SideCh2R", new MatrixValDefinition(204, 6, 0));
                _sidePixelSignal.Add("SideCh3R", new MatrixValDefinition(214, 6, 0));
                _sidePixelSignal.Add("SideCh4R", new MatrixValDefinition(208, 6, 0));
                _sidePixelSignal.Add("SideCh5R", new MatrixValDefinition(218, 6, 0));
                _sidePixelSignal.Add("SideCh6R", new MatrixValDefinition(228, 6, 0));
                _sidePixelSignal.Add("SideCh7R", new MatrixValDefinition(238, 6, 0));
                _sidePixelSignal.Add("SideCh8R", new MatrixValDefinition(232, 6, 0));
                _sidePixelSignal.Add("SideCh9R", new MatrixValDefinition(242, 6, 0));
                _sidePixelSignal.Add("SideCh10R", new MatrixValDefinition(252, 6, 0));
                _sidePixelSignal.Add("SideCh11R", new MatrixValDefinition(262, 6, 0));
                _sidePixelSignal.Add("SideCh12R", new MatrixValDefinition(256, 6, 0));
                _sidePixelSignal.Add("SideCh13R", new MatrixValDefinition(266, 6, 0));
                _sidePixelSignal.Add("SideCh14R", new MatrixValDefinition(276, 6, 0));
                _sidePixelSignal.Add("SideCh15R", new MatrixValDefinition(286, 6, 0));
                _sidePixelSignal.Add("SideCh16R", new MatrixValDefinition(280, 6, 0));
                _sidePixelSignal.Add("SideCh17R", new MatrixValDefinition(290, 6, 0));
                _sidePixelSignal.Add("SideCh18R", new MatrixValDefinition(300, 6, 0));
            }
        }

        private void AddAdbPixelSignal()
        {
            _adbPixelSignal.Add("CH1L", new MatrixValDefinition(2, 6, 0));
            _adbPixelSignal.Add("CH2L", new MatrixValDefinition(12, 6, 0));
            _adbPixelSignal.Add("CH3L", new MatrixValDefinition(22, 6, 0));
            _adbPixelSignal.Add("CH4L", new MatrixValDefinition(16, 6, 0));
            _adbPixelSignal.Add("CH5L", new MatrixValDefinition(26, 6, 0));
            _adbPixelSignal.Add("CH6L", new MatrixValDefinition(36, 6, 0));
            _adbPixelSignal.Add("CH7L", new MatrixValDefinition(46, 6, 0));
            _adbPixelSignal.Add("CH8L", new MatrixValDefinition(40, 6, 0));
            _adbPixelSignal.Add("CH9L", new MatrixValDefinition(50, 6, 0));
            _adbPixelSignal.Add("CH10L", new MatrixValDefinition(60, 6, 0));
            _adbPixelSignal.Add("CH11L", new MatrixValDefinition(70, 6, 0));
            _adbPixelSignal.Add("CH12L", new MatrixValDefinition(64, 6, 0));

            _adbPixelSignal.Add("CH1R", new MatrixValDefinition(98, 6, 0));
            _adbPixelSignal.Add("CH2R", new MatrixValDefinition(108, 6, 0));
            _adbPixelSignal.Add("CH3R", new MatrixValDefinition(118, 6, 0));
            _adbPixelSignal.Add("CH4R", new MatrixValDefinition(112, 6, 0));
            _adbPixelSignal.Add("CH5R", new MatrixValDefinition(122, 6, 0));
            _adbPixelSignal.Add("CH6R", new MatrixValDefinition(132, 6, 0));
            _adbPixelSignal.Add("CH7R", new MatrixValDefinition(142, 6, 0));
            _adbPixelSignal.Add("CH8R", new MatrixValDefinition(136, 6, 0));
            _adbPixelSignal.Add("CH9R", new MatrixValDefinition(146, 6, 0));
            _adbPixelSignal.Add("CH10R", new MatrixValDefinition(156, 6, 0));
            _adbPixelSignal.Add("CH11R", new MatrixValDefinition(166, 6, 0));
            _adbPixelSignal.Add("CH12R", new MatrixValDefinition(160, 6, 0));

            _adbPixelSignal.Add("Maxtrix-CH1R", new MatrixValDefinition(2, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH2R", new MatrixValDefinition(12, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH3R", new MatrixValDefinition(22, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH4R", new MatrixValDefinition(16, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH5R", new MatrixValDefinition(26, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH6R", new MatrixValDefinition(36, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH7R", new MatrixValDefinition(46, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH8R", new MatrixValDefinition(40, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH9R", new MatrixValDefinition(50, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH10R", new MatrixValDefinition(60, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH11R", new MatrixValDefinition(70, 6, 0));
            _adbPixelSignal.Add("Maxtrix-CH12R", new MatrixValDefinition(64, 6, 0));

            _adbPixelSignal.Add("CL-L", new MatrixValDefinition(266, 6, 0));
            _adbPixelSignal.Add("CL-R", new MatrixValDefinition(362, 6, 0));
        }

        private void UpdateSignal(Dictionary<string, MatrixValDefinition> signalLst, string name, byte value)
        {
            if (signalLst.ContainsKey(name))
                signalLst[name] = new MatrixValDefinition(signalLst[name].StartBit, signalLst[name].Len, value);
        }

        private Dictionary<string, MatrixValDefinition> _baseLampSignal = new Dictionary<string, MatrixValDefinition>();
        private Dictionary<string, MatrixValDefinition> _adbPixelSignal = new Dictionary<string, MatrixValDefinition>();
        private Dictionary<string, MatrixValDefinition> _sidePixelSignal = new Dictionary<string, MatrixValDefinition>();

        public enum Mode
        {
            FrontLamp,
            AdbMode,
            SidePixel
        }

        private void MainWork()
        {
            BaseLampCtrl50ms();
            ADBCtril40Ms();
            SideLampPixelCtrl40Ms();
            SwitchTurn400Ms();
        }

        private void BaseLampCtrl50ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (CanFd == null)
                        return;
                    if (_isSleep)
                        return;

                    if (NowMode == Mode.FrontLamp)
                    {
                        var keys = _baseLampSignal.Keys.ToList();
                        foreach (var key in keys)
                            _ledRequestMsg0x2B0.UpdateData(_baseLampSignal[key]);

                        lock (_lockCanSend)
                            CanFd.SendStandardCanFdData(_ledRequestMsg0x2B0.CanId, _ledRequestMsg0x2B0.MatrixData);
                    }
                    else if (NowMode == Mode.AdbMode)
                    {
                        lock (_lockCanSend)
                            CanFd.SendStandardCanFdData(_ledRequestMsg0x2B0.CanId, new byte[] { 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0xa0, 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    }
                    else if (NowMode == Mode.SidePixel)
                    {
                        lock (_lockCanSend)
                            CanFd.SendStandardCanFdData(_ledRequestMsg0x2B0.CanId, new byte[] { 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x2a, 0xa0, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
                    }
                },
                Interval = 50
            });
        }

        private bool _is294msg;

        private void ADBCtril40Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (CanFd == null)
                        return;
                    if (_isSleep)
                        return;

                    //if (NowMode != Mode.AdbMode)
                    //    return;

                    // Left CL
                    {
                        UpdateSignal(_adbPixelSignal, "CL-L", LeftCL >= 0x00 && LeftCL <= 0x3F ? LeftCL : (byte)0x00);
                        _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CL-L"]);
                    }

                    // Right CL
                    {
                        UpdateSignal(_adbPixelSignal, "CL-R", RightCL >= 0x00 && RightCL <= 0x3F ? RightCL : (byte)0x00);
                        _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CL-R"]);
                    }

                    if (NowMode == Mode.AdbMode)
                    {
                        // Left ADB
                        {
                            UpdateSignal(_adbPixelSignal, "CH1L", AdbCh1L >= 0x00 && AdbCh1L <= 0x3F ? AdbCh1L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH2L", AdbCh2L >= 0x00 && AdbCh2L <= 0x3F ? AdbCh2L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH3L", AdbCh3L >= 0x00 && AdbCh3L <= 0x3F ? AdbCh3L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH4L", AdbCh4L >= 0x00 && AdbCh4L <= 0x3F ? AdbCh4L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH5L", AdbCh5L >= 0x00 && AdbCh5L <= 0x3F ? AdbCh5L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH6L", AdbCh6L >= 0x00 && AdbCh6L <= 0x3F ? AdbCh6L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH7L", AdbCh7L >= 0x00 && AdbCh7L <= 0x3F ? AdbCh7L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH8L", AdbCh8L >= 0x00 && AdbCh8L <= 0x3F ? AdbCh8L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH9L", AdbCh9L >= 0x00 && AdbCh9L <= 0x3F ? AdbCh9L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH10L", AdbCh10L >= 0x00 && AdbCh10L <= 0x3F ? AdbCh10L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH11L", AdbCh11L >= 0x00 && AdbCh11L <= 0x3F ? AdbCh11L : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH12L", AdbCh12L >= 0x00 && AdbCh12L <= 0x3F ? AdbCh12L : (byte)0x00);

                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH1L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH2L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH3L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH4L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH5L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH6L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH7L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH8L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH9L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH10L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH11L"]);
                            _ledRequestMsg0x294.UpdateData(_adbPixelSignal["CH12L"]);
                        }

                        // Right ADB
                        {
                            UpdateSignal(_adbPixelSignal, "CH1R", AdbCh1R >= 0x00 && AdbCh1R <= 0x3F ? AdbCh1R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH2R", AdbCh2R >= 0x00 && AdbCh2R <= 0x3F ? AdbCh2R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH3R", AdbCh3R >= 0x00 && AdbCh3R <= 0x3F ? AdbCh3R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH4R", AdbCh4R >= 0x00 && AdbCh4R <= 0x3F ? AdbCh4R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH5R", AdbCh5R >= 0x00 && AdbCh5R <= 0x3F ? AdbCh5R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH6R", AdbCh6R >= 0x00 && AdbCh6R <= 0x3F ? AdbCh6R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH7R", AdbCh7R >= 0x00 && AdbCh7R <= 0x3F ? AdbCh7R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH8R", AdbCh8R >= 0x00 && AdbCh8R <= 0x3F ? AdbCh8R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH9R", AdbCh9R >= 0x00 && AdbCh9R <= 0x3F ? AdbCh9R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH10R", AdbCh10R >= 0x00 && AdbCh10R <= 0x3F ? AdbCh10R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH11R", AdbCh11R >= 0x00 && AdbCh11R <= 0x3F ? AdbCh11R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "CH12R", AdbCh12R >= 0x00 && AdbCh12R <= 0x3F ? AdbCh12R : (byte)0x00);

                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH1R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH2R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH3R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH4R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH5R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH6R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH7R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH8R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH9R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH10R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH11R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["CH12R"]);
                        }

                        // Right MATRIX
                        {
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH1R", MatrixCh1R >= 0x00 && MatrixCh1R <= 0x3F ? MatrixCh1R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH2R", MatrixCh2R >= 0x00 && MatrixCh2R <= 0x3F ? MatrixCh2R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH3R", MatrixCh3R >= 0x00 && MatrixCh3R <= 0x3F ? MatrixCh3R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH4R", MatrixCh4R >= 0x00 && MatrixCh4R <= 0x3F ? MatrixCh4R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH5R", MatrixCh5R >= 0x00 && MatrixCh5R <= 0x3F ? MatrixCh5R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH6R", MatrixCh6R >= 0x00 && MatrixCh6R <= 0x3F ? MatrixCh6R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH7R", MatrixCh7R >= 0x00 && MatrixCh7R <= 0x3F ? MatrixCh7R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH8R", MatrixCh8R >= 0x00 && MatrixCh8R <= 0x3F ? MatrixCh8R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH9R", MatrixCh9R >= 0x00 && MatrixCh9R <= 0x3F ? MatrixCh9R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH10R", MatrixCh10R >= 0x00 && MatrixCh10R <= 0x3F ? MatrixCh10R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH11R", MatrixCh11R >= 0x00 && MatrixCh11R <= 0x3F ? MatrixCh11R : (byte)0x00);
                            UpdateSignal(_adbPixelSignal, "Maxtrix-CH12R", MatrixCh12R >= 0x00 && MatrixCh12R <= 0x3F ? MatrixCh12R : (byte)0x00);

                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH1R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH2R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH3R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH4R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH5R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH6R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH7R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH8R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH9R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH10R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH11R"]);
                            _ledRequestMsg0x295.UpdateData(_adbPixelSignal["Maxtrix-CH12R"]);
                        }
                    }

                    lock (_lockCanSend)
                    {
                        CanFd.SendStandardCanFdData(
                            _is294msg ? _ledRequestMsg0x294.CanId : _ledRequestMsg0x295.CanId,
                            _is294msg ? _ledRequestMsg0x294.MatrixData : _ledRequestMsg0x295.MatrixData);
                        _is294msg = !_is294msg;
                    }
                },
                Interval = 40
            });
        }

        private void SideLampPixelCtrl40Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (CanFd == null)
                        return;
                    if (_isSleep)
                        return;

                    if (NowMode == Mode.SidePixel)
                    {
                        if (_isWelComeExecuted)
                        {
                            var keys1 = _welComeTrace.Keys.ToList();

                            for (var i = 0; i < keys1.Count; i++)
                            {
                                var key1 = keys1[i];
                                if (_welComeTraceIndex == i)
                                {
                                    var keys2 = _welComeTrace[key1].Keys.ToList();
                                    foreach (var key2 in keys2)
                                        lock (_lockCanSend)
                                            CanFd.SendStandardCanFdData(key2, _welComeTrace[key1][key2].ToArray());

                                    _welComeTraceIndex++;
                                    if (_welComeTraceIndex == _welComeTrace.Keys.Count)
                                        _welComeTraceIndex = 0;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 1; i <= 18; i++)
                            {
                                {
                                    var str = string.Format("SideCh{0}L", i);
                                    var field = (double)(GetType().GetField(str).GetValue(this));
                                    UpdateSignal(_sidePixelSignal, str, field >= 0 && field <= 100.8 ? (byte)(Math.Round(field / _pwmFactor, 0, MidpointRounding.AwayFromZero)) : (byte)0x00);
                                    _ledRequestMsg0x2A6.UpdateData(_sidePixelSignal[str]);
                                }

                                {
                                    var str = string.Format("SideCh{0}R", i);
                                    var field = (double)(GetType().GetField(str).GetValue(this));
                                    UpdateSignal(_sidePixelSignal, str, field >= 0 && field <= 100.8 ? (byte)(Math.Round(field / _pwmFactor, 0, MidpointRounding.AwayFromZero)) : (byte)0x00);
                                    _ledRequestMsg0x2A6.UpdateData(_sidePixelSignal[str]);
                                }
                            }

                            lock (_lockCanSend)
                                CanFd.SendStandardCanFdData(_ledRequestMsg0x2A6.CanId, _ledRequestMsg0x2A6.MatrixData);
                        }
                    }
                },
                Interval = 40
            });
        }

        private bool _isTurn400MsEnable;
        private bool _isTurnLOn;
        private bool _isTurnROn;

        private void SwitchTurn400Ms()
        {
            SetTimer(new MyTaskScheduler.TaskInfo
            {
                Action = () =>
                {
                    if (_isTurn400MsEnable)
                    {
                        if (_isTurnLOn)
                        {
                            if (_baseLampSignal["IFLDircnLghtOnReq_LHA"].Value == 0x00)
                                UpdateSignal(_baseLampSignal, "IFLDircnLghtOnReq_LHA", 0x01);
                            else
                                UpdateSignal(_baseLampSignal, "IFLDircnLghtOnReq_LHA", 0x00);
                        }

                        if (_isTurnROn)
                        {
                            if (_baseLampSignal["IFRDircnLghtOnReq_RHA"].Value == 0x00)
                                UpdateSignal(_baseLampSignal, "IFRDircnLghtOnReq_RHA", 0x01);
                            else
                                UpdateSignal(_baseLampSignal, "IFRDircnLghtOnReq_RHA", 0x00);
                        }
                    }
                    else
                    {
                        if (_isTurnLOn)
                        {
                            if (_baseLampSignal["IFLDircnLghtOnReq_LHA"].Value == 0x00)
                                UpdateSignal(_baseLampSignal, "IFLDircnLghtOnReq_LHA", 0x01);
                        }

                        if (_isTurnROn)
                        {
                            if (_baseLampSignal["IFRDircnLghtOnReq_RHA"].Value == 0x00)
                                UpdateSignal(_baseLampSignal, "IFRDircnLghtOnReq_RHA", 0x01);
                        }
                    }
                },
                Interval = 400
            });
        }

        [Description("打开CAN")]
        public void StartCan() => _isSleep = false;

        [Description("关闭CAN")]
        public void StopCan() => _isSleep = true;

        #region 前灯

        [Description("R/W,CL_L_PWM(0~63)")]
        public byte LeftCL;

        [Description("R/W,CL_R_PWM(0~63)")]
        public byte RightCL;

        [Description("前灯模式")]
        public void NormalLampMode() => NowMode = Mode.FrontLamp;

        [Description("DLP打开")]
        public void DlpOn()
        {
            UpdateSignal(_baseLampSignal, "IMainBeamLghtOnReq_DLPCM", 0x01);
        }

        [Description("DLP关闭")]
        public void DlpOff()
        {
            UpdateSignal(_baseLampSignal, "IMainBeamLghtOnReq_DLPCM", 0x00);
        }

        [Description("近光打开")]
        public void LbOn()
        {
            UpdateSignal(_baseLampSignal, "IDipdBeamLghtOnReq_RHA", 0x01);
            UpdateSignal(_baseLampSignal, "IDipdBeamLghtOnReq_LHA", 0x01);
        }

        [Description("近光关闭")]
        public void LbOff()
        {
            UpdateSignal(_baseLampSignal, "IDipdBeamLghtOnReq_RHA", 0x00);
            UpdateSignal(_baseLampSignal, "IDipdBeamLghtOnReq_LHA", 0x00);
        }

        [Description("远光打开L")]
        public void HbOnL() => UpdateSignal(_baseLampSignal, "IMainBeamLghtOnReq_LHA", 0x01);

        [Description("远光关闭L")]
        public void HbOffL() => UpdateSignal(_baseLampSignal, "IMainBeamLghtOnReq_LHA", 0x00);

        [Description("远光打开R")]
        public void HbOnR() => UpdateSignal(_baseLampSignal, "IMainBeamLghtOnReq_RHA", 0x01);

        [Description("远光关闭R")]
        public void HbOffR() => UpdateSignal(_baseLampSignal, "IMainBeamLghtOnReq_RHA", 0x00);

        [Description("左转打开")]
        public void LeftTurnOn()
        {
            UpdateSignal(_baseLampSignal, "IFLDircnLghtOnReq_LHA", 0x01);
            _isTurnLOn = true;
        }

        [Description("左转关闭")]
        public void LeftTurnOff()
        {
            UpdateSignal(_baseLampSignal, "IFLDircnLghtOnReq_LHA", 0x00);
            _isTurnLOn = false;
        }

        [Description("右转打开")]
        public void RightTurnOn()
        {
            UpdateSignal(_baseLampSignal, "IFRDircnLghtOnReq_RHA", 0x01);
            _isTurnROn = true;
        }

        [Description("右转关闭")]
        public void RightTurnOff()
        {
            UpdateSignal(_baseLampSignal, "IFRDircnLghtOnReq_RHA", 0x00);
            _isTurnROn = false;
        }

        [Description("转向灯流水使能打开")]
        public void TurnRunningEnable() => UpdateSignal(_baseLampSignal, "IDISucsLghtEnb", 0x01);

        [Description("转向灯流水使能关闭")]
        public void TurnRunningDisable() => UpdateSignal(_baseLampSignal, "IDISucsLghtEnb", 0x00);

        [Description("转向灯400ms使能打开")]
        public void SwitchTurn400MsEnable() => _isTurn400MsEnable = true;

        [Description("转向灯400ms使能关闭")]
        public void SwitchTurn400MsDisbale() => _isTurn400MsEnable = false;

        [Description("DRL打开L")]
        public void DrlOnL() => UpdateSignal(_baseLampSignal, "IDayTimeRunningLghtOnReq_LHA", 0x01);

        [Description("DRL关闭L")]
        public void DrlOffL() => UpdateSignal(_baseLampSignal, "IDayTimeRunningLghtOnReq_LHA", 0x00);

        [Description("PL打开L")]
        public void PlOnL() => UpdateSignal(_baseLampSignal, "ISideLghtOnReq_LHA", 0x01);

        [Description("PL关闭L")]
        public void PlOffL() => UpdateSignal(_baseLampSignal, "ISideLghtOnReq_LHA", 0x00);

        [Description("DRL打开R")]
        public void DrlOnR() => UpdateSignal(_baseLampSignal, "IDayTimeRunningLghtOnReq_RHA", 0x01);

        [Description("DRL关闭R")]
        public void DrlOffR() => UpdateSignal(_baseLampSignal, "IDayTimeRunningLghtOnReq_RHA", 0x00);

        [Description("PL打开R")]
        public void PlOnR() => UpdateSignal(_baseLampSignal, "ISideLghtOnReq_RHA", 0x01);

        [Description("PL关闭R")]
        public void PlOffR() => UpdateSignal(_baseLampSignal, "ISideLghtOnReq_RHA", 0x00);

        #endregion

        #region ADB

        [Description("R/W,ADB_CH1_L_PWM(0~63)")]
        public byte AdbCh1L;
        [Description("R/W,ADB_CH2_L_PWM(0~63)")]
        public byte AdbCh2L;
        [Description("R/W,ADB_CH3_L_PWM(0~63)")]
        public byte AdbCh3L;
        [Description("R/W,ADB_CH4_L_PWM(0~63)")]
        public byte AdbCh4L;
        [Description("R/W,ADB_CH5_L_PWM(0~63)")]
        public byte AdbCh5L;
        [Description("R/W,ADB_CH6_L_PWM(0~63)")]
        public byte AdbCh6L;
        [Description("R/W,ADB_CH7_L_PWM(0~63)")]
        public byte AdbCh7L;
        [Description("R/W,ADB_CH8_L_PWM(0~63)")]
        public byte AdbCh8L;
        [Description("R/W,ADB_CH9_L_PWM(0~63)")]
        public byte AdbCh9L;
        [Description("R/W,ADB_CH10_L_PWM(0~63)")]
        public byte AdbCh10L;
        [Description("R/W,ADB_CH11_L_PWM(0~63)")]
        public byte AdbCh11L;
        [Description("R/W,ADB_CH12_L_PWM(0~63)")]
        public byte AdbCh12L;

        [Description("R/W,ADB_CH1_R_PWM(0~63)")]
        public byte AdbCh1R;
        [Description("R/W,ADB_CH2_R_PWM(0~63)")]
        public byte AdbCh2R;
        [Description("R/W,ADB_CH3_R_PWM(0~63)")]
        public byte AdbCh3R;
        [Description("R/W,ADB_CH4_R_PWM(0~63)")]
        public byte AdbCh4R;
        [Description("R/W,ADB_CH5_R_PWM(0~63)")]
        public byte AdbCh5R;
        [Description("R/W,ADB_CH6_R_PWM(0~63)")]
        public byte AdbCh6R;
        [Description("R/W,ADB_CH7_R_PWM(0~63)")]
        public byte AdbCh7R;
        [Description("R/W,ADB_CH8_R_PWM(0~63)")]
        public byte AdbCh8R;
        [Description("R/W,ADB_CH9_R_PWM(0~63)")]
        public byte AdbCh9R;
        [Description("R/W,ADB_CH10_R_PWM(0~63)")]
        public byte AdbCh10R;
        [Description("R/W,ADB_CH11_R_PWM(0~63)")]
        public byte AdbCh11R;
        [Description("R/W,ADB_CH12_R_PWM(0~63)")]
        public byte AdbCh12R;

        [Description("R/W,MATRIX_CH1_R_PWM(0~63)")]
        public byte MatrixCh1R;
        [Description("R/W,MATRIX_CH2_R_PWM(0~63)")]
        public byte MatrixCh2R;
        [Description("R/W,MATRIX_CH3_R_PWM(0~63)")]
        public byte MatrixCh3R;
        [Description("R/W,MATRIX_CH4_R_PWM(0~63)")]
        public byte MatrixCh4R;
        [Description("R/W,MATRIX_CH5_R_PWM(0~63)")]
        public byte MatrixCh5R;
        [Description("R/W,MATRIX_CH6_R_PWM(0~63)")]
        public byte MatrixCh6R;
        [Description("R/W,MATRIX_CH7_R_PWM(0~63)")]
        public byte MatrixCh7R;
        [Description("R/W,MATRIX_CH8_R_PWM(0~63)")]
        public byte MatrixCh8R;
        [Description("R/W,MATRIX_CH9_R_PWM(0~63)")]
        public byte MatrixCh9R;
        [Description("R/W,MATRIX_CH10_R_PWM(0~63)")]
        public byte MatrixCh10R;
        [Description("R/W,MATRIX_CH11_R_PWM(0~63)")]
        public byte MatrixCh11R;
        [Description("R/W,MATRIX_CH12_R_PWM(0~63)")]
        public byte MatrixCh12R;

        [Description("ADB模式")]
        public void AdbMode() => NowMode = Mode.AdbMode;

        [Description("ADBLINE1~6")]
        public void AdbLine(int lineIndex)
        {
            var dic294msg = new Dictionary<int, string>
            {
                {1,"495000000000030754000000000000000000000000000000000000000000000000000000000000000000000000000000"},
                {2,"4956a1000000030754000000000000000000000000000000000000000000000000000000000000000000000000000000"},
                {3,"4956a1980000030754000000000000000000000000000000000000000000000000000000000000000000000000000000"},
                {4,"4956a1980000000014000000000000000000000000000000000000000000000000000000000000000000000000000000"},
                {5,"4956a1980000000754000000000000000000000000000000000000000000000000000000000000000000000000000000"},
                {6,"4956a19b7000030754000000000000000000000000000000000000000000000000000000000000000000000000000000"}
            };

            var dic295msg = new Dictionary<int, string>
            {
                {1,"5dc867b33e800000000000004956a19b7000000000000000000000000000000000000000000000000000000000000000"},
                {2,"5dc867b33000000b270000004956a19b7000000014000000000000000000000000000000000000000000000000000000"},
                {3,"5dc867b33000033b270000004956a19b7000000754000000000000000000000000000000000000000000000000000000"},
                {4,"5dc867b33000000b270000004956a1000000000754000000000000000000000000000000000000000000000000000000"},
                {5,"5dc867b33000033b270000004956a1980000000754000000000000000000000000000000000000000000000000000000"},
                {6,"5dc867b33e80eb3b270000004956a19b7000030754000000000000000000000000000000000000000000000000000000"}
            };

            if (lineIndex >= 1 && lineIndex <= 6)
            {
                var msg294 = dic294msg[lineIndex];
                var msg295 = dic295msg[lineIndex];

                // left adb
                {
                    var bytes294 = new List<byte>();
                    for (int i = 0; i < msg294.Length; i = i + 2)
                    {
                        var bStr = msg294.Substring(i, 2);
                        var bByte = Convert.ToByte(bStr, 16);
                        bytes294.Add(bByte);
                    }

                    var temp294 = new CanCommunicationMatrix.MotorolaMatrix(0x294, 48);
                    temp294.MatrixData = bytes294.ToArray();

                    AdbCh1L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH1L"].StartBit, _adbPixelSignal["CH1L"].Len));
                    AdbCh2L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH2L"].StartBit, _adbPixelSignal["CH2L"].Len));
                    AdbCh3L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH3L"].StartBit, _adbPixelSignal["CH3L"].Len));
                    AdbCh4L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH4L"].StartBit, _adbPixelSignal["CH4L"].Len));
                    AdbCh5L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH5L"].StartBit, _adbPixelSignal["CH5L"].Len));
                    AdbCh6L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH6L"].StartBit, _adbPixelSignal["CH6L"].Len));
                    AdbCh7L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH7L"].StartBit, _adbPixelSignal["CH7L"].Len));
                    AdbCh8L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH8L"].StartBit, _adbPixelSignal["CH8L"].Len));
                    AdbCh9L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH9L"].StartBit, _adbPixelSignal["CH9L"].Len));
                    AdbCh10L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH10L"].StartBit, _adbPixelSignal["CH10L"].Len));
                    AdbCh11L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH11L"].StartBit, _adbPixelSignal["CH11L"].Len));
                    AdbCh12L = (byte)(temp294.GetMatrixData(_adbPixelSignal["CH12L"].StartBit, _adbPixelSignal["CH12L"].Len));
                }

                // right adb/matrix
                {
                    var bytes295 = new List<byte>();
                    for (int i = 0; i < msg295.Length; i = i + 2)
                    {
                        var bStr = msg295.Substring(i, 2);
                        var bByte = Convert.ToByte(bStr, 16);
                        bytes295.Add(bByte);
                    }

                    var temp295 = new CanCommunicationMatrix.MotorolaMatrix(0x295, 48);
                    temp295.MatrixData = bytes295.ToArray();

                    AdbCh1R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH1R"].StartBit, _adbPixelSignal["CH1R"].Len));
                    AdbCh2R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH2R"].StartBit, _adbPixelSignal["CH2R"].Len));
                    AdbCh3R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH3R"].StartBit, _adbPixelSignal["CH3R"].Len));
                    AdbCh4R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH4R"].StartBit, _adbPixelSignal["CH4R"].Len));
                    AdbCh5R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH5R"].StartBit, _adbPixelSignal["CH5R"].Len));
                    AdbCh6R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH6R"].StartBit, _adbPixelSignal["CH6R"].Len));
                    AdbCh7R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH7R"].StartBit, _adbPixelSignal["CH7R"].Len));
                    AdbCh8R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH8R"].StartBit, _adbPixelSignal["CH8R"].Len));
                    AdbCh9R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH9R"].StartBit, _adbPixelSignal["CH9R"].Len));
                    AdbCh10R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH10R"].StartBit, _adbPixelSignal["CH10R"].Len));
                    AdbCh11R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH11R"].StartBit, _adbPixelSignal["CH11R"].Len));
                    AdbCh12R = (byte)(temp295.GetMatrixData(_adbPixelSignal["CH12R"].StartBit, _adbPixelSignal["CH12R"].Len));

                    MatrixCh1R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH1R"].StartBit, _adbPixelSignal["Maxtrix-CH1R"].Len));
                    MatrixCh2R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH2R"].StartBit, _adbPixelSignal["Maxtrix-CH2R"].Len));
                    MatrixCh3R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH3R"].StartBit, _adbPixelSignal["Maxtrix-CH3R"].Len));
                    MatrixCh4R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH4R"].StartBit, _adbPixelSignal["Maxtrix-CH4R"].Len));
                    MatrixCh5R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH5R"].StartBit, _adbPixelSignal["Maxtrix-CH5R"].Len));
                    MatrixCh6R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH6R"].StartBit, _adbPixelSignal["Maxtrix-CH6R"].Len));
                    MatrixCh7R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH7R"].StartBit, _adbPixelSignal["Maxtrix-CH7R"].Len));
                    MatrixCh8R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH8R"].StartBit, _adbPixelSignal["Maxtrix-CH8R"].Len));
                    MatrixCh9R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH9R"].StartBit, _adbPixelSignal["Maxtrix-CH9R"].Len));
                    MatrixCh10R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH10R"].StartBit, _adbPixelSignal["Maxtrix-CH10R"].Len));
                    MatrixCh11R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH11R"].StartBit, _adbPixelSignal["Maxtrix-CH11R"].Len));
                    MatrixCh12R = (byte)(temp295.GetMatrixData(_adbPixelSignal["Maxtrix-CH12R"].StartBit, _adbPixelSignal["Maxtrix-CH12R"].Len));
                }
            }
        }

        [Description("关闭所有Left-ADB像素")]
        public void AllLeftAdbPixelOff()
        {
            AdbCh1L = 0;
            AdbCh2L = 0;
            AdbCh3L = 0;
            AdbCh4L = 0;
            AdbCh5L = 0;
            AdbCh6L = 0;
            AdbCh7L = 0;
            AdbCh8L = 0;
            AdbCh9L = 0;
            AdbCh10L = 0;
            AdbCh11L = 0;
            AdbCh12L = 0;
        }

        [Description("关闭所有Right-ADB像素")]
        public void AllRightAdbPixelOff()
        {
            AdbCh1R = 0;
            AdbCh2R = 0;
            AdbCh3R = 0;
            AdbCh4R = 0;
            AdbCh5R = 0;
            AdbCh6R = 0;
            AdbCh7R = 0;
            AdbCh8R = 0;
            AdbCh9R = 0;
            AdbCh10R = 0;
            AdbCh11R = 0;
            AdbCh12R = 0;
        }

        [Description("关闭所有Right-MATRIX像素")]
        public void AllRightMatrixPixelOff()
        {
            MatrixCh1R = 0;
            MatrixCh2R = 0;
            MatrixCh3R = 0;
            MatrixCh4R = 0;
            MatrixCh5R = 0;
            MatrixCh6R = 0;
            MatrixCh7R = 0;
            MatrixCh8R = 0;
            MatrixCh9R = 0;
            MatrixCh10R = 0;
            MatrixCh11R = 0;
            MatrixCh12R = 0;
        }

        #endregion

        #region 动画

        [Description("像素模式")]
        public void SidePixelMode() => NowMode = Mode.SidePixel;

        private Dictionary<int, Dictionary<uint, List<byte>>> _welComeTrace = new Dictionary<int, Dictionary<uint, List<byte>>>();
        private bool _isWelComeExecuted;
        private int _welComeTraceIndex;

        [Description("执行WelCome动画")]
        public void ExecuteWelCome()
        {
            var filePath = Directory.GetCurrentDirectory() + @"\ControllerConfig\AUDI427动画指令.csv";

            _welComeTrace.Clear();

            if (File.Exists(filePath))
            {
                try
                {
                    var rows = MiniExcel.Query(filePath, excelType: ExcelType.CSV).ToList();
                    var groupIndex = 0;
                    if ((rows.Count - 1) % 11 == 0)
                    {
                        for (var i = 1; i < rows.Count - 1; i += 11)
                        {
                            _welComeTrace.Add(groupIndex, new Dictionary<uint, List<byte>>());

                            for (int j = i; j < i + 11; j++)
                            {
                                var canIdStr = rows[j].A.ToString();
                                var dataHexStr = rows[j].C.ToString().Replace(" ", "");

                                var canId = Convert.ToUInt32(canIdStr, 16);

                                if (!_welComeTrace[groupIndex].ContainsKey(canId))
                                {
                                    _welComeTrace[groupIndex].Add(canId, new List<byte>());

                                    for (int k = 0; k < dataHexStr.Length; k += 2)
                                    {
                                        var b = Convert.ToByte(dataHexStr.Substring(k, 2), 16);
                                        _welComeTrace[groupIndex][canId].Add(b);
                                    }
                                }
                            }

                            groupIndex++;
                        }
                    }
                }
                catch (Exception)
                {
                    _welComeTrace.Clear();
                }
            }

            _welComeTraceIndex = 0;
            _isWelComeExecuted = true;
        }

        [Description("停止WelCome动画")]
        public void AbortWelCome() => _isWelComeExecuted = false;

        private double _pwmFactor = 1.6d;
        private double _pwmMin = 0d;
        private double _pwmMax = 100.8d;

        [Description("R/W,SideCh1L")]
        public double SideCh1L = 0d;
        [Description("R/W,SideCh2L")]
        public double SideCh2L = 0d;
        [Description("R/W,SideCh3L")]
        public double SideCh3L = 0d;
        [Description("R/W,SideCh4L")]
        public double SideCh4L = 0d;
        [Description("R/W,SideCh5L")]
        public double SideCh5L = 0d;
        [Description("R/W,SideCh6L")]
        public double SideCh6L = 0d;
        [Description("R/W,SideCh7L")]
        public double SideCh7L = 0d;
        [Description("R/W,SideCh8L")]
        public double SideCh8L = 0d;
        [Description("R/W,SideCh9L")]
        public double SideCh9L = 0d;
        [Description("R/W,SideCh10L")]
        public double SideCh10L = 0d;
        [Description("R/W,SideCh11L")]
        public double SideCh11L = 0d;
        [Description("R/W,SideCh12L")]
        public double SideCh12L = 0d;
        [Description("R/W,SideCh13L")]
        public double SideCh13L = 0d;
        [Description("R/W,SideCh14L")]
        public double SideCh14L = 0d;
        [Description("R/W,SideCh15L")]
        public double SideCh15L = 0d;
        [Description("R/W,SideCh16L")]
        public double SideCh16L = 0d;
        [Description("R/W,SideCh17L")]
        public double SideCh17L = 0d;
        [Description("R/W,SideCh18L")]
        public double SideCh18L = 0d;

        [Description("R/W,SideCh1R")]
        public double SideCh1R = 0d;
        [Description("R/W,SideCh2R")]
        public double SideCh2R = 0d;
        [Description("R/W,SideCh3R")]
        public double SideCh3R = 0d;
        [Description("R/W,SideCh4R")]
        public double SideCh4R = 0d;
        [Description("R/W,SideCh5R")]
        public double SideCh5R = 0d;
        [Description("R/W,SideCh6R")]
        public double SideCh6R = 0d;
        [Description("R/W,SideCh7R")]
        public double SideCh7R = 0d;
        [Description("R/W,SideCh8R")]
        public double SideCh8R = 0d;
        [Description("R/W,SideCh9R")]
        public double SideCh9R = 0d;
        [Description("R/W,SideCh10R")]
        public double SideCh10R = 0d;
        [Description("R/W,SideCh11R")]
        public double SideCh11R = 0d;
        [Description("R/W,SideCh12R")]
        public double SideCh12R = 0d;
        [Description("R/W,SideCh13R")]
        public double SideCh13R = 0d;
        [Description("R/W,SideCh14R")]
        public double SideCh14R = 0d;
        [Description("R/W,SideCh15R")]
        public double SideCh15R = 0d;
        [Description("R/W,SideCh16R")]
        public double SideCh16R = 0d;
        [Description("R/W,SideCh17R")]
        public double SideCh17R = 0d;
        [Description("R/W,SideCh18R")]
        public double SideCh18R = 0d;

        #endregion
    }
}
