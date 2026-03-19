using System.Xml.Serialization;

namespace CommonUtility
{
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class LedCheckPara
    {

        private LedCheckParaVisionFuncs visionFuncsField;

        /// <remarks/>
        public LedCheckParaVisionFuncs VisionFuncs
        {
            get
            {
                return visionFuncsField;
            }
            set
            {
                visionFuncsField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class LedCheckParaVisionFuncs
    {

        private string productNameField;

        private LedCheckParaVisionFuncsVisionFunc[] visionFuncField;

        /// <remarks/>
        public string ProductName
        {
            get
            {
                return productNameField;
            }
            set
            {
                productNameField = value;
            }
        }

        /// <remarks/>
        [XmlElementAttribute("VisionFunc")]
        public LedCheckParaVisionFuncsVisionFunc[] VisionFunc
        {
            get
            {
                return visionFuncField;
            }
            set
            {
                visionFuncField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class LedCheckParaVisionFuncsVisionFunc
    {

        private string funcTypeField;

        private ushort funcIndexField;

        private string funcNameField;

        private string funcImgPathField;

        private LedCheckParaVisionFuncsVisionFuncCameraPara cameraParaField;

        private LedCheckParaVisionFuncsVisionFuncVisionPara visionParaField;

        /// <remarks/>
        public string FuncType
        {
            get
            {
                return funcTypeField;
            }
            set
            {
                funcTypeField = value;
            }
        }

        /// <remarks/>
        public ushort FuncIndex
        {
            get
            {
                return funcIndexField;
            }
            set
            {
                funcIndexField = value;
            }
        }

        /// <remarks/>
        public string FuncName
        {
            get
            {
                return funcNameField;
            }
            set
            {
                funcNameField = value;
            }
        }

        /// <remarks/>
        public string FuncImgPath
        {
            get
            {
                return funcImgPathField;
            }
            set
            {
                funcImgPathField = value;
            }
        }

        /// <remarks/>
        public LedCheckParaVisionFuncsVisionFuncCameraPara CameraPara
        {
            get
            {
                return cameraParaField;
            }
            set
            {
                cameraParaField = value;
            }
        }

        /// <remarks/>
        public LedCheckParaVisionFuncsVisionFuncVisionPara VisionPara
        {
            get
            {
                return visionParaField;
            }
            set
            {
                visionParaField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class LedCheckParaVisionFuncsVisionFuncCameraPara
    {

        private string cameraSnField;

        private int shutterField;

        private int delayTimeField;

        private int frameRateField;

        private int frameCountField;

        /// <remarks/>
        public string CameraSn
        {
            get
            {
                return cameraSnField;
            }
            set
            {
                cameraSnField = value;
            }
        }

        /// <remarks/>
        public int Shutter
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

        /// <remarks/>
        public int DelayTime
        {
            get
            {
                return delayTimeField;
            }
            set
            {
                delayTimeField = value;
            }
        }

        /// <remarks/>
        public int FrameRate
        {
            get
            {
                return frameRateField;
            }
            set
            {
                frameRateField = value;
            }
        }

        /// <remarks/>
        public int FrameCount
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
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class LedCheckParaVisionFuncsVisionFuncVisionPara
    {

        private string colorPlaneExtractionField;
        private string lookupTableField;

        private LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup[] shapesGroupsField;

        private LedCheckParaVisionFuncsVisionFuncVisionParaTemplatePara templateParaField;

        /// <remarks/>
        public string ColorPlaneExtraction
        {
            get
            {
                return colorPlaneExtractionField;
            }
            set
            {
                colorPlaneExtractionField = value;
            }
        }

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
        [XmlArrayItemAttribute("ShapesGroup", IsNullable = false)]
        public LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup[] ShapesGroups
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

        /// <remarks/>
        public LedCheckParaVisionFuncsVisionFuncVisionParaTemplatePara TemplatePara
        {
            get
            {
                return templateParaField;
            }
            set
            {
                templateParaField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroup
    {

        private LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape[] shapesField;

        private int groupIndexField;

        /// <remarks/>
        [XmlArrayItemAttribute("Shape", IsNullable = false)]
        public LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape[] Shapes
        {
            get
            {
                return shapesField;
            }
            set
            {
                shapesField = value;
            }
        }

        /// <remarks/>
        public int GroupIndex
        {
            get
            {
                return groupIndexField;
            }
            set
            {
                groupIndexField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShape
    {

        private int indexField;

        private LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour contourField;

        private double minField;

        private double maxField;

        private double valueField;

        /// <remarks/>
        public int Index
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

        /// <remarks/>
        public LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour Contour
        {
            get
            {
                return contourField;
            }
            set
            {
                contourField = value;
            }
        }

        /// <remarks/>
        public double Min
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
        public double Max
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
        public double Value
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
    public partial class LedCheckParaVisionFuncsVisionFuncVisionParaShapesGroupShapeContour
    {

        private string typeField;

        private string rectField;

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
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class LedCheckParaVisionFuncsVisionFuncVisionParaTemplatePara
    {

        //private string templateImgPathField;

        private string templateRoiRectangleField;

        private string templateCanterPointField;

        /// <remarks/>
        //public string TemplateImgPath
        //{
        //    get
        //    {
        //        return this.templateImgPathField;
        //    }
        //    set
        //    {
        //        this.templateImgPathField = value;
        //    }
        //}

        /// <remarks/>
        public string TemplateRoiRectangle
        {
            get
            {
                return templateRoiRectangleField;
            }
            set
            {
                templateRoiRectangleField = value;
            }
        }

        /// <remarks/>
        public string TemplateCanterPoint
        {
            get
            {
                return templateCanterPointField;
            }
            set
            {
                templateCanterPointField = value;
            }
        }
    }
}
