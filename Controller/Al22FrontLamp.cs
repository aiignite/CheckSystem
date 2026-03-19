using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using CommonUtility.BusLoader;

namespace Controller
{
    [Description("CAN-Product,A2LL前灯")]
    public sealed class Al22FrontLamp : ControllerBase
    {
        public CanBus Can;
        private VectorDbcEmulator VectorEmulator { get; set; }
        private bool IsCanStarted { get; set; }
        private byte CornerPer { get; set; }

        public Al22FrontLamp(string name)
            : base(name)
        {
            SysConfig(Directory.GetCurrentDirectory() + @"\ControllerConfig\Mld+_geny_24.24.161.dbc");
        }

        ~Al22FrontLamp()
        {
            Dispose();
        }

        [Description("开启CAN消息")]
        public void StartCanMsg()
        {
            IsCanStarted = true;
        }

        [Description("关闭CAN消息")]
        public void StopCanMsg()
        {
            IsCanStarted = false;
        }

        [Description("按百分比打开Corner")]
        public void CornerOn(string per)
        {
            int perValue;
            if (int.TryParse(per, out perValue))
            {
                if (perValue > 0 && perValue <= 100)
                {
                    CornerPer = (byte)(perValue * 2);
                }
            }
        }

        [Description("关闭Corner")]
        public void CornerOff()
        {
            CornerPer = 0x00;
        }

        #region Code from CANoe and DBC

        private void SysConfig(string dbcFilePath)
        {
            VectorEmulator =
                new VectorDbcEmulator(new[] { dbcFilePath });
            //InitVariables();
            //Prestart();
            Start();
        }

        private void Start()
        {
            VectorEmulator.SetTimer(Tmr_Refresh50Ms(), 50);
            VectorEmulator.SetTimer(Tmr_Refresh100Ms(), 100);
            VectorEmulator.SetTimer(Tmr_Refresh400Ms(), 400);
            VectorEmulator.SetTimer(Tmr_Refresh1000Ms(), 1000);
        }

        private Action Tmr_Refresh50Ms()
        {
            return () =>
            {
                var listCanData = new List<CanBus.CanDataPackage>
                {
                    new CanBus.CanDataPackage(0x201, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[4]),
                    new CanBus.CanDataPackage(0x1FF, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x203, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[4]),
                    new CanBus.CanDataPackage(0x204, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[] {CornerPer, CornerPer}),
                    new CanBus.CanDataPackage(0x202, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[5]),
                    new CanBus.CanDataPackage(0x102, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x103, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x114, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x115, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x116, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x117, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x118, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x119, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x104, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x105, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x106, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x107, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x108, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x109, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x10A, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x10B, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x10C, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x10D, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x10E, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x10F, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x110, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x111, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x112, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x113, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x200, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[7])
                };

                if (Can != null)
                    Can.SendCanDatas(listCanData.ToArray());
            };
        }

        private Action Tmr_Refresh100Ms()
        {
            return () =>
            {
                var listCanData = new List<CanBus.CanDataPackage>
                {
                    new CanBus.CanDataPackage(0x1E0, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[2]),
                    new CanBus.CanDataPackage(0x1E1, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[2]),
                    new CanBus.CanDataPackage(0x20D, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[6])
                };

                if (Can != null)
                    Can.SendCanDatas(listCanData.ToArray());
            };
        }

        private Action Tmr_Refresh400Ms()
        {
            return () =>
            {
                var listCanData = new List<CanBus.CanDataPackage>
                {
                    new CanBus.CanDataPackage(0x172, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8]),
                    new CanBus.CanDataPackage(0x20B, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[4])
                };

                if (Can != null)
                    Can.SendCanDatas(listCanData.ToArray());
            };
        }

        private Action Tmr_Refresh1000Ms()
        {
            return () =>
            {
                var listCanData = new List<CanBus.CanDataPackage>
                {
                    new CanBus.CanDataPackage(0x700, CanBus.CanProtocol.Can, CanBus.CanType.Standard,
                        CanBus.CanFormat.Data, new byte[8])
                };

                if (Can != null)
                    Can.SendCanDatas(listCanData.ToArray());
            };
        }

        #endregion
    }
}
