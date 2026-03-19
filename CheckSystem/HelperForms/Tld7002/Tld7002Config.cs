using System.Xml.Serialization;

namespace CheckSystem.HelperForms.Tld7002
{
    /// <summary>
    /// 
    /// </summary>
    [XmlType(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Tld7002Config
    {

        private Tld7002ConfigDeviceInfo deviceInfoField;

        /// <remarks/>
        public Tld7002ConfigDeviceInfo DeviceInfo
        {
            get
            {
                return deviceInfoField;
            }
            set
            {
                deviceInfoField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Tld7002ConfigDeviceInfo
    {

        private string deviceNameField;

        private Tld7002ConfigDeviceInfoRelays relaysField;

        private string[] filesField;

        /// <remarks/>
        public string DeviceName
        {
            get
            {
                return deviceNameField;
            }
            set
            {
                deviceNameField = value;
            }
        }

        /// <remarks/>
        public Tld7002ConfigDeviceInfoRelays Relays
        {
            get
            {
                return relaysField;
            }
            set
            {
                relaysField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("File", IsNullable = false)]
        public string[] Files
        {
            get
            {
                return filesField;
            }
            set
            {
                filesField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class Tld7002ConfigDeviceInfoRelays
    {
        private string vsField;
        private string gpin0Field;
        private string powerDelayField;
        private string inputButtonField;

        /// <remarks/>
        public string Vs
        {
            get
            {
                return vsField;
            }
            set
            {
                vsField = value;
            }
        }

        /// <remarks/>
        public string Gpin0
        {
            get
            {
                return gpin0Field;
            }
            set
            {
                gpin0Field = value;
            }
        }

        /// <remarks/>
        public string PowerDelay
        {
            get
            {
                return powerDelayField;
            }
            set
            {
                powerDelayField = value;
            }
        }

        /// <remarks/>
        public string InputButton
        {
            get
            {
                return inputButtonField;
            }
            set
            {
                inputButtonField = value;
            }
        }
    }
}

