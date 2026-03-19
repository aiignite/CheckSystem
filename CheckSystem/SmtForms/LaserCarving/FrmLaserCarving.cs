using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using CommonUtility.FileOperator;
using HslCommunication.ModBus;
using Newtonsoft.Json;
using RestSharp;
using Sunny.UI;

namespace CheckSystem.SmtForms.LaserCarving
{
    public partial class FrmLaserCarving : UIForm
    {
        public static IniFileHelper Setup =
          new IniFileHelper(string.Format(@"{0}\SMT配置文件\{1}", Program.SysDir, "LaserCarvingApi.ini"));

        private readonly string _getUrl = Setup.IniReadValue("LaserCarvingApi", "GetUrl");
        private readonly string _postUrl = Setup.IniReadValue("LaserCarvingApi", "PostUrl");
        private readonly string _header = Setup.IniReadValue("LaserCarvingApi", "Header");
        private readonly string _deviceSn = Setup.IniReadValue("LaserCarvingApi", "DeviceSn");
        private readonly string _txtFolderPath = Setup.IniReadValue("LaserCarvingApi", "TxtFolderPath");
        private readonly string _txtFileMaxCount = Setup.IniReadValue("LaserCarvingApi", "TxtFileMaxCount");
        private readonly string _plcIpAddress = Setup.IniReadValue("LaserCarvingApi", "PlcIpAddress");
        private readonly string _plcIpStation = Setup.IniReadValue("LaserCarvingApi", "PlcIpStation");
        private readonly string _startSignal = Setup.IniReadValue("LaserCarvingApi", "StartSignal");
        private readonly string _completeSignal = Setup.IniReadValue("LaserCarvingApi", "CompleteSignal");
        private readonly string _errorSignal = Setup.IniReadValue("LaserCarvingApi", "ErrorSignal");

        private Thread BackWork { get; set; }
        private ModbusTcpNet ModbusTcp { get; set; }
        private bool IsErrorCleared { get; set; }

        public FrmLaserCarving()
        {
            InitializeComponent();

            cmbDeviceSn.DropDownStyle = UIDropDownStyle.DropDownList;
            var intdex = 0;
            for (var i = 1; i < 11; i++)
            {
                var item = string.Format("IN14160000{0}", i.ToString().PadLeft(2, '0'));
                if (_deviceSn == item)
                    intdex = i - 1;
                cmbDeviceSn.Items.Add(item);
            }
            cmbDeviceSn.SelectedIndex = intdex;
            InitGgv();
            ledStart.On = false;
            ledComplete.On = false;
            ledError.On = false;

            if (BackWork != null)
            {
                BackWork.Abort();
                BackWork.Join();
            }

            BackWork = new Thread(BackWorkMethod);
            BackWork.Start();
        }

        private void BackWorkMethod()
        {
            ModbusTcp = new ModbusTcpNet(_plcIpAddress.Split(':')[0], int.Parse(_plcIpAddress.Split(':')[1]),
                byte.Parse(_plcIpStation));
            ModbusTcp.ConnectServer();

            while (BackWork.IsAlive)
            {
                if (!BackWork.IsAlive)
                    break;

                Thread.Sleep(25);

                var startAddrs = ushort.Parse(_startSignal) - 40000 - 1;
                var completeAddrs = ushort.Parse(_completeSignal) - 40000 - 1;
                var errorAddrs = ushort.Parse(_errorSignal) - 40000 - 1;
                var readStart = ModbusTcp.Read(startAddrs.ToString(), 1);
                ModbusTcp.Write(completeAddrs.ToString(), (ushort)0);
                ModbusTcp.Write(errorAddrs.ToString(), (ushort)0);

                var start = readStart;

                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        if (start.IsSuccess && start.Content[0] * 256 + start.Content[1] > 0)
                            ledStart.On = true;
                        else
                            ledStart.On = false;

                        ledComplete.On = false;
                        ledError.On = false;
                    }));
                }

                if (readStart.IsSuccess && readStart.Content[0] * 256 + readStart.Content[1] > 0)
                {
                    if (InvokeRequired)
                    {
                        var isSuccess = false;

                        Invoke(new Action(() =>
                        {
                            InitGgv();

                            #region api
                            LaserCarvingReqObject obj;
                            if (TryPostWebRequest(cmbDeviceSn.Text, out obj))
                            {
                                txtDeviceSn.Text = cmbDeviceSn.Text;
                                txtProgramName.Text = obj.data.PROGRAM_NAME;
                                txtTrayNo.Text = string.Empty;

                                if (obj.data.SN_DETAIL != null && obj.data.SN_DETAIL.Any())
                                {
                                    txtGroupName.Text = (obj.data.SN_DETAIL.Length + 1).ToString();

                                    foreach (var t in obj.data.SN_DETAIL)
                                        dgvCodeDetail.AddRow(t.SN, t.CODE1, t.CODE2, t.CODE3, t.CODE4, t.CODE5, t.CODE6);

                                    //dgvCodeDetail.AutoResizeRows();
                                    dgvCodeDetail.AutoResizeColumns();

                                    var post = new SN_DETAIL[dgvCodeDetail.RowCount];

                                    for (var i = 0; i < dgvCodeDetail.RowCount; i++)
                                    {
                                        var row = dgvCodeDetail.Rows[i];

                                        post[i] = new SN_DETAIL { SN = row.Cells[0].Value.ToString(), SN_LEVEL = "A" };
                                    }

                                    var postInfo = new PostInfo
                                    {
                                        SN_DETAIL = post,
                                        DEVICE_SN = cmbDeviceSn.Text,
                                        GROUP_NUM = txtGroupName.Text,
                                        PROGRAM_NAME = txtProgramName.Text,
                                        TRAYNO = txtTrayNo.Text
                                    };
                                    var postJsonStr = JsonConvert.SerializeObject(postInfo);

                                    var client = new RestClient(_postUrl);
                                    var request = new RestRequest(Method.POST);
                                    request.AddHeader("cache-control", "no-cache");
                                    request.AddParameter("application/json", postJsonStr, ParameterType.RequestBody);
                                    var response = client.Execute(request);

                                    if (response.StatusCode == HttpStatusCode.OK)
                                    {
                                        if (!Directory.Exists(_txtFolderPath))
                                            Directory.CreateDirectory(_txtFolderPath);

                                        var maxCount = int.Parse(_txtFileMaxCount);
                                        for (var i = 0; i < maxCount; i++)
                                        {
                                            var file = string.Format(@"{0}/{1}.txt", _txtFolderPath, i + 1);
                                            var fs = new FileStream(file, FileMode.Create, FileAccess.Write);
                                            fs.Close();
                                        }

                                        for (var i = 0; i < dgvCodeDetail.RowCount; i++)
                                        {
                                            var file = string.Format(@"{0}/{1}.txt", _txtFolderPath, i + 1);

                                            var value = string.Empty;
                                            for (var j = 0; j < dgvCodeDetail.ColumnCount; j++)
                                            {
                                                string str;
                                                if (dgvCodeDetail.Rows[i].Cells[j].Value != null && !string.IsNullOrEmpty(dgvCodeDetail.Rows[i].Cells[j].Value.ToString()))
                                                    str = dgvCodeDetail.Rows[i].Cells[j].Value + "\r\n";
                                                else
                                                    str = "\r\n";

                                                value += str;
                                            }

                                            if (!File.Exists(file))
                                            {
                                                var fs = new FileStream(file, FileMode.Create, FileAccess.Write);
                                                var sw = new StreamWriter(fs);

                                                sw.Write(value);
                                                sw.Flush();

                                                sw.Close();
                                                fs.Close();
                                            }
                                            else
                                            {
                                                var fs = new FileStream(file, FileMode.Open, FileAccess.Write);
                                                var sw = new StreamWriter(fs);

                                                sw.Write(value);
                                                sw.Flush();

                                                sw.Close();
                                                fs.Close();
                                            }
                                        }

                                        this.ShowSuccessTip("SUCCESS");
                                        isSuccess = true;
                                    }
                                    else
                                    {
                                        this.ShowErrorTip("POST UPLOAD FAIL");
                                    }
                                }
                                else
                                {
                                    this.ShowInfoTip("SUCCESS BUT NO DATAS");
                                }
                            }
                            else
                            {
                                this.ShowErrorTip("ERROR");
                            }
                            #endregion
                        }));

                        if (isSuccess)
                        {
                            BeginInvoke(new Action(() =>
                            {
                                ledComplete.On = true;
                            }));

                            while (true)
                            {
                                if (ModbusTcp.Write(completeAddrs.ToString(), (ushort)1).IsSuccess)
                                    break;
                                Thread.Sleep(25);
                            }

                            while (true)
                            {
                                readStart = ModbusTcp.Read(startAddrs.ToString(), 1);

                                var start1 = readStart;
                                BeginInvoke(new Action(() =>
                                {
                                    if (start1.IsSuccess && start1.Content[0] * 256 + start1.Content[1] > 0)
                                        ledStart.On = true;
                                    else
                                        ledStart.On = false;
                                }));

                                if (readStart.IsSuccess && readStart.Content[0] * 256 + readStart.Content[1] == 0)
                                {
                                    while (true)
                                    {
                                        if (ModbusTcp.Write(completeAddrs.ToString(), (ushort)0).IsSuccess)
                                            break;
                                        Thread.Sleep(25);
                                    }
                                    break;
                                }

                                Thread.Sleep(25);
                            }
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                IsErrorCleared = false;
                                btnClearError.Enabled = true;
                            }));

                            while (true)
                            {
                                ModbusTcp.Write(errorAddrs.ToString(), (ushort)1);

                                BeginInvoke(new Action(() =>
                                {
                                    ledError.On = true;
                                }));

                                if (IsErrorCleared)
                                {
                                    while (true)
                                    {
                                        if (ModbusTcp.Write(errorAddrs.ToString(), (ushort)0).IsSuccess)
                                            break;
                                        Thread.Sleep(25);
                                    }

                                    Invoke(new Action(() =>
                                    {
                                        IsErrorCleared = false;
                                        btnClearError.Enabled = false;
                                    }));
                                    break;
                                }

                                Thread.Sleep(25);
                            }
                        }
                    }
                }
            }
        }

        private void InitGgv()
        {
            dgvCodeDetail.Style = UIStyle.Gray;
            dgvCodeDetail.ReadOnly = true;
            dgvCodeDetail.RowHeadersVisible = false;
            dgvCodeDetail.AllowUserToAddRows = false;
            dgvCodeDetail.AllowUserToResizeRows = false;
            dgvCodeDetail.AllowUserToDeleteRows = false;
            dgvCodeDetail.MultiSelect = true;
            dgvCodeDetail.RowHeadersVisible = false;
            dgvCodeDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            txtDeviceSn.Text = string.Empty;
            txtGroupName.Text = string.Empty;
            txtProgramName.Text = string.Empty;
            txtTrayNo.Text = string.Empty;

            dgvCodeDetail.ClearAll();

            dgvCodeDetail.AddColumn("SN", "SN");
            dgvCodeDetail.AddColumn("CODE1", "CODE1");
            dgvCodeDetail.AddColumn("CODE2", "CODE2");
            dgvCodeDetail.AddColumn("CODE3", "CODE3");
            dgvCodeDetail.AddColumn("CODE4", "CODE4");
            dgvCodeDetail.AddColumn("CODE5", "CODE5");
            dgvCodeDetail.AddColumn("CODE6", "CODE6");
        }

        private bool TryPostWebRequest(string sn, out LaserCarvingReqObject obj)
        {
            obj = new LaserCarvingReqObject();
            var url = _getUrl;//"http://192.168.0.136:8901/LaserCarvingApi/LaserMarkerGetData";
            var header = _header;//"DEVICE_SN";
            var client =
                new RestClient(string.Format("{0}?{1}={2}", url, header, sn));
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            var response = client.Execute(request);

            if (response.StatusCode != HttpStatusCode.OK)
                return false;

            try
            {
                obj = JsonConvert.DeserializeObject<LaserCarvingReqObject>(response.Content);

                return obj != null && obj.data != null;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void btnConfig_Click(object sender, EventArgs e)
        {
            var option = new UIEditOption { AutoLabelWidth = true, Text = @"参数配置" };

            option.AddText("Header", "Header", _header, true);
            option.AddText("GetUrl", "GetUrl", _getUrl, true);
            option.AddText("PostUrl", "PostUrl", _postUrl, true);
            option.AddText("DeviceSn", "DeviceSn", _deviceSn, true);
            option.AddText("TxtFolderPath", "TxtFolderPath", _txtFolderPath, true);
            option.AddInteger("TxtFileMaxCount", "TxtFileMaxCount", int.Parse(_txtFileMaxCount));
            option.AddText("PlcIpAddress", "PlcIpAddress", _plcIpAddress, true);
            option.AddInteger("PlcIpStation", "PlcIpStation", int.Parse(_plcIpStation));
            option.AddInteger("StartSignal", "StartSignal", int.Parse(_startSignal));
            option.AddInteger("CompleteSignal", "CompleteSignal", int.Parse(_completeSignal));
            option.AddInteger("ErrorSignal", "ErrorSignal", int.Parse(_errorSignal));

            var frm = new UIEditForm(option);
            frm.Render();
            frm.ShowDialog();

            if (!frm.IsOK)
            {
                this.ShowInfoTip("用户已取消");
                return;
            }

            if (this.ShowAskDialog("是否确定更新？"))
            {
                Setup.IniWriteValue("LaserCarvingApi", "Header", frm["Header"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "GetUrl", frm["GetUrl"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "PostUrl", frm["PostUrl"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "DeviceSn", frm["DeviceSn"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "TxtFolderPath", frm["TxtFolderPath"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "TxtFileMaxCount", frm["TxtFileMaxCount"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "PlcIpAddress", frm["PlcIpAddress"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "PlcIpStation", frm["PlcIpStation"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "StartSignal", frm["StartSignal"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "CompleteSignal", frm["CompleteSignal"].ToString());
                Setup.IniWriteValue("LaserCarvingApi", "ErrorSignal", frm["ErrorSignal"].ToString());
            }
            else
            {
                this.ShowInfoTip("用户已取消");
            }
        }

        internal class PostInfo
        {
            public string DEVICE_SN { get; set; }
            public string TRAYNO { get; set; }
            public string GROUP_NUM { get; set; }
            public string PROGRAM_NAME { get; set; }

            public SN_DETAIL[] SN_DETAIL { get; set; }
        }

        internal class SN_DETAIL
        {
            public string SN { get; set; }
            public string SN_LEVEL { get; set; }
        }

        private void btnClearError_Click(object sender, EventArgs e)
        {
            if (this.ShowAskDialog("获取数据失败，确定后将重新获取"))
            {
                IsErrorCleared = true;
            }
            else
            {
                this.ShowInfoTip("用户已取消");
            }
        }
    }
}
