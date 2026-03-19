using System;
using System.Collections.Generic;
using System.Threading;
using HslCommunication.ModBus;

namespace Controller
{
    public class WirelessModbusTcpClient : ControllerBase
    {
        public ushort ReadMaxLenth = 120;

        #region Holding Regs
        public ushort Hr40001;
        public ushort Hr40002;
        public ushort Hr40003;
        public ushort Hr40004;
        public ushort Hr40005;
        public ushort Hr40006;
        public ushort Hr40007;
        public ushort Hr40008;
        public ushort Hr40009;
        public ushort Hr40010;
        public ushort Hr40011;
        public ushort Hr40012;
        public ushort Hr40013;
        public ushort Hr40014;
        public ushort Hr40015;
        public ushort Hr40016;
        public ushort Hr40017;
        public ushort Hr40018;
        public ushort Hr40019;
        public ushort Hr40020;
        public ushort Hr40021;
        public ushort Hr40022;
        public ushort Hr40023;
        public ushort Hr40024;
        public ushort Hr40025;
        public ushort Hr40026;
        public ushort Hr40027;
        public ushort Hr40028;
        public ushort Hr40029;
        public ushort Hr40030;
        public ushort Hr40031;
        public ushort Hr40032;
        public ushort Hr40033;
        public ushort Hr40034;
        public ushort Hr40035;
        public ushort Hr40036;
        public ushort Hr40037;
        public ushort Hr40038;
        public ushort Hr40039;
        public ushort Hr40040;
        public ushort Hr40041;
        public ushort Hr40042;
        public ushort Hr40043;
        public ushort Hr40044;
        public ushort Hr40045;
        public ushort Hr40046;
        public ushort Hr40047;
        public ushort Hr40048;
        public ushort Hr40049;
        public ushort Hr40050;
        public ushort Hr40051;
        public ushort Hr40052;
        public ushort Hr40053;
        public ushort Hr40054;
        public ushort Hr40055;
        public ushort Hr40056;
        public ushort Hr40057;
        public ushort Hr40058;
        public ushort Hr40059;
        public ushort Hr40060;
        public ushort Hr40061;
        public ushort Hr40062;
        public ushort Hr40063;
        public ushort Hr40064;
        public ushort Hr40065;
        public ushort Hr40066;
        public ushort Hr40067;
        public ushort Hr40068;
        public ushort Hr40069;
        public ushort Hr40070;
        public ushort Hr40071;
        public ushort Hr40072;
        public ushort Hr40073;
        public ushort Hr40074;
        public ushort Hr40075;
        public ushort Hr40076;
        public ushort Hr40077;
        public ushort Hr40078;
        public ushort Hr40079;
        public ushort Hr40080;
        public ushort Hr40081;
        public ushort Hr40082;
        public ushort Hr40083;
        public ushort Hr40084;
        public ushort Hr40085;
        public ushort Hr40086;
        public ushort Hr40087;
        public ushort Hr40088;
        public ushort Hr40089;
        public ushort Hr40090;
        public ushort Hr40091;
        public ushort Hr40092;
        public ushort Hr40093;
        public ushort Hr40094;
        public ushort Hr40095;
        public ushort Hr40096;
        public ushort Hr40097;
        public ushort Hr40098;
        public ushort Hr40099;
        public ushort Hr40100;
        public ushort Hr40101;
        public ushort Hr40102;
        public ushort Hr40103;
        public ushort Hr40104;
        public ushort Hr40105;
        public ushort Hr40106;
        public ushort Hr40107;
        public ushort Hr40108;
        public ushort Hr40109;
        public ushort Hr40110;
        public ushort Hr40111;
        public ushort Hr40112;
        public ushort Hr40113;
        public ushort Hr40114;
        public ushort Hr40115;
        public ushort Hr40116;
        public ushort Hr40117;
        public ushort Hr40118;
        public ushort Hr40119;
        public ushort Hr40120;
        #endregion

        #region Input Regs
        public ushort Ir40001;
        public ushort Ir40002;
        public ushort Ir40003;
        public ushort Ir40004;
        public ushort Ir40005;
        public ushort Ir40006;
        public ushort Ir40007;
        public ushort Ir40008;
        public ushort Ir40009;
        public ushort Ir40010;
        public ushort Ir40011;
        public ushort Ir40012;
        public ushort Ir40013;
        public ushort Ir40014;
        public ushort Ir40015;
        public ushort Ir40016;
        public ushort Ir40017;
        public ushort Ir40018;
        public ushort Ir40019;
        public ushort Ir40020;
        public ushort Ir40021;
        public ushort Ir40022;
        public ushort Ir40023;
        public ushort Ir40024;
        public ushort Ir40025;
        public ushort Ir40026;
        public ushort Ir40027;
        public ushort Ir40028;
        public ushort Ir40029;
        public ushort Ir40030;
        public ushort Ir40031;
        public ushort Ir40032;
        public ushort Ir40033;
        public ushort Ir40034;
        public ushort Ir40035;
        public ushort Ir40036;
        public ushort Ir40037;
        public ushort Ir40038;
        public ushort Ir40039;
        public ushort Ir40040;
        public ushort Ir40041;
        public ushort Ir40042;
        public ushort Ir40043;
        public ushort Ir40044;
        public ushort Ir40045;
        public ushort Ir40046;
        public ushort Ir40047;
        public ushort Ir40048;
        public ushort Ir40049;
        public ushort Ir40050;
        public ushort Ir40051;
        public ushort Ir40052;
        public ushort Ir40053;
        public ushort Ir40054;
        public ushort Ir40055;
        public ushort Ir40056;
        public ushort Ir40057;
        public ushort Ir40058;
        public ushort Ir40059;
        public ushort Ir40060;
        public ushort Ir40061;
        public ushort Ir40062;
        public ushort Ir40063;
        public ushort Ir40064;
        public ushort Ir40065;
        public ushort Ir40066;
        public ushort Ir40067;
        public ushort Ir40068;
        public ushort Ir40069;
        public ushort Ir40070;
        public ushort Ir40071;
        public ushort Ir40072;
        public ushort Ir40073;
        public ushort Ir40074;
        public ushort Ir40075;
        public ushort Ir40076;
        public ushort Ir40077;
        public ushort Ir40078;
        public ushort Ir40079;
        public ushort Ir40080;
        public ushort Ir40081;
        public ushort Ir40082;
        public ushort Ir40083;
        public ushort Ir40084;
        public ushort Ir40085;
        public ushort Ir40086;
        public ushort Ir40087;
        public ushort Ir40088;
        public ushort Ir40089;
        public ushort Ir40090;
        public ushort Ir40091;
        public ushort Ir40092;
        public ushort Ir40093;
        public ushort Ir40094;
        public ushort Ir40095;
        public ushort Ir40096;
        public ushort Ir40097;
        public ushort Ir40098;
        public ushort Ir40099;
        public ushort Ir40100;
        public ushort Ir40101;
        public ushort Ir40102;
        public ushort Ir40103;
        public ushort Ir40104;
        public ushort Ir40105;
        public ushort Ir40106;
        public ushort Ir40107;
        public ushort Ir40108;
        public ushort Ir40109;
        public ushort Ir40110;
        public ushort Ir40111;
        public ushort Ir40112;
        public ushort Ir40113;
        public ushort Ir40114;
        public ushort Ir40115;
        public ushort Ir40116;
        public ushort Ir40117;
        public ushort Ir40118;
        public ushort Ir40119;
        public ushort Ir40120;
        #endregion

        private ModbusTcpNet _modbusTcpClient { get; set; }
        private Thread _th { get; set; }

        public WirelessModbusTcpClient(string name)
            : base(name) { }

        public void InitRemoteIpAddress(string ipPort)
        {
            var sp = ipPort.Split(':');
           _modbusTcpClient = new ModbusTcpNet(sp[0], int.Parse(sp[1]));
           var result = _modbusTcpClient.ConnectServer();

            if (_th != null)
            {
                _th.Abort();
                _th.Join();
            }

            _th = new Thread(MainWork) { IsBackground = true };
            _th.Start();
        }
        private bool isRun = true;
        private void MainWork()
        {
            while (_th.IsAlive)
            {
                if (_th.IsAlive == false)
                    break;

                Thread.Sleep(50);
                if (isRun)
                {
                    var result1 = _modbusTcpClient.ReadInt16("1");         
                }
            }
        }

        public void TestWrite1()
        {
          //  var result2 = _modbusTcpClient.Write("1", new short[1]{1});
            var result1 = _modbusTcpClient.Write("3", (ushort)3000);
            var result2 = _modbusTcpClient.Write("1", (ushort)1);

        }
        public void TestWrite2()
        {
            //  var result2 = _modbusTcpClient.Write("1", new short[1]{1});
            var result1 = _modbusTcpClient.Write("3", (ushort)0);
            var result2 = _modbusTcpClient.Write("1", (ushort)0);

        }

    }
}
