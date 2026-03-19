// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 2019-09-30
//
// ***********************************************************************
// <copyright file="BasisColors.cs">
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
    /// Class BasisColors.
    /// </summary>
    public class BasisColors
    {
        /// <summary>
        /// The light
        /// </summary>
        private static Color _light = ColorTranslator.FromHtml("#f5f7fa");

        /// <summary>
        /// Gets the light.
        /// </summary>
        /// <value>The light.</value>
        public static Color Light
        {
            get { return _light; }
            internal set { _light = value; }
        }

        /// <summary>
        /// The medium
        /// </summary>
        private static Color _medium = ColorTranslator.FromHtml("#f0f2f5");

        /// <summary>
        /// Gets the medium.
        /// </summary>
        /// <value>The medium.</value>
        public static Color Medium
        {
            get { return _medium; }
            internal set { _medium = value; }
        }

        /// <summary>
        /// The dark
        /// </summary>
        private static Color _dark = ColorTranslator.FromHtml("#000000");

        /// <summary>
        /// Gets the dark.
        /// </summary>
        /// <value>The dark.</value>
        public static Color Dark
        {
            get { return _dark; }
            internal set { _dark = value; }
        }
    }
}
