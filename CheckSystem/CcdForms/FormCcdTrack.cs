using Controller;
using Sunny.UI;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace CheckSystem.CcdForms
{
    public partial class FormCcdTrack : UIForm
    {
        private CcdAutoAssemblyPlateTrack _ccdTrack = new CcdAutoAssemblyPlateTrack("_ccdTrack");
        private CcdTrackSiemensPlc _plc = new CcdTrackSiemensPlc("_plc");
        private BarcodeScanReaderLiuXian BarcodeScanReaderLiuXian = new BarcodeScanReaderLiuXian("scanner");

        public FormCcdTrack()
        {
            InitializeComponent();
            Load += FormCcdTrack_Load;
            WindowState = FormWindowState.Maximized;
            FormClosed += FormCcdTrack_FormClosed;
        }

        private void FormCcdTrack_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_th != null)
            {
                _th.Abort();
                _th.Join();
            }

            if (m_Thread1 != null)
            {
                m_Thread1.Abort();
                m_Thread1.Join();
            }
        }

        private void FormCcdTrack_Load(object sender, System.EventArgs e)
        {
            {
                var dataGridView = uiDataGridView1;
                dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dataGridView.AllowUserToResizeColumns = true;
                dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                //userDataGridGrayList.dataGridView.EditMode = DataGridViewEditMode.EditOnEnter;
                dataGridView.Margin = new Padding(3, 4, 3, 4);
                dataGridView.RowTemplate.Height = 30;
                dataGridView.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 7.5f, FontStyle.Regular);
                dataGridView.RowsDefaultCellStyle.Font = new Font("微软雅黑", 8, FontStyle.Regular);
                dataGridView.EnableHeadersVisualStyles = false;
                dataGridView.RowHeadersVisible = false;
                dataGridView.AllowUserToDeleteRows = false;
                dataGridView.AllowUserToAddRows = false;
                dataGridView.AllowUserToResizeRows = false;
                dataGridView.ReadOnly = true;
            }

            uiDataGridView1.AddColumn("载盘码", "载盘码", readOnly: true, fillWeight: 100);
            uiDataGridView1.AddColumn("位置", "位置", readOnly: true, fillWeight: 25);
            uiDataGridView1.AddColumn("产品码", "产品码", readOnly: true);
            uiDataGridView1.AddColumn("结果", "结果", readOnly: true, fillWeight: 25);

            if (_th != null)
            {
                _th.Abort();
                _th.Join();
            }
            _th = new Thread(new ThreadStart(MainWork));
            _th.Start();

            if (m_Thread1 != null)
            {
                m_Thread1.Abort();
                m_Thread1.Join();
            }
            m_Thread1 = new Thread(new ThreadStart(Resh));
            m_Thread1.Start();
        }

        private void toolStripButton1_Click(string trackBarcode)
        {
            var platebarcode = trackBarcode;
            uiDataGridView1.ClearRows();
            toolStripLabel1.Text = "正在查询";
            List<CcdAutoAssemblyPlateTrack.AckData> outData;
            if (_ccdTrack.SendQuery(platebarcode, out outData))
            {
                toolStripLabel1.Text = string.Format("当前载盘码：{0}，查询成功", platebarcode);
                if (outData.Any())
                {
                    for (int i = 0; i < outData.Count; i++)
                    {
                        uiDataGridView1.AddRow(new object[] { platebarcode, outData[i].Position, outData[i].ProductBarcode, outData[i].IsOk ? "OK" : "NG" });
                        if (!outData[i].IsOk)
                        {
                            for (var c = 0; c < uiDataGridView1.Rows[i].Cells.Count; c++)
                            {
                                uiDataGridView1.Rows[i].Cells[c].Style.BackColor = Color.Red;
                            }
                        }
                    }
                    var dgvHeight = uiDataGridView1.Size.Height;
                    var perHeight = (dgvHeight - 50) / outData.Count;
                    for (int i = 0; i < uiDataGridView1.RowCount; i++)
                        uiDataGridView1.Rows[i].Height = perHeight;
                    uiDataGridView1.ClearSelection();
                }
                else
                {
                    toolStripLabel1.Text = string.Format("当前载盘码：{0}，查询无数据", platebarcode);
                }
            }
            else
            {
                toolStripLabel1.Text = string.Format("当前载盘码：{0}，查询失败", platebarcode);
            }
        }

        private enum State
        {
            Idle,

            ReadCode,

            Track
        }

        private Thread _th { get; set; }
        private State _currentState = State.Idle;

        private void MainWork()
        {
            BarcodeScanReaderLiuXian.ConnectBarcodeScanner("192.168.1.40:502");
            _ccdTrack.ConnectServer("192.168.1.50", 508);
            var trackPlateBarcode = string.Empty;

            while (_th.IsAlive)
            {
                if (!_th.IsAlive)
                    break;

                Thread.Sleep(50);

                if (_currentState == State.Idle)
                {
                    if (_plc.Hr40002 == 1)
                    {
                        trackPlateBarcode = string.Empty;
                        _currentState = State.ReadCode;
                    }
                }
                else if (_currentState == State.ReadCode)
                {
                    BarcodeScanReaderLiuXian.ClearBuffer();
                    Thread.Sleep(50);
                    var retryCount = 10;
                    for (int i = 0; i < retryCount; i++)
                    {
                        BarcodeScanReaderLiuXian.ReadBarcode();
                        Thread.Sleep(250);
                        var len = BarcodeScanReaderLiuXian.GetBarcodeLength;
                        if (len > 5)
                        {
                            trackPlateBarcode = BarcodeScanReaderLiuXian.GetBarcodeStr.TrimEnd();
                            break;
                        }
                    }

                    _currentState = State.Track;
                }
                else if (_currentState == State.Track)
                {

                }
            }
        }

        Thread m_Thread1 { get; set; }

        private void Resh()
        {
            _plc.RefreshOutputs();
            _plc.CycleUpdate();
        }
    }
}
