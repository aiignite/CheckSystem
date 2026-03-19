using System.Text;
using System.Windows.Forms;
using System.Xml;
using Sunny.UI;

namespace CheckSystem.Yfas.Utility
{
    public class XmlTreeViewState
    {
        #region const...

        private const string XmlNodeTag = "node";
        private const string XmlNodeTextAtt = "text";
        private const string XmlNodeTagAtt = "tag";
        private const string XmlNodeImageIndexAtt = "imageindex";
        private const string XmlNodeExpandState = "expland";//展开状态
        private const string XmlNodeIsSelect = "lastselect";//最后选中的项
        private const string XmlNodeIndex = "index";//当前TreeView索引

        #endregion

        #region 保存TREEVIEW状态

        public void SaveTreeViewState(UITreeView treeView, string fileName)
        {
            var textWriter = new XmlTextWriter(fileName, Encoding.Unicode);
            textWriter.WriteStartDocument();
            textWriter.WriteStartElement("TreeView");
            SaveXmlNodes(treeView.Nodes, textWriter);
            textWriter.WriteEndElement();
            textWriter.Close();
        }

        #endregion

        #region 读取TreeView状态

        public void LoadTreeViewState(UITreeView treeView, string fileName)
        {
            XmlTextReader reader = null;

            try
            {
                treeView.Nodes.Clear();
                // disabling re-drawing of treeview till all nodes are added
                treeView.BeginUpdate();
                reader = new XmlTextReader(fileName);
                TreeNode parentNode = null;
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name == XmlNodeTag)
                        {
                            var newNode = new TreeNode();
                            var isEmptyElement = reader.IsEmptyElement;
                            // loading node attributes
                            var attributeCount = reader.AttributeCount;

                            if (attributeCount > 0)
                            {
                                for (var i = 0; i < attributeCount; i++)
                                {
                                    reader.MoveToAttribute(i);
                                    SetAttributeValue(newNode, reader.Name, reader.Value);
                                    SetTreeViewState(treeView);
                                }
                            }

                            //if (newNode.ImageIndex == (int)YfasCheckStateMachine.YfasTreeNodeType.Action)
                            //    newNode.Tag += string.Format("{0}",
                            //       YfasTestFlowConfigForm.ActionToString(JsonConvert.DeserializeObject<YfasCheckStateMachine.ExcActions>(newNode.Text)));
                            //else if (newNode.ImageIndex == (int)YfasCheckStateMachine.YfasTreeNodeType.Func)
                            //    newNode.Tag += string.Format("{0}",
                            //        YfasTestFlowConfigForm.FuncToString(JsonConvert.DeserializeObject<YfasCheckStateMachine.ExcFunc>(newNode.Text)));
                            //else if (newNode.ImageIndex == (int)YfasCheckStateMachine.YfasTreeNodeType.Para)
                            //    newNode.Tag += string.Format("{0}",
                            //       YfasTestFlowConfigForm.ParaToString(JsonConvert.DeserializeObject<YfasCheckStateMachine.ExcDetect>(newNode.Text)));

                            // add new node to Parent Node or TreeView

                            if (parentNode != null)
                                parentNode.Nodes.Add(newNode);
                            else
                                treeView.Nodes.Add(newNode);

                            // making current node 'ParentNode' if its not empty
                            if (!isEmptyElement)
                            {
                                parentNode = newNode;
                            }
                        }
                    }

                    // moving up to in TreeView if end tag is encountered
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == XmlNodeTag)
                        {
                            parentNode = parentNode.Parent;
                        }
                    }

                    else if (reader.NodeType == XmlNodeType.XmlDeclaration)
                    {
                        //Ignore Xml Declaration
                    }

                    else if (reader.NodeType == XmlNodeType.None)
                    {
                        return;
                    }

                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        parentNode.Nodes.Add(reader.Value);
                    }
                }
            }

            finally
            {
                treeView.EndUpdate();
                reader.Close();
            }
        }

        #endregion

        #region 设置 展开后的节点 最后选中的节点

        private TreeNode _lastSelectNode;//最后选中的节点
        private TreeNode _expandNode;//展开后的节点

        // Expland LastSelect

        private void SetTreeViewState(UITreeView treeView)
        {
            treeView.SelectedNode = _expandNode;

            //Expland
            if (_expandNode != null)
                _expandNode.Expand();

            //Select
            if (_lastSelectNode != null)
                treeView.SelectedNode = _lastSelectNode;
        }

        #endregion

        #region 保存XML节点

        private static void SaveXmlNodes(TreeNodeCollection nodesCollection, XmlWriter textWriter)
        {
            for (var i = 0; i < nodesCollection.Count; i++)
            {
                var node = nodesCollection[i];
                textWriter.WriteStartElement(XmlNodeTag);// "node";
                textWriter.WriteAttributeString(XmlNodeTextAtt, node.Text);// "text";
                textWriter.WriteAttributeString(XmlNodeImageIndexAtt, node.ImageIndex.ToString());//"imageindex";

                if (node.IsExpanded)
                    textWriter.WriteAttributeString(XmlNodeExpandState, node.IsExpanded.ToString());////展开状态
                if (node.IsSelected)
                    textWriter.WriteAttributeString(XmlNodeIsSelect, node.IsSelected.ToString());//是否选中
                if (node.Tag != null)
                    textWriter.WriteAttributeString(XmlNodeTagAtt, node.Tag.ToString());
                textWriter.WriteAttributeString(XmlNodeIndex, node.Index.ToString());//Index

                // add other node properties to serialize here
                if (node.Nodes.Count > 0)
                {
                    SaveXmlNodes(node.Nodes, textWriter);
                }
                textWriter.WriteEndElement();
            }
        }

        #endregion

        #region 设置XML属性

        private void SetAttributeValue(TreeNode node, string propertyName, string value)
        {
            if (propertyName == XmlNodeTextAtt) //text
            {
                node.Text = value;
            }

            else if (propertyName == XmlNodeImageIndexAtt) //ImageIndex
            {
                node.ImageIndex = int.Parse(value);
            }

            else if (propertyName == XmlNodeExpandState)
            {
                _expandNode = node;
            }

            else if (propertyName == XmlNodeIsSelect)
            {
                _lastSelectNode = node;
            }

            else if (propertyName == XmlNodeTagAtt)//tag
            {
                node.Tag = value;
            }

            else if (propertyName == XmlNodeIndex)
            {
                // 用来标识 这样看XML文件时结构清晰
            }
        }

        #endregion

        #region 把XML文件读取到TREE中

        public void LoadXmlFileInTreeView(TreeView treeView, string fileName)
        {
            XmlTextReader reader = null;

            try
            {
                treeView.BeginUpdate();
                reader = new XmlTextReader(fileName);
                var n = new TreeNode(fileName);
                treeView.Nodes.Add(n);

                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        var isEmptyElement = reader.IsEmptyElement;
                        var text = new StringBuilder();
                        text.Append(reader.Name);
                        var attributeCount = reader.AttributeCount;

                        if (attributeCount > 0)
                        {
                            text.Append(" ( ");
                            for (var i = 0; i < attributeCount; i++)
                            {
                                if (i != 0) text.Append(", ");
                                reader.MoveToAttribute(i);
                                text.Append(reader.Name);
                                text.Append(" = ");
                                text.Append(reader.Value);
                            }
                            text.Append(" ) ");
                        }

                        if (isEmptyElement)
                        {
                            n.Nodes.Add(text.ToString());
                        }
                        else
                        {
                            n = n.Nodes.Add(text.ToString());
                        }
                    }
                    else if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        n = n.Parent;
                    }
                    else if (reader.NodeType == XmlNodeType.XmlDeclaration)
                    {

                    }
                    else if (reader.NodeType == XmlNodeType.None)
                    {
                        return;
                    }

                    else if (reader.NodeType == XmlNodeType.Text)
                    {
                        n.Nodes.Add(reader.Value);
                    }
                }
            }
            finally
            {
                treeView.EndUpdate();
                if (reader != null) reader.Close();
            }
        }

        #endregion
    }
}