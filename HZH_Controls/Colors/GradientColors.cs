// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 2019-09-30
//
// ***********************************************************************
// <copyright file="GradientColors.cs">
//     Copyright by Huang Zhenghui(黄正辉) All, QQ group:568015492 QQ:623128629 Email:623128629@qq.com
// </copyright>
//
// Blog: https://www.cnblogs.com/bfyx
// GitHub：https://github.com/kwwwvagaa/NetWinformControl
// gitee：https://gitee.com/kwwwvagaa/net_winform_custom_control.git
//
// If you use this code, please keep this note.
// ***********************************************************************

using System.Drawing;

namespace HZH_Controls.Colors
{
    /// <summary>
    /// Class GradientColors.
    /// </summary>
    public class GradientColors
    {
        /// <summary>
        /// The orange
        /// </summary>
        private static Color[] _orange = { Color.FromArgb(252, 196, 136), Color.FromArgb(243, 138, 159) };

        /// <summary>
        /// Gets the orange.
        /// </summary>
        /// <value>The orange.</value>
        public static Color[] Orange
        {
            get { return _orange; }
            internal set { _orange = value; }
        }
        /// <summary>
        /// The light green
        /// </summary>
        private static Color[] _lightGreen = { Color.FromArgb(210, 251, 123), Color.FromArgb(152, 231, 160) };

        /// <summary>
        /// Gets the light green.
        /// </summary>
        /// <value>The light green.</value>
        public static Color[] LightGreen
        {
            get { return _lightGreen; }
            internal set { _lightGreen = value; }
        }

        /// <summary>
        /// The green
        /// </summary>
        private static Color[] _green = { Color.FromArgb(138, 241, 124), Color.FromArgb(32, 190, 179) };

        /// <summary>
        /// Gets the green.
        /// </summary>
        /// <value>The green.</value>
        public static Color[] Green
        {
            get { return _green; }
            internal set { _green = value; }
        }

        /// <summary>
        /// The blue
        /// </summary>
        private static Color[] _blue = { Color.FromArgb(193, 232, 251), Color.FromArgb(162, 197, 253) };

        /// <summary>
        /// Gets the blue.
        /// </summary>
        /// <value>The blue.</value>
        public static Color[] Blue
        {
            get { return _blue; }
            internal set { _blue = value; }
        }

        /// <summary>
        /// The blue green
        /// </summary>
        private static Color[] _blueGreen = { Color.FromArgb(122, 251, 218), Color.FromArgb(16, 193, 252) };

        /// <summary>
        /// Gets the blue green.
        /// </summary>
        /// <value>The blue green.</value>
        public static Color[] BlueGreen
        {
            get { return _blueGreen; }
            internal set { _blueGreen = value; }
        }

        /// <summary>
        /// The light violet
        /// </summary>
        private static Color[] _lightViolet = { Color.FromArgb(248, 192, 234), Color.FromArgb(164, 142, 210) };

        /// <summary>
        /// Gets the light violet.
        /// </summary>
        /// <value>The light violet.</value>
        public static Color[] LightViolet
        {
            get { return _lightViolet; }
            internal set { _lightViolet = value; }
        }

        /// <summary>
        /// The violet
        /// </summary>
        private static Color[] _violet = { Color.FromArgb(185, 154, 241), Color.FromArgb(137, 124, 242) };

        /// <summary>
        /// Gets the violet.
        /// </summary>
        /// <value>The violet.</value>
        public static Color[] Violet
        {
            get { return _violet; }
            internal set { _violet = value; }
        }

        /// <summary>
        /// The gray
        /// </summary>
        private static Color[] _gray = { Color.FromArgb(233, 238, 239), Color.FromArgb(147, 162, 175) };

        /// <summary>
        /// Gets the gray.
        /// </summary>
        /// <value>The gray.</value>
        public static Color[] Gray
        {
            get { return _gray; }
            internal set { _gray = value; }
        }
    }
}
