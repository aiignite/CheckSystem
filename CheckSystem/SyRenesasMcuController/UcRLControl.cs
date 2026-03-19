using Controller;
using Sunny.UI;
using System;

namespace CheckSystem.SyRenesasMcuController
{
    public partial class UcRlControl : UIUserControl
    {
        private SyRenesasMcuControllerSlaveWith8RLs RlInstance { get; set; }

        public UcRlControl(SyRenesasMcuControllerSlaveWith8RLs rlInstance, string slaveId)
        {
            InitializeComponent();
            lblSlaveId.Text = slaveId + @":";
            RlInstance = rlInstance;
        }

        private void btnSlaveSetRLs_Click(object sender, EventArgs e)
        {
            if (RlInstance != null)
                RlInstance.SlaveSetRLs();
        }

        private void RL_ActiveChanged(object sender, EventArgs e)
        {
            if (RlInstance != null)
            {
                var sw = sender as UISwitch;
                if (sw != null)
                {
                    var rlIndex = int.Parse(sw.Tag.ToString());

                    switch (rlIndex)
                    {
                        case 1:
                            RlInstance.Relay1 = RL1.Active;
                            break;

                        case 2:
                            RlInstance.Relay2 = RL2.Active;
                            break;

                        case 3:
                            RlInstance.Relay3 = RL3.Active;
                            break;

                        case 4:
                            RlInstance.Relay4 = RL4.Active;
                            break;

                        case 5:
                            RlInstance.Relay5 = RL5.Active;
                            break;

                        case 6:
                            RlInstance.Relay6 = RL6.Active;
                            break;

                        case 7:
                            RlInstance.Relay7 = RL7.Active;
                            break;

                        case 8:
                            RlInstance.Relay8 = RL8.Active;
                            break;
                    }
                }
            }
        }
    }
}
