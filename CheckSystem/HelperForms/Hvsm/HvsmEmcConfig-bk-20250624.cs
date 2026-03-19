namespace CheckSystem.HelperForms.Hvsm
{
    public class HvsmEmcConfig
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

            private DeviceConfigCyclePara[] cycleParasField;

            private StandbyModePara standbyModeParaField;

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
            [System.Xml.Serialization.XmlArrayItemAttribute("CyclePara", IsNullable = false)]
            public DeviceConfigCyclePara[] CycleParas
            {
                get
                {
                    return this.cycleParasField;
                }
                set
                {
                    this.cycleParasField = value;
                }
            }

            public StandbyModePara StandbyModePara
            {
                get
                {
                    return this.standbyModeParaField;
                }
                set
                {
                    this.standbyModeParaField = value;
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

            private string dataTypeField;

            private string okFormatField;

            private string valueField;

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
            public string DataType
            {
                get
                {
                    return this.dataTypeField;
                }
                set
                {
                    this.dataTypeField = value;
                }
            }

            /// <remarks/>
            public string OkFormat
            {
                get
                {
                    return this.okFormatField;
                }
                set
                {
                    this.okFormatField = value;
                }
            }

            /// <remarks/>
            public string Value
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
        public partial class DeviceConfigCyclePara
        {

            private int modeField;

            private double timeField;

            private DeviceConfigCycleParaHsd hsdField;

            private DeviceConfigCycleParaFan fanField;

            /// <remarks/>
            public int Mode
            {
                get
                {
                    return this.modeField;
                }
                set
                {
                    this.modeField = value;
                }
            }

            /// <remarks/>
            public double Time
            {
                get
                {
                    return this.timeField;
                }
                set
                {
                    this.timeField = value;
                }
            }

            /// <remarks/>
            public DeviceConfigCycleParaHsd Hsd
            {
                get
                {
                    return this.hsdField;
                }
                set
                {
                    this.hsdField = value;
                }
            }

            /// <remarks/>
            public DeviceConfigCycleParaFan Fan
            {
                get
                {
                    return this.fanField;
                }
                set
                {
                    this.fanField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class DeviceConfigCycleParaHsd
        {

            private int dutyField;

            private int freqField;

            /// <remarks/>
            public int Duty
            {
                get
                {
                    return this.dutyField;
                }
                set
                {
                    this.dutyField = value;
                }
            }

            /// <remarks/>
            public int Freq
            {
                get
                {
                    return this.freqField;
                }
                set
                {
                    this.freqField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class DeviceConfigCycleParaFan
        {

            private int dutyField;

            /// <remarks/>
            public int Duty
            {
                get
                {
                    return this.dutyField;
                }
                set
                {
                    this.dutyField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class StandbyModePara
        {

            private StandbyModeParaHsd[] hsdsField;

            private StandbyModeParaFan[] fansField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Hsd", IsNullable = false)]
            public StandbyModeParaHsd[] Hsds
            {
                get
                {
                    return this.hsdsField;
                }
                set
                {
                    this.hsdsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Fan", IsNullable = false)]
            public StandbyModeParaFan[] Fans
            {
                get
                {
                    return this.fansField;
                }
                set
                {
                    this.fansField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class StandbyModeParaHsd
        {
            private int dutyField;

            private int freqField;

            /// <remarks/>
            public int Duty
            {
                get
                {
                    return this.dutyField;
                }
                set
                {
                    this.dutyField = value;
                }
            }

            /// <remarks/>
            public int Freq
            {
                get
                {
                    return this.freqField;
                }
                set
                {
                    this.freqField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class StandbyModeParaFan
        {

            private int dutyField;

            /// <remarks/>
            public int Duty
            {
                get
                {
                    return this.dutyField;
                }
                set
                {
                    this.dutyField = value;
                }
            }
        }
    }
}
