// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 2019-09-30
//
// ***********************************************************************
// <copyright file="LineColors.cs">
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
    /// Class LineColors.
    /// </summary>
    public class LineColors
    {
        /// <summary>
        /// The more light
        /// </summary>
        private static Color _moreLight = ColorTranslator.FromHtml("#f2f6fc");

        /// <summary>
        /// Gets the more light.
        /// </summary>
        /// <value>The more light.</value>
        public static Color MoreLight
        {
            get { return _moreLight; }
            internal set { _moreLight = value; }
        }

        /// <summary>
        /// The light
        /// </summary>
        private static Color _light = ColorTranslator.FromHtml("#ebeef5");

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
        /// The dark
        /// </summary>
        private static Color _dark = ColorTranslator.FromHtml("#e4e7ed");

        /// <summary>
        /// Gets the dark.
        /// </summary>
        /// <value>The dark.</value>
        public static Color Dark
        {
            get { return _dark; }
            internal set { _dark = value; }
        }

        /// <summary>
        /// The more dark
        /// </summary>
        private static Color _moreDark = ColorTranslator.FromHtml("#dcdfe6");

        /// <summary>
        /// Gets the more dark.
        /// </summary>
        /// <value>The more dark.</value>
        public static Color MoreDark
        {
            get { return _moreDark; }
            internal set { _moreDark = value; }
        }
    }
}
