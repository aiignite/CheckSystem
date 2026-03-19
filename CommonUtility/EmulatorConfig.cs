using System.Xml.Serialization;

namespace CommonUtility
{
    /// <remarks/>
    [XmlType(AnonymousType = true)]
    [XmlRoot(Namespace = "", IsNullable = false)]
    public partial class EmulatorConfig
    {
        private string showTitleField;
        private EmulatorConfigInit initField;
        private EmulatorConfigController[] controllerConfigField;
        private EmulatorConfigButton[] buttonsField;
        private EmulatorConfigGridData[] gridDatasField;

        /// <remarks/>
        public string ShowTitle
        {
            get
            {
                return showTitleField;
            }
            set
            {
                showTitleField = value;
            }
        }

        /// <remarks/>
        public EmulatorConfigInit Init
        {
            get
            {
                return initField;
            }
            set
            {
                initField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("Controller", IsNullable = false)]
        public EmulatorConfigController[] ControllerConfig
        {
            get
            {
                return controllerConfigField;
            }
            set
            {
                controllerConfigField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("Button", IsNullable = false)]
        public EmulatorConfigButton[] Buttons
        {
            get
            {
                return buttonsField;
            }
            set
            {
                buttonsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItem("GridData", IsNullable = false)]
        public EmulatorConfigGridData[] GridDatas
        {
            get
            {
                return gridDatasField;
            }
            set
            {
                gridDatasField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class EmulatorConfigInit
    {
        private string enterFunctionField;

        /// <remarks/>
        public string EnterFunction
        {
            get
            {
                return enterFunctionField;
            }
            set
            {
                enterFunctionField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class EmulatorConfigController
    {
        private string controllerTypeField;
        private string controllerNameField;

        /// <remarks/>
        public string ControllerType
        {
            get
            {
                return controllerTypeField;
            }
            set
            {
                controllerTypeField = value;
            }
        }

        /// <remarks/>
        public string ControllerName
        {
            get
            {
                return controllerNameField;
            }
            set
            {
                controllerNameField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class EmulatorConfigButton
    {
        private string methodField;
        private string textField;
        private object paraField;

        /// <remarks/>
        public string Method
        {
            get
            {
                return methodField;
            }
            set
            {
                methodField = value;
            }
        }

        /// <remarks/>
        public string Text
        {
            get
            {
                return textField;
            }
            set
            {
                textField = value;
            }
        }

        /// <remarks/>
        public object Para
        {
            get
            {
                return paraField;
            }
            set
            {
                paraField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class EmulatorConfigGridData
    {

        private string nameField;

        private string typeField;

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
    }

}
