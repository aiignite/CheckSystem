using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DeviceDesign
{

    /// <remarks/>
    [SerializableAttribute]
    [DesignerCategoryAttribute(@"code")]
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DeviceHardware
    {

        private DeviceHardwareController[] _controllersField;

        private DeviceHardwareConnector[] _connectorsField;

        /// <remarks/>
        [XmlArrayItemAttribute("Controller", IsNullable = false)]
        public DeviceHardwareController[] Controllers
        {
            get
            {
                return _controllersField;
            }
            set
            {
                _controllersField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Connector", IsNullable = false)]
        public DeviceHardwareConnector[] Connectors
        {
            get
            {
                return _connectorsField;
            }
            set
            {
                _connectorsField = value;
            }
        }
    }

    /// <remarks/>
    [SerializableAttribute]
    [DesignerCategoryAttribute(@"code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceHardwareController
    {

        private string _nameField;

        private string _typeField;

        private string _rectangleField;

        private DeviceHardwareControllerLine[] _linesField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return _nameField;
            }
            set
            {
                _nameField = value;
            }
        }

        /// <remarks/>
        public string Type
        {
            get
            {
                return _typeField;
            }
            set
            {
                _typeField = value;
            }
        }

        /// <remarks/>
        public string Rectangle
        {
            get
            {
                return _rectangleField;
            }
            set
            {
                _rectangleField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Line", IsNullable = false)]
        public DeviceHardwareControllerLine[] Lines
        {
            get
            {
                return _linesField;
            }
            set
            {
                _linesField = value;
            }
        }
    }

    /// <remarks/>
    [SerializableAttribute]
    [DesignerCategory(@"code")]
    [XmlType(AnonymousType = true)]
    public partial class DeviceHardwareControllerLine
    {

        private string _nameField;

        private string _indexField;

        private string _isVisibleField;

        /// <remarks/>
        public string Name
        {
            get
            {
                return _nameField;
            }
            set
            {
                _nameField = value;
            }
        }

        /// <remarks/>
        public string Index
        {
            get
            {
                return _indexField;
            }
            set
            {
                _indexField = value;
            }
        }

        /// <remarks/>
        public string IsVisible
        {
            get
            {
                return _isVisibleField;
            }
            set
            {
                _isVisibleField = value;
            }
        }
    }

    /// <remarks/>
    [SerializableAttribute]
    [DesignerCategoryAttribute(@"code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceHardwareConnector
    {

        private object _sourceControllerField;

        private object _targetControllerField;

        private object[] _pointsField;

        /// <remarks/>
        public object SourceController
        {
            get
            {
                return _sourceControllerField;
            }
            set
            {
                _sourceControllerField = value;
            }
        }

        /// <remarks/>
        public object TargetController
        {
            get
            {
                return _targetControllerField;
            }
            set
            {
                _targetControllerField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Point", IsNullable = false)]
        public object[] Points
        {
            get
            {
                return _pointsField;
            }
            set
            {
                _pointsField = value;
            }
        }
    }


}
