using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CommonUtility;
using Sunny.UI;

namespace CheckSystem.MaterialHelperForms
{
    public partial class HikDataValidationForm : UIForm
    {
        private string KeyBarcode { get; set; }
        private readonly List<MyValidationState> _toValidateCode = new List<MyValidationState>();

        internal class MyValidationState
        {
            public string Code;
            public bool IsGet;
        }

        public HikDataValidationForm(string keyBarcode, IEnumerable<string> toValidateCode)
        {
            InitializeComponent();

            KeyBarcode = keyBarcode;

            dgvValidation.Style = UIStyle.Gray;
            dgvValidation.ReadOnly = true;
            dgvValidation.RowHeadersVisible = false;
            dgvValidation.AllowUserToAddRows = false;
            dgvValidation.AllowUserToResizeRows = false;
            dgvValidation.AllowUserToDeleteRows = false;
            dgvValidation.MultiSelect = true;
            dgvValidation.RowHeadersVisible = false;
            dgvValidation.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvValidation.AddColumn("条码内容", "条码内容");

            dgvValidation.AddRow(KeyBarcode);
            foreach (var t in toValidateCode)
            {
                _toValidateCode.Add(new MyValidationState { Code = t });
                dgvValidation.AddRow(t);
            }

            HikScanerClass.PushImage += HikScanerClass_PushImage;
            Closed += HikDataValidationForm_Closed;
        }

        private void HikDataValidationForm_Closed(object sender, EventArgs e)
        {
            HikScanerClass.PushImage -= HikScanerClass_PushImage;
        }

        private bool _isGetKeyBarcode;
        //private bool _isGenerating;
        private bool _isValidatedOk;
        private Graphics _mObjGc;
        private readonly Pen _pen = new Pen(Color.Green, 3);

        private void HikScanerClass_PushImage(
            Image img, string deviceSn, List<HikScanerClass.BarcodeStruct> barcodeScanResult)
        {
            dgvValidation.ClearSelection();

            if (_isValidatedOk)
                return;

            if (HikScanerClass.MBitMap1 != null)
            {
                //pictureBox1.Image = HikScanerClass.MBitMap1;
                //_mObjGc = pictureBox1.CreateGraphics();
                //pictureBox1.Refresh();

                //var stPointList = new Point[4];

                //stPointList[0].X = (int)(HikScanerClass.MBitMap1.Width * 0.3);
                //stPointList[0].Y = (int)(HikScanerClass.MBitMap1.Height * 0.3);

                //stPointList[1].X = (int)(HikScanerClass.MBitMap1.Width * 0.7);
                //stPointList[1].Y = (int)(HikScanerClass.MBitMap1.Height * 0.3);

                //stPointList[2].X = (int)(HikScanerClass.MBitMap1.Width * 0.3);
                //stPointList[2].Y = (int)(HikScanerClass.MBitMap1.Height * 0.7);

                //stPointList[3].X = (int)(HikScanerClass.MBitMap1.Width * 0.7);
                //stPointList[3].Y = (int)(HikScanerClass.MBitMap1.Height * 0.7);

                //_mObjGc.DrawPolygon(_pen, stPointList);
            }

            if (!_isGetKeyBarcode)
            {
                var keyCodeIndex = barcodeScanResult.FindIndex(f => f.Barcode == KeyBarcode);
                if (keyCodeIndex != -1 && HikScanerClass.MBitMap1 != null)
                {
                    var keyCode = barcodeScanResult[keyCodeIndex];

                    var scanResult = new HikScanerClass.BarcodeScanResult
                    {
                        Height = (ushort)HikScanerClass.MBitMap1.Height,
                        Width = (ushort)HikScanerClass.MBitMap1.Width
                    };

                    if (keyCode.Position[0].X > scanResult.Width * 0.15 &&
                        keyCode.Position[0].X < scanResult.Width * 0.85 &&
                        keyCode.Position[0].Y > scanResult.Height * 0.15 &&
                        keyCode.Position[0].Y < scanResult.Height * 0.85)
                    {
                        dgvValidation.ClearSelection();
                        dgvValidation.Rows[0].DefaultCellStyle.BackColor = Color.Green;
                        _isGetKeyBarcode = true;
                    }
                }
            }
            else
            {
                foreach (var t in barcodeScanResult)
                {
                    var index = _toValidateCode.FindIndex(f => !f.IsGet && f.Code == t.Barcode);

                    if (index != -1)
                    {                       
                        _toValidateCode[index].IsGet = true;
                        dgvValidation.ClearSelection();
                        dgvValidation.Rows[index++].DefaultCellStyle.BackColor = Color.Green;
                        //break;
                    }
                }

                if (_toValidateCode.FindAll(f => !f.IsGet).Count == 0)
                {
                    _isValidatedOk = true;
                    DialogResult = DialogResult.OK;
                }
            }
        }
    }
}
