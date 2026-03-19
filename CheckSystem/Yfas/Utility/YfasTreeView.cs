using System.Xml.Serialization;

namespace CheckSystem.Yfas.Utility
{
    public class YfasTreeView
    {
        /// <remarks/>
        [XmlType(AnonymousType = true)]
        [XmlRootAttribute(Namespace = "", IsNullable = false)]
        public partial class TreeView
        {

            private TreeViewNode nodeField;

            /// <remarks/>
            public TreeViewNode node
            {
                get
                {
                    return nodeField;
                }
                set
                {
                    nodeField = value;
                }
            }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class TreeViewNode
        {

            private TreeViewNodeNode[] nodeField;

            private string textField;

            private sbyte imageindexField;

            private string explandField;

            private byte indexField;

            /// <remarks/>
            [XmlElementAttribute("node")]
            public TreeViewNodeNode[] node
            {
                get
                {
                    return nodeField;
                }
                set
                {
                    nodeField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string text
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
            [XmlAttributeAttribute]
            public sbyte imageindex
            {
                get
                {
                    return imageindexField;
                }
                set
                {
                    imageindexField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string expland
            {
                get
                {
                    return explandField;
                }
                set
                {
                    explandField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte index
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
        public partial class TreeViewNodeNode
        {

            private TreeViewNodeNodeNode[] nodeField;

            private string textField;

            private byte imageindexField;

            private string explandField;

            private byte indexField;

            private string lastselectField;

            /// <remarks/>
            [XmlElementAttribute("node")]
            public TreeViewNodeNodeNode[] node
            {
                get
                {
                    return nodeField;
                }
                set
                {
                    nodeField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string text
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
            [XmlAttributeAttribute]
            public byte imageindex
            {
                get
                {
                    return imageindexField;
                }
                set
                {
                    imageindexField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string expland
            {
                get
                {
                    return explandField;
                }
                set
                {
                    explandField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte index
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
            [XmlAttributeAttribute]
            public string lastselect
            {
                get
                {
                    return lastselectField;
                }
                set
                {
                    lastselectField = value;
                }
            }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class TreeViewNodeNodeNode
        {

            private TreeViewNodeNodeNodeNode nodeField;

            private string textField;

            private byte imageindexField;

            private byte indexField;

            private string explandField;

            /// <remarks/>
            public TreeViewNodeNodeNodeNode node
            {
                get
                {
                    return nodeField;
                }
                set
                {
                    nodeField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string text
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
            [XmlAttributeAttribute]
            public byte imageindex
            {
                get
                {
                    return imageindexField;
                }
                set
                {
                    imageindexField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte index
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
            [XmlAttributeAttribute]
            public string expland
            {
                get
                {
                    return explandField;
                }
                set
                {
                    explandField = value;
                }
            }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class TreeViewNodeNodeNodeNode
        {

            private TreeViewNodeNodeNodeNodeNode[] nodeField;

            private string textField;

            private byte imageindexField;

            private byte indexField;

            private string explandField;

            /// <remarks/>
            [XmlElementAttribute("node")]
            public TreeViewNodeNodeNodeNodeNode[] node
            {
                get
                {
                    return nodeField;
                }
                set
                {
                    nodeField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string text
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
            [XmlAttributeAttribute]
            public byte imageindex
            {
                get
                {
                    return imageindexField;
                }
                set
                {
                    imageindexField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte index
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
            [XmlAttributeAttribute]
            public string expland
            {
                get
                {
                    return explandField;
                }
                set
                {
                    explandField = value;
                }
            }
        }

        /// <remarks/>
        [XmlTypeAttribute(AnonymousType = true)]
        public partial class TreeViewNodeNodeNodeNodeNode
        {

            private TreeViewNodeNodeNodeNodeNodeNode nodeField;

            private string textField;

            private byte imageindexField;

            private string explandField;

            private byte indexField;

            /// <remarks/>
            public TreeViewNodeNodeNodeNodeNodeNode node
            {
                get
                {
                    return nodeField;
                }
                set
                {
                    nodeField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string text
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
            [XmlAttributeAttribute]
            public byte imageindex
            {
                get
                {
                    return imageindexField;
                }
                set
                {
                    imageindexField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public string expland
            {
                get
                {
                    return explandField;
                }
                set
                {
                    explandField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte index
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
        public partial class TreeViewNodeNodeNodeNodeNodeNode
        {

            private string textField;

            private byte imageindexField;

            private byte indexField;

            /// <remarks/>
            [XmlAttributeAttribute]
            public string text
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
            [XmlAttributeAttribute]
            public byte imageindex
            {
                get
                {
                    return imageindexField;
                }
                set
                {
                    imageindexField = value;
                }
            }

            /// <remarks/>
            [XmlAttributeAttribute]
            public byte index
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
    }
}
