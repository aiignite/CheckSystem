using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace StateMachine
{
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class Systemconfig
    {
        private SystemconfigSysteminfo _systeminfoField;

        private SystemconfigFormdesigner _formdesignerField;

        private SystemconfigController[] _controllersField;

        private SystemconfigSensoractuator[] _sensoractuatorsField;

        private SystemconfigProcessparas _processparasField;

        private SystemconfigWorkstation[] _workstationsField;

        /// <remarks/>
        public SystemconfigSysteminfo systeminfo
        {
            get
            {
                return _systeminfoField;
            }
            set
            {
                _systeminfoField = value;
            }
        }

        /// <remarks/>
        public SystemconfigFormdesigner formdesigner
        {
            get
            {
                return _formdesignerField;
            }
            set
            {
                _formdesignerField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("controller", IsNullable = false)]
        public SystemconfigController[] controllers
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
        [XmlArrayItem("sensoractuator", IsNullable = false)]
        public SystemconfigSensoractuator[] sensoractuators
        {
            get
            {
                return _sensoractuatorsField;
            }
            set
            {
                _sensoractuatorsField = value;
            }
        }

        /// <remarks/>
        public SystemconfigProcessparas processparas
        {
            get
            {
                return _processparasField;
            }
            set
            {
                _processparasField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("workstation", IsNullable = false)]
        public SystemconfigWorkstation[] workstations
        {
            get
            {
                return _workstationsField;
            }
            set
            {
                _workstationsField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigSysteminfo
    {
        private string _devicenoField;

        private string _deviceversionField;

        /// <remarks/>
        public string deviceno
        {
            get
            {
                return _devicenoField;
            }
            set
            {
                _devicenoField = value;
            }
        }

        /// <remarks/>
        public string deviceversion
        {
            get
            {
                return _deviceversionField;
            }
            set
            {
                _deviceversionField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigFormdesigner
    {
        private string _colField;

        private string _rowField;

        private string _colpercentField;

        private string _rowpixField;

        private SystemconfigFormdesignerControl[] _controlsField;

        /// <remarks/>
        public string col
        {
            get
            {
                return _colField;
            }
            set
            {
                _colField = value;
            }
        }

        /// <remarks/>
        public string row
        {
            get
            {
                return _rowField;
            }
            set
            {
                _rowField = value;
            }
        }

        /// <remarks/>
        public string colpercent
        {
            get
            {
                return _colpercentField;
            }
            set
            {
                _colpercentField = value;
            }
        }

        /// <remarks/>
        public string rowpix
        {
            get
            {
                return _rowpixField;
            }
            set
            {
                _rowpixField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("control", IsNullable = false)]
        public SystemconfigFormdesignerControl[] controls
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
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigFormdesignerControl
    {
        private string _nameField;

        private string _typeField;

        private string _colField;

        private string _rowField;

        private string _colspanField;

        private string _rowspanField;

        /// <remarks/>
        public string name
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
        public string type
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
        public string col
        {
            get
            {
                return _colField;
            }
            set
            {
                _colField = value;
            }
        }

        /// <remarks/>
        public string row
        {
            get
            {
                return _rowField;
            }
            set
            {
                _rowField = value;
            }
        }

        /// <remarks/>
        public string colspan
        {
            get
            {
                return _colspanField;
            }
            set
            {
                _colspanField = value;
            }
        }

        /// <remarks/>
        public string rowspan
        {
            get
            {
                return _rowspanField;
            }
            set
            {
                _rowspanField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigController
    {
        private string _nameField;

        private string _typeField;

        private string _initparasField;

        /// <remarks/>
        public string name
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
        public string type
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
        public string initparas
        {
            get
            {
                return _initparasField;
            }
            set
            {
                _initparasField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigSensoractuator
    {
        private string _nameField;

        private string _datatypeField;

        private string _controllerField;

        private string _fieldField;

        /// <remarks/>
        public string name
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
        public string datatype
        {
            get
            {
                return _datatypeField;
            }
            set
            {
                _datatypeField = value;
            }
        }

        /// <remarks/>
        public string controller
        {
            get
            {
                return _controllerField;
            }
            set
            {
                _controllerField = value;
            }
        }

        /// <remarks/>
        public string field
        {
            get
            {
                return _fieldField;
            }
            set
            {
                _fieldField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigProcessparas
    {
        private string _nameField;

        private byte _processnoField;

        private string _parasversionField;

        private SystemconfigProcessparasPara[] _parasField;

        /// <remarks/>
        public string name
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
        public byte processno
        {
            get
            {
                return _processnoField;
            }
            set
            {
                _processnoField = value;
            }
        }

        /// <remarks/>
        public string parasversion
        {
            get
            {
                return _parasversionField;
            }
            set
            {
                _parasversionField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("para", IsNullable = false)]
        public SystemconfigProcessparasPara[] paras
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
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigProcessparasPara
    {
        private string _nameField;

        private string _datatypeField;

        private string _okformatField;

        private string _valueField;

        private string _minField;

        private string _maxField;

        private string _unitField;

        private string _displaycontrolField;

        private string _controllerfieldField;

        private string _controllerfieldoffsetField;

        /// <remarks/>
        public string name
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
        public string datatype
        {
            get
            {
                return _datatypeField;
            }
            set
            {
                _datatypeField = value;
            }
        }

        /// <remarks/>
        public string okformat
        {
            get
            {
                return _okformatField;
            }
            set
            {
                _okformatField = value;
            }
        }

        /// <remarks/>
        public string value
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
        public string min
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
        public string max
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
        public string unit
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
        public string displaycontrol
        {
            get
            {
                return _displaycontrolField;
            }
            set
            {
                _displaycontrolField = value;
            }
        }

        /// <remarks/>
        public string controllerfield
        {
            get
            {
                return _controllerfieldField;
            }
            set
            {
                _controllerfieldField = value;
            }
        }

        /// <remarks/>
        public string controllerfieldoffset
        {
            get
            {
                return _controllerfieldoffsetField;
            }
            set
            {
                _controllerfieldoffsetField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigWorkstation
    {
        private string _nameField;

        private string _initstatusunitField;

        private string _currentstatusunitField;

        private SystemconfigWorkstationStatusunit[] _statusunitsField;

        private SystemconfigWorkstationCondition[] _conditionsField;

        /// <remarks/>
        public string name
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
        public string initstatusunit
        {
            get
            {
                return _initstatusunitField;
            }
            set
            {
                _initstatusunitField = value;
            }
        }

        /// <remarks/>
        public string currentstatusunit
        {
            get
            {
                return _currentstatusunitField;
            }
            set
            {
                _currentstatusunitField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("statusunit", IsNullable = false)]
        public SystemconfigWorkstationStatusunit[] statusunits
        {
            get
            {
                return _statusunitsField;
            }
            set
            {
                _statusunitsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("condition", IsNullable = false)]
        public SystemconfigWorkstationCondition[] conditions
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
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigWorkstationStatusunit
    {
        private string _nameField;

        private string _noField;

        private string _enterfunctionField;

        private string _duringfunctionField;

        private string _positionsizeField;

        private string _enternoteField;

        private string _duringnoteField;

        /// <remarks/>
        public string name
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
        public string no
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
        public string enterfunction
        {
            get
            {
                return _enterfunctionField;
            }
            set
            {
                _enterfunctionField = value;
            }
        }

        /// <remarks/>
        public string duringfunction
        {
            get
            {
                return _duringfunctionField;
            }
            set
            {
                _duringfunctionField = value;
            }
        }

        /// <remarks/>
        public string positionsize
        {
            get
            {
                return _positionsizeField;
            }
            set
            {
                _positionsizeField = value;
            }
        }

        /// <remarks/>
        public string enternote
        {
            get
            {
                return _enternoteField;
            }
            set
            {
                _enternoteField = value;
            }
        }

        /// <remarks/>
        public string duringnote
        {
            get
            {
                return _duringnoteField;
            }
            set
            {
                _duringnoteField = value;
            }
        }
    }

    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    public partial class SystemconfigWorkstationCondition
    {

        private string _nameField;

        private string _sourcestatusnameField;

        private string _targetstatusnameField;

        private string _conditionfunctionField;

        private string _exitfunctionField;

        private string _middlepositionField;

        private string _conditionnoteField;

        private string _exitnoteField;

        /// <remarks/>
        public string name
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
        public string sourcestatusname
        {
            get
            {
                return _sourcestatusnameField;
            }
            set
            {
                _sourcestatusnameField = value;
            }
        }

        /// <remarks/>
        public string targetstatusname
        {
            get
            {
                return _targetstatusnameField;
            }
            set
            {
                _targetstatusnameField = value;
            }
        }

        /// <remarks/>
        public string conditionfunction
        {
            get
            {
                return _conditionfunctionField;
            }
            set
            {
                _conditionfunctionField = value;
            }
        }

        /// <remarks/>
        public string exitfunction
        {
            get
            {
                return _exitfunctionField;
            }
            set
            {
                _exitfunctionField = value;
            }
        }

        /// <remarks/>
        public string middleposition
        {
            get
            {
                return _middlepositionField;
            }
            set
            {
                _middlepositionField = value;
            }
        }

        /// <remarks/>
        public string conditionnote
        {
            get
            {
                return _conditionnoteField;
            }
            set
            {
                _conditionnoteField = value;
            }
        }

        /// <remarks/>
        public string exitnote
        {
            get
            {
                return _exitnoteField;
            }
            set
            {
                _exitnoteField = value;
            }
        }
    }
}
