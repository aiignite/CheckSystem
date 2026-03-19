using CommonUtility.BusLoader;
using Controller;
using Sunny.UI;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckSystem.HelperForms
{
    public partial class HelaMdf : UIForm
    {
        //private SyRenesasMcuControllerMaster _syRenesasMcuController = new SyRenesasMcuControllerMaster("CAN卡");
        private SyControllerWith56Pin _syRenesasMcuController = new SyControllerWith56Pin("CAN卡");
        //private KebodaLdmSeries _kebodaLdmSeries = new KebodaLdmSeries("科博达模块");
        private KebodaLdmSeries _kebodaLdmSeries = new KebodaLdmSeries("科博达模块");

        private AfsValueHolder _leftAfsRunPosHolder;
        private BindingSource _leftAfsBindingSource;

        private AfsValueHolder _rightAfsRunPosHolder;
        private BindingSource _rightAfsBindingSource;

        public HelaMdf()
        {
            InitializeComponent();
            CanBus.PushCanMsg += CanBus_PushCanMsg;
            Load += HelaMdf_Load;
            Closed += HelaMdf_Closed;
        }

        private void HelaMdf_Load(object sender, System.EventArgs e)
        {
            InitializeControls();
            InitController();
        }

        private void HelaMdf_Closed(object sender, EventArgs e)
        {
            _kebodaLdmSeries.Dispose();
            _syRenesasMcuController.Dispose();
        }

        private void InitController()
        {
            try
            {
                foreach (var c in this.Controls)
                {
                    if (c is UISwitch && ((UISwitch)c).Name == "uiSwitch1")
                        continue;

                    ((Control)c).Enabled = false;
                }

                //_syRenesasMcuController.InitRemoteIpAddress("192.168.1.28:8088");
                //_kebodaLdmSeries.Can = _syRenesasMcuController.GatewayCan1;

                _syRenesasMcuController.InitRemoteIpAddress("192.168.1.28:8088");
                _kebodaLdmSeries.Can = _syRenesasMcuController.GatwayCan1;

                _kebodaLdmSeries.IsMqb = true;
                _kebodaLdmSeries.Kl15 = true;
                _kebodaLdmSeries.StartHelaMdfControlSignal();
                //_kebodaLdmSeries.ModuleAwake();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private void InitializeControls()
        {
            BindingLeftAfsControls();
            BindingRighttAfsControls();
        }

        private void BindingLeftAfsControls()
        {
            _leftAfsRunPosHolder = new AfsValueHolder { BaseValue = 0, MinAngleValue = -7, MaxAngleValue = 13, MinBaseValue = -700, MaxBaseValue = 1300 };
            trackBar2.Minimum = (int)(_leftAfsRunPosHolder.MinAngleValue / AfsValueHolder.ScaleFactor);
            trackBar2.Maximum = (int)(_leftAfsRunPosHolder.MaxAngleValue / AfsValueHolder.ScaleFactor);
            txtLeftAfsRunPos.Minimum = (decimal)_leftAfsRunPosHolder.MinAngleValue;
            txtLeftAfsRunPos.Maximum = (decimal)_leftAfsRunPosHolder.MaxAngleValue;
            txtLeftAfsMin.Value = (decimal)_leftAfsRunPosHolder.MinAngleValue;
            txtLeftAfsMax.Value = (decimal)_leftAfsRunPosHolder.MaxAngleValue;

            _leftAfsBindingSource = new BindingSource { DataSource = _leftAfsRunPosHolder };
            trackBar2.DataBindings.Add("Value", _leftAfsBindingSource, "BaseValue", true, DataSourceUpdateMode.OnPropertyChanged);
            trackBar2.DataBindings.Add("Minimum", _leftAfsBindingSource, "MinBaseValue", true, DataSourceUpdateMode.OnPropertyChanged);
            trackBar2.DataBindings.Add("Maximum", _leftAfsBindingSource, "MaxBaseValue", true, DataSourceUpdateMode.OnPropertyChanged);
            lblLeftAfsRunPos.DataBindings.Add("Text", _leftAfsBindingSource, "AngleValue", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLeftAfsRunPos.DataBindings.Add("Value", _leftAfsBindingSource, "AngleValue", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLeftAfsRunPer.DataBindings.Add("Value", _leftAfsBindingSource, "PerValue", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLeftAfsMin.DataBindings.Add("Value", _leftAfsBindingSource, "MinAngleValue", true, DataSourceUpdateMode.OnPropertyChanged);
            txtLeftAfsMax.DataBindings.Add("Value", _leftAfsBindingSource, "MaxAngleValue", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void BindingRighttAfsControls()
        {
            _rightAfsRunPosHolder = new AfsValueHolder { BaseValue = 0, MinAngleValue = -13, MaxAngleValue = 7, MinBaseValue = -1300, MaxBaseValue = 700 };
            trackBar3.Minimum = (int)(_rightAfsRunPosHolder.MinAngleValue / AfsValueHolder.ScaleFactor);
            trackBar3.Maximum = (int)(_rightAfsRunPosHolder.MaxAngleValue / AfsValueHolder.ScaleFactor);
            txtRightAfsRunPos.Minimum = (decimal)_rightAfsRunPosHolder.MinAngleValue;
            txtRightAfsRunPos.Maximum = (decimal)_rightAfsRunPosHolder.MaxAngleValue;
            txtRightAfsMin.Value = (decimal)_rightAfsRunPosHolder.MinAngleValue;
            txtRightAfsMax.Value = (decimal)_rightAfsRunPosHolder.MaxAngleValue;

            _rightAfsBindingSource = new BindingSource { DataSource = _rightAfsRunPosHolder };
            trackBar3.DataBindings.Add("Value", _rightAfsBindingSource, "BaseValue", true, DataSourceUpdateMode.OnPropertyChanged);
            trackBar3.DataBindings.Add("Minimum", _rightAfsBindingSource, "MinBaseValue", true, DataSourceUpdateMode.OnPropertyChanged);
            trackBar3.DataBindings.Add("Maximum", _rightAfsBindingSource, "MaxBaseValue", true, DataSourceUpdateMode.OnPropertyChanged);
            lblRightAfsRunPos.DataBindings.Add("Text", _rightAfsBindingSource, "AngleValue", true, DataSourceUpdateMode.OnPropertyChanged);
            txtRightAfsRunPos.DataBindings.Add("Value", _rightAfsBindingSource, "AngleValue", true, DataSourceUpdateMode.OnPropertyChanged);
            txtRightAfsRunPer.DataBindings.Add("Value", _rightAfsBindingSource, "PerValue", true, DataSourceUpdateMode.OnPropertyChanged);
            txtRightAfsMin.DataBindings.Add("Value", _rightAfsBindingSource, "MinAngleValue", true, DataSourceUpdateMode.OnPropertyChanged);
            txtRightAfsMax.DataBindings.Add("Value", _rightAfsBindingSource, "MaxAngleValue", true, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void CanBus_PushCanMsg(string name, CanBus.CanDataPackage data, CanBus.OnPushCanDataType onPushCanDataType)
        {
            if (!string.IsNullOrEmpty(name) &&
                _kebodaLdmSeries.Can != null && !string.IsNullOrEmpty(_kebodaLdmSeries.Can.Name) && _kebodaLdmSeries.Can.Name == name &&
                (onPushCanDataType == CanBus.OnPushCanDataType.FilterRx || onPushCanDataType == CanBus.OnPushCanDataType.NotFilterRx))
            {
                if (data.CanId == 0x197)
                {
                    try
                    {
                        var value = KebodaLdmSeries.GetIntelValue(data.CanData, 0, 12);
                        UpdateTxtValue(txtMdfLeftRealPos, Math.Round((value * 0.05), 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else if (data.CanId == 0x199)
                {
                    try
                    {
                        var value = KebodaLdmSeries.GetIntelValue(data.CanData, 0, 12);
                        UpdateTxtValue(txtMdfRightRealPos, Math.Round((value * 0.05), 2, MidpointRounding.AwayFromZero).ToString(CultureInfo.InvariantCulture));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else if (data.CanId == 0x19A)
                {
                    try
                    {
                        var value = KebodaLdmSeries.GetIntelValue(data.CanData, 0, 12);
                        var pos = Math.Round((value * 0.015625 - 31.96875), 2, MidpointRounding.AwayFromZero);
                        var per = (double)0;
                        per = pos > 0
                            ? Math.Round((pos / (trackBar2.Maximum * AfsValueHolder.ScaleFactor)) * 100, 2,
                                MidpointRounding.AwayFromZero)
                            : Math.Round((pos / (trackBar2.Minimum * AfsValueHolder.ScaleFactor)) * 100, 2,
                                MidpointRounding.AwayFromZero);

                        UpdateTxtValue(txtLeftAfsRealPos, pos.ToString(CultureInfo.InvariantCulture));
                        UpdateTxtValue(txtLeftAfsRealPer, per.ToString(CultureInfo.InvariantCulture));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                else if (data.CanId == 0x19C)
                {
                    try
                    {
                        var value = KebodaLdmSeries.GetIntelValue(data.CanData, 0, 12);
                        var pos = Math.Round((value * 0.015625 - 31.96875), 2, MidpointRounding.AwayFromZero);
                        var per = (double)0;
                        per = pos > 0
                            ? Math.Round((pos / (trackBar3.Maximum * AfsValueHolder.ScaleFactor)) * 100, 2,
                                MidpointRounding.AwayFromZero)
                            : Math.Round((pos / (trackBar3.Minimum * AfsValueHolder.ScaleFactor)) * 100, 2,
                                MidpointRounding.AwayFromZero);
                        UpdateTxtValue(txtRightAfsRealPos, pos.ToString(CultureInfo.InvariantCulture));
                        UpdateTxtValue(txtRightAfsRealPer, per.ToString(CultureInfo.InvariantCulture));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
            }
        }

        private delegate void UpdateTxtValueDelegate(UITextBox control, string value);
        private void UpdateTxtValue(UITextBox control, string value)
        {
            var updateTxtVoltValueDelegate = new UpdateTxtValueDelegate(UpdateTxtValue);

            if (control.InvokeRequired)
            {
                Invoke(updateTxtVoltValueDelegate, control, value);
            }
            else
            {
                control.Text = value;
            }
        }

        private async void uiSwitch1_ActiveChanged(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (uiSwitch1.Active)
                {
                    Thread.Sleep(4500);
                    _kebodaLdmSeries.ModuleAwake();

                    BeginInvoke(new Action(() =>
                    {
                        foreach (var c in this.Controls)
                        {
                            if (c is UISwitch && ((UISwitch)c).Name == "uiSwitch1")
                                continue;

                            ((Control)c).Enabled = uiSwitch1.Active;
                        }
                    }));
                }
                else
                {
                    BeginInvoke(new Action(() =>
                    {
                        foreach (var c in this.Controls)
                        {
                            if (c is UISwitch && ((UISwitch)c).Name == "uiSwitch1")
                                continue;

                            ((Control)c).Enabled = uiSwitch1.Active;
                        }
                    }));

                    //Thread.Sleep(5000);
                    _kebodaLdmSeries.ModuleSleep();
                }


            });
        }

        #region MDF

        private void swLowBeam_ActiveChanged(object sender, EventArgs e)
        {
            if (swLowBeam.Active)
                _kebodaLdmSeries.LLP_L_LowBeamOn();
            else
                _kebodaLdmSeries.LLP_L_LowBeamOff();
        }

        private void swLeftHighBeam_ActiveChanged(object sender, EventArgs e)
        {
            if (swLeftHighBeam.Active)
                _kebodaLdmSeries.LeftHighBeamOn();
            else
                _kebodaLdmSeries.LeftHighBeamOff();
        }

        private void swRightHighBeam_ActiveChanged(object sender, EventArgs e)
        {
            if (swRightHighBeam.Active)
                _kebodaLdmSeries.RightHighBeamOn();
            else
                _kebodaLdmSeries.RightHighBeamOff();
        }

        private void swHbRampL_ActiveChanged(object sender, EventArgs e)
        {

        }

        private void swHbRampR_ActiveChanged(object sender, EventArgs e)
        {

        }

        private void btnCity_Click(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(txtCity.DoubleValue / AfsValueHolder.ScaleFactor);
            trackBar1_Scroll(sender, e);
        }

        private void btnCountry_Click(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(txtCountry.DoubleValue / AfsValueHolder.ScaleFactor);
            trackBar1_Scroll(sender, e);
        }

        private void btnHighway_Click(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(txtHighway.DoubleValue / AfsValueHolder.ScaleFactor);
            trackBar1_Scroll(sender, e);
        }

        private void btnHighBeam_Click(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(txtHighBeam.DoubleValue / AfsValueHolder.ScaleFactor);
            trackBar1_Scroll(sender, e);
        }

        private void btnMdf_Click(object sender, EventArgs e)
        {
            trackBar1.Value = (int)(txtMdf.DoubleValue / AfsValueHolder.ScaleFactor);
            trackBar1_Scroll(sender, e);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            var decimalValue = Math.Round(trackBar1.Value * AfsValueHolder.ScaleFactor, 2, MidpointRounding.AwayFromZero);
            txtRealMdfPosRun.Value = (decimal)decimalValue;
        }

        private void uiCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            trackBar1_Scroll(sender, e);

            if (uiCheckBox1.Checked)
            {
                if (txtRealMdfPosRun.Value >= (decimal)(txtHighBeamOnMin.DoubleValue) &&
                    txtRealMdfPosRun.Value <= (decimal)(txtHighBeamOnMax.DoubleValue))
                {
                    swLeftHighBeam.Active = true;
                    swRightHighBeam.Active = true;
                }
                else
                {
                    swLeftHighBeam.Active = false;
                    swRightHighBeam.Active = false;
                }
            }
        }

        private void txtHighBeamOnMin_TextChanged(object sender, EventArgs e)
        {
            trackBar1_Scroll(sender, e);

            if (uiCheckBox1.Checked)
            {
                if (txtRealMdfPosRun.Value >= (decimal)(txtHighBeamOnMin.DoubleValue) &&
                    txtRealMdfPosRun.Value <= (decimal)(txtHighBeamOnMax.DoubleValue))
                {
                    swLeftHighBeam.Active = true;
                    swRightHighBeam.Active = true;
                }
                else
                {
                    swLeftHighBeam.Active = false;
                    swRightHighBeam.Active = false;
                }
            }
        }

        private void txtHighBeamOnMax_TextChanged(object sender, EventArgs e)
        {
            trackBar1_Scroll(sender, e);

            if (uiCheckBox1.Checked)
            {
                if (txtRealMdfPosRun.Value >= (decimal)(txtHighBeamOnMin.DoubleValue) &&
                    txtRealMdfPosRun.Value <= (decimal)(txtHighBeamOnMax.DoubleValue))
                {
                    swLeftHighBeam.Active = true;
                    swRightHighBeam.Active = true;
                }
                else
                {
                    swLeftHighBeam.Active = false;
                    swRightHighBeam.Active = false;
                }
            }
        }

        private void txtRealMdfPosRun_ValueChanged(object sender, EventArgs e)
        {
            _kebodaLdmSeries.MdfGoTo((double)txtRealMdfPosRun.Value);
            lblRunPos.Text = txtRealMdfPosRun.Value.ToString(CultureInfo.InvariantCulture);

            if (uiCheckBox1.Checked)
            {
                if (txtRealMdfPosRun.Value >= (decimal)(txtHighBeamOnMin.DoubleValue) &&
                    txtRealMdfPosRun.Value <= (decimal)(txtHighBeamOnMax.DoubleValue))
                {
                    swLeftHighBeam.Active = true;
                    swRightHighBeam.Active = true;
                }
                else
                {
                    swLeftHighBeam.Active = false;
                    swRightHighBeam.Active = false;
                }
            }

            trackBar1.Value = (int)(txtRealMdfPosRun.Value / (decimal)AfsValueHolder.ScaleFactor);
        }

        private void txtRealMdfPosRun_ValueChanged(object sender, double value)
        {
            _kebodaLdmSeries.MdfGoTo((double)txtRealMdfPosRun.Value);
            lblRunPos.Text = txtRealMdfPosRun.Value.ToString(CultureInfo.InvariantCulture);

            if (uiCheckBox1.Checked)
            {
                if (txtRealMdfPosRun.Value >= (decimal)(txtHighBeamOnMin.DoubleValue) &&
                    txtRealMdfPosRun.Value <= (decimal)(txtHighBeamOnMax.DoubleValue))
                {
                    swLeftHighBeam.Active = true;
                    swRightHighBeam.Active = true;
                }
                else
                {
                    swLeftHighBeam.Active = false;
                    swRightHighBeam.Active = false;
                }
            }

            trackBar1.Value = (int)(value / AfsValueHolder.ScaleFactor);
        }

        #endregion

        #region AFS

        private void lblLeftAfsRunPos_TextChanged(object sender, EventArgs e)
        {
            _kebodaLdmSeries.LeftAfsGoTo(double.Parse(lblLeftAfsRunPos.Text));

            if (uiCheckBox2.Checked && _rightAfsRunPosHolder != null)
                _rightAfsRunPosHolder.AngleValue = Math.Round(0.5 * double.Parse(lblLeftAfsRunPos.Text), 2, MidpointRounding.AwayFromZero);
        }

        private void lblRightAfsRunPos_TextChanged(object sender, EventArgs e)
        {
            _kebodaLdmSeries.RightAfsGoTo(double.Parse(lblRightAfsRunPos.Text));
        }

        private void txtLeftAfsMinMax_ValueChanged(object sender, EventArgs e)
        {
            //var angleValue = txtLeftAfsRunPos.Value;

            //if (txtLeftAfsMax.Value != 0 && txtLeftAfsMin.Value != 0)
            //{
            //    var per = (double)0;
            //    if (angleValue >= 0)
            //        per = (double)Math.Round((angleValue / Math.Abs(txtLeftAfsMax.Value)) * 100, 2,
            //            MidpointRounding.AwayFromZero);
            //    else
            //        per = (double)Math.Round((angleValue / Math.Abs(txtLeftAfsMin.Value)) * 100, 2,
            //            MidpointRounding.AwayFromZero);

            //    if (per >= (double)txtLeftAfsRunPer.Minimum && per <= (double)txtLeftAfsRunPer.Maximum)
            //    {
            //        txtLeftAfsRunPer.Value = (decimal)per;
            //    }
            //}
        }

        public class AfsValueHolder : INotifyPropertyChanged
        {
            public const double ScaleFactor = 0.01f;

            private int _baseValue;
            private int _minBaseValue;
            private int _maxBaseValue;
            private double _minAngleValue;
            private double _maxAngleValue;
            private double _angleValue;
            private double _perValue;

            public int BaseValue
            {
                get { return _baseValue; }
                set
                {
                    if (value < MinBaseValue)
                        value = MinBaseValue;
                    else if (value > MaxBaseValue)
                        value = MaxBaseValue;

                    if (Math.Abs(_baseValue - value) != 0)
                    {
                        _baseValue = value;
                        CalByBaseValue();
                        OnPropertyChanged("BaseValue");
                    }
                }
            }

            public int MinBaseValue
            {
                get { return _minBaseValue; }
                set
                {
                    if (Math.Abs(_minBaseValue - value) != 0)
                    {
                        _minBaseValue = value;
                        OnPropertyChanged("MinBaseValue");
                    }
                }
            }

            public int MaxBaseValue
            {
                get { return _maxBaseValue; }
                set
                {
                    if (Math.Abs(_maxBaseValue - value) != 0)
                    {
                        _maxBaseValue = value;
                        OnPropertyChanged("MaxBaseValue");
                    }
                }
            }

            public double MinAngleValue
            {
                get { return _minAngleValue; }
                set
                {
                    if (Math.Abs(_minAngleValue - value) != 0)
                    {
                        _minAngleValue = value;
                        MinBaseValue = (int)Math.Round(_minAngleValue / ScaleFactor, 0, MidpointRounding.AwayFromZero);
                        BaseValue = _baseValue;
                        OnPropertyChanged("MinAngleValue");
                    }
                }
            }

            public double MaxAngleValue
            {
                get { return _maxAngleValue; }
                set
                {
                    if (Math.Abs(_maxAngleValue - value) != 0)
                    {
                        _maxAngleValue = value;
                        MaxBaseValue = (int)Math.Round(_maxAngleValue / ScaleFactor, 0, MidpointRounding.AwayFromZero);
                        BaseValue = _baseValue;
                        OnPropertyChanged("MaxAngleValue");
                    }
                }
            }

            public double AngleValue
            {
                get { return _angleValue; }
                set
                {
                    if (Math.Abs(_angleValue - value) != 0)
                    {
                        _angleValue = value;
                        BaseValue = (int)Math.Round(_angleValue / ScaleFactor, 0, MidpointRounding.AwayFromZero);
                        OnPropertyChanged("AngleValue");
                    }
                }
            }

            public double PerValue
            {
                get { return _perValue; }
                set
                {
                    if (Math.Abs(_perValue - value) != 0)
                    {
                        _perValue = value;

                        if (PerValue >= 0)
                            AngleValue = Math.Round(_perValue * 0.01 * Math.Abs(_maxAngleValue), 2, MidpointRounding.AwayFromZero);
                        else
                            AngleValue = Math.Round(_perValue * 0.01 * Math.Abs(_minAngleValue), 2, MidpointRounding.AwayFromZero);

                        OnPropertyChanged("PerValue");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }

            private void CalByBaseValue()
            {
                AngleValue = Math.Round(_baseValue * ScaleFactor, 2, MidpointRounding.AwayFromZero);

                var per = (double)0;
                if (_angleValue >= 0)
                    per = Math.Round((_angleValue / Math.Abs(_maxAngleValue)) * 100, 2,
                        MidpointRounding.AwayFromZero);
                else
                    per = Math.Round((_angleValue / Math.Abs(_minAngleValue)) * 100, 2,
                        MidpointRounding.AwayFromZero);
                PerValue = per;
            }
        }

        #endregion
    }
}
