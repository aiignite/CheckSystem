using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.HikSdk;

namespace CheckSystem.CcdForms
{
    public partial class FormLedParaXmlList : Form
    {
        private readonly List<LedCheckPara> _listLedCheckParas =
            new List<LedCheckPara>();
        private readonly List<LedCheckParaVisionFuncsVisionFunc> _listFuncs =
            new List<LedCheckParaVisionFuncsVisionFunc>();
        private readonly List<string> _listFilePaths = new List<string>();
        private readonly CameraControl _camera;

        public FormLedParaXmlList(CameraControl cameraControl = null)
        {
            InitializeComponent();
            _camera = cameraControl;

            var folder = string.Format(@"{0}\图像检测配置文件", Program.SysDir);

            foreach (
                var f in
                    Directory.GetDirectories(folder)
                        .SelectMany(d => Directory.GetFiles(d).Where(f => f.EndsWith(@".xml"))))
            {
                try
                {
                    var ledCheckPara = XmlHelper.Deserialize<LedCheckPara>(f);
                    _listLedCheckParas.Add(ledCheckPara);
                    lstXml.Items.Add(ledCheckPara.VisionFuncs.ProductName);
                    _listFilePaths.Add(f);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            lstXml.SelectedIndexChanged += lstXml_SelectedIndexChanged;
        }

        private void lstXml_SelectedIndexChanged(object sender, EventArgs e)
        {
            _listFuncs.Clear();
            lstFunc.Items.Clear();

            if (lstXml.SelectedIndex < 0)
                return;

            if (_listLedCheckParas[lstXml.SelectedIndex].VisionFuncs.VisionFunc == null)
                return;

            foreach (var t in _listLedCheckParas[lstXml.SelectedIndex].VisionFuncs.VisionFunc)
            {
                _listFuncs.Add(t);
                lstFunc.Items.Add(t.FuncName);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (lstXml.SelectedIndex < 0 || lstFunc.SelectedIndex < 0)
                return;
            var setForm =
                new FormLedParaSet(_listFilePaths[lstXml.SelectedIndex], lstFunc.Text, _camera);
            setForm.ShowDialog();
            Close();
        }

        private void lstFunc_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (lstXml.SelectedIndex < 0 || lstFunc.SelectedIndex < 0)
                return;
            var setForm =
                new FormLedParaSet(_listFilePaths[lstXml.SelectedIndex], lstFunc.Text, _camera);
            setForm.ShowDialog();
            Close();
        }
    }
}
