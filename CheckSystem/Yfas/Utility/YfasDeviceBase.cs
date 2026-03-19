using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Controller;
using HZH_Controls.IconFont;

namespace CheckSystem.Yfas.Utility
{
    public static class YfasDeviceBase
    {
        public static string User;
        public static LimitType Limit = LimitType.User;

        public static List<YfasIoController> YfasIoControllers = new List<YfasIoController>();
        public static ZlgUsbCanFd200U CanController;
        public static ToomossUsb2XxxSlaveLin SlaveLinContoller;
        public static Sgm458Msm ProductController;
        //public static Sgm458Plg ProductController;
        public static CheckApp CheckApp;

        public static ImageList Images = new ImageList();
        public static string XmlFileNameLoad = string.Format(@"{0}\{1}\{2}", Program.SysDir,
                @"ControllerConfig\YFAS", "TreeViewBufferLoad.xml");
        //@"E:\Projects-2022\延锋-458座椅模块\调试整理\TreeViewBufferLoad.xml";
        public static string XmlFileNameSaved = string.Format(@"{0}\{1}\{2}", Program.SysDir,
                @"ControllerConfig\YFAS", "TreeViewBufferSaved.xml");
        //@"E:\Projects-2022\延锋-458座椅模块\调试整理\TreeViewBufferSaved.xml";

        public class YfasIoController
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string IpPort { get; set; }
            public SyControllerWith56Pin SyController { get; set; }
            public bool IsUse { get; set; }
        }

        public static void LoadImages()
        {
            Images.Images.Add(FontImages.GetIcon(FontIcons.A_fa_play, 32,
                Color.DodgerBlue));
            Images.Images.Add(FontImages.GetIcon(FontIcons.A_fa_question, 32,
                Color.DodgerBlue));
            Images.Images.Add(FontImages.GetIcon(FontIcons.A_fa_hourglass, 32,
                Color.DodgerBlue));
        }

        public static void InitController()
        {
            LoadSyController();
            ConnectController();
        }

        private static void LoadSyController()
        {
            var baseIp = 30;

            for (var i = 0; i < 5; i++)
            {
                var ipAddr = string.Format("192.168.1.{0}:8088", baseIp++);
                //var ipAddr = string.Format("127.0.0.1:80{0}", baseIp++);

                YfasIoControllers.Add(new YfasIoController
                {
                    Id = i + 1,
                    Name = "控制器" + (i + 1),
                    IsUse = i < 3,
                    IpPort = ipAddr
                });
            }
        }

        public static void DisposeController()
        {
            if (ProductController != null)
                ProductController.Dispose();
            if (CanController != null)
                CanController.Dispose();
            if (SlaveLinContoller != null)
                SlaveLinContoller.Dispose();
            if (CheckApp != null)
                CheckApp.Dispose();
            foreach (var c in YfasIoControllers.Where(c => c != null && c.SyController != null))
                c.SyController.Dispose();
        }

        private static void ConnectController()
        {
            CheckApp = new CheckApp("SystemApp");

            foreach (var c in YfasIoControllers.Where(c => c.IsUse))
            {
                c.SyController = new SyControllerWith56Pin(c.Name);
                c.SyController.InitRemoteIpAddress(c.IpPort);
                //c.SyController.GatheringFrequency
                c.SyController.StartAutoRefresh();
            }

            CanController = new ZlgUsbCanFd200U("ZLG");
            SlaveLinContoller = new ToomossUsb2XxxSlaveLin("ToomossSlaveLin");
            ProductController = new Sgm458Msm("458座椅模块")
            {
                DiagnosticCan1 = CanController.ZlgCanChannel0,
                AppCan2 = CanController.ZlgCanChannel1,
                LedLin = YfasIoControllers[0].SyController.GatewayLin,
                MsmLin = SlaveLinContoller.SlaveLin1
            };
            //ProductController = new Sgm458Plg("458座椅模块") { Can = CanController.ZlgCanChannel0 };
        }

        public static void SetCarModel(string row, string pos)
        {
            if (string.Equals(row, "row1", StringComparison.CurrentCultureIgnoreCase))
            {
                if (string.Equals(pos, "dr", StringComparison.CurrentCultureIgnoreCase))
                {
                    ProductController.SetDr();
                    ProductController.DtcBlcakListFilePath = string.Format(@"{0}\{1}\{2}", Program.SysDir,
                        @"ControllerConfig\YFAS", "副驾驶DTC黑名单.txt");
                    ProductController.ReadDtcBlackListFromFile();
                    ProductController.StartSlaveLin();
                }
                else if (string.Equals(pos, "pa", StringComparison.CurrentCultureIgnoreCase))
                {
                    ProductController.SetPa();
                    ProductController.DtcBlcakListFilePath = string.Format(@"{0}\{1}\{2}", Program.SysDir,
                                    @"ControllerConfig\YFAS", "副驾驶DTC黑名单.txt");
                    ProductController.ReadDtcBlackListFromFile();
                    ProductController.StartSlaveLin();
                }
            }
            else if (string.Equals(row, "row2", StringComparison.CurrentCultureIgnoreCase))
            {
                if (string.Equals(pos, "2l", StringComparison.CurrentCultureIgnoreCase))
                {
                    ProductController.Set2L();
                    ProductController.DtcBlcakListFilePath = string.Format(@"{0}\{1}\{2}", Program.SysDir,
                        @"ControllerConfig\YFAS", "副驾驶DTC黑名单.txt");
                    ProductController.ReadDtcBlackListFromFile();
                    //ProductController.StopSlaveLin();
                }
                else if (string.Equals(pos, "2r", StringComparison.CurrentCultureIgnoreCase))
                {
                    ProductController.Set2R();
                    ProductController.DtcBlcakListFilePath = string.Format(@"{0}\{1}\{2}", Program.SysDir,
                        @"ControllerConfig\YFAS", "副驾驶DTC黑名单.txt");
                    ProductController.ReadDtcBlackListFromFile();
                    //ProductController.StopSlaveLin();
                }
            }
            else if (string.Equals(row, "row3", StringComparison.CurrentCultureIgnoreCase))
            {
                if (string.Equals(pos, "3l", StringComparison.CurrentCultureIgnoreCase))
                {
                    ProductController.Set3R();
                    ProductController.DtcBlcakListFilePath = string.Format(@"{0}\{1}\{2}", Program.SysDir,
                        @"ControllerConfig\YFAS", "副驾驶DTC黑名单.txt");
                    ProductController.ReadDtcBlackListFromFile();
                    ProductController.StartSlaveLin();
                }
                else if (string.Equals(pos, "3r", StringComparison.CurrentCultureIgnoreCase))
                {
                    ProductController.Set3R();
                    ProductController.DtcBlcakListFilePath = string.Format(@"{0}\{1}\{2}", Program.SysDir,
                        @"ControllerConfig\YFAS", "副驾驶DTC黑名单.txt");
                    ProductController.ReadDtcBlackListFromFile();
                    ProductController.StartSlaveLin();
                }
            }
        }

        public static ControllerBase GetController(int id)
        {
            ControllerBase controller = null;
            if (id == 0) // 458 controller
                controller = ProductController;
            else if (id >= 1 && id <= 5) // 56pin
                controller = YfasIoControllers.Find(f => f.Id == id).SyController;
            else if (id == -1) // checkApp
                controller = CheckApp;

            return controller;
        }

        public enum LimitType
        {
            User,

            Admin,

            SystemAdmmin
        }
    }
}
