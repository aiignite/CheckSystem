using System.Runtime.InteropServices;

namespace CommonUtility
{
    public static class TsclibDll
    {
        public const string DllFilename = @"\DllImport\TSCLIB.dll";

        [DllImport(DllFilename, EntryPoint = "about")]
        public static extern int about();

        [DllImport(DllFilename, EntryPoint = "openport")]
        public static extern int openport(string printername);

        [DllImport(DllFilename, EntryPoint = "barcode")]
        public static extern int barcode(
            string x, string y, string type,
            string height, string readable, string rotation,
            string narrow, string wide, string code);

        [DllImport(DllFilename, EntryPoint = "clearbuffer")]
        public static extern int clearbuffer();

        [DllImport(DllFilename, EntryPoint = "closeport")]
        public static extern int closeport();

        [DllImport(DllFilename, EntryPoint = "downloadpcx")]
        public static extern int downloadpcx(string filename, string imageName);

        [DllImport(DllFilename, EntryPoint = "formfeed")]
        public static extern int formfeed();

        [DllImport(DllFilename, EntryPoint = "nobackfeed")]
        public static extern int nobackfeed();

        [DllImport(DllFilename, EntryPoint = "printerfont")]
        public static extern int printerfont(
            string x, string y, string fonttype,
            string rotation, string xmul, string ymul,
            string text);

        [DllImport(DllFilename, EntryPoint = "printlabel")]
        public static extern int printlabel(string set, string copy);

        [DllImport(DllFilename, EntryPoint = "sendcommand")]
        public static extern int sendcommand(string printercommand);

        [DllImport(DllFilename, EntryPoint = "setup")]
        public static extern int setup(string width, string height,
                  string speed, string density,
                  string sensor, string vertical,
                  string offset);

        [DllImport(DllFilename, EntryPoint = "windowsfont")]
        public static extern int windowsfont(
            int x, int y, int fontheight,
            int rotation, int fontstyle, int fontunderline,
            string szFaceName, string content);
    }
}
