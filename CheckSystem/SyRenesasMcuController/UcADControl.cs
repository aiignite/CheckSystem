using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using Sunny.UI;

namespace CheckSystem.SyRenesasMcuController
{
    public partial class UcAdControl : UIUserControl
    {
        private SyRenesasMcuControllerSlaveWith12ADs AdInstance { get; set; }

        public UcAdControl(SyRenesasMcuControllerSlaveWith12ADs adInstance, string slaveId)
        {
            InitializeComponent();
            lblSlaveId.Text = slaveId + @":";
            AdInstance = adInstance;
        }

        private void btnSlaveReadADs_Click(object sender, EventArgs e)
        {
            if (AdInstance != null)
            {
                AdInstance.SlaveReadADs();
                RefreshADs();
            }
        }

        public void RefreshADs()
        {
            if (AdInstance != null)
            {
                curr1.DoubleValue = AdInstance.Current1;
                curr2.DoubleValue = AdInstance.Current2;
                curr3.DoubleValue = AdInstance.Current3;
                curr4.DoubleValue = AdInstance.Current4;
                curr5.DoubleValue = AdInstance.Current5;
                curr6.DoubleValue = AdInstance.Current6;

                res1.DoubleValue = AdInstance.Resistance1;

                volt2.DoubleValue = AdInstance.Voltage2;
                volt3.DoubleValue = AdInstance.Voltage3;
                volt4.DoubleValue = AdInstance.Voltage4;
                volt5.DoubleValue = AdInstance.Voltage5;
                volt6.DoubleValue = AdInstance.Voltage6;
                volt7.DoubleValue = AdInstance.Voltage7;
            }
        }
    }
}
