using System;
using System.ComponentModel;
using CommonUtility;
using Newtonsoft.Json;

namespace Controller
{
    public sealed class HikBarcodeScaner : ControllerBase
    {
        private readonly HikScanerClass _hikScaner = new HikScanerClass();
        public string ReadBarcodeResults = string.Empty;
        public string ReadBarcodes = string.Empty;

        public HikBarcodeScaner(string name)
            : base(name) { }

        ~HikBarcodeScaner()
        {
            if (_hikScaner == null)
                return;
            try
            {
                _hikScaner.StopGrabbing();
                _hikScaner.CloseDevices();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        [Description("打开设备")]
        public bool OpenScanner()
        {
            return _hikScaner.OpenDevices();
        }

        [Description("关闭设备")]
        public void CloseScanner()
        {
            _hikScaner.CloseDevices();
        }

        [Description("打开采集")]
        public bool StartGrabbing()
        {
            return _hikScaner.StartGrabbing();
        }

        [Description("关闭采集")]
        public void StopGrabbing()
        {
            _hikScaner.StopGrabbing();
        }

        [Description("设置为连续模式")]
        public void SetContinuesMode(string deviceSn)
        {
            _hikScaner.SetContinuesMode(deviceSn);
        }

        [Description("设置为软触发模式")]
        public void SetSoftTriggerMode(string deviceSn)
        {
            _hikScaner.SetTriggerMode(deviceSn);
        }

        [Description("软触发一次")]
        public void SoftTrigger(string deviceSn, string delayMs)
        {
            ReadBarcodeResults = string.Empty;
            ReadBarcodes = string.Empty;

            int ms;
            if (int.TryParse(delayMs, out ms))
            {
                if (ms <= 0 || ms > 10000)
                {
                    ms = 2500;
                }
            }

            string results;
            if (_hikScaner.TriggerSoftware(deviceSn, out results, ms) && !string.IsNullOrEmpty(results))
            {
                var tempResult = JsonConvert.DeserializeObject<HikScanerClass.BarcodeScanResult>(results);

                ReadBarcodeResults = results;

                for (var i = 0; i < tempResult.BarcodeStructs.Count; i++)
                {
                    ReadBarcodes += tempResult.BarcodeStructs[i].Barcode;
                    if (i != tempResult.BarcodeStructs.Count - 1)
                    {
                        ReadBarcodes += "\r\n";
                    }
                }
            }

            //Thread.Sleep(250);
        }
    }
}
