using System;
using System.Drawing;
using System.IO;
using System.Text;
using ICSharpCode.TextEditor.Document;

namespace DeviceDesign
{
    public partial class FormSystemConfigXml : FormBase
    {
        private string _filePath;

        public FormSystemConfigXml()
        {
            InitializeComponent();
        }

        private void FormSystemConfigXml_Load(object sender, EventArgs e)
        {
            GetConfigFilePath();
            TextEditInit();
        }

        private void TextEditInit()
        {
            textEditorControl.ShowEOLMarkers = false;
            textEditorControl.ShowHRuler = false;
            textEditorControl.ShowInvalidLines = false;
            textEditorControl.ShowMatchingBracket = true;
            textEditorControl.ShowSpaces = false;
            textEditorControl.ShowTabs = false;
            textEditorControl.ShowVRuler = false;
            textEditorControl.AllowCaretBeyondEOL = false;
            textEditorControl.ShowLineNumbers = false;
            textEditorControl.EnableFolding = true;
            textEditorControl.IsReadOnly = true;
            textEditorControl.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("XML");
            textEditorControl.Encoding = Encoding.GetEncoding("GB2312");
            textEditorControl.Font = new Font("微软雅黑", 12F, FontStyle.Regular, GraphicsUnit.Point, 134);

            textEditorControl.Text = "";
            var sr = new StreamReader(_filePath, Encoding.Default);

            textEditorControl.Text = sr.ReadToEnd();

            sr.Close();
        }

        private void GetConfigFilePath()
        {
            _filePath = ClassComm.FilePathDeviceConfig;
        }
    }
}
