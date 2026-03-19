using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace CommonUtility
{

    // 注意: 生成的代码可能至少需要 .NET Framework 4.5 或 .NET Core/Standard 2.0。
    /// <remarks/>
    [Serializable]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true)]
    [XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class ecuRecordList
    {
        private ecuRecordListEcuRecord[] _ecuRecordField;

        /// <remarks/>
        [XmlElementAttribute("ecuRecord")]
        public ecuRecordListEcuRecord[] ecuRecord
        {
            get
            {
                return _ecuRecordField;
            }
            set
            {
                _ecuRecordField = value;
            }
        }
    }

    /// <remarks/>
    [SerializableAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(AnonymousType = true)]
    public partial class ecuRecordListEcuRecord
    {
        private string _ecuidField;
        private string _serialNoField;
        private string _mkm4Field;
        private string _mkm5Field;
        private string _ukm4Field;
        private string _ukm5Field;

        /// <summary>
        /// ECUID
        /// Convert.ToBase64String(GetByteArray(ecuIdStruct.EucIdHex, false)),
        /// </summary>
        public string ecuid
        {
            get
            {
                return _ecuidField;
            }
            set
            {
                _ecuidField = value;
            }
        }

        /// <summary>
        /// 生产序列号
        /// ValueHelper.GetHextStr(Encoding.ASCII.GetBytes(ecuIdStruct.TrackInfo)).Replace(" ", ""),
        /// </summary>
        [XmlElementAttribute(DataType = "integer")]
        public string serialNo
        {
            get
            {
                return _serialNoField;
            }
            set
            {
                _serialNoField = value;
            }
        }

        /// <summary>
        /// MasterKey-M4
        /// Convert.ToBase64String(GetByteArray(ecuIdStruct.MasterEcuKeyM4Hex, false)),
        /// </summary>
        public string mkm4
        {
            get
            {
                return _mkm4Field;
            }
            set
            {
                _mkm4Field = value;
            }
        }

        /// <summary>
        /// MasterKey-M5
        /// Convert.ToBase64String(GetByteArray(ecuIdStruct.MasterEcuKeyM5Hex, false)),
        /// </summary>
        public string mkm5
        {
            get
            {
                return _mkm5Field;
            }
            set
            {
                _mkm5Field = value;
            }
        }

        /// <summary>
        /// UnlockKey-M4
        /// Convert.ToBase64String(GetByteArray(ecuIdStruct.UnlockEcuKeyM4Hex, false)),
        /// </summary>
        public string ukm4
        {
            get
            {
                return _ukm4Field;
            }
            set
            {
                _ukm4Field = value;
            }
        }

        /// <summary>
        /// UnlockKey-M5
        /// Convert.ToBase64String(GetByteArray(ecuIdStruct.UnlockEcuKeyM5Hex, false))
        /// </summary>
        public string ukm5
        {
            get
            {
                return _ukm5Field;
            }
            set
            {
                _ukm5Field = value;
            }
        }
    }
}
