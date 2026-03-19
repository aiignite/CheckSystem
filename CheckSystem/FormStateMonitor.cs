using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controller;
using Go;
using HZH_Controls.IconFont;
using Newtonsoft.Json.Linq;
using StateMachine;

namespace CheckSystem
{
    public partial class FormStateMonitor : Form
    {
        private generator _timeAction;

        private State ThisStateMachine { get; set; }
        private DeviceConfigWorkStation SelectedWorkstation { get; set; }
        private DeviceConfigStatusUnit SelectedStatusUnit { get; set; }
        private DeviceConfigCondition SelectedCondition { get; set; }
        private readonly List<DeviceConfigCondition> _conditions = new List<DeviceConfigCondition>();
        private readonly List<DeviceConfigStatusUnit> _statusUnits = new List<DeviceConfigStatusUnit>();

        //private Thread RefreshTh { get; set; }

        #region 画图参数
        private double _imageScale = 0.5;
        private const int A4StandardWidth = 1786;
        private const int A4StandardHeight = 1260;
        private static readonly AdjustableArrowCap LineArrow = new AdjustableArrowCap(6, 12, true);

        private readonly Font _fontName = new Font("微软雅黑", 14, FontStyle.Regular);
        private readonly Font _fontNote = new Font("微软雅黑", 10, FontStyle.Regular);
        private readonly Font _currentShowConditionFontNote = new Font("微软雅黑", 14, FontStyle.Bold);
        private static readonly Color BackgroundColor = SystemColors.Info;
        private readonly SolidBrush _brushBrown = new SolidBrush(Color.Brown);
        private readonly Pen _penDefault = new Pen(Color.Black, 1) { CustomEndCap = LineArrow };
        private readonly Pen _penSelectSu = new Pen(Color.Red, 4) { CustomEndCap = LineArrow };
        private readonly Pen _penCurrentSu = new Pen(Color.Green, 15) { CustomEndCap = LineArrow };
        private readonly Pen _penMultipleSelect = new Pen(Color.LightSlateGray, 6.5f) { CustomEndCap = LineArrow };
        private readonly Pen _penSelectCondition = new Pen(Color.Red, 2);
        private readonly Pen _penMidPen = new Pen(Color.Red, 5);
        private readonly SolidBrush _brushLightBlue = new SolidBrush(Color.LightBlue);
        private readonly SolidBrush _brushDarkGreen = new SolidBrush(Color.DarkGreen);
        private readonly SolidBrush _brushDrakRed = new SolidBrush(Color.DarkRed);
        private readonly SolidBrush _brushGreen = new SolidBrush(Color.Green);
        private readonly SolidBrush _brushRed = new SolidBrush(Color.Red);
        private readonly SolidBrush _brushBlue = new SolidBrush(Color.Blue);
        private readonly SolidBrush _brushDarkGoldenrod = new SolidBrush(Color.DarkGoldenrod);
        private readonly SolidBrush _brushAntiqueWhite = new SolidBrush(Color.AntiqueWhite);
        private readonly SolidBrush _brushGray = new SolidBrush(Color.Gray);

        private EnumMousePointPosition _mousePointPosition { get; set; }

        /// <summary>
        /// 用于存放img
        /// </summary>
        private PictureBox PanelMain { get; set; }

        /// <summary>
        /// 用于显示图像
        /// </summary>
        private Bitmap Img { get; set; }

        /// <summary>
        /// 当前光标坐标
        /// </summary>
        private Point CurrentCursor { get; set; }

        /// <summary>
        /// 上一次光标坐标
        /// </summary>
        private Point LastCursor { get; set; }

        /// <summary>
        /// 是否在移动img
        /// </summary>
        private bool IsImgMoving { get; set; }

        /// <summary>
        /// img的偏移量
        /// 当前光标坐标.X-上一次光标坐标.X
        /// </summary>
        private int ImgMoveOffSetX { get; set; }

        /// <summary>
        /// img的偏移量
        /// 当前光标坐标.Y-上一次光标坐标.Y
        /// </summary>
        private int ImgMoveOffSetY { get; set; }
        #endregion

        public FormStateMonitor(State st)
        {
            InitializeComponent();
            Icon = FontImages.GetIcon(FontIcons.E_social_googledrive, 32,
                Color.DodgerBlue);
            WindowState = FormWindowState.Maximized;
            ThisStateMachine = st;
            InitTreeView();

            //CheckForIllegalCrossThreadCalls = false;

            #region 初始化画图参数

            SetStyle(
               ControlStyles.DoubleBuffer |
               ControlStyles.UserPaint |
               ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(
                ControlStyles.ResizeRedraw |
                ControlStyles.Selectable, true);
            //UpdateStyles();

            panel1.HorizontalScroll.Enabled = false;
            panel1.VerticalScroll.Enabled = false;
            panel1.AutoScroll = false;

            PanelMain = new PictureBox
            {
                Parent = panel1,
                Location = new Point(),
                Dock = DockStyle.None,
                Width = A4StandardWidth,
                Height = A4StandardHeight
            };

            //获取控件的Type,设置双缓存
            var dgvType = PanelMain.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            //为控件的属性设置值
            if (properInfo != null) properInfo.SetValue(PanelMain, true, null);

            Img = new Bitmap(A4StandardWidth, A4StandardHeight);
            var g = Graphics.FromImage(Img);
            g.Clear(BackgroundColor);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            PanelMain.Image = Img;

            #endregion

            Load += FormStateMonitor_Load;
            Closed += FormStateMonitor_Closed;
        }

        private void FormStateMonitor_Load(object sender, EventArgs e)
        {
            PanelMain.MouseWheel += PanelMain_MouseWheel;
            PanelMain.MouseDown += PanelMain_MouseDown;
            PanelMain.MouseUp += PanelMain_MouseUp;
            PanelMain.MouseMove += PanelMain_MouseMove;

            _timeAction = generator.tgo(FormSelection.MainStrand, RefreshPaint);

            //if (RefreshTh != null)
            //{
            //    RefreshTh.Abort();
            //    RefreshTh.Join();
            //}

            //RefreshTh = new Thread(RefreshPaint) { IsBackground = true };
            //RefreshTh.Start();
        }

        private void FormStateMonitor_Closed(object sender, EventArgs e)
        {
            //if (RefreshTh == null)
            //    return;

            //RefreshTh.Abort();
            //RefreshTh.Join();

            _timeAction.stop();
        }

        /// <summary>
        /// 刷新Image
        /// </summary>
        private async Task RefreshPaint()
        {
            while (true)
            {
                await generator.sleep(10);

                //Thread.Sleep(10);
                //if (!RefreshTh.IsAlive)
                //    break;

                if (SelectedWorkstation == null)
                    continue;

                lock (ThisStateMachine)
                {
                    try
                    {
                        var g = Graphics.FromImage(Img);
                        g.Clear(BackgroundColor);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.ScaleTransform((float)_imageScale, (float)_imageScale);
                        g.TranslateTransform(ImgMoveOffSetX, ImgMoveOffSetY);

                        var workstation = ThisStateMachine.LstWorkstations.Find(f => f.Name == SelectedWorkstation.Name);
                        var su = workstation.LstClassStatusUnits.Find(
                            t => string.Equals(t.StatusEnName, workstation.CurrentStatusUint, StringComparison.CurrentCultureIgnoreCase));

                        var currentStateUnit =
                            _statusUnits.Find(
                                f =>
                                    f.WorkStationName == SelectedWorkstation.Name &&
                                    string.Equals(f.Name, su.StatusEnName, StringComparison.CurrentCultureIgnoreCase));

                        // draw
                        #region 画条件线
                        foreach (var condition in _conditions)
                        {
                            var suSource = _statusUnits.Find(f => f.Name == condition.SourceSuName);
                            var suTarget = _statusUnits.Find(f => f.Name == condition.TargetSuName);
                            if (suTarget == null || suSource == null)
                                continue;
                            var suSourceRect = GetRectsOfStatusUnit(suSource).First();
                            var suTargetRect = GetRectsOfStatusUnit(suTarget).First();
                            var suTargetRectCenter = GetRectCenter(suTargetRect);
                            var pStart = GetRectCenter(suSourceRect);
                            var pEnd = GetCrossPoint(
                                suTargetRect, pStart, suTargetRectCenter);
                            var pMiddle = GetMiddlePoint(condition);

                            if (suSource == currentStateUnit)
                                g.DrawCurve(_penMultipleSelect, new PointF[] { pStart, pMiddle, pEnd });
                            else
                                g.DrawCurve(condition == SelectedCondition ? _penSelectCondition : _penDefault,
                                        new PointF[] { pStart, pMiddle, pEnd });

                            // 画曲线中间点
                            var rectMidPoint = new Rectangle(pMiddle.X - 2, pMiddle.Y - 2, 4, 4);
                            g.DrawRectangle(_penDefault, rectMidPoint);
                            g.FillRectangle(_brushBlue, rectMidPoint);

                            var strDisplay = string.Empty;
                            if (!string.IsNullOrEmpty(condition.ConditionFunction) && condition.ConditionFunction.Length > 0)
                                strDisplay = "退出条件：" + Environment.NewLine + condition.ConditionFunction;
                            if (!string.IsNullOrEmpty(condition.ExitFunction) && condition.ExitFunction.Length > 0)
                                strDisplay += "退出函数：" + Environment.NewLine + condition.ExitFunction;

                            if (suSource == currentStateUnit)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(condition.ConditionFunction) && condition.ConditionFunction.Length > 0)
                                    {
                                        if (!condition.ConditionFunction.Contains(".Para."))
                                        {
                                            g.DrawString("退出条件：", _currentShowConditionFontNote, _brushBrown,
                                                    new Rectangle(pMiddle.X, pMiddle.Y, 1000, 400));

                                            var conditionFuncs = condition.ConditionFunction.Split(new[] { "\r", "\n" },
                                                StringSplitOptions.RemoveEmptyEntries);

                                            for (var i = 0; i < conditionFuncs.Length; i++)
                                            {
                                                strDisplay = string.Empty;
                                                var c = conditionFuncs[i];
                                                var isPass = false;

                                                if (c.EndsWith("&&") || c.EndsWith("||"))
                                                {
                                                    var orAndValue = c.Substring(c.Length - 2, 2);
                                                    c = c.Substring(0, c.Length - 2);

                                                    var cLeft = c.GetStrFromLeftSign();
                                                    var cRight = c.GetStrFromRightSign();

                                                    var cLeftValue = string.Empty;
                                                    var cRightValue = string.Empty;

                                                    var conntectValue = c.Substring(cLeft.Length);
                                                    conntectValue = conntectValue.Substring(0, conntectValue.Length - cRight.Length);

                                                    if (cLeft.Contains(".Field."))
                                                    {
                                                        var temp = GetFieldValue(cLeft);
                                                        if (string.IsNullOrEmpty(temp))
                                                            strDisplay += cLeft + conntectValue;
                                                        else
                                                        {
                                                            cLeftValue = temp;
                                                            strDisplay += string.Format("{0}【当前值： {1}】", cLeft, temp);
                                                        }
                                                    }
                                                    else if (cLeft.Contains(".Part."))
                                                    {
                                                        var temp = GetPartValue(cLeft);
                                                        if (string.IsNullOrEmpty(temp))
                                                            strDisplay += cLeft + conntectValue;
                                                        else
                                                        {
                                                            cLeftValue = temp;
                                                            strDisplay += string.Format("{0}【当前值： {1}】", cLeft, temp);
                                                        }
                                                    }

                                                    strDisplay += conntectValue;

                                                    if (cRight.Contains(".Field."))
                                                    {
                                                        var temp = GetFieldValue(cRight);
                                                        if (string.IsNullOrEmpty(temp))
                                                            strDisplay += cRight + conntectValue;
                                                        else
                                                        {
                                                            cRightValue = temp;
                                                            strDisplay += string.Format("{0}【当前值： {1}】", cRight, temp);
                                                        }
                                                    }
                                                    else if (cRight.Contains(".Part."))
                                                    {
                                                        var temp = GetPartValue(cRight);
                                                        if (string.IsNullOrEmpty(temp))
                                                            strDisplay += cRight + conntectValue;
                                                        else
                                                        {
                                                            cRightValue = temp;
                                                            strDisplay += string.Format("{0}【当前值： {1}】", cRight, temp);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        cRightValue = cRight;
                                                        strDisplay += cRight;
                                                    }

                                                    strDisplay += orAndValue;

                                                    try
                                                    {
                                                        if (conntectValue == "==")
                                                            isPass = string.Equals(cLeftValue, cRightValue);
                                                        else if (conntectValue == "!=")
                                                            isPass = !string.Equals(cLeftValue, cRightValue);
                                                        else if (conntectValue == ">=")
                                                            isPass = double.Parse(cLeftValue) >=
                                                                         double.Parse(cRightValue);
                                                        else if (conntectValue == "<=")
                                                            isPass = double.Parse(cLeftValue) <=
                                                                         double.Parse(cRightValue);
                                                        else if (conntectValue == ">")
                                                            isPass = double.Parse(cLeftValue) >
                                                                         double.Parse(cRightValue);
                                                        else if (conntectValue == "<")
                                                            isPass = double.Parse(cLeftValue) <
                                                                        double.Parse(cRightValue);
                                                    }
                                                    catch (Exception)
                                                    {
                                                        isPass = false;
                                                    }
                                                }
                                                else
                                                {
                                                    strDisplay += c;
                                                }

                                                g.DrawString(strDisplay, _currentShowConditionFontNote,
                                                    isPass ? _brushGreen : _brushRed,
                                                    new Rectangle(pMiddle.X,
                                                        pMiddle.Y + _currentShowConditionFontNote.Height * (i + 1), 1000,
                                                        400));
                                            }

                                            if (!string.IsNullOrEmpty(condition.ExitFunction) &&
                                                condition.ExitFunction.Length > 0)
                                            {
                                                strDisplay = "退出函数：" + Environment.NewLine + condition.ExitFunction;
                                                g.DrawString(strDisplay, _currentShowConditionFontNote, _brushBrown,
                                                    new Rectangle(pMiddle.X,
                                                        pMiddle.Y +
                                                        _currentShowConditionFontNote.Height * (conditionFuncs.Length + 1),
                                                        1000, 400));
                                            }

                                        }
                                        else
                                        {
                                            g.DrawString(strDisplay, _fontNote, _brushBrown,
                                                new Rectangle(pMiddle.X, pMiddle.Y, 400, 300));
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                    // ignored
                                }
                            }
                            else
                            {
                                g.DrawString(strDisplay, _fontNote, _brushBrown,
                                    new Rectangle(pMiddle.X, pMiddle.Y, 400, 300));
                            }
                        }

                        #endregion

                        #region 画状态框
                        foreach (var stateUnit in _statusUnits)
                        {
                            var lstRects = GetRectsOfStatusUnit(stateUnit);

                            foreach (var rect in lstRects)
                            {
                                if (currentStateUnit != null && stateUnit == currentStateUnit)
                                    g.DrawRectangle(_penCurrentSu, rect);
                                else
                                    g.DrawRectangle(
                                            stateUnit == SelectedStatusUnit ? _penSelectSu : _penDefault, rect);

                                if (string.IsNullOrEmpty(stateUnit.Name))
                                    g.FillRectangle(_brushLightBlue, rect);
                                else if (stateUnit.Name.ToUpper().Equals(StateMachineHelper.EDefaultStateUnits.CheckFail.ToString().ToUpper()))
                                    g.FillRectangle(_brushDrakRed, rect);
                                else if (stateUnit.Name.ToUpper().Equals(StateMachineHelper.EDefaultStateUnits.PreStart.ToString().ToUpper()))
                                    g.FillRectangle(_brushDarkGoldenrod, rect);
                                else if (stateUnit.Name.ToUpper().Equals(StateMachineHelper.EDefaultStateUnits.CheckEnd.ToString().ToUpper()))
                                    g.FillRectangle(_brushGray, rect);
                                else
                                    g.FillRectangle(_brushLightBlue, rect);
                                string suText;
                                if (lstRects.IndexOf(rect) != 0)
                                    suText = stateUnit.Name + "@" + lstRects.IndexOf(rect);
                                else
                                    suText = stateUnit.Name;

                                g.DrawString(suText, _fontName, _brushBlue, rect.X + 20, rect.Y + 20);

                                var strDisplay = string.Empty;
                                if (!string.IsNullOrEmpty(stateUnit.EnterFunction) && stateUnit.EnterFunction.Length > 0)
                                    strDisplay = "进入函数:" + Environment.NewLine + stateUnit.EnterFunction;
                                if (!string.IsNullOrEmpty(stateUnit.DuringFunction) && stateUnit.DuringFunction.Length > 0)
                                    strDisplay += "执行函数:" + Environment.NewLine + stateUnit.DuringFunction;
                                g.DrawString(strDisplay, _fontNote, _brushBlue, rect.Right, rect.Bottom);
                            }
                        }
                        #endregion

                        g = PanelMain.CreateGraphics();
                        g.DrawImage(Img, 0, 0);
                        g.Dispose();
                    }
                    catch (Exception)
                    {
                        // ignored
                    }
                }
            }
        }

        private void InitTreeView()
        {
            treeView.Nodes.Clear();

            var treeRootNode = new TreeNode("工作站列表", 1, 1);
            treeView.Nodes.Add(treeRootNode);

            foreach (var w in ThisStateMachine.DeviceConfig.WorkStations)
                treeRootNode.Nodes.Add(new TreeNode { Text = w.Name, ImageIndex = 2, Tag = w.Name });
            treeView.Nodes[0].BackColor = Color.DarkOrange;

            treeView.ExpandAll();
            treeView.NodeMouseClick += treeView_NodeMouseClick;

            if (treeView.Nodes[0].Nodes.Count <= 0)
                return;

            lock (ThisStateMachine)
            {
                var thisDeviceConfig = ThisStateMachine.DeviceConfig;

                var ws =
                    thisDeviceConfig.WorkStations.ToList()
                        .Find(f => f.Name == treeView.Nodes[0].Nodes[0].Text);

                if (ws == null) return;
                SelectedWorkstation = ws;

                _statusUnits.Clear();
                _conditions.Clear();

                foreach (var s in thisDeviceConfig.StatusUnits.ToList()
                    .FindAll(t => t.WorkStationName.Equals(SelectedWorkstation.Name)))
                    _statusUnits.Add(s);

                foreach (var c in thisDeviceConfig.Conditions.ToList()
                    .FindAll(t => t.WorkStationName.Equals(SelectedWorkstation.Name)))
                    _conditions.Add(c);
            }
        }

        private void treeView_NodeMouseClick(
            object sender, TreeNodeMouseClickEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    break;

                case MouseButtons.Left:
                    if (e.Node.Nodes.Count == 0)//说明当前选中节点没有子节点
                    {
                        lock (ThisStateMachine)
                        {
                            var thisDeviceConfig = ThisStateMachine.DeviceConfig;

                            var ws = thisDeviceConfig.WorkStations.ToList().Find(f => f.Name == e.Node.Text);
                            if (ws != null)
                            {
                                SelectedWorkstation = ws;

                                _statusUnits.Clear();
                                _conditions.Clear();

                                foreach (var s in thisDeviceConfig.StatusUnits.ToList()
                                    .FindAll(t => t.WorkStationName.Equals(SelectedWorkstation.Name)))
                                    _statusUnits.Add(s);

                                foreach (var c in thisDeviceConfig.Conditions.ToList()
                                    .FindAll(t => t.WorkStationName.Equals(SelectedWorkstation.Name)))
                                    _conditions.Add(c);
                            }
                        }
                    }
                    break;

                case MouseButtons.None:
                    break;

                case MouseButtons.Middle:
                    break;

                case MouseButtons.XButton1:
                    break;

                case MouseButtons.XButton2:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        #region 其它方法

        private string GetFieldValue(string field)
        {
            try
            {
                field = field.StrTrim();

                foreach (var c in from c in ThisStateMachine.LstControllers
                                  let controller = c as ControllerBase
                                  where controller != null && controller.Name ==
                                        field.Split(new[] { ".Field." }, StringSplitOptions.RemoveEmptyEntries)[0]
                                  select c)
                {
                    var f =
                        c.GetType().GetField(field.Split(new[] { ".Field." }, StringSplitOptions.RemoveEmptyEntries)[1]);
                    var fValuetp = f.GetValue(c);

                    var fValue = string.Empty;// f.GetValue(c).ToString();
                    if (fValuetp != null)
                        fValue = fValuetp.ToString();

                    if (f.FieldType == typeof(bool) || f.FieldType == typeof(bool?))
                        fValue = fValue == bool.TrueString ? "1" : "0";

                    return fValue;
                }

            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        private string GetPartValue(string part)
        {
            try
            {
                part = part.StrTrim();

                var findPart =
                    ThisStateMachine.DeviceConfig.Parts.ToList()
                        .Find(
                            f =>
                                f.ProcessNo == part.Split(new[] { ".Part." }, StringSplitOptions.RemoveEmptyEntries)[0] &&
                                f.Name == part.Split(new[] { ".Part." }, StringSplitOptions.RemoveEmptyEntries)[1]);

                if (findPart != null)
                    return GetFieldValue(findPart.ControllerField);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }

        #endregion

        #region 鼠标事件
        /// <summary>
        /// 鼠标按钮松开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelMain_MouseUp(object sender, MouseEventArgs e)
        {
            IsImgMoving = false;
        }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelMain_MouseMove(object sender, MouseEventArgs e)
        {
            #region 实时记录当前鼠标位置
            LastCursor = CurrentCursor;
            CurrentCursor = e.Location;
            if (e.Location.X >= A4StandardWidth)
                CurrentCursor = new Point(A4StandardWidth, CurrentCursor.Y);
            else if (e.Location.X <= 0)
                CurrentCursor = new Point(0, CurrentCursor.Y);
            if (e.Location.Y > A4StandardHeight)
                CurrentCursor = new Point(CurrentCursor.X, A4StandardHeight);
            else if (e.Location.Y <= 0)
                CurrentCursor = new Point(CurrentCursor.X, 0);
            //Text = string.Format(@"{0},{1}", CurrentCursor.X, CurrentCursor.Y);
            #endregion

            if (SelectedWorkstation == null)
                return;

            var curr = GetCusorWithBeforeOffset(CurrentCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY);
            var pre = GetCusorWithBeforeOffset(LastCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY);

            switch (e.Button)
            {
                #region 按住鼠标左键移动

                case MouseButtons.Left: // 按住鼠标左键移动
                    if (SelectedStatusUnit != null && SelectedCondition == null) // 移动状态单元 或 放大缩小状态单元
                    {
                        var rect = GetRectOfSu(SelectedStatusUnit);
                        switch (_mousePointPosition)
                        {
                            case EnumMousePointPosition.MouseDrag:
                                rect.X = rect.X + curr.X - pre.X;
                                rect.Y = rect.Y + curr.Y - pre.Y;
                                break;

                            case EnumMousePointPosition.MouseSizeBottom:
                                if (curr.Y - rect.Top > 50)
                                    rect.Height = curr.Y - rect.Top;
                                break;

                            case EnumMousePointPosition.MouseSizeBottomRight:
                                if (curr.Y - rect.Top > 50 && curr.X - rect.Left > 100)
                                {
                                    rect.Height = curr.Y - rect.Top;
                                    rect.Width = curr.X - rect.Left;
                                }
                                break;

                            case EnumMousePointPosition.MouseSizeRight:
                                if (curr.X - rect.Left > 100)
                                    rect.Width = curr.X - rect.Left;
                                break;

                            case EnumMousePointPosition.MouseSizeTop:
                                if (rect.Height - curr.Y + rect.Y > 50)
                                {
                                    rect.Height = rect.Height - curr.Y + rect.Y;
                                    rect.Y = curr.Y;
                                }
                                break;

                            case EnumMousePointPosition.MouseSizeLeft:
                                if (rect.Width - curr.X + rect.X > 100)
                                {
                                    rect.Width = rect.Width - curr.X + rect.X;
                                    rect.X = curr.X;
                                }
                                break;

                            case EnumMousePointPosition.MouseSizeBottomLeft:
                                if (curr.Y - rect.Top > 50 && rect.Width + curr.X - rect.X > 100)
                                {
                                    rect.Height = curr.Y - rect.Top;
                                    rect.Width = rect.Width + curr.X - rect.X;
                                    rect.X = curr.X;
                                }
                                break;

                            case EnumMousePointPosition.MouseSizeTopRight:
                                if (rect.Height - curr.Y + rect.Y > 50 && curr.X - rect.X > 100)
                                {
                                    rect.Height = rect.Height - curr.Y + rect.Y;
                                    rect.Width = curr.X - rect.X;
                                    rect.Y = curr.Y;
                                }
                                break;

                            case EnumMousePointPosition.MouseSizeTopLeft:
                                if (rect.Height - curr.Y + rect.Y > 50 && rect.Width - curr.X + rect.X > 100)
                                {
                                    rect.Width = rect.Width - curr.X + rect.X;
                                    rect.Height = rect.Height - curr.Y + rect.Y;
                                    rect.X = curr.X;
                                    rect.Y = curr.Y;
                                }
                                break;

                            case EnumMousePointPosition.MouseSizeNone:
                                break;

                            default:
                                throw new ArgumentOutOfRangeException();
                        }

                        SelectedStatusUnit.PositionSize =
                            rect.Left + "," +
                            rect.Top + "," +
                            rect.Width + "," +
                            rect.Height;
                    }
                    else if (SelectedStatusUnit == null && SelectedCondition != null) // 移动条件线
                    {
                        SelectedCondition.MiddlePisiton =
                            string.Format("{0},{1}",
                                GetCusorWithBeforeOffset(CurrentCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY)
                                    .X,
                                GetCusorWithBeforeOffset(CurrentCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY)
                                    .Y);
                    }
                    break;

                #endregion

                #region 按住鼠标右键移动

                case MouseButtons.Right: // 按住鼠标右键移动
                    if (IsImgMoving) // 移动img，通过scale和Translate缩放和平移img，此处不计算比例
                    {
                        ImgMoveOffSetX = CurrentCursor.X - LastCursor.X + ImgMoveOffSetX;
                        ImgMoveOffSetY = CurrentCursor.Y - LastCursor.Y + ImgMoveOffSetY;
                    }
                    break;
                #endregion

                #region 不按键，鼠标移动，更新鼠标形状
                case MouseButtons.None: // 不按键，鼠标移动，更新鼠标形状
                    // 更新鼠标形状
                    // 状态框鼠标显示
                    lock (ThisStateMachine)
                    {
                        foreach (var stateUnit in _statusUnits)
                        {
                            var lstRects = GetRectsOfStatusUnit(stateUnit);

                            foreach (var rect in lstRects)
                            {
                                _mousePointPosition = MousePointPosition(rect,
                                    GetCusorWithBeforeOffset(CurrentCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY)); //'判断光标的位置状态    
                                switch (_mousePointPosition) //'改变光标    
                                {
                                    case EnumMousePointPosition.MouseSizeNone:
                                        Cursor = Cursors.Arrow; //'箭头    
                                        break;
                                    case EnumMousePointPosition.MouseDrag:
                                        Cursor = Cursors.Hand; //'Hand                                    
                                        break;
                                    case EnumMousePointPosition.MouseSizeBottom:
                                        Cursor = Cursors.SizeNS; //'南北    
                                        break;
                                    case EnumMousePointPosition.MouseSizeTop:
                                        Cursor = Cursors.SizeNS; //'南北    
                                        break;
                                    case EnumMousePointPosition.MouseSizeLeft:
                                        Cursor = Cursors.SizeWE; //'东西    
                                        break;
                                    case EnumMousePointPosition.MouseSizeRight:
                                        Cursor = Cursors.SizeWE; //'东西    
                                        break;
                                    case EnumMousePointPosition.MouseSizeBottomLeft:
                                        Cursor = Cursors.SizeNESW; //'东北到南西    
                                        break;
                                    case EnumMousePointPosition.MouseSizeBottomRight:
                                        Cursor = Cursors.SizeNWSE; //'东南到西北    
                                        break;
                                    case EnumMousePointPosition.MouseSizeTopLeft:
                                        Cursor = Cursors.SizeNWSE; //'东南到西北    
                                        break;
                                    case EnumMousePointPosition.MouseSizeTopRight:
                                        Cursor = Cursors.SizeNESW; //'东北到南西    
                                        break;
                                    default:
                                        Cursor = Cursors.Default;
                                        break;
                                }
                                if (Cursor != Cursors.Default)
                                    break;
                            }
                            if (Cursor != Cursors.Default)
                                break;
                        }
                    }

                    //条件线中间点鼠标显示
                    foreach (var pMiddle in _conditions.Select(GetMiddlePoint))
                    {
                        if (RectOfPoint(curr).Contains(pMiddle))
                        {
                            Cursor = Cursors.Hand;
                            break;
                        }

                        if (Cursor != Cursors.Default)
                            break;
                    }
                    break;
                #endregion

                case MouseButtons.Middle:
                    break;

                case MouseButtons.XButton1:
                    break;

                case MouseButtons.XButton2:
                    break;

                default:
                    return;
            }
        }

        /// <summary>
        /// 鼠标按钮按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (SelectedWorkstation == null)
                return;

            //var currentCursorWithOffset =
            //    new Point((int)(CurrentCursor.X / _imageScale), (int)(CurrentCursor.Y / _imageScale));
            //currentCursorWithOffset.Offset(-ImgMoveOffSetX, -ImgMoveOffSetY);
            var findUnit =
                _statusUnits.Find(t =>
                    GetRectOfSu(t)
                        .Contains(GetCusorWithBeforeOffset(CurrentCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY)));
            var findCondition =
                _conditions.ToList().Find(t =>
                    RectOfPoint(GetCusorWithBeforeOffset(CurrentCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY))
                        .Contains(GetMiddlePoint(t)));
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (findUnit != null && findCondition == null)
                    {
                        SelectedStatusUnit = findUnit;
                        SelectedCondition = null;
                    }
                    else if (findUnit == null && findCondition != null)
                    {
                        SelectedStatusUnit = null;
                        SelectedCondition = findCondition;
                    }
                    else
                    {
                        SelectedStatusUnit = null;
                        SelectedCondition = null;

                        Cursor = Cursors.Cross;
                    }
                    break;

                case MouseButtons.Right:
                    if (findUnit != null && findCondition == null)
                    {
                        SelectedStatusUnit = findUnit;
                        SelectedCondition = null;
                    }
                    else if (findUnit == null && findCondition == null)
                    {
                        Cursor = Cursors.NoMove2D;
                        IsImgMoving = true;
                        CurrentCursor = e.Location;
                        SelectedCondition = null;
                        SelectedStatusUnit = null;
                    }
                    break;

                case MouseButtons.None:
                    break;
                case MouseButtons.Middle:
                    break;
                case MouseButtons.XButton1:
                    break;
                case MouseButtons.XButton2:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 鼠标滚轮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelMain_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Location.X > A4StandardWidth || e.Location.Y > A4StandardHeight)
                return;

            if (e.Delta > 0)
            {
                if (_imageScale > 1.55)
                    return;
                _imageScale *= 1.55;
            }
            else
            {
                if (_imageScale < 0.25)
                    return;
                _imageScale /= 1.25;
            }

            panel1.VerticalScroll.Value = panel1.VerticalScroll.Minimum;
            Thread.Sleep(10);
        }
        #endregion

        #region 画图方法
        private static Point GetCusorWithBeforeOffset(
            Point p, double scale, int offetX, int offsetY)
        {
            var currentCursorWithOffset =
                new Point((int)(p.X / scale), (int)(p.Y / scale));
            currentCursorWithOffset.Offset(-offetX, -offsetY);
            return currentCursorWithOffset;
        }

        private static List<Rectangle> GetRectsOfStatusUnit(
            DeviceConfigStatusUnit su)
        {
            var lstRects = new List<Rectangle>();

            if (su == null)
                return null;

            var position = su.PositionSize;

            if (position == null ||
                !position.Contains(",") ||
                position.Split(',').Length % 4 != 0)
            {
                position = "200,200,180,90";
            }

            var lstNum = position.Split(',');

            for (var i = 0; i < lstNum.Length / 4; i++)
            {
                var rect = new Rectangle
                {
                    X = int.Parse(lstNum[i * 4 + 0]),
                    Y = int.Parse(lstNum[i * 4 + 1]),
                    Width = int.Parse(lstNum[i * 4 + 2]),
                    Height = int.Parse(lstNum[i * 4 + 3])
                };
                lstRects.Add(rect);
            }

            return lstRects;
        }

        private static Rectangle GetRectOfSu(
            DeviceConfigStatusUnit su)
        {
            var rect = new Rectangle(50, 50, 180, 90);
            if (su == null)
                return rect;
            var position = su.PositionSize;

            if (string.IsNullOrEmpty(position) || !position.Contains(",") || position.Split(',').Length != 4)
            {
                position = "50,50,180,90";
            }
            var lstNum = position.Split(',');
            rect.X = int.Parse(lstNum[0]);
            rect.Y = int.Parse(lstNum[1]);
            rect.Width = int.Parse(lstNum[2]);
            rect.Height = int.Parse(lstNum[3]);

            return rect;
        }

        private static Point GetMiddlePoint(
            DeviceConfigCondition condition)
        {
            var p = new Point();

            var middle = condition.MiddlePisiton;
            if (middle == null || middle.Split(',').Length != 2)
            {
                MessageBox.Show(@"条件中间点数据不对");
                middle = "100,100";
            }
            var lst = middle.Split(',');
            p.X = int.Parse(lst[0]);
            p.Y = int.Parse(lst[1]);

            return p;
        }

        private static EnumMousePointPosition MousePointPosition(
            Rectangle rect, Point point)
        {
            if (RectOfPoint(rect.Location).Contains(point))
                return EnumMousePointPosition.MouseSizeTopLeft; ;
            if (RectOfPoint(new Point(rect.Right, rect.Bottom)).Contains(point))
                return EnumMousePointPosition.MouseSizeBottomRight;
            if (RectOfPoint(new Point(rect.Left, rect.Bottom)).Contains(point))
                return EnumMousePointPosition.MouseSizeBottomLeft;
            if (RectOfPoint(new Point(rect.Right, rect.Top)).Contains(point))
                return EnumMousePointPosition.MouseSizeTopRight;

            var rectAround = new Rectangle
            {
                X = rect.X - 3,
                Y = rect.Y,
                Width = 6,
                Height = rect.Height
            };
            if (rectAround.Contains(point))
                return EnumMousePointPosition.MouseSizeLeft;
            rectAround.X = rect.Right - 3;
            if (rectAround.Contains(point))
                return EnumMousePointPosition.MouseSizeRight;
            rectAround.X = rect.X;
            rectAround.Y = rect.Y - 3;
            rectAround.Width = rect.Width;
            rectAround.Height = 6;
            if (rectAround.Contains(point))
                return EnumMousePointPosition.MouseSizeTop;
            rectAround.Y = rect.Bottom - 6;
            if (rectAround.Contains(point))
                return EnumMousePointPosition.MouseSizeBottom;

            return rect.Contains(point) ? EnumMousePointPosition.MouseDrag : EnumMousePointPosition.MouseSizeNone;
        }

        private static Point GetRectCenter(
            Rectangle rect)
        {
            var p = new Point
            {
                X = rect.Left + rect.Width / 2,
                Y = rect.Top + rect.Height / 2
            };
            return p;
        }

        private static Point GetCrossPoint(
           Rectangle rect, Point pStart, Point pEnd)
        {
            var pCross = new Point();

            if (pEnd.X == pStart.X)
            {
                if (pStart.Y > rect.Bottom)
                {
                    pCross.X = pEnd.X;
                    pCross.Y = rect.Bottom;
                }
                else if (pStart.Y < rect.Top)
                {
                    pCross.X = pEnd.X;
                    pCross.Y = rect.Top;
                }
                return pCross;
            }

            if (pEnd.Y == pStart.Y)
            {
                if (pStart.X > rect.Right)
                {
                    pCross.X = rect.Right;
                    pCross.Y = pStart.Y;
                }
                else if (pStart.X < rect.Left)
                {
                    pCross.X = rect.Left;
                    pCross.Y = pStart.Y;
                }
                return pCross;
            }

            var k = (double)(pEnd.Y - pStart.Y) / (pEnd.X - pStart.X);

            var lstPoints = new List<Point>();

            var pRight = new Point { X = rect.Right - 1 };
            pRight.Y = (int)((pRight.X - pStart.X) * k) + pStart.Y;

            var pLeft = new Point { X = rect.Left + 1 };
            pLeft.Y = (int)((pLeft.X - pStart.X) * k) + pStart.Y;

            var pTop = new Point { Y = rect.Top + 1 };
            pTop.X = (int)((pTop.Y - pStart.Y) / k) + pStart.X;

            var pBottom = new Point { Y = rect.Bottom - 1 };
            pBottom.X = (int)((pBottom.Y - pStart.Y) / k) + pStart.X;

            if (rect.Contains(pRight)) lstPoints.Add(pRight);
            if (rect.Contains(pLeft)) lstPoints.Add(pLeft);
            if (rect.Contains(pTop)) lstPoints.Add(pTop);
            if (rect.Contains(pBottom)) lstPoints.Add(pBottom);

            var distance = int.MaxValue;

            foreach (var p in lstPoints)
            {
                var dis = (p.Y - pStart.Y) * (p.Y - pStart.Y) + (p.X - pStart.X) * (p.X - pStart.X);
                if (dis >= distance)
                    continue;
                pCross = p;
                distance = dis;
            }

            return pCross;
        }

        private static Rectangle RectOfPoint(Point p, int r = 5)
        {
            return new Rectangle(p.X - r, p.Y - r, r * 2, r * 2);
        }

        #endregion

        private enum EnumMousePointPosition
        {
            /// <summary>
            /// 无
            /// </summary>
            MouseSizeNone = 0,

            /// <summary>
            /// 拉伸右边框
            /// </summary>
            MouseSizeRight = 1,

            /// <summary>
            /// 拉伸左边框
            /// </summary>
            MouseSizeLeft = 2,

            /// <summary>
            /// 拉伸下边框
            /// </summary>
            MouseSizeBottom = 3,

            /// <summary>
            /// 拉伸上边框
            /// </summary>
            MouseSizeTop = 4,

            /// <summary>
            /// 拉伸左上角
            /// </summary>
            MouseSizeTopLeft = 5,

            /// <summary>
            /// 拉伸右上角
            /// </summary>
            MouseSizeTopRight = 6,

            /// <summary>
            /// 拉伸左下角
            /// </summary>
            MouseSizeBottomLeft = 7,

            /// <summary>
            /// 拉伸右下角
            /// </summary>
            MouseSizeBottomRight = 8,

            /// <summary>
            /// 鼠标拖动
            /// </summary>
            MouseDrag = 9
        }
    }
}
