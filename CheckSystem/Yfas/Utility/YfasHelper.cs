using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Sunny.UI;

namespace CheckSystem.Yfas.Utility
{
    public static class YfasHelper
    {
        public static void WriteTxt(string filePath, string value)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (var sr = new StreamWriter(fileStream, Encoding.Unicode))
                {
                    sr.Write(value);
                }
            }
        }

        public static void CreateTreeView(UITreeView uiTreeView, string treeviewText)
        {
            //var xmlFileName = @"C:\Users\B765\Desktop\TreeView.xml";

            var baseNode = new TreeNode { Text = @"OnStart" };

            if (!string.IsNullOrEmpty(treeviewText))
            {
                try
                {
                    WriteTxt(YfasDeviceBase.XmlFileNameLoad, treeviewText);

                    uiTreeView.Focus();

                    if (File.Exists(YfasDeviceBase.XmlFileNameLoad))
                    {
                        var treeState = new XmlTreeViewState();
                        treeState.LoadTreeViewState(uiTreeView, YfasDeviceBase.XmlFileNameLoad);
                    }
                    File.Delete(YfasDeviceBase.XmlFileNameLoad);
                }
                catch (Exception)
                {
                    uiTreeView.Nodes.Clear();
                    uiTreeView.Nodes.Add(baseNode);
                    uiTreeView.SelectedNode = baseNode;
                }
            }
            else
            {
                uiTreeView.Nodes.Clear();
                uiTreeView.Nodes.Add(baseNode);
                uiTreeView.SelectedNode = baseNode;
            }

            uiTreeView.SelectedNode = uiTreeView.Nodes[0];
            uiTreeView.ExpandAll();
        }
    }
}
