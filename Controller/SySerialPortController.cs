using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Controller
{
    public sealed class SySerialPortController : ControllerBase
    {
        #region Member
        public string currentData;
        private double currentDatadouble;
        SerialPort mySerialPort;
        Encoding encoding;
        byte[] buffer;
        List<ReadSlice> readSlices;
        List<string> sendList;
        Task sendTask;
        string[] previousData;
        struct ReadSlice
        {
            public int readStartIndex;
            public int readCount;
        }
        #endregion

        private Func<byte[], bool> failterFunc = new Func<byte[], bool>((bytes) =>
        {
            if (bytes.Length % 10 == 0 && (bytes[0] == 0X2B || bytes[0] == 0X2D))
            {
                return true;
            }
            return false;
        });


        #region Variable
        List<int> baudrateList = new List<int> { 4800, 9600, 19200, 38400, 115200 };
        CancellationTokenSource tokenSource;
        #endregion

        #region Construct

        public SySerialPortController(string name)
            : base(name)
        {
            this.Name = name;
            encoding = Encoding.Default;
            buffer = new byte[256];
            readSlices = new List<ReadSlice>();
            sendList = new List<string>();
            previousData = new string[1024];
        }
        #endregion

        #region Methods
        public void Init(string portName_BaudRate)
        {
            var parm = portName_BaudRate.Split(':');
            mySerialPort = new SerialPort(parm[0]);
            mySerialPort.BaudRate = int.Parse(parm[1]);
            mySerialPort.Parity = Parity.None;
            mySerialPort.StopBits = StopBits.One;
            mySerialPort.DataBits = 8;
            mySerialPort.Handshake = Handshake.None;
            mySerialPort.ReadTimeout = 200;
            mySerialPort.RtsEnable = true;
            mySerialPort.Open();
            mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }
        public void ReceivedBytesThreshold(int i)
        {
            mySerialPort.ReceivedBytesThreshold = i;
        }
        public void SetEncoding(string str)
        {
            switch (str.ToLower())
            {
                case "ascii":
                    encoding = Encoding.ASCII;
                    break;
                case "utf8":
                case "utf-8":
                    encoding = Encoding.UTF8;
                    break;
                case "unicode":
                    encoding = Encoding.Unicode;
                    break;
                default:
                    break;
            }
        }

        [Retry(30, true, Interval = 1000)]
        public bool Send(string str)
        {

            var sendByte = StringToBytes(str);
            mySerialPort.Write(sendByte, 0, sendByte.Length);

            return !string.IsNullOrEmpty(currentData);
        }

        [Description("添加周期发送命令值")]
        public void AddSendStr(string str)
        {
            sendList.Add(str.Replace("@", ","));
        }

        [Description("开始周期发送命令")]
        public void StartOnPeriodSend()
        {
            tokenSource = new CancellationTokenSource();
            if (!sendList.Any())
                return;
            sendTask = Task.Run(() =>
            {
                while (!tokenSource.IsCancellationRequested)
                {
                    foreach (var item in sendList)
                    {
                        if (true)
                        {
                            Send(item);
                            Thread.Sleep(50);
                        }
                    }
                }
            }, tokenSource.Token);
        }

        [Description("停止周期发送命令")]
        public void StopOnperiodSend()
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
        }

        public void AddReadSlice(string startIndex_Count)
        {
            var parm = startIndex_Count.Split(':');
            readSlices.Add(new ReadSlice { readStartIndex = int.Parse(parm[0]), readCount = int.Parse(parm[1]) });
        }

        public float GetMaxValue()
        {
            float max = -999f;
            foreach (var item in previousData)
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                float temp;
                if (float.TryParse(item, out temp))
                {
                    max = temp > max ? temp : max;
                }
            }
            return max;
        }

        public float GetMinValue()
        {

            float min = 999f;
            foreach (var item in previousData)
            {
                if (string.IsNullOrEmpty(item))
                    continue;
                float temp;
                if (float.TryParse(item, out temp))
                {
                    min = temp < min ? temp : min;
                }
            }
            return min;
        }

        [Description("清除缓存")]
        public void ClearPerviousData()
        {
            previousData = new string[1024];
        }

        int i = 0;
        private void DataReceivedHandler(
                    object sender,
                    SerialDataReceivedEventArgs e)
        {
            currentDatadouble = -9999;
            currentData = string.Empty;
            SerialPort sp = (SerialPort)sender;

            if (sp.BytesToRead != sp.ReceivedBytesThreshold)
            {
                sp.DiscardInBuffer();
                return;
            }

            sp.Encoding = encoding;
            sp.Read(buffer, 0, sp.ReceivedBytesThreshold);
            /*
                        if (!failterFunc.Invoke(buffer))
                        {
                            return;
                        }
            */
            //Console.WriteLine();
            sp.DiscardInBuffer();
            string indata = sp.ReadExisting();
            foreach (var item in readSlices)
            {
                currentData += encoding.GetString(buffer, item.readStartIndex, item.readCount);
            }
            previousData[i] = currentData;
            double.TryParse(currentData, out currentDatadouble);
            i = i < 1023 ? ++i : 0;


        }
        #endregion

        [Description("R,缓存位置1")]
        public double data1Value = -9999;

        [Description("R,缓存位置2")]
        public double data2Value = -9999;

        [Description("R,缓存位位置1与缓存位置2的差值 ")]
        public double data12Value = -9999;

        [Description("读取值存入缓存位置1")]
        public void Get1Value()
        {
            data1Value = currentDatadouble;
            Console.WriteLine("数值1 ：" + data1Value);
        }

        [Description("读取值存入缓存位2")]
        public void Get2Value()
        {
            data2Value = currentDatadouble;
            Console.WriteLine("数值2 ：" + data2Value);
        }

        [Description("读取缓存位位置1与缓存位置2的差值")]
        public void GetValue12()
        {
            data12Value = Math.Abs(data2Value - data1Value);
            Console.WriteLine("计算 ：" + data12Value);
        }

        private static byte[] StringToBytes(string str)
        {
            var _str = str.Replace(",", " ").Split(' ').ToList();
            var rec = new List<byte>();

            for (int i = 0; i < _str.Count; i++)
            {
                _str[i] = _str[i].Replace("0x", "0X").Replace("0X", String.Empty);
                if (string.IsNullOrWhiteSpace(_str[i]))
                    continue;
                rec.Add(StringToByte(_str[i].Trim()));
            }
            return rec.ToArray();
        }

        private static byte StringToByte(string str)
        {

            if (string.IsNullOrEmpty(str))
            {
                return 0;
            }
            return BitConverter.GetBytes(Convert.ToInt32("0X" + str, 16))[0];
        }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public class Retry : Attribute
    {
        public int times;
        public object expectValue;
        public int Interval;
        public Retry(int _times, object _expectValue, int interval = 1000)
        {
            times = _times;
            expectValue = _expectValue;
        }
    }
}
