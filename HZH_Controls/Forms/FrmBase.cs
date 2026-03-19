// ***********************************************************************
// Assembly         : HZH_Controls
// Created          : 08-08-2019
//
// ***********************************************************************
// <copyright file="FrmBase.cs">
//     Copyright by Huang Zhenghui(黄正辉) All, QQ group:568015492 QQ:623128629 Email:623128629@qq.com
// </copyright>
//
// Blog: https://www.cnblogs.com/bfyx
// GitHub：https://github.com/kwwwvagaa/NetWinformControl
// gitee：https://gitee.com/kwwwvagaa/net_winform_custom_control.git
//
// If you use this code, please keep this note.
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using HZH_Controls.Helpers;

namespace HZH_Controls.Forms
{
    /// <summary>
    /// Class FrmBase.
    /// Implements the <see cref="System.Windows.Forms.Form" />
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    [Designer("System.Windows.Forms.Design.ParentControlDesigner, System.Design", typeof(System.ComponentModel.Design.IDesigner))]
    public partial class FrmBase : Form
    {
        /// <summary>
        /// Gets or sets the hot keys.
        /// </summary>
        /// <value>The hot keys.</value>
        [Description("定义的热键列表"), Category("自定义")]
        public Dictionary<int, string> HotKeys { get; set; }

        /// <summary>
        /// Delegate HotKeyEventHandler
        /// </summary>
        /// <param name="strHotKey">The string hot key.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public delegate bool HotKeyEventHandler(string strHotKey);

        /// <summary>
        /// 热键事件
        /// </summary>
        [Description("热键事件"), Category("自定义")]
        public event HotKeyEventHandler HotKeyDown;

        #region 字段属性

        /// <summary>
        /// 失去焦点关闭
        /// </summary>
        bool _isLoseFocusClose;

        /// <summary>
        /// 是否重绘边框样式
        /// </summary>
        private bool _redraw;

        /// <summary>
        /// 是否显示圆角
        /// </summary>
        private bool _isShowRegion;

        /// <summary>
        /// 边圆角大小
        /// </summary>
        private int _regionRadius = 10;

        /// <summary>
        /// 边框颜色
        /// </summary>
        private Color _borderStyleColor;

        /// <summary>
        /// 边框宽度
        /// </summary>
        private int _borderStyleSize;

        /// <summary>
        /// 边框样式
        /// </summary>
        private ButtonBorderStyle _borderStyleType;

        /// <summary>
        /// 是否显示模态
        /// </summary>
        private bool _isShowMaskDialog;

        /// <summary>
        /// 蒙版窗体
        /// </summary>
        /// <value><c>true</c> if this instance is show mask dialog; otherwise, <c>false</c>.</value>
        [Description("是否显示蒙版窗体")]
        public bool IsShowMaskDialog
        {
            get
            {
                return _isShowMaskDialog;
            }
            set
            {
                _isShowMaskDialog = value;
            }
        }

        /// <summary>
        /// 边框宽度
        /// </summary>
        /// <value>The size of the border style.</value>
        [Description("边框宽度")]
        public int BorderStyleSize
        {
            get
            {
                return _borderStyleSize;
            }
            set
            {
                _borderStyleSize = value;
            }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        /// <value>The color of the border style.</value>
        [Description("边框颜色")]
        public Color BorderStyleColor
        {
            get
            {
                return _borderStyleColor;
            }
            set
            {
                _borderStyleColor = value;
            }
        }

        /// <summary>
        /// 边框样式
        /// </summary>
        /// <value>The type of the border style.</value>
        [Description("边框样式")]
        public ButtonBorderStyle BorderStyleType
        {
            get
            {
                return _borderStyleType;
            }
            set
            {
                _borderStyleType = value;
            }
        }

        /// <summary>
        /// 边框圆角
        /// </summary>
        /// <value>The region radius.</value>
        [Description("边框圆角")]
        public int RegionRadius
        {
            get
            {
                return _regionRadius;
            }
            set
            {
                _regionRadius = Math.Max(value, 1);
            }
        }

        /// <summary>
        /// 是否显示自定义绘制内容
        /// </summary>
        /// <value><c>true</c> if this instance is show region; otherwise, <c>false</c>.</value>
        [Description("是否显示自定义绘制内容")]
        public bool IsShowRegion
        {
            get
            {
                return _isShowRegion;
            }
            set
            {
                _isShowRegion = value;
            }
        }

        /// <summary>
        /// 是否显示重绘边框
        /// </summary>
        /// <value><c>true</c> if redraw; otherwise, <c>false</c>.</value>
        [Description("是否显示重绘边框")]
        public bool Redraw
        {
            get
            {
                return _redraw;
            }
            set
            {
                _redraw = value;
            }
        }

        /// <summary>
        /// The is full size
        /// </summary>
        private bool _isFullSize = true;

        /// <summary>
        /// 是否全屏
        /// </summary>
        /// <value><c>true</c> if this instance is full size; otherwise, <c>false</c>.</value>
        [Description("是否全屏")]
        public bool IsFullSize
        {
            get { return _isFullSize; }
            set { _isFullSize = value; }
        }

        /// <summary>
        /// 失去焦点自动关闭
        /// </summary>
        /// <value><c>true</c> if this instance is lose focus close; otherwise, <c>false</c>.</value>
        [Description("失去焦点自动关闭")]
        public bool IsLoseFocusClose
        {
            get
            {
                return _isLoseFocusClose;
            }
            set
            {
                _isLoseFocusClose = value;
            }
        }
        #endregion

        /// <summary>
        /// Gets a value indicating whether this instance is desing mode.
        /// </summary>
        /// <value><c>true</c> if this instance is desing mode; otherwise, <c>false</c>.</value>
        private static bool IsDesingMode
        {
            get
            {
                var returnFlag = false;
                if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
                    returnFlag = true;
                else if (System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv")
                    returnFlag = true;
                return returnFlag;
            }
        }

        #region 初始化
        /// <summary>
        /// Initializes a new instance of the <see cref="FrmBase" /> class.
        /// </summary>
        public FrmBase()
        {
            InitializeComponent();
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            //base.HandleCreated += new EventHandler(this.FrmBase_HandleCreated);
            //base.HandleDestroyed += new EventHandler(this.FrmBase_HandleDestroyed);        
            KeyDown += FrmBase_KeyDown;
            FormClosing += FrmBase_FormClosing;
        }

        /// <summary>
        /// Handles the FormClosing event of the FrmBase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="FormClosingEventArgs" /> instance containing the event data.</param>
        private void FrmBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isLoseFocusClose)
                MouseHook.OnMouseActivity -= hook_OnMouseActivity;
        }

        /// <summary>
        /// Handles the Load event of the FrmBase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void FrmBase_Load(object sender, EventArgs e)
        {
            if (!IsDesingMode)
                if (_isFullSize)
                    SetFullSize();

            if (_isLoseFocusClose)
                MouseHook.OnMouseActivity += hook_OnMouseActivity;
        }

        #endregion

        #region 方法区

        /// <summary>
        /// Handles the OnMouseActivity event of the hook control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void hook_OnMouseActivity(object sender, MouseEventArgs e)
        {
            try
            {
                if (!_isLoseFocusClose || e.Clicks <= 0)
                    return;

                if (e.Button != MouseButtons.Left && e.Button != MouseButtons.Right)
                    return;

                if (IsDisposed)
                    return;

                if (!ClientRectangle.Contains(PointToClient(e.Location)))
                    Close();
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// 全屏
        /// </summary>
        public void SetFullSize()
        {
            FormBorderStyle = FormBorderStyle.None;
            WindowState = FormWindowState.Maximized;
        }

        /// <summary>
        /// Does the escape.
        /// </summary>
        protected virtual void DoEsc()
        {
            Close();
        }

        /// <summary>
        /// Does the enter.
        /// </summary>
        protected virtual void DoEnter()
        {
        }

        /// <summary>
        /// 设置重绘区域
        /// </summary>
        public void SetWindowRegion()
        {
            var rect = new Rectangle(-1, -1, Width + 1, Height);
            var path = GetRoundedRectPath(rect, _regionRadius);
            Region = new Region(path);
        }

        /// <summary>
        /// 获取重绘区域
        /// </summary>
        /// <param name="rect">The rect.</param>
        /// <param name="radius">The radius.</param>
        /// <returns>GraphicsPath.</returns>
        private static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            var rect2 = new Rectangle(rect.Location, new Size(radius, radius));
            var graphicsPath = new GraphicsPath();
            graphicsPath.AddArc(rect2, 180f, 90f);
            rect2.X = rect.Right - radius;
            graphicsPath.AddArc(rect2, 270f, 90f);
            rect2.Y = rect.Bottom - radius;
            rect2.Width += 1;
            rect2.Height += 1;
            graphicsPath.AddArc(rect2, 360f, 90f);
            rect2.X = rect.Left;
            graphicsPath.AddArc(rect2, 90f, 90f);
            graphicsPath.CloseFigure();
            return graphicsPath;
        }

        /// <summary>
        /// 将窗体显示为具有指定所有者的模式对话框。
        /// </summary>
        /// <param name="owner">任何实现 <see cref="T:System.Windows.Forms.IWin32Window" />（表示将拥有模式对话框的顶级窗口）的对象。</param>
        /// <returns><see cref="T:System.Windows.Forms.DialogResult" /> 值之一。</returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new DialogResult ShowDialog(IWin32Window owner)
        {
            try
            {
                if (!_isShowMaskDialog || owner == null)
                    return base.ShowDialog(owner);

                var frmOwner = (Control)owner;
                var frmTransparent = new FrmTransparent
                {
                    Width = frmOwner.Width,
                    Height = frmOwner.Height
                };
                var location = frmOwner.PointToScreen(new Point(0, 0));
                frmTransparent.Location = location;
                frmTransparent.frmchild = this;
                frmTransparent.IsShowMaskDialog = false;
                return frmTransparent.ShowDialog(owner);
            }
            catch (NullReferenceException)
            {
                return DialogResult.None;
            }
        }

        /// <summary>
        /// 将窗体显示为模式对话框，并将当前活动窗口设置为它的所有者。
        /// </summary>
        /// <returns><see cref="T:System.Windows.Forms.DialogResult" /> 值之一。</returns>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.EnvironmentPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="UnmanagedCode, ControlEvidence" />
        ///   <IPermission class="System.Security.Permissions.UIPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        ///   <IPermission class="System.Diagnostics.PerformanceCounterPermission, System, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true" />
        /// </PermissionSet>
        public new DialogResult ShowDialog()
        {
            return base.ShowDialog();
        }
        #endregion

        #region 事件区


        /// <summary>
        /// 关闭时发生
        /// </summary>
        /// <param name="e">一个包含事件数据的 <see cref="T:System.EventArgs" />。</param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            var owner = Owner as FrmTransparent;
            if (owner != null)
                owner.Close();
        }

        /// <summary>
        /// 快捷键
        /// </summary>
        /// <param name="msg">通过引用传递的 <see cref="T:System.Windows.Forms.Message" />，它表示要处理的 Win32 消息。</param>
        /// <param name="keyData"><see cref="T:System.Windows.Forms.Keys" /> 值之一，它表示要处理的键。</param>
        /// <returns>如果控件处理并使用击键，则为 true；否则为 false，以允许进一步处理。</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            const int num = 256;
            const int num2 = 260;
            bool result;
            if (msg.Msg == num | msg.Msg == num2)
            {
                if (keyData == (Keys)262259)
                {
                    result = true;
                    return result;
                }
                if (keyData != Keys.Enter)
                {
                    if (keyData == Keys.Escape)
                    {
                        DoEsc();
                    }
                }
                else
                {
                    DoEnter();
                }
            }
            result = false;
            if (result)
                return result;
            else
                return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Handles the KeyDown event of the FrmBase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs" /> instance containing the event data.</param>
        protected void FrmBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (HotKeyDown == null || HotKeys == null)
                return;

            var blnCtrl = false;
            var blnAlt = false;
            var blnShift = false;
            if (e.Control)
                blnCtrl = true;
            if (e.Alt)
                blnAlt = true;
            if (e.Shift)
                blnShift = true;
            if (!HotKeys.ContainsKey(e.KeyValue))
                return;
            var strKey = string.Empty;
            if (blnCtrl)
                strKey += "Ctrl+";
            if (blnAlt)
                strKey += "Alt+";
            if (blnShift)
                strKey += "Shift+";
            strKey += HotKeys[e.KeyValue];

            if (!HotKeyDown(strKey))
                return;
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        /// <summary>
        /// 重绘事件
        /// </summary>
        /// <param name="e">包含事件数据的 <see cref="T:System.Windows.Forms.PaintEventArgs" />。</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_isShowRegion)
                SetWindowRegion();
            base.OnPaint(e);
            if (_redraw)
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, _borderStyleColor, _borderStyleSize,
                       _borderStyleType, _borderStyleColor, _borderStyleSize, _borderStyleType, _borderStyleColor,
                       _borderStyleSize, _borderStyleType, _borderStyleColor, _borderStyleSize, _borderStyleType);
        }
        #endregion


        #region 窗体拖动    English:Form drag
        /// <summary>
        /// Releases the capture.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        /// <param name="wMsg">The w MSG.</param>
        /// <param name="wParam">The w parameter.</param>
        /// <param name="lParam">The l parameter.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        /// <summary>
        /// The wm syscommand
        /// </summary>
        public const int WmSyscommand = 0x0112;
        /// <summary>
        /// The sc move
        /// </summary>
        public const int ScMove = 0xF010;
        /// <summary>
        /// The htcaption
        /// </summary>
        public const int Htcaption = 0x0002;

        /// <summary>
        /// 通过Windows的API控制窗体的拖动
        /// </summary>
        /// <param name="hwnd">The HWND.</param>
        public new static void MouseDown(IntPtr hwnd)
        {
            ReleaseCapture();
            SendMessage(hwnd, WmSyscommand, ScMove + Htcaption, 0);
        }
        #endregion

        /// <summary>
        /// 在构造函数中调用设置窗体移动
        /// </summary>
        /// <param name="cs">The cs.</param>
        protected void InitFormMove(params Control[] cs)
        {
            foreach (var c in cs.Where(c => c != null && !c.IsDisposed))
                c.MouseDown += c_MouseDown;
        }

        /// <summary>
        /// Handles the MouseDown event of the c control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseEventArgs" /> instance containing the event data.</param>
        private void c_MouseDown(object sender, MouseEventArgs e)
        {
            MouseDown(Handle);
        }
    }
}
