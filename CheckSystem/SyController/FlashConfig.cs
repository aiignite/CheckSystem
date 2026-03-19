using System.Xml.Serialization;

namespace CheckSystem.SyController
{

    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class FlashConfig
    {

        private FlashConfigFlash[] flashField;

        /// <remarks/>
        [XmlElementAttribute("Flash")]
        public FlashConfigFlash[] Flash
        {
            get
            {
                return flashField;
            }
            set
            {
                flashField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class FlashConfigFlash
    {

        private string contentField;

        private string parameterNameField;

        private string startByteField;

        private string byteLengthField;

        private string isReserveField;

        private string dataTypeField;

        /// <remarks/>
        public string Content
        {
            get
            {
                return contentField;
            }
            set
            {
                contentField = value;
            }
        }

        /// <remarks/>
        public string ParameterName
        {
            get
            {
                return parameterNameField;
            }
            set
            {
                parameterNameField = value;
            }
        }

        /// <remarks/>
        public string StartByte
        {
            get
            {
                return startByteField;
            }
            set
            {
                startByteField = value;
            }
        }

        /// <remarks/>
        public string ByteLength
        {
            get
            {
                return byteLengthField;
            }
            set
            {
                byteLengthField = value;
            }
        }

        /// <remarks/>
        public string IsReserve
        {
            get
            {
                return isReserveField;
            }
            set
            {
                isReserveField = value;
            }
        }

        /// <remarks/>
        public string DataType
        {
            get
            {
                return dataTypeField;
            }
            set
            {
                dataTypeField = value;
            }
        }
    }
}
