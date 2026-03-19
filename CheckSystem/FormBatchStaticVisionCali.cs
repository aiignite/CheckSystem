using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;
using Controller;
using NationalInstruments.Vision;
using NationalInstruments.Vision.Analysis;
using NationalInstruments.Vision.WindowsForms;
using OpenCvSharp;
using OpenCvSharp.Extensions;
using StateMachine;
using Sunny.UI;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using ImageProcessing = CheckSystem.VisionDetection.Vision.ImageProcessing;

namespace CheckSystem
{
    public partial class FormBatchStaticVisionCali : UIForm
    {
        private State _state;
        private readonly ImageViewer _mainImageViewer = new ImageViewer { Dock = DockStyle.Fill };
        private string _nowBuffBase64 = string.Empty;

        public FormBatchStaticVisionCali(State state)
        {
            InitializeComponent();
            _state = state;
            imgPreview.Controls.Add(_mainImageViewer);
            WindowState = FormWindowState.Maximized;
            Load += FormBatchStaticVisionCali_Load;
        }

        private void FormBatchStaticVisionCali_Load(object sender, EventArgs e)
        {
            InitImageViewer(_mainImageViewer);
            LoadCamerabBuff();
            LoadRoiPara();
        }

        private void InitImageViewer(ImageViewer imageViewer)
        {
            ImageShowTool(imageViewer);
            imageViewer.SizeChanged += imageViewer_SizeChanged;
        }

        private static void ImageShowTool(ImageViewer imageViewer)
        {
            imageViewer.ToolsShown = ViewerTools.ZoomIn |
                                     ViewerTools.ZoomOut |
                                     ViewerTools.Pan |
                                     ViewerTools.Selection;
            imageViewer.ActiveTool = ViewerTools.Selection;
            imageViewer.ZoomToFit = true;
            imageViewer.ShowToolbar = true;
            imageViewer.ShowScrollbars = true;
            imageViewer.ShowImageInfo = true;
        }

        private static void imageViewer_SizeChanged(object sender, EventArgs e)
        {
            var imageViewer = sender as ImageViewer;
            if (imageViewer != null)
                ImageShowTool(imageViewer);
        }

        private void LoadCamerabBuff()
        {
            var cameraTreeNode = new TreeNode { Text = @"图像缓存列表" };

            if (_state != null)
            {
                var visions = _state.LstControllers.Where(f => (f != null && f.GetType() == typeof(VisionAnalysis))).Select(t => t as VisionAnalysis);
                foreach (var vision in visions)
                {
                    var keys = vision.StaticImageBuff.Keys.ToList();
                    if (keys.Any())
                    {
                        var eachCameraNode = new TreeNode(string.Format("{0}_[{1}]", vision.Name, vision.CameraSn)) { Tag = vision.Name };

                        for (var i = 0; i < keys.Count; i++)
                        {
                            var key = keys[i];
                            var keyNode = new TreeNode(string.Format("缓存名称_[{0}]", key)) { Tag = key };
                            for (var j = 0; j < vision.StaticImageBuff[key].Length; j++)
                            {
                                var imageNode = new TreeNode("图像缓存" + j);
                                imageNode.Tag = j;
                                keyNode.Nodes.Add(imageNode);
                            }

                            eachCameraNode.Nodes.Add(keyNode);
                        }

                        cameraTreeNode.Nodes.Add(eachCameraNode);
                    }
                }
            }

            treeViewCamerasList.Nodes.Add(cameraTreeNode);
            treeViewCamerasList.ExpandAll();
            treeViewCamerasList.BeforeCollapse += (ns1, ne1) => { ne1.Cancel = true; };
            treeViewCamerasList.BeforeCheck += (ns1, ne1) => { ne1.Cancel = ne1.Node.Level != 3; };
            treeViewCamerasList.AfterCheck += (ns1, ne1) => TreeViewSingleSelectedAndChecked(ns1 as UITreeView, ne1);
            treeViewCamerasList.AfterSelect += (afs, afe) =>
            {
                var nodeLevel = afe.Node.Level;
                if (nodeLevel == 3)
                {
                    var parentBuffNameNode = afe.Node.Parent;
                    var parentCameraNode = afe.Node.Parent.Parent;

                    var buffIndex = (int)afe.Node.Tag;
                    var buffName = parentBuffNameNode.Tag.ToString();
                    var cameraName = parentCameraNode.Tag?.ToString();

                    var vision = _state.LstControllers.Where(f => (f != null && f.GetType() == typeof(VisionAnalysis))).First(t => ((VisionAnalysis)t).Name == cameraName) as VisionAnalysis;
                    _nowBuffBase64 = vision?.StaticImageBuff[buffName][buffIndex].Item2;
                }
                else
                {
                    _nowBuffBase64 = string.Empty;
                }

                CopySrcToShow();
            };
        }

        private void CopySrcToShow()
        {
            var imgMat = new Mat();
            if (!string.IsNullOrEmpty(_nowBuffBase64))
            {
                var imgBitMap = MyCamera.Base64StringToBitmap(_nowBuffBase64);

                if (imgBitMap != null)
                {
                    imgMat?.Dispose();
                    imgMat = null;
                    imgMat = BitmapConverter.ToMat(imgBitMap);
                    imgBitMap?.Dispose();
                }
                else
                {
                    imgMat?.Dispose();
                    imgMat = null;
                    imgMat = new Mat(768, 1024, MatType.CV_8UC1, Scalar.Black);
                    var tpb = BitmapConverter.ToBitmap(imgMat);
                    _nowBuffBase64 = MyCamera.BitmapToBase64String(tpb);
                    tpb.Dispose();
                }
            }
            else
            {
                imgMat?.Dispose();
                imgMat = null;
                imgMat = new Mat(768, 1024, MatType.CV_8UC1, Scalar.Black);
                var tpb = BitmapConverter.ToBitmap(imgMat);
                _nowBuffBase64 = MyCamera.BitmapToBase64String(tpb);
                tpb.Dispose();
            }

            var img = MyCamera.MatToVisionImage(imgMat);
            imgMat?.Dispose();
            imgMat = null;
            Algorithms.Copy(img, _mainImageViewer.Image);
            img.Dispose();
        }

        private void LoadRoiPara()
        {
            {
                parentRoiTreeview.BeginUpdate();

                var paraGroup = _state.DeviceConfig.Paras.ToList()
                    .FindAll(
                    fa =>
                    fa.DataType?.ToLower() == "roi" &&
                    _state.DeviceConfig.Conditions.ToList().FindAll(f => f.ConditionFunction.ToLower().Contains(string.Format("{0}.para.{1}", fa.ProcessNo, fa.Name).ToLower())).Count > 0 &&
                    _state.DeviceConfig.Rois?.ToList().FindAll(f => f.Name.ToLower() == fa.OkFormat?.ToLower()).Count > 0)
                    .OrderBy(f => int.Parse(f.ProcessNo.Substring(f.ProcessNo.Length - 3, 3))).GroupBy(p => p.ProcessNo);
                foreach (var gp in paraGroup)
                {
                    var node = new TreeNode(gp.Key);

                    for (var i = 0; i < gp.Count(); i++)
                    {
                        var childNode = new TreeNode(gp.ElementAt(i).Name);
                        node.Nodes.Add(childNode);
                    }

                    parentRoiTreeview.Nodes.Add(node);
                }

                parentRoiTreeview.ExpandAll();
                parentRoiTreeview.BeforeCollapse += (ns1, ne1) => { ne1.Cancel = true; };
                parentRoiTreeview.BeforeCheck += (ns1, ne1) => { ne1.Cancel = ne1.Node.Level != 1; };

                parentRoiTreeview.EndUpdate();
            }
        }

        /// <summary>
        /// 树形框-单选模式的实现,放在事件 _AfterCheck下
        /// </summary>
        /// <param name="tv"></param>
        /// <param name="e"></param>
        public static void TreeViewSingleSelectedAndChecked(UITreeView tv, TreeViewEventArgs e)
        {
            if (e.Action == TreeViewAction.ByMouse)
            {
                if (e.Node.Checked)
                {
                    tv.SelectedNode = e.Node;
                    CancelCheckedExceptOne(e.Node.Parent.Nodes, e.Node);
                }
            }
        }

        private static void CancelCheckedExceptOne(TreeNodeCollection tnc, TreeNode tn)
        {
            foreach (TreeNode item in tnc)
            {
                if (item != tn)
                    item.Checked = false;
                if (item.Nodes.Count > 0)
                    CancelCheckedExceptOne(item.Nodes, tn);
            }
        }

        public static class TreeViewCheck
        {
            /// <summary>
            /// 系列节点 Checked 属性控制
            /// </summary>
            /// <param name="e"></param>
            public static void CheckControl(TreeViewEventArgs e)
            {
                if (e.Action != TreeViewAction.Unknown)
                {
                    if (e.Node != null && !Convert.IsDBNull(e.Node))
                    {
                        CheckParentNode(e.Node);
                        if (e.Node.Nodes.Count > 0)
                        {
                            CheckAllChildNodes(e.Node, e.Node.Checked);
                        }
                    }
                }
            }

            #region 私有方法

            //改变所有子节点的状态
            private static void CheckAllChildNodes(TreeNode pn, bool IsChecked)
            {
                foreach (TreeNode tn in pn.Nodes)
                {
                    tn.Checked = IsChecked;

                    if (tn.Nodes.Count > 0)
                    {
                        CheckAllChildNodes(tn, IsChecked);
                    }
                }
            }

            //改变父节点的选中状态，此处为所有子节点不选中时才取消父节点选中，可以根据需要修改
            private static void CheckParentNode(TreeNode curNode)
            {
                bool bChecked = false;

                if (curNode.Parent != null)
                {
                    foreach (TreeNode node in curNode.Parent.Nodes)
                    {
                        if (node.Checked)
                        {
                            bChecked = true;
                            break;
                        }
                    }

                    if (bChecked)
                    {
                        curNode.Parent.Checked = true;
                        CheckParentNode(curNode.Parent);
                    }
                    else
                    {
                        curNode.Parent.Checked = false;
                        CheckParentNode(curNode.Parent);
                    }
                }
            }

            #endregion
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (!UIMessageBox.ShowAsk("是否要执行标定"))
                return;

            var offset = 0.15;
            if (_mainImageViewer.Image.Width < 1000 &&
                _mainImageViewer.Image.Height < 1000)
                offset = 0.2;

            var option = new UIEditOption
            {
                AutoLabelWidth = true,
                Text = "确认"
            };

            var list = new List<string>();
            var selectIndex = 0;
            var baseIndex = 0;
            for (var i = 5; i <= 35; i = i + 5)
            {
                list.Add(string.Format("±{0}%", i));

                if (Math.Abs(offset * 100 - i) == 0)
                    selectIndex = baseIndex;
                baseIndex++;
            }

            option.AddCombobox("Percent", "±百分比", list.ToArray(), selectIndex, true, true);

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                ShowInfoTip("操作取消");
                return;
            }

            offset = double.Parse(list[(int)frm["Percent"]].TrimStart('±').TrimEnd('%')) / 100;

            if (_state.DeviceConfig.Rois == null)
                _state.DeviceConfig.Rois = new DeviceConfigRoi[0];

            var listToCopyParas = new List<string>();
            for (var i = 0; i < parentRoiTreeview.Nodes.Count; i++)
            {
                var processNode = parentRoiTreeview.Nodes[i];
                var process = processNode.Text;

                for (var j = 0; j < processNode.Nodes.Count; j++)
                {
                    if (processNode.Nodes[j].Checked)
                    {
                        listToCopyParas.Add(string.Format("{0}.para.{1}", process, processNode.Nodes[j].Text));
                    }
                }
            }

            for (var i = 0; i < parentRoiTreeview.Nodes.Count; i++)
            {
                var processNode = parentRoiTreeview.Nodes[i];
                var process = processNode.Text;

                for (var j = 0; j < processNode.Nodes.Count; j++)
                {
                    var paraNode = processNode.Nodes[j];

                    if (paraNode.Checked)
                    {
                        var paraName = paraNode.Text;

                        var para = _state.DeviceConfig.Paras.ToList().Find(f => f.Name.ToLower() == paraName.ToLower());
                        if (para != null && !string.IsNullOrEmpty(para.OkFormat) && !string.IsNullOrEmpty(para.ControllerFieldOffset) && !string.IsNullOrEmpty(para.ControllerField))
                        {
                            var toCopyRoiName = para.OkFormat;

                            var rois = _state.DeviceConfig.Rois.ToList().FindAll(f => f.Name.ToLower() == toCopyRoiName.ToLower());
                            if (rois.Any())
                            {
                                var lsitRect = rois.Select(ro => new Rect((int)double.Parse(ro.RectX), (int)double.Parse(ro.RectY), (int)double.Parse(ro.RectWidth), (int)double.Parse(ro.RectHeight))).ToList();
                                var field = para.ControllerField.Split(new string[] { ".Field." }, StringSplitOptions.RemoveEmptyEntries);

                                var needCaliParas = _state.DeviceConfig.Paras.ToList().Where(
                                    f =>
                                    !listToCopyParas.Contains(string.Format("{0}.para.{1}", f.ProcessNo, f.Name)) &&
                                    f.ControllerField.ToLower() == para.ControllerField.ToLower() &&
                                    f.ProcessNo == para.ProcessNo).ToArray();

                                foreach (var targetPara in needCaliParas)
                                {
                                    if (field.Length == 2)
                                    {
                                        var cameraName = field[0];
                                        var isCali = false;

                                        for (var k = 0; k < treeViewCamerasList.Nodes[0].Nodes.Count; k++)
                                        {
                                            var cameraNode = treeViewCamerasList.Nodes[0].Nodes[k];
                                            if (cameraName == cameraNode.Tag?.ToString())
                                            {
                                                for (var b = 0; b < cameraNode.Nodes.Count; b++)
                                                {
                                                    var buffNode = cameraNode.Nodes[b];
                                                    if (buffNode.Tag?.ToString() == targetPara.ControllerFieldOffset)
                                                    {
                                                        for (var bb = 0; bb < buffNode.Nodes.Count; bb++)
                                                        {
                                                            var buffChildNode = buffNode.Nodes[bb];
                                                            if (buffChildNode.Checked)
                                                            {
                                                                var buffIndex = bb;
                                                                var cameras = _state.LstControllers.Where(f => (f != null && f.GetType() == typeof(VisionAnalysis))).Select(t => t as VisionAnalysis).ToList();
                                                                var camera = cameras.Find(f => f.Name == cameraName);

                                                                if (camera != null && camera.StaticImageBuff.ContainsKey(targetPara.ControllerFieldOffset) && camera.StaticImageBuff.Count > 0 && buffIndex >= 0 && buffIndex <= camera.StaticImageBuff.Count - 1)
                                                                {
                                                                    var buffBase64Str = camera.StaticImageBuff[targetPara.ControllerFieldOffset][buffIndex].Item2;
                                                                    using (var srcBitmap = MyCamera.Base64StringToBitmap(buffBase64Str))
                                                                    using (var srcMat = BitmapConverter.ToMat(srcBitmap))
                                                                    {
                                                                        if (srcMat.Channels() != 1)
                                                                            Cv2.CvtColor(srcMat, srcMat, ColorConversionCodes.BGR2GRAY);

                                                                        var tpRoi = _state.DeviceConfig.Rois.ToList();
                                                                        tpRoi.RemoveAll(f => f.Name.ToLower() == targetPara.OkFormat.ToLower());

                                                                        for (var roiIndex = 0; roiIndex < lsitRect.Count; roiIndex++)
                                                                        {
                                                                            var roiMat = new Mat(srcMat, MyCamera.GetRectInMat(srcMat.Width, srcMat.Height, lsitRect[roiIndex]));
                                                                            var gray = Cv2.Mean(roiMat);
                                                                            var grayPer = Math.Round(gray.Val0, 2, MidpointRounding.AwayFromZero);//Math.Round(ImageProcessing.GetGrayValue(_mainImageViewer.Image, shape), 2);
                                                                            var grayMinPer = Math.Round(grayPer * (1 - offset) < 0 ? 0 : (grayPer * (1 - 0)) <= 20 ? 0 : grayPer * (1 - offset), 2);
                                                                            var grayMaxPer = Math.Round(grayMinPer < 1 ? 20 : grayPer * (1 + offset) > 255 ? 255 : grayPer * (1 + offset), 2);

                                                                            roiMat.Dispose();

                                                                            //dgvGrays.AddRow(i + 1, grayPer, grayMinPer, grayMaxPer);

                                                                            tpRoi.Add(new DeviceConfigRoi
                                                                            {
                                                                                Name = targetPara.OkFormat,
                                                                                Group = "0",
                                                                                RectX = Math.Round((double)lsitRect[roiIndex].Left, 0, MidpointRounding.AwayFromZero).ToString(),
                                                                                RectY = Math.Round((double)lsitRect[roiIndex].Top, 0, MidpointRounding.AwayFromZero).ToString(),
                                                                                RectWidth = Math.Round((double)lsitRect[roiIndex].Width, 0, MidpointRounding.AwayFromZero).ToString(),
                                                                                RectHeight = Math.Round((double)lsitRect[roiIndex].Height, 0, MidpointRounding.AwayFromZero).ToString(),
                                                                                Min = grayMinPer.ToString(),
                                                                                Max = grayMaxPer.ToString()
                                                                            });
                                                                        }

                                                                        srcBitmap.Dispose();
                                                                        srcMat.Dispose();

                                                                        _state.DeviceConfig.Rois = tpRoi.ToArray();
                                                                    }
                                                                }

                                                                isCali = true;
                                                                break;
                                                            }
                                                        }
                                                    }

                                                    if (isCali)
                                                        break;
                                                }
                                            }

                                            if (isCali)
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            var toSavePara = XmlHelper.Deserialize<DeviceConfig>(_state.XmlFilePath);
            toSavePara.Rois = _state.DeviceConfig.Rois.ToArray();
            XmlHelper.SerializeToFile(toSavePara, _state.XmlFilePath, Encoding.UTF8);
            UIMessageBox.Show("已批量更新");
        }
    }
}