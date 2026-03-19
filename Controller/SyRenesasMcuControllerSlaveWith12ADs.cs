using Controller.HardwareController;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

namespace Controller
{
    /// <summary>
    /// AD从站
    /// 0x101 - 0x10F
    /// </summary>
    public sealed class SyRenesasMcuControllerSlaveWith12ADs : ControllerBase, ICcdIoController
    {
        [Description("R,电流1")]
        public double Current1 = -9999;
        [Description("R,电流2")]
        public double Current2 = -9999;
        [Description("R,电流3")]
        public double Current3 = -9999;
        [Description("R,电流4")]
        public double Current4 = -9999;
        [Description("R,电流5")]
        public double Current5 = -9999;
        [Description("R,电流6")]
        public double Current6 = -9999;

        [Description("R,分压测电阻1")]
        public double Resistance1 = -9999;

        [Description("R,电压2")]
        public double Voltage2 = -9999;
        [Description("R,电压3")]
        public double Voltage3 = -9999;
        [Description("R,电压4")]
        public double Voltage4 = -9999;
        [Description("R,电压5")]
        public double Voltage5 = -9999;
        [Description("R,电压6")]
        public double Voltage6 = -9999;
        [Description("R,电压7")]
        public double Voltage7 = -9999;

        /// <summary>
        /// AD从站
        /// 0x101 - 0x10F
        /// </summary>
        /// <param name="name"></param>
        public SyRenesasMcuControllerSlaveWith12ADs(string name) : base(name) { }

        private string _connectedMasterIp = string.Empty;
        private uint _slaveId;

        public bool ConnectMaster(string masterIp, string slaveId)
        {
            if (!SyRenesasMcuControllerMaster.SlaveTryConnectMaster(masterIp, slaveId, this))
                return false;

            _connectedMasterIp = masterIp;
            _slaveId = Convert.ToUInt32(slaveId, 16);

            return true;
        }

        [Description("刷新从站AD值")]
        public void SlaveReadADs()
        {
            Current1 = -9999;
            Current2 = -9999;
            Current3 = -9999;
            Current4 = -9999;
            Current5 = -9999;
            Current6 = -9999;

            Resistance1 = -9999;

            Voltage2 = -9999;
            Voltage3 = -9999;
            Voltage4 = -9999;
            Voltage5 = -9999;
            Voltage6 = -9999;
            Voltage7 = -9999;

            SyRenesasMcuControllerMaster.SlaveReadADs(_connectedMasterIp, _slaveId);
        }

        public bool SetOutputs()
        {
            return false;
        }

        public void GetInputs()
        {
            SlaveReadADs();
        }

        public void ResetOutPuts()
        {
            return;
        }

        #region SYZ,读呼吸灯电流、电压

        public List<double> Currents1 = new List<double>();
        public List<double> Currents2 = new List<double>();
        public List<double> Currents3 = new List<double>();
        public List<double> Currents4 = new List<double>();
        public List<double> Currents5 = new List<double>();
        public List<double> Currents6 = new List<double>();

        public List<double> Voltages2 = new List<double>();
        public List<double> Voltages3 = new List<double>();
        public List<double> Voltages4 = new List<double>();
        public List<double> Voltages5 = new List<double>();
        public List<double> Voltages6 = new List<double>();
        public List<double> Voltages7 = new List<double>();

        [Description("循环刷新从站AD值")]
        public void ReadCurrentsVoltages(int n)
        {
            Currents1.Clear();
            Currents2.Clear();
            Currents3.Clear();
            Currents4.Clear();
            Currents5.Clear();
            Currents6.Clear();

            Voltages2.Clear();
            Voltages3.Clear();
            Voltages4.Clear();
            Voltages5.Clear();
            Voltages6.Clear();
            Voltages7.Clear();

            for (int i = 0; i < n; i++)
            {
                SlaveReadADs();

                if (Current1 != -9999)
                    Currents1.Add(Current1);
                if (Current2 != -9999)
                    Currents2.Add(Current2);
                if (Current3 != -9999)
                    Currents3.Add(Current3);
                if (Current4 != -9999)
                    Currents4.Add(Current4);
                if (Current5 != -9999)
                    Currents5.Add(Current5);
                if (Current6 != -9999)
                    Currents6.Add(Current6);

                if (Voltage2 != -9999)
                    Voltages2.Add(Voltage2);
                if (Voltage3 != -9999)
                    Voltages3.Add(Voltage3);
                if (Voltage4 != -9999)
                    Voltages4.Add(Voltage4);
                if (Voltage5 != -9999)
                    Voltages5.Add(Voltage5);
                if (Voltage6 != -9999)
                    Voltages6.Add(Voltage6);
                if (Voltage7 != -9999)
                    Voltages7.Add(Voltage7);

                Thread.Sleep(200);
            }
        }

        [Description("读取CurrentList中满足范围值")]
        public void RefreshCurrent(int idx, int min, int max)
        {
            List<double> Currents = (List<double>)GetType().GetField($"Currents{idx}").GetValue(this);
            var Current = GetType().GetField($"Current{idx}");

            Current.SetValue(this, -9999);
            if (Currents1.Count == 0)
                return;

            Currents.Sort();
            Currents.Reverse();
            Current.SetValue(this, Currents[0]);
        }

        [Description("读取VoltageList中满足范围值")]
        public void RefreshVoltage(int idx, int min, int max)
        {
            List<double> Voltages = (List<double>)GetType().GetField($"Voltages{idx}").GetValue(this);
            var Voltage = GetType().GetField($"Voltage{idx}");

            Voltage.SetValue(this, -9999);

            if (Voltages2.Count == 0)
                return;

            Voltages.Sort();
            Voltages.Reverse();
            Voltage.SetValue(this, Voltages[0]);
        }

        #endregion
    }
}
