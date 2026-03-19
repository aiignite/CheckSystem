using System;
using System.Text;
using CommonUtility;
using CommonUtility.FileOperator;

namespace CheckSystem.MaterialHelperForms
{
    public static class HikSetup
    {
        public static IniFileHelper Setup =
            new IniFileHelper(string.Format(@"{0}\仓库电子料标签生成\{1}", Program.SysDir, "StockSysSetup.ini"));

        public static string TriggerControllerIpPort = "192.168.1.28:8088";
        public static string LogFolder = string.Format(@"{0}\仓库电子料标签生成", Program.SysDir);
        public static string LogJpeg = @"LogJpeg.jpg";
        public static string HiRobotMachineSn = "00L17741467";
        public static string HiRobotMachineSn2 = "00L17741467";
        public static int TriggerDelayMs = 1500;
        public static string TriggerTimeout = "1500";
        //private readonly object _lockStockInScaners = new object();
        public static MySerialPort FeedInMaterialScaner;
        public static MySerialPort FeedInBoxScanner;
        public static bool IsFeedInBoxScanerTriggerMode;
        public static MySerialPort FeedInMaterialBarcodePrinter;
        public static MySerialPort ToSapPort;
        public static int ToSapDelay = 2500;
        public static int FeedInScanDelayMs;
        public static int MaxCount = 25000;

        public static string PrintWidth;
        public static string PrintHeigth;
        public static string PrintGap;
        public static string PrintDensity;
        public static string PrintSpeed;
        public static string PrintDirection;
        public static string PrintFunc;

        public static string PrintPn;
        public static string PrintDcl;
        public static string PrintBin;
        public static string PrintPno;
        public static string PrintSup;
        public static string PrintQty;
        public static string PrintDate;
        public static string PrintNo;
        public static string PrintQualevel;
        public static string PrintQrcode;

        public static void ReadSetup()
        {
            HiRobotMachineSn = Setup.IniReadValue("HikRobotParas", "DeviceSN");
            HiRobotMachineSn2 = Setup.IniReadValue("HikRobotParas", "DeviceSN2");
            TriggerTimeout = Setup.IniReadValue("HikRobotParas", "SoftTriggerTimeoutMs");
            TriggerControllerIpPort = Setup.IniReadValue("HikRobotParas", "TriggerControllerIpPort");

            var readTriggerTimeout = Setup.IniReadValue("HikRobotParas", "SoftTriggerDelayMs");
            int timeOut;
            if (int.TryParse(readTriggerTimeout, out timeOut))
                TriggerDelayMs = timeOut;
            LogJpeg = Setup.IniReadValue("HikRobotParas", "LogName");

            int maxCount;
            if (int.TryParse(Setup.IniReadValue("PrintParas", "MaxCount"), out maxCount) &&
                maxCount > 0)
                MaxCount = maxCount;

            ReadPrintParas();
        }

        public static void ReadPrintParas()
        {
            PrintWidth = Setup.IniReadValue("PrintParas", "Width");
            PrintHeigth = Setup.IniReadValue("PrintParas", "Heigth");
            PrintGap = Setup.IniReadValue("PrintParas", "Gap");
            PrintDensity = Setup.IniReadValue("PrintParas", "DENSITY");
            PrintSpeed = Setup.IniReadValue("PrintParas", "SPEED");
            PrintDirection = Setup.IniReadValue("PrintParas", "DIRECTION");
            PrintFunc = Setup.IniReadValue("PrintParas", "PRINT");

            PrintPn = Setup.IniReadValue("PrintParas", "PN");
            PrintDcl = Setup.IniReadValue("PrintParas", "DCL");
            PrintBin = Setup.IniReadValue("PrintParas", "BIN");
            PrintPno = Setup.IniReadValue("PrintParas", "PNO");
            PrintSup = Setup.IniReadValue("PrintParas", "SUP");
            PrintQty = Setup.IniReadValue("PrintParas", "QTY");
            PrintDate = Setup.IniReadValue("PrintParas", "DATE");
            PrintNo = Setup.IniReadValue("PrintParas", "NO");
            PrintQualevel = Setup.IniReadValue("PrintParas", "Qualevel");
            PrintQrcode = Setup.IniReadValue("PrintParas", "QRCODE");
        }

        public static void PrintLabel(
            string pn, string dcl, string bin, string pno, string sup, string qty, string date, string no, string qualevel, string qrCode,
            string pnPos = "", string dclPos = "", string binPos = "", string pnoPos = "", string supPos = "", string qtyPos = "", string datePos = "", string noPos = "", string qualevelPos = "", string qrCodePos = "",
            string widthPara = "", string heightPara = "", string gapPara = "", string densityPara = "", string speedPara = "", string dirPara = "", string funcPara = "")
        {
            try
            {
                ReadPrintParas();

                var printCmd = new StringBuilder();
                printCmd.Append("SIZE " + (!string.IsNullOrEmpty(widthPara) ? widthPara : PrintWidth) + " mm, " + (!string.IsNullOrEmpty(heightPara) ? heightPara : PrintHeigth) + " mm\r\n");
                printCmd.Append("GAP " + (!string.IsNullOrEmpty(gapPara) ? gapPara : PrintGap) + " mm\r\n");
                printCmd.Append("DENSITY " + (!string.IsNullOrEmpty(densityPara) ? densityPara : PrintDensity) + "\r\n");
                printCmd.Append("SPEED " + (!string.IsNullOrEmpty(speedPara) ? speedPara : PrintSpeed) + "\r\n");
                printCmd.Append("CLS\r\n");
                printCmd.Append("DIRECTION " + (!string.IsNullOrEmpty(dirPara) ? dirPara : PrintDirection) + "\r\n");

                // TEXT 27,400,"4",0,1,1,"NO:0002"
                printCmd.Append(string.Format("TEXT {0},\"{1}\"\r\n", !string.IsNullOrEmpty(pnPos) ? pnPos : PrintPn, pn)); // TEXT 27,400,"4",0,1,1,"NO:0002"
                printCmd.Append(string.Format("TEXT {0},\"{1}\"\r\n", !string.IsNullOrEmpty(dclPos) ? dclPos : PrintDcl, dcl));
                printCmd.Append(string.Format("TEXT {0},\"{1}\"\r\n", !string.IsNullOrEmpty(binPos) ? binPos : PrintBin, bin));
                printCmd.Append(string.Format("TEXT {0},\"{1}\"\r\n", !string.IsNullOrEmpty(pnoPos) ? pnoPos : PrintPno, pno));
                printCmd.Append(string.Format("TEXT {0},\"{1}\"\r\n", !string.IsNullOrEmpty(supPos) ? supPos : PrintSup, sup));
                printCmd.Append(string.Format("TEXT {0},\"{1}\"\r\n", !string.IsNullOrEmpty(qtyPos) ? qtyPos : PrintQty, qty));
                printCmd.Append(string.Format("TEXT {0},\"{1}\"\r\n", !string.IsNullOrEmpty(datePos) ? datePos : PrintDate, date));
                printCmd.Append(string.Format("TEXT {0},\"{1}\"\r\n", !string.IsNullOrEmpty(noPos) ? noPos : PrintNo, no));
                printCmd.Append(string.Format("TEXT {0},\"{1}\"\r\n", !string.IsNullOrEmpty(qualevelPos) ? qualevelPos : PrintQualevel, qualevel));
                printCmd.Append(string.Format("QRCODE {0},\"{1}\"\r\n", !string.IsNullOrEmpty(qrCodePos) ? qrCodePos : PrintQrcode, qrCode)); // QRCODE 430,200,L,7,A,0,M2,S7,"ST1100000008@20230216@0002@11007004@1000@@@@ZHJTSSSSSS@1049@@A@Q@A@@@"

                printCmd.Append("PRINT " + (!string.IsNullOrEmpty(funcPara) ? funcPara : PrintFunc) + "\r\n");

                if (FeedInMaterialBarcodePrinter == null)
                    return;
                if (Setup.IniReadValue("SerialPort", "IsPrintSendAscii") == 1.ToString())
                    FeedInMaterialBarcodePrinter.SendCommand(printCmd.ToString());
                else
                    FeedInMaterialBarcodePrinter.SendCommand(Encoding.ASCII.GetBytes(printCmd.ToString()));
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
