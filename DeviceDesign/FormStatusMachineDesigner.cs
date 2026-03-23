using StateMachine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace DeviceDesign
{
    public partial class FormStatusMachineDesigner : FormBase
    {
        private double _imageScale = 0.5;
        private const int A4StandardWidth = 1786;
        private const int A4StandardHeight = 1260;
        private static readonly AdjustableArrowCap LineArrow = new AdjustableArrowCap(6, 12, true);

        private readonly Font _fontName = new Font("微软雅黑", 14, FontStyle.Regular);
        private readonly Font _fontNote = new Font("微软雅黑", 10, FontStyle.Regular);
        private static readonly Color BackgroundColor = SystemColors.Info;
        private readonly SolidBrush _brushBrown = new SolidBrush(Color.Brown);
        private readonly Pen _penDefault = new Pen(Color.Black, 1) { CustomEndCap = LineArrow };
        private readonly Pen _penSelectSu = new Pen(Color.Red, 4) { CustomEndCap = LineArrow };
        private readonly Pen _penMultipleSelect = new Pen(Color.Gray, 4) { CustomEndCap = LineArrow };
        private readonly Pen _penSelectCondition = new Pen(Color.Red, 2);
        private readonly Pen _penMidPen = new Pen(Color.Red, 5);
        private readonly SolidBrush _brushLightBlue = new SolidBrush(Color.LightBlue);
        private readonly SolidBrush _brushDarkGreen = new SolidBrush(Color.DarkGreen);
        private readonly SolidBrush _brushDrakRed = new SolidBrush(Color.DarkRed);
        private readonly SolidBrush _brushBlue = new SolidBrush(Color.Blue);
        private readonly SolidBrush _brushDarkGoldenrod = new SolidBrush(Color.DarkGoldenrod);
        private readonly SolidBrush _brushAntiqueWhite = new SolidBrush(Color.AntiqueWhite);
        private readonly SolidBrush _brushGray = new SolidBrush(Color.Gray);

        private EnumMousePointPosition _mousePointPosition { get; set; }

        private bool _bBeginDraw;
        private bool _bDisplayStatusUnitNote = true;
        private bool _bDisplayConditionNote = true;

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

        private readonly string _filePath = ClassComm.FilePathDeviceConfig;// @"D:\DeviceDesignSystemConfig.xml";
        private DeviceConfigWorkStation _selectedWorkstation;
        private readonly List<DeviceConfigCondition> _conditions = new List<DeviceConfigCondition>();
        private readonly List<DeviceConfigStatusUnit> _statusUnits = new List<DeviceConfigStatusUnit>();

        private DeviceConfigStatusUnit _selectedStatusUnit;
        private DeviceConfigCondition _selectedCondition;
        private DeviceConfigStatusUnit _copiedStatusUnit;

        private const int DefaultStatusUnitWidth = 180;
        private const int DefaultStatusUnitHeight = 80;

        public FormStatusMachineDesigner()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            KeyPreview = true;
            //WindowState = FormWindowState.Maximized;

            //填充工作站combobox
            toolStripComboBoxWorkstation.ComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            foreach (var workstation in ClassComm.DeviceConfig.WorkStations.Where(workstation => toolStripComboBoxWorkstation.ComboBox != null))
                toolStripComboBoxWorkstation.Items.Add(workstation.Name);

            toolStripButtonDelete.Click += toolStripButtonDelete_Click;
            toolStripButtonAddStatusUnit.Click += toolStripButtonAddStatusUnit_Click;
            toolStripButtonAddCondition.Click += toolStripButtonAddCondition_Click;
            //toolStripButtonStatusNote.Click += ToolStripButtonStatusNote_Click;
            //toolStripButtonConditionNote.Click += ToolStripButtonConditionNote_Click;
            //toolStripButtonSaveImage.Click += toolStripButtonSave_Click;
            toolStripButtonSave.Click += toolStripButtonSave_Click;
            //toolStripButtonVirtualStatusUnit.Click += ToolStripButtonVirtualStatusUnit_Click;
            toolStripComboBoxWorkstation.SelectedIndexChanged += ToolStripComboBoxWorkstation_SelectedIndexChanged;

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
            RefreshPaint();

            PanelMain.MouseWheel += PanelMain_MouseWheel;
            PanelMain.MouseDown += PanelMain_MouseDown;
            PanelMain.MouseUp += PanelMain_MouseUp;
            PanelMain.MouseMove += PanelMain_MouseMove;
            PanelMain.DoubleClick += PanelMain_DoubleClick;
        }

        /// <summary>
        /// 画图
        /// </summary>
        private void RefreshPaint()
        {
            if (_selectedWorkstation == null)
                return;

            var g = Graphics.FromImage(Img);
            g.Clear(BackgroundColor);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.ScaleTransform((float)_imageScale, (float)_imageScale);
            g.TranslateTransform(ImgMoveOffSetX, ImgMoveOffSetY);

            var selectConditions = new List<DeviceConfigCondition>();
            if (_isUserDrawRect)
            {
                var pOld = GetCusorWithBeforeOffset(new Point(_userDrawRectLeft, _userDrawRectTop), _imageScale,
                    ImgMoveOffSetX, ImgMoveOffSetY);
                var pNew = GetCusorWithBeforeOffset(CurrentCursor, _imageScale,
                    ImgMoveOffSetX, ImgMoveOffSetY);

                var drawRect = new Rectangle(
                    pOld.X, pOld.Y,
                    pNew.X - pOld.X,
                    pNew.Y - pOld.Y);

                if (drawRect.Height <= 0 && pNew.X > pOld.X)
                {
                    g.DrawLine(_penMultipleSelect, pOld, new Point(pNew.X, pOld.Y));
                }
                if (drawRect.Width <= 0 && pNew.Y > pOld.Y)
                {
                    g.DrawLine(_penMultipleSelect, pOld, new Point(pOld.X, pNew.Y));
                }
                if (drawRect.Height > 0 && drawRect.Width > 0)
                {
                    g.DrawRectangle(_penMultipleSelect, drawRect);

                    selectConditions.AddRange(
                        from t in _conditions
                        let middlePoint =
                            GetCusorWithBeforeOffset(GetMiddlePoint(t), _imageScale, ImgMoveOffSetX, ImgMoveOffSetY)
                        where drawRect.Contains(middlePoint)
                        select t);
                }
            }

            // draw
            #region 画条件线
            foreach (var condition in _conditions)
            {
                var suSource = _statusUnits.Find(f => f.Name == condition.SourceSuName);
                //_statusUnits[condition.SourceSuName];
                var suTarget = _statusUnits.Find(f => f.Name == condition.TargetSuName);
                //_statusUnits[condition.TargetSuName];
                if (suTarget == null || suSource == null)
                    continue;
                var suSourceRect = GetRectsOfStatusUnit(suSource).First();
                var suTargetRect = GetRectsOfStatusUnit(suTarget).First();
                var suTargetRectCenter = GetRectCenter(suTargetRect);
                var pStart = GetRectCenter(suSourceRect);
                var pEnd = GetCrossPoint(
                    suTargetRect, pStart, suTargetRectCenter);
                var pMiddle = GetMiddlePoint(condition);

                //if (selectConditions.Contains(condition) && condition != SelectedCondition)
                //{
                //    g.DrawCurve(_penSelectCondition,
                //        new PointF[] { pStart, pMiddle, pEnd });
                //}
                //else if (condition == SelectedCondition)
                //{
                //    g.DrawCurve(_penSelectCondition,
                //          new PointF[] { pStart, pMiddle, pEnd });
                //}
                //else
                //{
                //    g.DrawCurve(_penDefault,
                //       new PointF[] { pStart, pMiddle, pEnd });
                //}
                g.DrawCurve(condition == _selectedCondition ? _penSelectCondition : _penDefault,
                    new PointF[] { pStart, pMiddle, pEnd });

                if (_bDisplayConditionNote)
                {
                    var strDisplay = string.Empty;
                    if (!string.IsNullOrEmpty(condition.ConditionFunction) && condition.ConditionFunction.Length > 0)
                        strDisplay = "退出条件：" + Environment.NewLine + condition.ConditionFunction;
                    if (!string.IsNullOrEmpty(condition.ExitFunction) && condition.ExitFunction.Length > 0)
                        strDisplay += "退出函数：" + Environment.NewLine + condition.ExitFunction;
                    g.DrawString(strDisplay, _fontNote, _brushBrown,
                        new Rectangle(pMiddle.X, pMiddle.Y, 400, 300));
                }

                // 画曲线中间点
                var rectMidPoint = new Rectangle(pMiddle.X - 2, pMiddle.Y - 2, 4, 4);
                g.DrawRectangle(_penDefault, rectMidPoint);
                g.FillRectangle(_brushBlue, rectMidPoint);
            }

            if (_bBeginDraw)
            {
                var selectedStatusUnitRect = GetRectOfSu(_selectedStatusUnit);
                //selectedStatusUnitRect = new Rectangle();

                g.DrawLine(
                    _penDefault,
                    GetRectCenter(selectedStatusUnitRect),
                    GetCusorWithBeforeOffset(CurrentCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY));
            }

            #endregion

            #region 画状态框
            foreach (var stateUnit in _statusUnits)
            {
                var lstRects = GetRectsOfStatusUnit(stateUnit);

                foreach (var rect in lstRects)
                {
                    g.DrawRectangle(
                        stateUnit == _selectedStatusUnit ? _penSelectSu : _penDefault, rect);
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

                    if (!_bDisplayStatusUnitNote)
                        continue;

                    var strDisplay = string.Empty;
                    //if (!string.IsNullOrEmpty(stateUnit.EnterNote) && stateUnit.EnterNote.Length > 0)
                    //    strDisplay = "进入函数:" + Environment.NewLine + stateUnit.EnterNote;
                    //if (!string.IsNullOrEmpty(stateUnit.DuringNote) && stateUnit.DuringNote.Length > 0)
                    //    strDisplay += "执行函数:" + Environment.NewLine + stateUnit.DuringNote;
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

            Thread.Sleep(10);
            //panelMain.CreateGraphics().DrawImage(img, 0, 0);
        }

        #region 鼠标事件
        /// <summary>
        /// 鼠标按钮松开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                _bBeginDraw = false;
            IsImgMoving = false;
            _isUserDrawRect = false;
            RefreshPaint();
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

            if (_selectedWorkstation == null)
                return;

            var curr = GetCusorWithBeforeOffset(CurrentCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY);
            var pre = GetCusorWithBeforeOffset(LastCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY);

            switch (e.Button)
            {
                #region 按住鼠标左键移动

                case MouseButtons.Left: // 按住鼠标左键移动
                    if (_selectedStatusUnit != null && _selectedCondition == null) // 移动状态单元 或 放大缩小状态单元
                    {
                        var rect = GetRectOfSu(_selectedStatusUnit);
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

                        _selectedStatusUnit.PositionSize =
                            rect.Left + "," +
                            rect.Top + "," +
                            rect.Width + "," +
                            rect.Height;
                    }
                    else if (_selectedStatusUnit == null && _selectedCondition != null) // 移动条件线
                    {
                        _selectedCondition.MiddlePisiton =
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
                    if (IsImgMoving && !_bBeginDraw) // 移动img，通过scale和Translate缩放和平移img，此处不计算比例
                    {
                        ImgMoveOffSetX = CurrentCursor.X - LastCursor.X + ImgMoveOffSetX;
                        ImgMoveOffSetY = CurrentCursor.Y - LastCursor.Y + ImgMoveOffSetY;
                    }
                    else if (!IsImgMoving && _bBeginDraw) // 新增条件线.
                    {
                        // 画箭头曲线，如果画到终点状态框中，则选中当前曲线
                        //如果鼠标在其他状态框中
                        var suTarget = _statusUnits.Find(t => GetRectOfSu(t).Contains(curr) && t != _selectedStatusUnit);

                        if (suTarget != null)
                        {
                            var conditionName = _selectedStatusUnit.Name + "_" + suTarget.Name;
                            var condition = _conditions.ToList().Find(t => t.Name == conditionName);
                            if (condition != null)
                                MessageBox.Show(conditionName + @"条件已经存在");
                            else
                            {
                                var suSourceRect = GetRectOfSu(_selectedStatusUnit);
                                var pStart = GetRectCenter(suSourceRect);
                                var suTargetRect = GetRectOfSu(suTarget);
                                var suTargetRectCenter = GetRectCenter(suTargetRect);
                                var pEnd = GetCrossPoint(suTargetRect, pStart, suTargetRectCenter);

                                var dcCondition = new DeviceConfigCondition
                                {
                                    SourceSuName = _selectedStatusUnit.Name,
                                    TargetSuName = suTarget.Name,
                                    Name = _selectedStatusUnit.Name + "_" + suTarget.Name,
                                    MiddlePisiton = (pEnd.X - pStart.X) / 2 + pStart.X + "," +
                                                    ((pEnd.Y - pStart.Y) / 2 + pStart.Y),
                                    WorkStationName = _selectedWorkstation.Name,
                                    ConditionFunction = string.Empty,
                                    ExitFunction = string.Empty
                                };

                                _conditions.Add(dcCondition);
                                //存储到xml文件中
                                var lstCondition = ClassComm.DeviceConfig.Conditions.ToList()
                                    .FindAll(t => t.WorkStationName != _selectedWorkstation.Name);
                                lstCondition.AddRange(_conditions);
                                ClassComm.DeviceConfig.Conditions = lstCondition.ToArray();

                                _selectedCondition = dcCondition;
                                _selectedStatusUnit = null;
                            }
                            _bBeginDraw = false;
                        }
                    }
                    break;
                #endregion

                #region 不按键，鼠标移动，更新鼠标形状
                case MouseButtons.None: // 不按键，鼠标移动，更新鼠标形状
                    // 更新鼠标形状
                    // 状态框鼠标显示
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

            RefreshPaint();
        }

        private bool _isUserDrawRect;
        private int _userDrawRectTop;
        private int _userDrawRectLeft;

        /// <summary>
        /// 鼠标按钮按下事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PanelMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (_selectedWorkstation == null)
                return;

            //var currentCursorWithOffset =
            //    new Point((int)(CurrentCursor.X / _imageScale), (int)(CurrentCursor.Y / _imageScale));
            //currentCursorWithOffset.Offset(-ImgMoveOffSetX, -ImgMoveOffSetY);
            var findUnit =
                _statusUnits.ToList().Find(t =>
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
                        _selectedStatusUnit = findUnit;
                        _selectedCondition = null;
                        RefreshPaint();
                    }
                    else if (findUnit == null && findCondition != null)
                    {
                        _selectedStatusUnit = null;
                        _selectedCondition = findCondition;
                        RefreshPaint();
                    }
                    else
                    {
                        _selectedStatusUnit = null;
                        _selectedCondition = null;

                        Cursor = Cursors.Cross;
                        _userDrawRectLeft = e.Location.X;
                        _userDrawRectTop = e.Location.Y;
                        _isUserDrawRect = true;
                    }
                    break;

                case MouseButtons.Right:
                    if (findUnit != null && findCondition == null)
                    {
                        _selectedStatusUnit = findUnit;
                        _selectedCondition = null;
                        _bBeginDraw = true;
                        RefreshPaint();
                    }
                    else if (findUnit == null && findCondition == null)
                    {
                        Cursor = Cursors.NoMove2D;
                        IsImgMoving = true;
                        CurrentCursor = e.Location;
                        _selectedCondition = null;
                        _selectedStatusUnit = null;
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

            RefreshPaint();
            panel1.VerticalScroll.Value = panel1.VerticalScroll.Minimum;
            Thread.Sleep(10);
        }

        private void PanelMain_DoubleClick(object sender, EventArgs e)
        {
            if (_selectedStatusUnit != null)
            {
                var findUnit =
                    _statusUnits.ToList().Find(t =>
                        GetRectOfSu(t)
                            .Contains(GetCusorWithBeforeOffset(CurrentCursor, _imageScale, ImgMoveOffSetX, ImgMoveOffSetY)));

                if (findUnit != null && findUnit.Name == _selectedStatusUnit.Name)
                {
                    //把选择的状态框传递给新的窗体进行操作
                    var frmStatusUnit = new FormStatusUnit(_selectedWorkstation.Name, _selectedStatusUnit.Name);
                    frmStatusUnit.Show();
                }
            }
            else if (_selectedCondition != null)
            {
                var frmCondition = new FormConditions(_selectedWorkstation.Name, _selectedCondition.Name);
                frmCondition.Show();
            }

            RefreshPaint();
        }
        #endregion

        #region 按键响应事件

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (_selectedWorkstation != null)
            {
                if (keyData == (Keys.Control | Keys.C))
                {
                    CopySelectedStatusUnit();
                    return true;
                }

                if (keyData == (Keys.Control | Keys.V))
                {
                    PasteCopiedStatusUnit();
                    return true;
                }

                if (keyData == Keys.Delete)
                {
                    DeleteSelectedItem();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void toolStripButtonAddStatusUnit_Click(
            object sender, EventArgs e)
        {
            if (_selectedWorkstation == null)
            {
                MessageBox.Show(@"请先选择工作站！");
                return;
            }

            var frmStatusUnit = new FormStatusUnit(_selectedWorkstation.Name, string.Empty);
            frmStatusUnit.PointStatusUnitInit = GetCenteredStatusUnitLocation(new Size(DefaultStatusUnitWidth, DefaultStatusUnitHeight));
            frmStatusUnit.ShowDialog();

            _statusUnits.Clear();
            foreach (var s in ClassComm.DeviceConfig.StatusUnits.ToList()
                .FindAll(t => t.WorkStationName.Equals(_selectedWorkstation.Name)))
                _statusUnits.Add(s);

            RefreshPaint();
        }

        private void toolStripButtonAddCondition_Click(
            object sender, EventArgs e)
        {
            if (_selectedWorkstation == null)
            {
                MessageBox.Show(@"请先选择工作站！");
                return;
            }

            var frmCondition = new FormConditions(_selectedWorkstation.Name, string.Empty);
            frmCondition.ShowDialog();

            RefreshPaint();
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if (_selectedStatusUnit != null)
            {
                if (MessageBox.Show(@"确定删除选中的状态单元和状态单元连接的条件单元？", @"删除状态", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    var lstSu = ClassComm.DeviceConfig.StatusUnits.ToList().FindAll(t => t.WorkStationName != _selectedWorkstation.Name);

                    var findToRemove = _statusUnits.FindIndex(f => f.Name == _selectedStatusUnit.Name);
                    if (findToRemove != -1)
                        _statusUnits.RemoveAt(findToRemove);

                    lstSu.AddRange(_statusUnits);
                    ClassComm.DeviceConfig.StatusUnits = lstSu.ToArray();

                    var lstConditions = ClassComm.DeviceConfig.Conditions.ToList().FindAll(t => t.WorkStationName != _selectedWorkstation.Name);
                    var temp =
                        _conditions.Where(
                            c =>
                                c.SourceSuName == _selectedStatusUnit.Name || c.TargetSuName == _selectedStatusUnit.Name)
                            .ToList();

                    foreach (var t in temp)
                        _conditions.Remove(t);
                    lstConditions.AddRange(_conditions);
                    ClassComm.DeviceConfig.Conditions = lstConditions.ToArray();
                }
            }
            else if (_selectedCondition != null)
            {
                if (MessageBox.Show(@"确定删除选中的条件单元？", @"删除条件", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    var lstCondition = ClassComm.DeviceConfig.Conditions.ToList().FindAll(t => t.WorkStationName != _selectedWorkstation.Name);

                    var findToRemove = _conditions.FindIndex(f => f.Name == _selectedCondition.Name);
                    if (findToRemove != -1)
                        _conditions.RemoveAt(findToRemove);

                    lstCondition.AddRange(_conditions);

                    ClassComm.DeviceConfig.Conditions = lstCondition.ToArray();
                }
            }

            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, _filePath, Encoding.UTF8);
            RefreshPaint();
        }

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, _filePath, Encoding.UTF8);
        }

        private void ToolStripComboBoxWorkstation_SelectedIndexChanged(
            object sender, EventArgs e)
        {
            _selectedWorkstation = null;
            _selectedStatusUnit = null;
            _selectedCondition = null;

            //根据选择的工作站配置文件更新当前状态单元列表
            var workstationName = toolStripComboBoxWorkstation.Text;
            _selectedWorkstation =
                ClassComm.DeviceConfig.WorkStations.ToList().Find(t => t.Name == workstationName);

            _conditions.Clear();
            _statusUnits.Clear();

            foreach (var c in ClassComm.DeviceConfig.Conditions.ToList()
                .FindAll(t => t.WorkStationName.Equals(_selectedWorkstation.Name)))
                _conditions.Add(c);

            foreach (var s in ClassComm.DeviceConfig.StatusUnits.ToList()
                .FindAll(t => t.WorkStationName.Equals(_selectedWorkstation.Name)))
                _statusUnits.Add(s);

            // toolStripComboBoxWorkstation.Enabled = false;
            RefreshPaint();

            //toolStripComboBoxWorkstation.Enabled = false;
            PanelMain.Focus();
        }

        #endregion

        #region 方法

        #region 复制粘贴删除功能

        private void CopySelectedStatusUnit()
        {
            if (_selectedStatusUnit == null)
            {
                return;
            }

            _copiedStatusUnit = CloneStatusUnit(_selectedStatusUnit);
        }

        private void PasteCopiedStatusUnit()
        {
            if (_selectedWorkstation == null)
            {
                return;
            }

            if (_copiedStatusUnit == null)
            {
                return;
            }

            var newStatusUnit = CloneStatusUnit(_copiedStatusUnit);
            var sourceRect = GetRectOfSu(_copiedStatusUnit);
            var statusSize = new Size(sourceRect.Width, sourceRect.Height);
            var location = GetCenteredStatusUnitLocation(statusSize);

            newStatusUnit.WorkStationName = _selectedWorkstation.Name;
            newStatusUnit.Name = GenerateUniqueStatusUnitName(_copiedStatusUnit.Name);
            newStatusUnit.PositionSize = $"{location.X},{location.Y},{statusSize.Width},{statusSize.Height}";

            var lstStatusUnits = ClassComm.DeviceConfig.StatusUnits?.ToList() ?? new List<DeviceConfigStatusUnit>();
            lstStatusUnits.Add(newStatusUnit);
            ClassComm.DeviceConfig.StatusUnits = lstStatusUnits.ToArray();

            _selectedStatusUnit = newStatusUnit;
            _selectedCondition = null;

            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, _filePath, Encoding.UTF8);
            RefreshWorkstationData();
        }

        private bool DeleteSelectedItem()
        {
            if (_selectedWorkstation == null)
                return false;

            if (_selectedStatusUnit != null)
            {
                if (MessageBox.Show(@"确定删除选中的状态单元和状态单元连接的条件单元？", @"删除状态", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return false;

                var statusName = _selectedStatusUnit.Name;
                ClassComm.DeviceConfig.StatusUnits = (ClassComm.DeviceConfig.StatusUnits ?? new DeviceConfigStatusUnit[0])
                    .Where(t => !(t.WorkStationName == _selectedWorkstation.Name && t.Name == statusName))
                    .ToArray();

                ClassComm.DeviceConfig.Conditions = (ClassComm.DeviceConfig.Conditions ?? new DeviceConfigCondition[0])
                    .Where(t => !(t.WorkStationName == _selectedWorkstation.Name &&
                                  (t.SourceSuName == statusName || t.TargetSuName == statusName)))
                    .ToArray();

                _selectedStatusUnit = null;
                _selectedCondition = null;
            }
            else if (_selectedCondition != null)
            {
                if (MessageBox.Show(@"确定删除选中的条件单元？", @"删除条件", MessageBoxButtons.OKCancel) != DialogResult.OK)
                    return false;

                var conditionName = _selectedCondition.Name;
                ClassComm.DeviceConfig.Conditions = (ClassComm.DeviceConfig.Conditions ?? new DeviceConfigCondition[0])
                    .Where(t => !(t.WorkStationName == _selectedWorkstation.Name && t.Name == conditionName))
                    .ToArray();

                _selectedCondition = null;
            }
            else
            {
                return false;
            }

            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, _filePath, Encoding.UTF8);
            RefreshWorkstationData();
            return true;
        }

        private void RefreshWorkstationData()
        {
            if (_selectedWorkstation == null)
            {
                RefreshPaint();
                return;
            }

            var selectedStatusName = _selectedStatusUnit?.Name;
            var selectedConditionName = _selectedCondition?.Name;

            _statusUnits.Clear();
            if (ClassComm.DeviceConfig.StatusUnits != null)
            {
                _statusUnits.AddRange(ClassComm.DeviceConfig.StatusUnits
                    .Where(t => t.WorkStationName == _selectedWorkstation.Name));
            }

            _conditions.Clear();
            if (ClassComm.DeviceConfig.Conditions != null)
            {
                _conditions.AddRange(ClassComm.DeviceConfig.Conditions
                    .Where(t => t.WorkStationName == _selectedWorkstation.Name));
            }

            _selectedStatusUnit = string.IsNullOrEmpty(selectedStatusName)
                ? null
                : _statusUnits.FirstOrDefault(t => t.Name == selectedStatusName);
            _selectedCondition = string.IsNullOrEmpty(selectedConditionName)
                ? null
                : _conditions.FirstOrDefault(t => t.Name == selectedConditionName);

            RefreshPaint();

            if (PanelMain != null && !PanelMain.IsDisposed)
                PanelMain.Focus();
        }

        private DeviceConfigStatusUnit CloneStatusUnit(DeviceConfigStatusUnit source)
        {
            if (source == null)
                return null;

            return new DeviceConfigStatusUnit
            {
                WorkStationName = source.WorkStationName,
                Name = source.Name,
                EnterFunction = source.EnterFunction,
                DuringFunction = source.DuringFunction,
                PositionSize = source.PositionSize,
                TimeOut = source.TimeOut,
                EnterNote = source.EnterNote,
                DuringNote = source.DuringNote
            };
        }

        private string GenerateUniqueStatusUnitName(string sourceName)
        {
            var baseName = string.IsNullOrWhiteSpace(sourceName) ? "新状态" : sourceName.Trim();
            var index = 1;
            string candidate;
            do
            {
                candidate = $"{baseName}_{index}";
                index++;
            } while (_statusUnits.Any(t => string.Equals(t.Name, candidate, StringComparison.OrdinalIgnoreCase)));

            return candidate;
        }

        #endregion

        #region 画布中央定位功能

        private Point GetViewportCenter()
        {
            return new Point(panel1.ClientSize.Width / 2, panel1.ClientSize.Height / 2);
        }

        private Point GetViewportCenterInLogicalCoordinates()
        {
            return GetCusorWithBeforeOffset(GetViewportCenter(), _imageScale, ImgMoveOffSetX, ImgMoveOffSetY);
        }

        private Point GetCenteredStatusUnitLocation(Size size)
        {
            var logicalCenter = GetViewportCenterInLogicalCoordinates();
            return new Point(
                Math.Max(0, logicalCenter.X - size.Width / 2),
                Math.Max(0, logicalCenter.Y - size.Height / 2));
        }

        #endregion

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
