// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 2019-09-30
//
// ***********************************************************************
// <copyright file="StatusColors.cs">
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
    /// 状态颜色
    /// </summary>
    public class StatusColors
    {
        /// <summary>
        /// The primary
        /// </summary>
        private static Color _primary = ColorTranslator.FromHtml("#409eff");

        /// <summary>
        /// Gets or sets the primary.
        /// </summary>
        /// <value>The primary.</value>
        public static Color Primary
        {
            get { return _primary; }
            internal set { _primary = value; }
        }

        /// <summary>
        /// The success
        /// </summary>
        private static Color _success = ColorTranslator.FromHtml("#67c23a");

        /// <summary>
        /// Gets or sets the success.
        /// </summary>
        /// <value>The success.</value>
        public static Color Success
        {
            get { return _success; }
            internal set { _success = value; }
        }

        /// <summary>
        /// The warning
        /// </summary>
        private static Color _warning = ColorTranslator.FromHtml("#e6a23c");

        /// <summary>
        /// Gets or sets the warning.
        /// </summary>
        /// <value>The warning.</value>
        public static Color Warning
        {
            get { return _warning; }
            internal set { _warning = value; }
        }

        /// <summary>
        /// The danger
        /// </summary>
        private static Color _danger = ColorTranslator.FromHtml("#f56c6c");

        /// <summary>
        /// Gets or sets the danger.
        /// </summary>
        /// <value>The danger.</value>
        public static Color Danger
        {
            get { return _danger; }
            internal set { _danger = value; }
        }

        /// <summary>
        /// The information
        /// </summary>
        private static Color _info = ColorTranslator.FromHtml("#909399");

        /// <summary>
        /// Gets or sets the information.
        /// </summary>
        /// <value>The information.</value>
        public static Color Info
        {
            get { return _info; }
            internal set { _info = value; }
        }
    }
}
