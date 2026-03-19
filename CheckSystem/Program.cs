using CommonUtility;
using CommonUtility.FileOperator;
using CommonUtility.WlanOperator;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Security;
using System.Threading;
using System.Windows.Forms;

namespace CheckSystem
{
    //-----------------------------------------------------------------------------------------------------
    internal static class Program
    {
        public static string SysDir = Directory.GetCurrentDirectory();
        public static IniFileHelper SysSetup = new IniFileHelper(string.Format(@"{0}\{1}", SysDir, "Setup.ini"));
        public static string DeviceNo = SysSetup.IniReadValue("System", "DeviceNo");
        public static bool IsAutoConnectWifi = SysSetup.IniReadValue("System", "IsAutoConnectWifi") == "1";
        public static bool IsCanRunMultiple = SysSetup.IniReadValue("System", "IsCanRunMultiple") == "1";
        public static bool IsOfflineMode = SysSetup.IniReadValue("System", "IsOfflineMode") == "1";
        public static string OffLineEndTime = SysSetup.IniReadValue("System", "OffLineEndTime");
        public static string OffLineStartTime = SysSetup.IniReadValue("System", "OffLineStartTime");

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e) { }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) { }

        //-----------------------------------------------------------------------------------------------------
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        [HandleProcessCorruptedStateExceptions] //捕获c++异常
        [SecurityCritical] //捕获c++异常
        private static void Main()
        {
            //{
            //    var c56 = new Controller.SyControllerWith56Pin("56pin");
            //    c56.InitRemoteIpAddress("192.168.1.28:8088");
            //    var c18333 = new Controller.Bd18333Euv("18333");
            //    c18333.UartCan = c56.GatewaySci2;
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            var getCurrentMethod = MethodBase.GetCurrentMethod();
            if (getCurrentMethod == null)
            {
                _ = MessageBox.Show(text: @"未找到程序命名空间，无法打开！");
                return;
            }

            var declaringType = getCurrentMethod.DeclaringType;
            if (declaringType == null)
            {
                _ = MessageBox.Show(text: @"未找到程序命名空间，无法打开！");
                return;
            }

            var namespaceStr = declaringType.Namespace;
            var mutex = new Mutex(false, namespaceStr);
            var running = !mutex.WaitOne(0, false);
            if (!running || IsCanRunMultiple)
            {
                LocalDbHelper.InitSqLiteLocal();
                if (IsAutoConnectWifi)
                {
                    WlanHelper.ConnectTargetWifi(@"SEEYAO-SC-AGV001", "XY80319800");
                    Thread.Sleep(100);
                }

                SyProductionSaveCheckData.StartSyProductionSaveCheckData();
                Application.Run(new FormSelection());
            }
            else
                _ = MessageBox.Show(text: @"应用程序已经在运行中!!");
        }
    }
}