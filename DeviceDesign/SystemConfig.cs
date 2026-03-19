namespace DeviceDesign
{

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class Systemconfig
    {

        private SystemconfigSysteminfo systeminfoField;

        private SystemconfigFormdesigner formdesignerField;

        private SystemconfigController[] controllersField;

        private SystemconfigSensoractuator[] sensoractuatorsField;

        private SystemconfigProcessparas processparasField;

        private SystemconfigWorkstation[] workstationsField;

        /// <remarks/>
        public SystemconfigSysteminfo systeminfo
        {
            get
            {
                return this.systeminfoField;
            }
            set
            {
                this.systeminfoField = value;
            }
        }

        /// <remarks/>
        public SystemconfigFormdesigner formdesigner
        {
            get
            {
                return this.formdesignerField;
            }
            set
            {
                this.formdesignerField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("controller", IsNullable = false)]
        public SystemconfigController[] controllers
        {
            get
            {
                return this.controllersField;
            }
            set
            {
                this.controllersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("sensoractuator", IsNullable = false)]
        public SystemconfigSensoractuator[] sensoractuators
        {
            get
            {
                return this.sensoractuatorsField;
            }
            set
            {
                this.sensoractuatorsField = value;
            }
        }

        /// <remarks/>
        public SystemconfigProcessparas processparas
        {
            get
            {
                return this.processparasField;
            }
            set
            {
                this.processparasField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("workstation", IsNullable = false)]
        public SystemconfigWorkstation[] workstations
        {
            get
            {
                return this.workstationsField;
            }
            set
            {
                this.workstationsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigSysteminfo
    {

        private string devicenoField;

        private string deviceversionField;

        /// <remarks/>
        public string deviceno
        {
            get
            {
                return this.devicenoField;
            }
            set
            {
                this.devicenoField = value;
            }
        }

        /// <remarks/>
        public string deviceversion
        {
            get
            {
                return this.deviceversionField;
            }
            set
            {
                this.deviceversionField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigFormdesigner
    {

        private string colField;

        private string rowField;

        private string colpercentField;

        private string rowpixField;

        private SystemconfigFormdesignerControl[] controlsField;

        /// <remarks/>
        public string col
        {
            get
            {
                return this.colField;
            }
            set
            {
                this.colField = value;
            }
        }

        /// <remarks/>
        public string row
        {
            get
            {
                return this.rowField;
            }
            set
            {
                this.rowField = value;
            }
        }

        /// <remarks/>
        public string colpercent
        {
            get
            {
                return this.colpercentField;
            }
            set
            {
                this.colpercentField = value;
            }
        }

        /// <remarks/>
        public string rowpix
        {
            get
            {
                return this.rowpixField;
            }
            set
            {
                this.rowpixField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("control", IsNullable = false)]
        public SystemconfigFormdesignerControl[] controls
        {
            get
            {
                return this.controlsField;
            }
            set
            {
                this.controlsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigFormdesignerControl
    {

        private string nameField;

        private string typeField;

        private string colField;

        private string rowField;

        private string colspanField;

        private string rowspanField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string col
        {
            get
            {
                return this.colField;
            }
            set
            {
                this.colField = value;
            }
        }

        /// <remarks/>
        public string row
        {
            get
            {
                return this.rowField;
            }
            set
            {
                this.rowField = value;
            }
        }

        /// <remarks/>
        public string colspan
        {
            get
            {
                return this.colspanField;
            }
            set
            {
                this.colspanField = value;
            }
        }

        /// <remarks/>
        public string rowspan
        {
            get
            {
                return this.rowspanField;
            }
            set
            {
                this.rowspanField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigController
    {

        private string nameField;

        private string typeField;

        private string initparasField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        public string initparas
        {
            get
            {
                return this.initparasField;
            }
            set
            {
                this.initparasField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigSensoractuator
    {

        private string nameField;

        private string datatypeField;

        private string controllerField;

        private string fieldField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string datatype
        {
            get
            {
                return this.datatypeField;
            }
            set
            {
                this.datatypeField = value;
            }
        }

        /// <remarks/>
        public string controller
        {
            get
            {
                return this.controllerField;
            }
            set
            {
                this.controllerField = value;
            }
        }

        /// <remarks/>
        public string field
        {
            get
            {
                return this.fieldField;
            }
            set
            {
                this.fieldField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigProcessparas
    {

        private string nameField;

        private byte processnoField;

        private string parasversionField;

        private SystemconfigProcessparasPara[] parasField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public byte processno
        {
            get
            {
                return this.processnoField;
            }
            set
            {
                this.processnoField = value;
            }
        }

        /// <remarks/>
        public string parasversion
        {
            get
            {
                return this.parasversionField;
            }
            set
            {
                this.parasversionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("para", IsNullable = false)]
        public SystemconfigProcessparasPara[] paras
        {
            get
            {
                return this.parasField;
            }
            set
            {
                this.parasField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigProcessparasPara
    {

        private string nameField;

        private string datatypeField;

        private string okformatField;

        private string valueField;

        private string minField;

        private string maxField;

        private string unitField;

        private string displaycontrolField;

        private string controllerfieldField;

        private string controllerfieldoffsetField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string datatype
        {
            get
            {
                return this.datatypeField;
            }
            set
            {
                this.datatypeField = value;
            }
        }

        /// <remarks/>
        public string okformat
        {
            get
            {
                return this.okformatField;
            }
            set
            {
                this.okformatField = value;
            }
        }

        /// <remarks/>
        public string value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        public string min
        {
            get
            {
                return this.minField;
            }
            set
            {
                this.minField = value;
            }
        }

        /// <remarks/>
        public string max
        {
            get
            {
                return this.maxField;
            }
            set
            {
                this.maxField = value;
            }
        }

        /// <remarks/>
        public string unit
        {
            get
            {
                return this.unitField;
            }
            set
            {
                this.unitField = value;
            }
        }

        /// <remarks/>
        public string displaycontrol
        {
            get
            {
                return this.displaycontrolField;
            }
            set
            {
                this.displaycontrolField = value;
            }
        }

        /// <remarks/>
        public string controllerfield
        {
            get
            {
                return this.controllerfieldField;
            }
            set
            {
                this.controllerfieldField = value;
            }
        }

        /// <remarks/>
        public string controllerfieldoffset
        {
            get
            {
                return this.controllerfieldoffsetField;
            }
            set
            {
                this.controllerfieldoffsetField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigWorkstation
    {

        private string nameField;

        private string initstatusunitField;

        private string currentstatusunitField;

        private SystemconfigWorkstationStatusunit[] statusunitsField;

        private SystemconfigWorkstationCondition[] conditionsField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string initstatusunit
        {
            get
            {
                return this.initstatusunitField;
            }
            set
            {
                this.initstatusunitField = value;
            }
        }

        /// <remarks/>
        public string currentstatusunit
        {
            get
            {
                return this.currentstatusunitField;
            }
            set
            {
                this.currentstatusunitField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("statusunit", IsNullable = false)]
        public SystemconfigWorkstationStatusunit[] statusunits
        {
            get
            {
                return this.statusunitsField;
            }
            set
            {
                this.statusunitsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("condition", IsNullable = false)]
        public SystemconfigWorkstationCondition[] conditions
        {
            get
            {
                return this.conditionsField;
            }
            set
            {
                this.conditionsField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigWorkstationStatusunit
    {

        private string nameField;

        private string noField;

        private string enterfunctionField;

        private string duringfunctionField;

        private string positionsizeField;

        private string enternoteField;

        private string duringnoteField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string no
        {
            get
            {
                return this.noField;
            }
            set
            {
                this.noField = value;
            }
        }

        /// <remarks/>
        public string enterfunction
        {
            get
            {
                return this.enterfunctionField;
            }
            set
            {
                this.enterfunctionField = value;
            }
        }

        /// <remarks/>
        public string duringfunction
        {
            get
            {
                return this.duringfunctionField;
            }
            set
            {
                this.duringfunctionField = value;
            }
        }

        /// <remarks/>
        public string positionsize
        {
            get
            {
                return this.positionsizeField;
            }
            set
            {
                this.positionsizeField = value;
            }
        }

        /// <remarks/>
        public string enternote
        {
            get
            {
                return this.enternoteField;
            }
            set
            {
                this.enternoteField = value;
            }
        }

        /// <remarks/>
        public string duringnote
        {
            get
            {
                return this.duringnoteField;
            }
            set
            {
                this.duringnoteField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class SystemconfigWorkstationCondition
    {

        private string nameField;

        private string sourcestatusnameField;

        private string targetstatusnameField;

        private string conditionfunctionField;

        private string exitfunctionField;

        private string middlepositionField;

        private string conditionnoteField;

        private string exitnoteField;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        public string sourcestatusname
        {
            get
            {
                return this.sourcestatusnameField;
            }
            set
            {
                this.sourcestatusnameField = value;
            }
        }

        /// <remarks/>
        public string targetstatusname
        {
            get
            {
                return this.targetstatusnameField;
            }
            set
            {
                this.targetstatusnameField = value;
            }
        }

        /// <remarks/>
        public string conditionfunction
        {
            get
            {
                return this.conditionfunctionField;
            }
            set
            {
                this.conditionfunctionField = value;
            }
        }

        /// <remarks/>
        public string exitfunction
        {
            get
            {
                return this.exitfunctionField;
            }
            set
            {
                this.exitfunctionField = value;
            }
        }

        /// <remarks/>
        public string middleposition
        {
            get
            {
                return this.middlepositionField;
            }
            set
            {
                this.middlepositionField = value;
            }
        }

        /// <remarks/>
        public string conditionnote
        {
            get
            {
                return this.conditionnoteField;
            }
            set
            {
                this.conditionnoteField = value;
            }
        }

        /// <remarks/>
        public string exitnote
        {
            get
            {
                return this.exitnoteField;
            }
            set
            {
                this.exitnoteField = value;
            }
        }
    }


}
