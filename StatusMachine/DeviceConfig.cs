using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace StateMachine
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class DeviceConfig
    {
        private DeviceConfigDeviceInfo _deviceInfoField;

        private DeviceConfigFormLayout _formLayoutField;

        private DeviceConfigControl[] _controlsField;

        private DeviceConfigController[] _controllersField;

        private DeviceConfigProperty[] _controllerPropertiesField;

        private DeviceConfigPart[] _partsField;

        private DeviceConfigProcess[] _processesField;

        private DeviceConfigPara[] _parasField;

        private DeviceConfigGear[] _gearsField;

        private DeviceConfigRoi[] _roisField;

        private DeviceConfigWorkStation[] _workStationsField;

        private DeviceConfigStatusUnit[] _statusUnitsField;

        private DeviceConfigCondition[] _conditionsField;

        /// <remarks/>
        public DeviceConfigDeviceInfo DeviceInfo
        {
            get
            {
                return _deviceInfoField;
            }
            set
            {
                _deviceInfoField = value;
            }
        }

        /// <remarks/>
        public DeviceConfigFormLayout FormLayout
        {
            get
            {
                return _formLayoutField;
            }
            set
            {
                _formLayoutField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Control", IsNullable = false)]
        public DeviceConfigControl[] Controls
        {
            get
            {
                return _controlsField;
            }
            set
            {
                _controlsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Controller", IsNullable = false)]
        public DeviceConfigController[] Controllers
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
        [XmlArrayItemAttribute("Property", IsNullable = false)]
        public DeviceConfigProperty[] ControllerProperties
        {
            get
            {
                return _controllerPropertiesField;
            }
            set
            {
                _controllerPropertiesField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Part", IsNullable = false)]
        public DeviceConfigPart[] Parts
        {
            get
            {
                return _partsField;
            }
            set
            {
                _partsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Process", IsNullable = false)]
        public DeviceConfigProcess[] Processes
        {
            get
            {
                return _processesField;
            }
            set
            {
                _processesField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Para", IsNullable = false)]
        public DeviceConfigPara[] Paras
        {
            get
            {
                return _parasField;
            }
            set
            {
                _parasField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Gear", IsNullable = false)]
        public DeviceConfigGear[] Gears
        {
            get
            {
                return _gearsField;
            }
            set
            {
                _gearsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Roi", IsNullable = false)]
        public DeviceConfigRoi[] Rois
        {
            get
            {
                return _roisField;
            }
            set
            {
                _roisField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("WorkStation", IsNullable = false)]
        public DeviceConfigWorkStation[] WorkStations
        {
            get
            {
                return _workStationsField;
            }
            set
            {
                _workStationsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("StatusUnit", IsNullable = false)]
        public DeviceConfigStatusUnit[] StatusUnits
        {
            get
            {
                return _statusUnitsField;
            }
            set
            {
                _statusUnitsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Condition", IsNullable = false)]
        public DeviceConfigCondition[] Conditions
        {
            get
            {
                return _conditionsField;
            }
            set
            {
                _conditionsField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigDeviceInfo
    {
        private string _deviceNoField;
        private string _deviceNameFiled;
        private string _deviceVersionField;
        private string _deviceUpdateTimeField;
        private string _guidField;

        /// <remarks/>
        public string DeviceNo
        {
            get
            {
                return _deviceNoField;
            }
            set
            {
                _deviceNoField = value;
            }
        }

        public string DeviceName
        {
            get
            {
                return _deviceNameFiled;
            }
            set
            {
                _deviceNameFiled = value;
            }
        }

        /// <remarks/>
        public string DeviceVersion
        {
            get
            {
                return _deviceVersionField;
            }
            set
            {
                _deviceVersionField = value;
            }
        }

        /// <remarks/>
        [DisplayName("@LDateTimePicker:")]

        public string DeviceUpdateTime
        {
            get
            {
                return _deviceUpdateTimeField;
            }
            set
            {
                _deviceUpdateTimeField = value;
            }
        }


        /// <remarks/>
        public string Guid
        {
            get
            {
                return _guidField;
            }
            set
            {
                _guidField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigFormLayout
    {

        private string _columnCountField;

        private string _rowCountField;

        private string _columnPercentField;

        private string _rowPixCountField;

        /// <remarks/>
        public string ColumnCount
        {
            get
            {
                return _columnCountField;
            }
            set
            {
                _columnCountField = value;
            }
        }

        /// <remarks/>
        public string RowCount
        {
            get
            {
                return _rowCountField;
            }
            set
            {
                _rowCountField = value;
            }
        }

        /// <remarks/>
        public string ColumnPercent
        {
            get
            {
                return _columnPercentField;
            }
            set
            {
                _columnPercentField = value;
            }
        }

        /// <remarks/>
        public string RowPixCount
        {
            get
            {
                return _rowPixCountField;
            }
            set
            {
                _rowPixCountField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigControl
    {

        private string _nameField;

        private string _typeField;

        private string _rowPositionField;

        private string _columnPositionField;

        private string _rowSpanField;

        private string _columnSpanField;

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
        public string RowPosition
        {
            get
            {
                return _rowPositionField;
            }
            set
            {
                _rowPositionField = value;
            }
        }

        /// <remarks/>
        public string ColumnPosition
        {
            get
            {
                return _columnPositionField;
            }
            set
            {
                _columnPositionField = value;
            }
        }

        /// <remarks/>
        public string RowSpan
        {
            get
            {
                return _rowSpanField;
            }
            set
            {
                _rowSpanField = value;
            }
        }

        /// <remarks/>
        public string ColumnSpan
        {
            get
            {
                return _columnSpanField;
            }
            set
            {
                _columnSpanField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigController
    {

        private string _nameField;

        private string _typeField;

        private string _noteField;

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
        [DisplayName("@LComboBox:ControllerTypes")]
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
        public string Note
        {
            get
            {
                return _noteField;
            }
            set
            {
                _noteField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable()]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigProperty
    {

        private string _controllerNameField;

        private string _nameField;

        private string _valueField;

        private string _noteField;

        /// <remarks/>
        public string ControllerName
        {
            get
            {
                return _controllerNameField;
            }
            set
            {
                _controllerNameField = value;
            }
        }

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
        public string Value
        {
            get
            {
                return _valueField;
            }
            set
            {
                _valueField = value;
            }
        }

        /// <remarks/>
        public string Note
        {
            get
            {
                return _noteField;
            }
            set
            {
                _noteField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigPart
    {
        private string _processNoField;
        private string _nameField;

        private string _dataTypeField;

        private string _controllerNameField;

        private string _controllerFieldField;

        public string ProcessNo
        {
            get
            {
                return _processNoField;
            }
            set
            {
                _processNoField = value;
            }
        }

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
        public string DataType
        {
            get
            {
                return _dataTypeField;
            }
            set
            {
                _dataTypeField = value;
            }
        }

        /// <remarks/>
        public string ControllerName
        {
            get
            {
                return _controllerNameField;
            }
            set
            {
                _controllerNameField = value;
            }
        }

        /// <remarks/>
        public string ControllerField
        {
            get
            {
                return _controllerFieldField;
            }
            set
            {
                _controllerFieldField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigProcess
    {

        private string _nameField;

        private string _noField;

        private string _productNameField;

        private string _productNoField;

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
        public string No
        {
            get
            {
                return _noField;
            }
            set
            {
                _noField = value;
            }
        }

        /// <remarks/>
        public string ProductName
        {
            get
            {
                return _productNameField;
            }
            set
            {
                _productNameField = value;
            }
        }

        /// <remarks/>
        public string ProductNo
        {
            get
            {
                return _productNoField;
            }
            set
            {
                _productNoField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigPara
    {
        private string _processNoField;
        private string _nameField;
        private string _dataTypeField;
        private string _okFormatField;
        private string _valueField;
        private string _minField;
        private string _maxField;
        private string _unitField;
        private string _controlNameField;
        private string _controllerFieldField;
        private string _controllerFieldOffsetField;

        /// <remarks/>
        public string ProcessNo
        {
            get
            {
                return _processNoField;
            }
            set
            {
                _processNoField = value;
            }
        }

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
        public string DataType
        {
            get
            {
                return _dataTypeField;
            }
            set
            {
                _dataTypeField = value;
            }
        }

        /// <remarks/>
        public string OkFormat
        {
            get
            {
                return _okFormatField;
            }
            set
            {
                _okFormatField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return _valueField;
            }
            set
            {
                _valueField = value;
            }
        }

        /// <remarks/>
        public string Min
        {
            get
            {
                return _minField;
            }
            set
            {
                _minField = value;
            }
        }

        /// <remarks/>
        public string Max
        {
            get
            {
                return _maxField;
            }
            set
            {
                _maxField = value;
            }
        }

        /// <remarks/>
        public string Unit
        {
            get
            {
                return _unitField;
            }
            set
            {
                _unitField = value;
            }
        }

        /// <remarks/>
        public string ControlName
        {
            get
            {
                return _controlNameField;
            }
            set
            {
                _controlNameField = value;
            }
        }

        /// <remarks/>
        public string ControllerField
        {
            get
            {
                return _controllerFieldField;
            }
            set
            {
                _controllerFieldField = value;
            }
        }

        /// <remarks/>
        public string ControllerFieldOffset
        {
            get
            {
                return _controllerFieldOffsetField;
            }
            set
            {
                _controllerFieldOffsetField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigGear
    {

        private string _nameField;

        private string _matchingCodeContentField;

        private string _matchingCodeIndexField;

        private string _gear1ContentField;

        private string _gear1IndexField;

        private string _gear2ContentField;

        private string _gear2IndexField;

        private string _gear3ContentField;

        private string _gear3IndexField;

        private string _gear4ContentField;

        private string _gear4IndexField;

        private string _gear5ContentField;

        private string _gear5IndexField;

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
        public string MatchingCodeContent
        {
            get
            {
                return _matchingCodeContentField;
            }
            set
            {
                _matchingCodeContentField = value;
            }
        }

        /// <remarks/>
        public string MatchingCodeIndex
        {
            get
            {
                return _matchingCodeIndexField;
            }
            set
            {
                _matchingCodeIndexField = value;
            }
        }

        /// <remarks/>
        public string Gear1Content
        {
            get
            {
                return _gear1ContentField;
            }
            set
            {
                _gear1ContentField = value;
            }
        }

        /// <remarks/>
        public string Gear1Index
        {
            get
            {
                return _gear1IndexField;
            }
            set
            {
                _gear1IndexField = value;
            }
        }

        /// <remarks/>
        public string Gear2Content
        {
            get
            {
                return _gear2ContentField;
            }
            set
            {
                _gear2ContentField = value;
            }
        }

        /// <remarks/>
        public string Gear2Index
        {
            get
            {
                return _gear2IndexField;
            }
            set
            {
                _gear2IndexField = value;
            }
        }

        /// <remarks/>
        public string Gear3Content
        {
            get
            {
                return _gear3ContentField;
            }
            set
            {
                _gear3ContentField = value;
            }
        }

        /// <remarks/>
        public string Gear3Index
        {
            get
            {
                return _gear3IndexField;
            }
            set
            {
                _gear3IndexField = value;
            }
        }

        /// <remarks/>
        public string Gear4Content
        {
            get
            {
                return _gear4ContentField;
            }
            set
            {
                _gear4ContentField = value;
            }
        }

        /// <remarks/>
        public string Gear4Index
        {
            get
            {
                return _gear4IndexField;
            }
            set
            {
                _gear4IndexField = value;
            }
        }

        /// <remarks/>
        public string Gear5Content
        {
            get
            {
                return _gear5ContentField;
            }
            set
            {
                _gear5ContentField = value;
            }
        }

        /// <remarks/>
        public string Gear5Index
        {
            get
            {
                return _gear5IndexField;
            }
            set
            {
                _gear5IndexField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigRoi 
    {
        private string _nameField = string.Empty;
        private string _lookUpTypeField = string.Empty;
        private string _groupField = string.Empty;
        private string _rectXField = string.Empty;
        private string _rectYField = string.Empty;
        private string _rectWidthField = string.Empty;
        private string _rectHeightField = string.Empty;
        private string _minField = string.Empty;
        private string _maxField = string.Empty;

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

        //public string LookUpTypeField 
        //{
        //    get
        //    {
        //        return _lookUpTypeField;
        //    }
        //    set
        //    {
        //        _lookUpTypeField = value;
        //    }
        //}

        public string Group
        {
            get
            {
                return _groupField;
            }
            set
            {
                _groupField = value;
            }
        }

        public string RectX
        {
            get
            {
                return _rectXField;
            }
            set
            {
                _rectXField = value;
            }
        }

        public string RectY
        {
            get
            {
                return _rectYField;
            }
            set
            {
                _rectYField = value;
            }
        }

        public string RectWidth
        {
            get
            {
                return _rectWidthField;
            }
            set
            {
                _rectWidthField = value;
            }
        }

        public string RectHeight
        {
            get
            {
                return _rectHeightField;
            }
            set
            {
                _rectHeightField = value;
            }
        }

        public string Min
        {
            get
            {
                return _minField;
            }
            set
            {
                _minField = value;
            }
        }

        public string Max
        {
            get
            {
                return _maxField;
            }
            set
            {
                _maxField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigWorkStation
    {

        private string _nameField;

        //private string _initStatusUnitField;

        //private string _currentStatusUnitField;

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

        ///// <remarks/>
        //public string InitStatusUnit
        //{
        //    get
        //    {
        //        return _initStatusUnitField;
        //    }
        //    set
        //    {
        //        _initStatusUnitField = value;
        //    }
        //}

        ///// <remarks/>
        //public string CurrentStatusUnit
        //{
        //    get
        //    {
        //        return _currentStatusUnitField;
        //    }
        //    set
        //    {
        //        _currentStatusUnitField = value;
        //    }
        //}
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigStatusUnit
    {

        private string _workStationNameField;

        private string _nameField;

        private string _enterFunctionField;

        private string _duringFunctionField;

        private string _positionSizeField;

        private string _timeOutField;

        public string EnterNote;
        public string DuringNote;

        /// <remarks/>
        public string WorkStationName
        {
            get
            {
                return _workStationNameField;
            }
            set
            {
                _workStationNameField = value;
            }
        }

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
        public string EnterFunction
        {
            get
            {
                return _enterFunctionField;
            }
            set
            {
                _enterFunctionField = value;
            }
        }

        /// <remarks/>
        public string DuringFunction
        {
            get
            {
                return _duringFunctionField;
            }
            set
            {
                _duringFunctionField = value;
            }
        }

        public string PositionSize
        {
            get { return _positionSizeField; }
            set { _positionSizeField = value; }
        }

        /// <remarks/>
        public string TimeOut
        {
            get
            {
                return _timeOutField;
            }
            set
            {
                _timeOutField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class DeviceConfigCondition
    {

        private string _workStationNameField;

        private string _nameField;

        private string _sourceSuNameField;

        private string _targetSuNameField;

        private string _conditionFunctionField;

        private string _conditionNoteField;

        private string _exitFunctionField;

        private string _exitNoteField;

        private string _middlePisitonField;

        /// <remarks/>
        public string WorkStationName
        {
            get
            {
                return _workStationNameField;
            }
            set
            {
                _workStationNameField = value;
            }
        }

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
        public string SourceSuName
        {
            get
            {
                return _sourceSuNameField;
            }
            set
            {
                _sourceSuNameField = value;
            }
        }

        /// <remarks/>
        public string TargetSuName
        {
            get
            {
                return _targetSuNameField;
            }
            set
            {
                _targetSuNameField = value;
            }
        }

        /// <remarks/>
        public string ConditionFunction
        {
            get
            {
                return _conditionFunctionField;
            }
            set
            {
                _conditionFunctionField = value;
            }
        }

        /// <remarks/>
        public string ConditionNote
        {
            get
            {
                return _conditionNoteField;
            }
            set
            {
                _conditionNoteField = value;
            }
        }

        /// <remarks/>
        public string ExitFunction
        {
            get
            {
                return _exitFunctionField;
            }
            set
            {
                _exitFunctionField = value;
            }
        }

        /// <remarks/>
        public string ExitNote
        {
            get
            {
                return _exitNoteField;
            }
            set
            {
                _exitNoteField = value;
            }
        }

        /// <remarks/>
        public string MiddlePisiton
        {
            get
            {
                return _middlePisitonField;
            }
            set
            {
                _middlePisitonField = value;
            }
        }
    }


}
