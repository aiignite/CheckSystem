using System.Xml.Serialization;

namespace CommonUtility
{

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class RobotJogging
    {

        private RobotJoggingRobotProgram[] robotProgramsField;

        private RobotJoggingRobotPallet[] robotPalletsField;

        /// <remarks/>
        [XmlArrayItemAttribute("RobotProgram", IsNullable = false)]
        public RobotJoggingRobotProgram[] RobotPrograms
        {
            get
            {
                return robotProgramsField;
            }
            set
            {
                robotProgramsField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("RobotPallet", IsNullable = false)]
        public RobotJoggingRobotPallet[] RobotPallets
        {
            get
            {
                return robotPalletsField;
            }
            set
            {
                robotPalletsField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class RobotJoggingRobotProgram
    {

        private string robotNameField;

        private string nameField;

        private string noteField;

        private RobotJoggingRobotProgramAxis[] axisListField;

        private string[] blocksField;

        /// <remarks/>
        public string RobotName
        {
            get
            {
                return robotNameField;
            }
            set
            {
                robotNameField = value;
            }
        }

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
        public string Note
        {
            get
            {
                return noteField;
            }
            set
            {
                noteField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Axis", IsNullable = false)]
        public RobotJoggingRobotProgramAxis[] AxisList
        {
            get
            {
                return axisListField;
            }
            set
            {
                axisListField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Block", IsNullable = false)]
        public string[] Blocks
        {
            get
            {
                return blocksField;
            }
            set
            {
                blocksField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class RobotJoggingRobotProgramAxis
    {

        private string axisNoField;

        private string axisNoteField;

        /// <remarks/>
        public string AxisNo
        {
            get
            {
                return axisNoField;
            }
            set
            {
                axisNoField = value;
            }
        }

        /// <remarks/>
        public string AxisNote
        {
            get
            {
                return axisNoteField;
            }
            set
            {
                axisNoteField = value;
            }
        }
    }

    /// <remarks/>
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class RobotJoggingRobotPallet
    {

        private string pattetIndexField;

        private string pattetNameField;

        private string[] blocksField;

        /// <remarks/>
        public string PattetIndex
        {
            get
            {
                return pattetIndexField;
            }
            set
            {
                pattetIndexField = value;
            }
        }

        /// <remarks/>
        public string PattetName
        {
            get
            {
                return pattetNameField;
            }
            set
            {
                pattetNameField = value;
            }
        }

        /// <remarks/>
        [XmlArrayItemAttribute("Block", IsNullable = false)]
        public string[] Blocks
        {
            get
            {
                return blocksField;
            }
            set
            {
                blocksField = value;
            }
        }
    }
}
