namespace CheckSystem.HelperForms.GeeleyDx1h
{
    public class Dx1hEmcConfig
    {


        // 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。
        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class DeviceConfig
        {

            private DeviceConfigPara[] parasField;

            private DeviceConfigMotorPara[] motorParasField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Para", IsNullable = false)]
            public DeviceConfigPara[] Paras
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

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("MotorPara", IsNullable = false)]
            public DeviceConfigMotorPara[] MotorParas
            {
                get
                {
                    return this.motorParasField;
                }
                set
                {
                    this.motorParasField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class DeviceConfigPara
        {

            private string nameField;

            private double minField;

            private double maxField;

            /// <remarks/>
            public string Name
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
            public double Min
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
            public double Max
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
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class DeviceConfigMotorPara
        {

            private string nameField;

            private double minField;

            private double maxField;

            /// <remarks/>
            public string Name
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
            public double Min
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
            public double Max
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
        }



    }
}
