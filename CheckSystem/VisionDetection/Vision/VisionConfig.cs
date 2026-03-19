using System.Xml.Serialization;

namespace CheckSystem.VisionDetection.Vision
{
    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class VisionConfig
    {

        private VisionConfigDeviceInfo deviceInfoField;
        private VisionConfigPara[] paraInfoField;
        private VisionConfigTestFlow[] testFlowInfoField;
        private VisionConfigVisionFunc[] visionInfoField;
        private VisionConfigBarcode[] barcodeInfoField;

        /// <remarks/>
        public VisionConfigDeviceInfo DeviceInfo
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

        /// <remarks/>
        [XmlArrayItemAttribute("Para", IsNullable = false)]
        public VisionConfigPara[] ParaInfo
        {
            get
            {
                return paraInfoField;
            }
            set
            {
                paraInfoField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("TestFlow", IsNullable = false)]
        public VisionConfigTestFlow[] TestFlowInfo
        {
            get
            {
                return testFlowInfoField;
            }
            set
            {
                testFlowInfoField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("VisionFunc", IsNullable = false)]
        public VisionConfigVisionFunc[] VisionInfo
        {
            get
            {
                return visionInfoField;
            }
            set
            {
                visionInfoField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Barcode", IsNullable = false)]
        public VisionConfigBarcode[] BarcodeInfo
        {
            get
            {
                return barcodeInfoField;
            }
            set
            {
                barcodeInfoField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigDeviceInfo
    {
        private string deviceNoField;
        private string deviceNo2Field;
        private string deviceNameField;
        private string deviceVersionField;
        private string deviceUpdateTimeField;
        private string deviceGuidField;
        private string deviceLeftOrRightField;
        private VisionConfigDeviceInfoActions actionsField;
        private VisionConfigDeviceInfoCustomContrller[] customContrllerField;

        /// <remarks/>
        public string DeviceGuid
        {
            get
            {
                return deviceGuidField;
            }
            set
            {
                deviceGuidField = value;
            }
        }

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

        public string DeviceNo2
        {
            get
            {
                return deviceNo2Field;
            }
            set
            {
                deviceNo2Field = value;
            }
        }

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

        public string DeviceLeftOrRight
        {
            get
            {
                return deviceLeftOrRightField;
            }
            set
            {
                deviceLeftOrRightField = value;
            }
        }

        /// <remarks/>
        public VisionConfigDeviceInfoActions Actions
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

        /// <remarks/>
        [XmlElementAttribute("CustomContrller")]
        public VisionConfigDeviceInfoCustomContrller[] CustomContrller
        {
            get
            {
                return customContrllerField;
            }
            set
            {
                customContrllerField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigPara
    {

        private string paraNameField;

        private string paraTypeField;

        private string paraLeftOrRightField;

        private VisionConfigParaParaReleysList paraReleysListField;

        private string paraDelayMsField;

        private VisionConfigParaParaMethods paraMethodsField;

        private string paraBindingField;

        private string paraUnitField;

        private VisionConfigParaParaGroup[] paraGroupsField;

        private string powerParaField;

        /// <remarks/>
        public string ParaName
        {
            get
            {
                return paraNameField;
            }
            set
            {
                paraNameField = value;
            }
        }

        /// <remarks/>
        public string ParaType
        {
            get
            {
                return paraTypeField;
            }
            set
            {
                paraTypeField = value;
            }
        }

        /// <remarks/>
        public string ParaLeftOrRight
        {
            get
            {
                return paraLeftOrRightField;
            }
            set
            {
                paraLeftOrRightField = value;
            }
        }

        /// <remarks/>
        public VisionConfigParaParaReleysList ParaReleysList
        {
            get
            {
                return paraReleysListField;
            }
            set
            {
                paraReleysListField = value;
            }
        }

        /// <remarks/>
        public string ParaDelayMs
        {
            get
            {
                return paraDelayMsField;
            }
            set
            {
                paraDelayMsField = value;
            }
        }

        /// <remarks/>
        public VisionConfigParaParaMethods ParaMethods
        {
            get
            {
                return paraMethodsField;
            }
            set
            {
                paraMethodsField = value;
            }
        }

        /// <remarks/>
        public string ParaBinding
        {
            get
            {
                return paraBindingField;
            }
            set
            {
                paraBindingField = value;
            }
        }

        /// <remarks/>
        public string ParaUnit
        {
            get
            {
                return paraUnitField;
            }
            set
            {
                paraUnitField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("ParaGroup", IsNullable = false)]
        public VisionConfigParaParaGroup[] ParaGroups
        {
            get
            {
                return paraGroupsField;
            }
            set
            {
                paraGroupsField = value;
            }
        }

        /// <remarks/>
        public string PowerPara
        {
            get
            {
                return powerParaField;
            }
            set
            {
                powerParaField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigParaParaReleysList
    {

        private string paraReleysOnListField;

        private string paraReleysOffListField;

        /// <remarks/>
        public string ParaReleysOnList
        {
            get
            {
                return paraReleysOnListField;
            }
            set
            {
                paraReleysOnListField = value;
            }
        }

        /// <remarks/>
        public string ParaReleysOffList
        {
            get
            {
                return paraReleysOffListField;
            }
            set
            {
                paraReleysOffListField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigParaParaMethods
    {

        private string paraMethodsBeforeField;

        private string paraMethodsAfterField;

        /// <remarks/>
        public string ParaMethodsBefore
        {
            get
            {
                return paraMethodsBeforeField;
            }
            set
            {
                paraMethodsBeforeField = value;
            }
        }

        /// <remarks/>
        public string ParaMethodsAfter
        {
            get
            {
                return paraMethodsAfterField;
            }
            set
            {
                paraMethodsAfterField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigParaParaGroup
    {

        private string paraGroupNameField;

        private string paraGroupMinField;

        private string paraGroupMaxField;

        private string paraGroupValueField;

        private string paraGroupKField;

        private string paraGroupBField;

        /// <remarks/>
        public string ParaGroupName
        {
            get
            {
                return paraGroupNameField;
            }
            set
            {
                paraGroupNameField = value;
            }
        }

        /// <remarks/>
        public string ParaGroupMin
        {
            get
            {
                return paraGroupMinField;
            }
            set
            {
                paraGroupMinField = value;
            }
        }

        /// <remarks/>
        public string ParaGroupMax
        {
            get
            {
                return paraGroupMaxField;
            }
            set
            {
                paraGroupMaxField = value;
            }
        }

        /// <remarks/>
        public string ParaGroupValue
        {
            get
            {
                return paraGroupValueField;
            }
            set
            {
                paraGroupValueField = value;
            }
        }

        /// <remarks/>
        public string ParaGroupK
        {
            get
            {
                return paraGroupKField;
            }
            set
            {
                paraGroupKField = value;
            }
        }

        /// <remarks/>
        public string ParaGroupB
        {
            get
            {
                return paraGroupBField;
            }
            set
            {
                paraGroupBField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigTestFlow
    {

        private string testFlowValueField;

        private VisionConfigTestFlowTestFlow[] testFlowField;

        /// <remarks/>
        public string TestFlowValue
        {
            get
            {
                return testFlowValueField;
            }
            set
            {
                testFlowValueField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("TestFlow")]
        public VisionConfigTestFlowTestFlow[] TestFlow
        {
            get
            {
                return testFlowField;
            }
            set
            {
                testFlowField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigTestFlowTestFlow
    {

        private string testFlowValueField;

        /// <remarks/>
        public string TestFlowValue
        {
            get
            {
                return testFlowValueField;
            }
            set
            {
                testFlowValueField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigVisionFunc
    {

        private string visionFuncNameField;

        private VisionConfigVisionFuncCamera[] visionFuncDetailLField;

        private VisionConfigVisionFuncCamera[] visionFuncDetailRField;

        /// <remarks/>
        public string VisionFuncName
        {
            get
            {
                return visionFuncNameField;
            }
            set
            {
                visionFuncNameField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Camera", IsNullable = false)]
        public VisionConfigVisionFuncCamera[] VisionFuncDetailL
        {
            get
            {
                return visionFuncDetailLField;
            }
            set
            {
                visionFuncDetailLField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Camera", IsNullable = false)]
        public VisionConfigVisionFuncCamera[] VisionFuncDetailR
        {
            get
            {
                return visionFuncDetailRField;
            }
            set
            {
                visionFuncDetailRField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigVisionFuncCamera
    {
        private string userIdField;
        private string shutterField;
        private string frameCountField;
        private string SequenceTypeField;
        private string SequenceDelayField;
        private VisionConfigVisionFuncCameraAnalysis analysisField;

        /// <remarks/>
        public string UserId
        {
            get
            {
                return userIdField;
            }
            set
            {
                userIdField = value;
            }
        }

        /// <remarks/>
        public string Shutter
        {
            get
            {
                return shutterField;
            }
            set
            {
                shutterField = value;
            }
        }
        public string SequenceType
        {
            get
            {
                return SequenceTypeField;
            }
            set
            {
                SequenceTypeField = value;
            }
        }

        public string SequenceDelay
        {
            get
            {
                return SequenceDelayField;
            }
            set
            {
                SequenceDelayField = value;
            }
        }

        public string FrameCount
        {
            get
            {
                return frameCountField;
            }
            set
            {
                frameCountField = value;
            }
        }

        /// <remarks/>
        public VisionConfigVisionFuncCameraAnalysis Analysis
        {
            get
            {
                return analysisField;
            }
            set
            {
                analysisField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigVisionFuncCameraAnalysis
    {

        private string lookupTableField;

        private string[] caliRegionField;

        private VisionConfigVisionFuncCameraAnalysisShapesShape[][] shapesGroupsField;

        /// <remarks/>
        public string LookupTable
        {
            get
            {
                return lookupTableField;
            }
            set
            {
                lookupTableField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("CaliRegionCounter", IsNullable = false)]
        public string[] CaliRegion
        {
            get
            {
                return caliRegionField;
            }
            set
            {
                caliRegionField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Shapes", IsNullable = false)]
        [XmlArrayItemAttribute("Shape", IsNullable = false, NestingLevel = 1)]
        public VisionConfigVisionFuncCameraAnalysisShapesShape[][] ShapesGroups
        {
            get
            {
                return shapesGroupsField;
            }
            set
            {
                shapesGroupsField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigVisionFuncCameraAnalysisShapesShape
    {

        private string typeField;

        private string rectField;

        private string minField;

        private string maxField;

        private string valueField;

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
        public string Rect
        {
            get
            {
                return rectField;
            }
            set
            {
                rectField = value;
            }
        }

        /// <remarks/>
        public string Min
        {
            get
            {
                return minField;
            }
            set
            {
                minField = value;
            }
        }

        /// <remarks/>
        public string Max
        {
            get
            {
                return maxField;
            }
            set
            {
                maxField = value;
            }
        }

        /// <remarks/>
        public string Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigBarcode
    {

        private string nameField;
        private string lengthField;

        private VisionConfigBarcodeKeyWord keyWordField;

        private string[][] groupsField;

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

        public string Length
        {
            get
            {
                return lengthField;
            }
            set
            {
                lengthField = value;
            }
        }

        /// <remarks/>
        public VisionConfigBarcodeKeyWord KeyWord
        {
            get
            {
                return keyWordField;
            }
            set
            {
                keyWordField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Group", IsNullable = false)]
        [XmlArrayItemAttribute("Value", IsNullable = false, NestingLevel = 1)]
        public string[][] Groups
        {
            get
            {
                return groupsField;
            }
            set
            {
                groupsField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigBarcodeKeyWord
    {

        private VisionConfigBarcodeKeyWordValue valueField;

        private string indexField;

        /// <remarks/>
        public VisionConfigBarcodeKeyWordValue Value
        {
            get
            {
                return valueField;
            }
            set
            {
                valueField = value;
            }
        }

        /// <remarks/>
        public string Index
        {
            get
            {
                return indexField;
            }
            set
            {
                indexField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigBarcodeKeyWordValue
    {

        private string lField;

        private string rField;

        /// <remarks/>
        public string L
        {
            get
            {
                return lField;
            }
            set
            {
                lField = value;
            }
        }

        /// <remarks/>
        public string R
        {
            get
            {
                return rField;
            }
            set
            {
                rField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigDeviceInfoActions
    {

        private VisionConfigDeviceInfoActionsStart startField;

        private VisionConfigDeviceInfoActionsBang bangField;

        private VisionConfigDeviceInfoActionsPropulsionCylinder propulsionCylinderField;

        /// <remarks/>
        public VisionConfigDeviceInfoActionsStart Start
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
        public VisionConfigDeviceInfoActionsBang Bang
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

        public VisionConfigDeviceInfoActionsPropulsionCylinder PropulsionCylinder
        {
            get
            {
                return propulsionCylinderField;
            }
            set
            {
                propulsionCylinderField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigDeviceInfoActionsStart
    {

        private string typeField;

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
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigDeviceInfoActionsBang
    {

        private string isBangField;

        private string delayField;

        /// <remarks/>
        public string IsBang
        {
            get
            {
                return isBangField;
            }
            set
            {
                isBangField = value;
            }
        }

        /// <remarks/>
        public string Delay
        {
            get
            {
                return delayField;
            }
            set
            {
                delayField = value;
            }
        }
    }

    public partial class VisionConfigDeviceInfoActionsPropulsionCylinder
    {
        private string isPropulsionCylinder;
        private string binding;
        private string delay;

        /// <remarks/>
        public string IsPropulsionCylinder
        {
            get
            {
                return isPropulsionCylinder;
            }
            set
            {
                isPropulsionCylinder = value;
            }
        }

        /// <remarks/>
        public string Binding
        {
            get
            {
                return binding;
            }
            set
            {
                binding = value;
            }
        }

        /// <remarks/>
        public string Delay
        {
            get
            {
                return delay;
            }
            set
            {
                delay = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class VisionConfigDeviceInfoCustomContrller
    {

        private string nameField;

        private string typeField;

        private string initParaField;

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
        public string InitPara
        {
            get
            {
                return initParaField;
            }
            set
            {
                initParaField = value;
            }
        }
    }


}
