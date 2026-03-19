using System;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CommonUtility;
using HZH_Controls.IconFont;
using System.Collections.Generic;
using CommonUtility.FileOperator;

namespace CheckSystem.MaterialHelperForms
{
    public partial class HikSerialPortToSap : Form
    {
        private readonly IniFileHelper _setup =
            new IniFileHelper(string.Format(@"{0}\仓库电子料标签生成\{1}", Program.SysDir, "StockSysSetup.ini"));

        private const string NgReplay = "NG";
        private bool _isSapStockIn;

        [DllImport("USER32.DLL")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        public HikSerialPortToSap()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Icon = FontImages.GetIcon(FontIcons.E_icon_check, 32,
               Color.DodgerBlue);

            var comBaudRate = _setup.IniReadValue("SAP", "RecvSerialPort");

            try
            {
                var com = comBaudRate.Split(':')[0];
                var baudRate = int.Parse(comBaudRate.Split(':')[1]);

                var recvPort = new SerialPort(com, baudRate);
                recvPort.Open();

                if (!recvPort.IsOpen)
                    return;
                recvPort.DataReceived += _recvPort_DataReceived;
                btnStart.Enabled = true;
                richTextBox1.BackColor = Color.LightSlateGray;

                if (_worker != null)
                {
                    _worker.Abort();
                    _worker.Join();
                }
                _worker = new Thread(Work) { IsBackground = true };
                _worker.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void HikSerialPortToSap_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                EqueueTaskNull();

                if (_wh != null)
                    _wh.Close();

                if (_worker != null)
                {
                    _worker.Abort();
                    _worker.Join();
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private void _recvPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(250);
            var serialPort = sender as SerialPort;
            if (serialPort == null)
                return;
            var len = serialPort.BytesToRead;
            var buff = new byte[len];
            serialPort.Read(buff, 0, len);

            Invoke(new Action(() =>
            {
                try
                {
                    var resultList = Encoding.ASCII.GetString(buff).Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                    if (!resultList.Any())
                        return;
                    var other2Str = resultList[resultList.Count - 1].TrimEnd();

                    if (other2Str.Length <= 12)
                    {
                        serialPort.WriteLine(NgReplay);
                        return;
                    }

                    if (!_isSapStockIn)
                    {
                        serialPort.WriteLine(NgReplay);
                        return;
                    }

                    serialPort.WriteLine(other2Str);
                    EqueueSendTask(other2Str);
                }
                catch (Exception)
                {
                    serialPort.WriteLine(NgReplay);
                }
            }));
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            const string showValue =
                "是否确认开始入库？\r\n请先将鼠标点击SAP界面中的入库输入框，再点击确定。\r\n入库过程中请勿操作任何鼠标、键盘等按钮！";
            if (
              MessageBox.Show(showValue, @"确认",
                  MessageBoxButtons.OKCancel) != DialogResult.OK)
                return;

            _isSapStockIn = true;
            btnStart.Enabled = false;
            btnStart.BackColor = Color.DarkGreen;
        }

        private bool ToSap(string other2)
        {
            try
            {
                var other2Str = other2;
                //stockInNo = other2Str.Split('@')[0];

                richTextBox1.Text = other2Str;
                richTextBox1.BackColor = Color.LightSlateGray;

                var printFormIpClassName = _setup.IniReadValue("SAP", "PrintFormIpClassName");
                var printFormIpWindowName = _setup.IniReadValue("SAP", "PrintFormIpWindowName");

                var sapFormName = _setup.IniReadValue("SAP", "FormName");

                var delayMsStr = _setup.IniReadValue("SAP", "SendKeyDelayMs");
                int delayMs;
                if (!int.TryParse(delayMsStr, out delayMs))
                    delayMs = 4500;

                var backSpaceCountStr = _setup.IniReadValue("SAP", "BackSpaceCount");
                int backSpacecount;
                if (!int.TryParse(backSpaceCountStr, out backSpacecount))
                    backSpacecount = 100;

                for (int i = 0; i < 5; i++)
                {
                    var isHavePrintWindow = FindWindow(printFormIpClassName.ToLower() == "null" ? null : printFormIpClassName, printFormIpWindowName);
                    if (isHavePrintWindow != IntPtr.Zero)
                    {
                        // 如果有打印窗口，先显示，并关闭
                        //ShowWindow(isHavePrintWindow, 1);
                        SetForegroundWindow(isHavePrintWindow);
                        Thread.Sleep(100);

                        SendKeys.SendWait("~"); //发命令关闭打印窗口
                        Thread.Sleep(500);
                    }
                    else
                    {
                        break;
                    }
                }

                var winform = FindWindow(null, sapFormName);

                if (winform == IntPtr.Zero)
                {
                    richTextBox1.BackColor = Color.Red;
                    return false;
                }

                //ShowWindow(winform, 1);
                SetForegroundWindow(winform);
                Thread.Sleep(50);

                for (var i = 0; i < backSpacecount; i++)
                    SendKeys.SendWait("{BACKSPACE}");
                for (var i = 0; i < backSpacecount; i++)
                    SendKeys.SendWait("{DEL}");
                Thread.Sleep(100);
                var sendKeys = string.Empty;

                var sendOther2Str = other2Str.Substring(0, other2Str.Length - 12);
                foreach (var t in sendOther2Str)
                {
                    var asciiHex = Encoding.ASCII.GetBytes(t.ToString())[0];
                    if (asciiHex >= 0x41 && asciiHex <= 0x5A)
                        sendKeys += string.Format("+{0}", t);
                    else
                        sendKeys += string.Format("{0}", t);
                }

                SendKeys.SendWait(sendKeys);
                Thread.Sleep(200);
                SendKeys.SendWait("{ENTER}");
                Thread.Sleep(1000);

                // SAP会调用打印窗体，然后输入“~”完成打印
                var enterTime = DateTime.Now;
                while (true)
                {
                    var printWindow = FindWindow(printFormIpClassName.ToLower() == "null" ? null : printFormIpClassName, printFormIpWindowName);
                    if (printWindow != IntPtr.Zero) //打印窗口出现
                    {
                        SetForegroundWindow(printWindow);
                        SendKeys.SendWait("~"); //发命令关闭打印窗口，~即{ENTER}，为回车键
                        Thread.Sleep(200);

                        for (var i = 0; i < 5; i++)
                        {
                            var restPrint = FindWindow(printFormIpClassName.ToLower() == "null" ? null : printFormIpClassName, printFormIpWindowName);
                            if (restPrint != IntPtr.Zero)
                            {
                                SetForegroundWindow(restPrint);
                                SendKeys.SendWait("~"); //发命令关闭打印窗口，~即{ENTER}，为回车键
                                Thread.Sleep(200);
                            }
                            else
                            {
                                break;
                            }
                        }

                        break;
                    }

                    if (ValueHelper.GetTimeSpanMs(enterTime, DateTime.Now) <= delayMs)
                        continue;
                    richTextBox1.BackColor = Color.Red;
                    return false;
                }

                enterTime = DateTime.Now;
                while (true)
                {
                    var printWindow = FindWindow(printFormIpClassName.ToLower() == "null" ? null : printFormIpClassName, printFormIpWindowName);

                    if (printWindow == IntPtr.Zero) //打印窗口关闭
                    {
                        richTextBox1.BackColor = Color.Green;
                        return true;
                    }

                    if (ValueHelper.GetTimeSpanMs(enterTime, DateTime.Now) <= delayMs)
                        continue;
                    richTextBox1.BackColor = Color.Red;
                    return false;
                }
            }
            catch (Exception)
            {
                richTextBox1.BackColor = Color.Red;
                return false;
            }
        }

        private readonly Queue<Func<bool>> _sendTasks = new Queue<Func<bool>>();
        private readonly object _locker = new object();
        private readonly EventWaitHandle _wh = new AutoResetEvent(false);
        private readonly Thread _worker;

        private void Work()
        {
            while (_worker.IsAlive)
            {
                if (!_worker.IsAlive)
                    return;

                Func<bool> mySendQueueWork = null;
                lock (_locker)
                {
                    if (_sendTasks.Count > 0)
                    {
                        button1.Text = @"剩余未入：" + (_sendTasks.Count - 1);

                        mySendQueueWork = _sendTasks.Dequeue();

                        if (mySendQueueWork == null)
                            return;
                    }
                }

                if (mySendQueueWork != null)
                {
                    mySendQueueWork.Invoke();
                }
                else
                    _wh.WaitOne();
            }
        }

        public void EqueueSendTask(string other2)
        {
            lock (_locker)
            {
                _sendTasks.Enqueue(() => ToSap(other2));
            }
            _wh.Set();
        }

        private void EqueueTaskNull()
        {
            lock (_locker)
                _sendTasks.Enqueue(null);
            _wh.Set();
        }
    }
}
