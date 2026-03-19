using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace Controller
{
    public sealed class CcdAutoAssemblyLaserPrinter : ControllerBase
    {
        public CcdAutoAssemblyLaserPrinter
            (string name) : base(name)
        {
        }

        private readonly object _lockBarcode = new object();

        [Description("写入文档")]
        public void PrintBarcode1(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    WriteTxt(filePath);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        public void PrintBarcodeNG(string filePath)
        {
            try
            {
                lock (_lockBarcode)
                {
                    WriteTxt2(filePath);
                }
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void WriteTxt(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                var list = new List<string>();

                DateTime currentTime = new DateTime();
                currentTime = DateTime.Now;
                int year = currentTime.Year;
                int month = currentTime.Month;
                int day = currentTime.Day;
                list.Add(year.ToString().Substring(3, 1));
                if (month == 10)
                {
                    list.Add("X");
                }
                else if (month == 11)
                {
                    list.Add("Y");
                }
                else if (month == 12)
                {
                    list.Add("Z");
                }
                else
                {
                    list.Add(month.ToString());
                }

                if (day == 10)
                {
                    list.Add("A");
                }
                else if (day == 11)
                {
                    list.Add("B");
                }
                else if (day == 12)
                {
                    list.Add("C");
                }
                else if (day == 13)
                {
                    list.Add("D");
                }
                else if (day == 14)
                {
                    list.Add("E");
                }
                else if (day == 15)
                {
                    list.Add("F");
                }
                else if (day == 16)
                {
                    list.Add("G");
                }
                else if (day == 17)
                {
                    list.Add("H");
                }
                else if (day == 18)
                {
                    list.Add("J");
                }
                else if (day == 19)
                {
                    list.Add("K");
                }
                else if (day == 20)
                {
                    list.Add("L");
                }
                else if (day == 21)
                {
                    list.Add("M");
                }
                else if (day == 22)
                {
                    list.Add("N");
                }
                else if (day == 23)
                {
                    list.Add("P");
                }
                else if (day == 24)
                {
                    list.Add("Q");
                }
                else if (day == 25)
                {
                    list.Add("R");
                }
                else if (day == 26)
                {
                    list.Add("S");
                }
                else if (day == 27)
                {
                    list.Add("T");
                }
                else if (day == 28)
                {
                    list.Add("U");
                }
                else if (day == 29)
                {
                    list.Add("V");
                }
                else if (day == 30)
                {
                    list.Add("W");
                }
                else if (day == 31)
                {
                    list.Add("X");
                }
                else
                {
                    list.Add(day.ToString());
                }

                string str = string.Empty;
                foreach (var item in list)
                    str += item;

                lines[0] = str.Trim();

                File.WriteAllLines(filePath, lines);
            }
            catch (Exception)
            {
                // ignored
            }
        }

        private static void WriteTxt2(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                var list = new List<string>();
                list.Add(".");
                lines = list.ToArray();
                File.WriteAllLines(filePath, lines);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
