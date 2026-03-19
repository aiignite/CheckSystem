using Controller.HardwareController;
using System;
using System.ComponentModel;

namespace Controller
{
    /// <summary>
    /// 继电器从站
    /// 0x201 - 0x20F
    /// </summary>
    public sealed class SyRenesasMcuControllerSlaveWith8RLs : ControllerBase, ICcdIoController
    {
        [Description("R/W,继电器1")]
        public bool Relay1;

        [Description("R/W,继电器2")]
        public bool Relay2;

        [Description("R/W,继电器3")]
        public bool Relay3;

        [Description("R/W,继电器4")]
        public bool Relay4;

        [Description("R/W,继电器5")]
        public bool Relay5;

        [Description("R/W,继电器6")]
        public bool Relay6;

        [Description("R/W,继电器7")]
        public bool Relay7;

        [Description("R/W,继电器8")]
        public bool Relay8;

        /// <summary>
        /// 继电器从站
        /// 0x201 - 0x20F
        /// </summary>
        /// <param name="name"></param>
        public SyRenesasMcuControllerSlaveWith8RLs(string name) : base(name) { }

        private string _connectedMasterIp = string.Empty;
        private uint _slaveId;

        /// <summary>
        /// 连接主站
        /// </summary>
        /// <param name="masterIp">主站IP</param>
        /// <param name="slaveId">从站ID，0x201 - 0x20F</param>
        /// <returns></returns>
        public bool ConnectMaster(string masterIp, string slaveId)
        {
            if (!SyRenesasMcuControllerMaster.SlaveTryConnectMaster(masterIp, slaveId, this))
                return false;

            _connectedMasterIp = masterIp;
            _slaveId = Convert.ToUInt32(slaveId, 16);

            return true;
        }

        [Description("设置从站继电器状态")]
        public void SlaveSetRLs()
        {
            SyRenesasMcuControllerMaster.SlaveSetRLs(_connectedMasterIp, _slaveId);
        }

        [Description("刷新从站继电器状态")]
        public void SlaveRefreshRLs()
        {

        }

        public bool SetOutputs()
        {
            SlaveSetRLs();

            return true;
        }

        public void GetInputs()
        {

        }

        public void ResetOutPuts()
        {
            Relay1 = false;
            Relay2 = false;
            Relay3 = false;
            Relay4 = false;
            Relay5 = false;
            Relay6 = false;
            Relay7 = false;
            Relay8 = false;
            SetOutputs();
        }
    }
}
