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
            private DeviceConfigTemperaturePara temperatureParaField;
            private DeviceConfigPara[] parasField;
            private DeviceConfigSingleCycleParaCyclePara[][] cycleParasField;
            private DeviceConfigStandbyModePara standbyModeParaField;
            private DeviceConfigDutOffsetHsdOffset[][] offsetParaField;

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
            [System.Xml.Serialization.XmlArrayItemAttribute("SingleCyclePara", IsNullable = false)]
            [System.Xml.Serialization.XmlArrayItemAttribute("CyclePara", IsNullable = false, NestingLevel = 1)]
            public DeviceConfigSingleCycleParaCyclePara[][] CycleParas
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

            /// <remarks/>
            public DeviceConfigStandbyModePara StandbyModePara
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

            /// <remarks/>
            public DeviceConfigTemperaturePara TemperaturePara
            {
                get
                {
                    return this.temperatureParaField;
                }
                set
                {
                    this.temperatureParaField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("DutOffset", IsNullable = false)]
            [System.Xml.Serialization.XmlArrayItemAttribute("HsdOffset", IsNullable = false, NestingLevel = 1)]
            public DeviceConfigDutOffsetHsdOffset[][] OffsetPara
            {
                get
                {
                    return this.offsetParaField;
                }
                set
                {
                    this.offsetParaField = value;
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
        public partial class DeviceConfigSingleCycleParaCyclePara
        {

            private int modeField;

            private double timeField;

            private DeviceConfigSingleCycleParaCycleParaHsd hsdField;

            private DeviceConfigSingleCycleParaCycleParaFan fanField;

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
            public DeviceConfigSingleCycleParaCycleParaHsd Hsd
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
            public DeviceConfigSingleCycleParaCycleParaFan Fan
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
        public partial class DeviceConfigSingleCycleParaCycleParaHsd
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
        public partial class DeviceConfigSingleCycleParaCycleParaFan
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
        public partial class DeviceConfigStandbyModePara
        {

            private DeviceConfigStandbyModeParaHsd[] hsdsField;

            private DeviceConfigStandbyModeParaFan[] fansField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Hsd", IsNullable = false)]
            public DeviceConfigStandbyModeParaHsd[] Hsds
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
            public DeviceConfigStandbyModeParaFan[] Fans
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
        public partial class DeviceConfigStandbyModeParaHsd
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
        public partial class DeviceConfigStandbyModeParaFan
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
        public partial class DeviceConfigTemperaturePara
        {

            private string isEnableField;

            private double valueField;

            private string sysmbolField;

            private int ntcChannelField;

            /// <remarks/>
            public string IsEnable
            {
                get
                {
                    return this.isEnableField;
                }
                set
                {
                    this.isEnableField = value;
                }
            }

            /// <remarks/>
            public double Value
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
            public string Sysmbol
            {
                get
                {
                    return this.sysmbolField;
                }
                set
                {
                    this.sysmbolField = value;
                }
            }

            /// <remarks/>
            public int NtcChannel
            {
                get
                {
                    return this.ntcChannelField;
                }
                set
                {
                    this.ntcChannelField = value;
                }
            }
        }

        /// <remarks/>
        [System.SerializableAttribute()]
        [System.ComponentModel.DesignerCategoryAttribute("code")]
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class DeviceConfigDutOffsetHsdOffset
        {

            private double kField;

            private double bField;

            private string isEnableField;

            private double thresholdField;

            private double showValueField;

            /// <remarks/>
            public double K
            {
                get
                {
                    return this.kField;
                }
                set
                {
                    this.kField = value;
                }
            }

            /// <remarks/>
            public double B
            {
                get
                {
                    return this.bField;
                }
                set
                {
                    this.bField = value;
                }
            }

            /// <remarks/>
            public string IsEnable
            {
                get
                {
                    return this.isEnableField;
                }
                set
                {
                    this.isEnableField = value;
                }
            }

            /// <remarks/>
            public double Threshold
            {
                get
                {
                    return this.thresholdField;
                }
                set
                {
                    this.thresholdField = value;
                }
            }

            /// <remarks/>
            public double ShowValue
            {
                get
                {
                    return this.showValueField;
                }
                set
                {
                    this.showValueField = value;
                }
            }
        }
    }
}
