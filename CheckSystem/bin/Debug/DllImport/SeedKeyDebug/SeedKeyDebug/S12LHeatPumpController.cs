using System.Runtime.InteropServices;

namespace SeedKeyDebug
{
    public class S12LHeatPumpController
    {
        public int SecurityAccess(int seed, byte subFunc)
        {
            //var seedBytes = new byte[] { 0x63, 0xaf, 0x69, 0xe0 };

            //if (seedBytes.Length != 4)
            //    return -1;
            var key = S12LCalcKey.encipher(seed, subFunc);
            return key;
        }

        internal static class S12LCalcKey
        {
            [DllImport(@"\DllImport\seedkey_UDS.dll", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
            internal static extern int encipher(int wSeed, byte securityLevel);
        }
    }
}
