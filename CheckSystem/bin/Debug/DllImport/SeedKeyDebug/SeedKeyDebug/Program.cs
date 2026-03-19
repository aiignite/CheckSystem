using System;

namespace SeedKeyDebug
{
    internal class Program
    {
        private static void Main()
        {
            //var x = Console.Read();
            //Console.WriteLine(x);
            //Console.Read();
            var arg = Console.ReadLine();

            if (arg != null)
            {
                try
                {
                    //Console.WriteLine(arg);

                    //arg = "5411854,5";

                    var str = arg.Split(',');
                    var seed = int.Parse(str[0]);
                    var func = byte.Parse(str[1]);

                    var s12L = new S12LHeatPumpController();

                    var key = s12L.SecurityAccess(seed, func);

                    Console.WriteLine("Key:" + key);
                }
                catch (Exception)
                {
                    // ignored
                }
            }
            Console.ReadLine();
        }
    }
}
