using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckSystem.SmtForms.DataUploader
{
    public class PanaconicNpmDat
    {
        public string Lane { get; set; }
        public string FAdd { get; set; }
        public string FeederSerial { get; set; }
        public int Pickup { get; set; }
        public int PMiss { get; set; }
        public int RMiss { get; set; }
        public int DMiss { get; set; }
        public int MMiss { get; set; }
        public int HMiss { get; set; }
        public int TRSMiss { get; set; }
    }
}
