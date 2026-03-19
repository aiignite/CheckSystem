using OpenCvSharp;
using System;
using System.Windows.Forms;

namespace UserControls.CvDisplaySrc
{
    public class MouseActionEventArgs : EventArgs
    {
        public Point Pos { get; protected set; }

        public MouseButtons MouseButton { get; protected set; }

        public MouseActionEventArgs(Point pos, MouseButtons mouseButton)
        {
            Pos = pos;
            MouseButton = mouseButton;
        }
    }
}
