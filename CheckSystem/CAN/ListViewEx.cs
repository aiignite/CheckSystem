using System.Windows.Forms;

namespace CheckSystem.CAN
{
    public class ListViewEx : ListView
    {
        public ListViewEx()
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.EnableNotifyMessage , true);
            UpdateStyles();
        }
    }
}
