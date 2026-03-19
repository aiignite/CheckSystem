using System.Xml.Serialization;

namespace CheckSystem.VisionDetection.Vision
{
    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class VisionDeviceConfig
    {

        private VisionDeviceConfigDeviceInfo deviceInfoField;

        /// <remarks/>
        public VisionDeviceConfigDeviceInfo DeviceInfo
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
    public partial class VisionDeviceConfigDeviceInfo
    {

        private string deviceNoField;

        private string deviceVersionField;

        private string deviceUpdateTimeField;

        private VisionDeviceConfigDeviceInfoControllers controllersField;

        private VisionDeviceConfigDeviceInfoBidings bidingsField;

        private VisionDeviceConfigDeviceInfoActions actionsField;

        /// <remarks/>
        public string DeviceNo
        {
            get
            {
                return deviceNoField;
            }
            set
            {
                deviceNoField = value;
            }
        }

        /// <remarks/>
        public string DeviceVersion
        {
            get
            {
                return deviceVersionField;
            }
            set
            {
                deviceVersionField = value;
            }
        }

        /// <remarks/>
        public string DeviceUpdateTime
        {
            get
            {
                return deviceUpdateTimeField;
            }
            set
            {
                deviceUpdateTimeField = value;
            }
        }

        /// <remarks/>
        public VisionDeviceConfigDeviceInfoControllers Controllers
        {
            get
            {
                return controllersField;
            }
            set
            {
                controllersField = value;
            }
        }

        /// <remarks/>
        public VisionDeviceConfigDeviceInfoBidings Bidings
        {
            get
            {
                return bidingsField;
            }
            set
            {
                bidingsField = value;
            }
        }

        /// <remarks/>
        public VisionDeviceConfigDeviceInfoActions Actions
        {
            get
            {
                return actionsField;
            }
            set
            {
                actionsField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoControllers
    {

        private VisionDeviceConfigDeviceInfoControllersBarcodeScaner[] barcodeScanerField;

        private VisionDeviceConfigDeviceInfoControllersPower[] powerField;

        private VisionDeviceConfigDeviceInfoControllersIoController[] ioControllerField;

        /// <remarks/>
          [XmlElementAttribute("BarcodeScaner")]
        public VisionDeviceConfigDeviceInfoControllersBarcodeScaner[] BarcodeScaner
        {
            get
            {
                return barcodeScanerField;
            }
            set
            {
                barcodeScanerField = value;
            }
        }

        /// <remarks/>
         [XmlElementAttribute("Power")]
        public VisionDeviceConfigDeviceInfoControllersPower[] Power
        {
            get
            {
                return powerField;
            }
            set
            {
                powerField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("IoController")]
        public VisionDeviceConfigDeviceInfoControllersIoController[] IoController
        {
            get
            {
                return ioControllerField;
            }
            set
            {
                ioControllerField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoControllersBarcodeScaner
    {

        private string nameField;

        private string addressField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        public string Address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoControllersPower
    {

        private string nameField;

        private string typeField;

        private string addressField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        public string Address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoControllersIoController
    {

        private string nameField;

        private string typeField;

        private string addressField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return nameField;
            }
            set
            {
                nameField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return typeField;
            }
            set
            {
                typeField = value;
            }
        }

        /// <remarks/>
        public string Address
        {
            get
            {
                return addressField;
            }
            set
            {
                addressField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoBidings
    {

        private VisionDeviceConfigDeviceInfoBidingsVolt[] voltField;

        private VisionDeviceConfigDeviceInfoBidingsCurr[] currField;

        private VisionDeviceConfigDeviceInfoBidingsStartDi startDiField;

        private VisionDeviceConfigDeviceInfoBidingsRelay[] relayField;

        /// <remarks/>
        [XmlElementAttribute("Volt")]
        public VisionDeviceConfigDeviceInfoBidingsVolt[] Volt
        {
            get
            {
                return voltField;
            }
            set
            {
                voltField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("Curr")]
        public VisionDeviceConfigDeviceInfoBidingsCurr[] Curr
        {
            get
            {
                return currField;
            }
            set
            {
                currField = value;
            }
        }

        /// <remarks/>
        public VisionDeviceConfigDeviceInfoBidingsStartDi StartDi
        {
            get
            {
                return startDiField;
            }
            set
            {
                startDiField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("Relay")]
        public VisionDeviceConfigDeviceInfoBidingsRelay[] Relay
        {
            get
            {
                return relayField;
            }
            set
            {
                relayField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoBidingsVolt
    {

        private string controllerField;

        private string fieldField;

        /// <remarks/>
        public string Controller
        {
            get
            {
                return controllerField;
            }
            set
            {
                controllerField = value;
            }
        }

        /// <remarks/>
        public string Field
        {
            get
            {
                return fieldField;
            }
            set
            {
                fieldField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoBidingsCurr
    {

        private string controllerField;

        private string fieldField;

        /// <remarks/>
        public string Controller
        {
            get
            {
                return controllerField;
            }
            set
            {
                controllerField = value;
            }
        }

        /// <remarks/>
        public string Field
        {
            get
            {
                return fieldField;
            }
            set
            {
                fieldField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoBidingsStartDi
    {

        private string controllerField;

        private string fieldField;

        /// <remarks/>
        public string Controller
        {
            get
            {
                return controllerField;
            }
            set
            {
                controllerField = value;
            }
        }

        /// <remarks/>
        public string Field
        {
            get
            {
                return fieldField;
            }
            set
            {
                fieldField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoBidingsRelay
    {

        private string controllerField;

        private string fieldField;

        /// <remarks/>
        public string Controller
        {
            get
            {
                return controllerField;
            }
            set
            {
                controllerField = value;
            }
        }

        /// <remarks/>
        public string Field
        {
            get
            {
                return fieldField;
            }
            set
            {
                fieldField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoActions
    {

        private VisionDeviceConfigDeviceInfoActionsStart startField;

        private VisionDeviceConfigDeviceInfoActionsBang bangField;

        /// <remarks/>
        public VisionDeviceConfigDeviceInfoActionsStart Start
        {
            get
            {
                return startField;
            }
            set
            {
                startField = value;
            }
        }

        /// <remarks/>
        public VisionDeviceConfigDeviceInfoActionsBang Bang
        {
            get
            {
                return bangField;
            }
            set
            {
                bangField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoActionsStart
    {
        private string controllerField;

        private string fieldField;

        /// <remarks/>
        public string Controller
        {
            get
            {
                return controllerField;
            }
            set
            {
                controllerField = value;
            }
        }

        /// <remarks/>
        public string Field
        {
            get
            {
                return fieldField;
            }
            set
            {
                fieldField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionDeviceConfigDeviceInfoActionsBang
    {
        private string controllerField;
        private string fieldField;

        /// <remarks/>
        public string Controller
        {
            get
            {
                return controllerField;
            }
            set
            {
                controllerField = value;
            }
        }

        /// <remarks/>
        public string Field
        {
            get
            {
                return fieldField;
            }
            set
            {
                fieldField = value;
            }
        }
    }
}
