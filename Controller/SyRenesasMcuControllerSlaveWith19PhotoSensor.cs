using System;
using System.ComponentModel;

namespace Controller
{
    /// <summary>
    /// 光敏传感器采集从站
    /// 0x501 - 0x50F
    /// </summary>
    public sealed class SyRenesasMcuControllerSlaveWith19PhotoSensor : ControllerBase
    {
        [Description("R,光敏传感器采样1")] public double PhotoSensor1 = -9999;
        [Description("R,光敏传感器采样2")] public double PhotoSensor2 = -9999;
        [Description("R,光敏传感器采样3")] public double PhotoSensor3 = -9999;
        [Description("R,光敏传感器采样4")] public double PhotoSensor4 = -9999;
        [Description("R,光敏传感器采样5")] public double PhotoSensor5 = -9999;
        [Description("R,光敏传感器采样6")] public double PhotoSensor6 = -9999;
        [Description("R,光敏传感器采样7")] public double PhotoSensor7 = -9999;
        [Description("R,光敏传感器采样8")] public double PhotoSensor8 = -9999;
        [Description("R,光敏传感器采样9")] public double PhotoSensor9 = -9999;
        [Description("R,光敏传感器采样10")] public double PhotoSensor10 = -9999;
        [Description("R,光敏传感器采样11")] public double PhotoSensor11 = -9999;
        [Description("R,光敏传感器采样12")] public double PhotoSensor12 = -9999;
        [Description("R,光敏传感器采样13")] public double PhotoSensor13 = -9999;
        [Description("R,光敏传感器采样14")] public double PhotoSensor14 = -9999;
        [Description("R,光敏传感器采样15")] public double PhotoSensor15 = -9999;
        [Description("R,光敏传感器采样16")] public double PhotoSensor16 = -9999;
        [Description("R,光敏传感器采样17")] public double PhotoSensor17 = -9999;
        [Description("R,光敏传感器采样18")] public double PhotoSensor18 = -9999;
        [Description("R,光敏传感器采样19")] public double PhotoSensor19 = -9999;

        /// <summary>
        /// 光敏传感器采集从站
        /// 0x501 - 0x50F
        /// </summary>
        public SyRenesasMcuControllerSlaveWith19PhotoSensor(string name) :
            base(name)
        {

        }

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

        [Description("刷新从站光敏传感器采样值")]
        public void SlaveRefreshSensorValues()
        {
            PhotoSensor1 = -9999;
            PhotoSensor2 = -9999;
            PhotoSensor3 = -9999;
            PhotoSensor4 = -9999;
            PhotoSensor5 = -9999;
            PhotoSensor6 = -9999;
            PhotoSensor7 = -9999;
            PhotoSensor8 = -9999;
            PhotoSensor9 = -9999;
            PhotoSensor10 = -9999;
            PhotoSensor11 = -9999;
            PhotoSensor12 = -9999;
            PhotoSensor13 = -9999;
            PhotoSensor14 = -9999;
            PhotoSensor15 = -9999;
            PhotoSensor16 = -9999;
            PhotoSensor17 = -9999;
            PhotoSensor18 = -9999;
            PhotoSensor19 = -9999;

            SyRenesasMcuControllerMaster.SlaveReadSensorValues(_connectedMasterIp, _slaveId);
        }
    }
}
