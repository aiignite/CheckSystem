using System.Runtime.InteropServices;

namespace CommonUtility
{
    public static class CommonMethods
    {
        [DllImport("user32.dll")]
        public static extern int MessageBeep(uint uType);
    }
}
