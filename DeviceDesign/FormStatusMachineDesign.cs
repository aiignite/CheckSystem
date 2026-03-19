using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceDesign
{
    public partial class FormStatusMachineDesign : FormBase
    {
        public StatusMachine.DeviceConfigWorkStation SelectedWorkstation = null;

        private string _filePath = ClassComm.FilePathDeviceConfig;// @"D:\DeviceDesignSystemConfig.xml";
        private List<StatusMachine.DeviceConfigCondition> _conditions;
        private List<StatusMachine.DeviceConfigStatusUnit> _statusUnits;

        private StatusMachine.DeviceConfigStatusUnit _selectedStatusUnit;
        private StatusMachine.DeviceConfigCondition _selectedCondition;

        private readonly Pen _penDefault = new Pen(Color.Black, 2);
        private readonly Pen _penSelectSu = new Pen(Color.Red, 4);
        private readonly Pen _penSelectCondition = new Pen(Color.Red, 2);
        private readonly Pen _penMidPen = new Pen(Color.Red, 5);
        private readonly SolidBrush _theBrush = new SolidBrush(Color.LightBlue);

        private bool _bBeginDraw; //开始画箭头标记

        private readonly List<ConditionLine> _lstConditionLines = new List<ConditionLine>();
        private readonly List<StatusUnitRect> _lstStatusUnitRects = new List<StatusUnitRect>();
        private readonly List<SystemconfigWorkstation> _lstWorkstations = new List<SystemconfigWorkstation>();

        private bool _bDisplayStatusUnitNote;
        private bool _bDisplayConditionNote;


        //鼠标右键选中时和location的位置偏差
        private int _nDeltaX;
        private int _nDeltaY;
        const int Band = 5;
        private EnumMousePointPosition _mousePointPosition;
        private Point _pStart, _pEnd;
        private enum EnumMousePointPosition
        {
            MouseSizeNone = 0, //'无    
            MouseSizeRight = 1, //'拉伸右边框    
            MouseSizeLeft = 2, //'拉伸左边框    
            MouseSizeBottom = 3, //'拉伸下边框    
            MouseSizeTop = 4, //'拉伸上边框    
            MouseSizeTopLeft = 5, //'拉伸左上角    
            MouseSizeTopRight = 6, //'拉伸右上角    
            MouseSizeBottomLeft = 7, //'拉伸左下角    
            MouseSizeBottomRight = 8, //'拉伸右下角    
            MouseDrag = 9   // '鼠标拖动    
        }

        public FormStatusMachineDesign()
        {
            InitializeComponent();
        }

        private void FormStatusMachineDesign_Load(object sender, EventArgs e)
        {

            //填充工作站combobox
            foreach (var workstation in ClassComm.DeviceConfig.WorkStations)
            {
                toolStripComboBoxWorkstation.ComboBox?.Items.Add(workstation.Name);
            }
            toolStripComboBoxWorkstation.SelectedIndexChanged += ToolStripComboBoxWorkstation_SelectedIndexChanged;

            //设置画笔形状和箭头线属性
            SetPenStyle();
        }

        #region 界面放大缩小按钮操作
        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            panelMain.Size = new Size((int)((double)panelMain.Width * 2), (int)((double)panelMain.Height * 2));
        }

        private void toolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            panelMain.Size = new Size((int)((double)panelMain.Width / 1.3), (int)((double)panelMain.Height / 1.3));

        }

        private void FormStatusMachineDesign_SizeChanged(object sender, EventArgs e)
        {
            panelMain.Size = panel1.Size;

        }



        #endregion

        #region 画图和鼠标事件


        private Rectangle GetRectOfSu(StatusMachine.DeviceConfigStatusUnit su)
        {
            Rectangle rect = new Rectangle(50, 50, 180, 90);
            if (su == null)
                return rect;
            string position = su.PositionSize; 

            if (position == null ||!position.Contains(",") || position.Split(',').Length != 4)
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

        private Point GetMiddlePoint(StatusMachine.DeviceConfigCondition condition)
        {
            Point p = new Point();

            string middle = condition.MiddlePisiton;
            if (middle == null || middle.Split(',').Length != 2)
            {
                MessageBox.Show("条件中间点数据不对");
                middle = "100,100";
            }
            var lst = middle.Split(',');
            p.X = int.Parse(lst[0]);
            p.Y = int.Parse(lst[1]);

            return p;
        }

        private void panelMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (SelectedWorkstation == null) return;

            Size scrollOffset = new Size(panel1.AutoScrollPosition);
            var pNow = new Point(e.X-scrollOffset.Width, e.Y-scrollOffset.Height);//转换为panel坐标

            #region 左键事件

            if (e.Button == MouseButtons.Left)
            {
                StatusMachine.DeviceConfigStatusUnit statusunit = _selectedStatusUnit;

                if (statusunit != null)
                {
                    Rectangle rect = GetRectOfSu(statusunit);
                    #region 调整状态框的大小和位置
                    switch (_mousePointPosition)
                    {
                        case EnumMousePointPosition.MouseDrag:
                            rect.X = pNow.X - _nDeltaX;
                            rect.Y = pNow.Y - _nDeltaY;

                            break;
                        case EnumMousePointPosition.MouseSizeBottom:
                            rect.Height = pNow.Y - rect.Top;
                            break;
                        case EnumMousePointPosition.MouseSizeBottomRight:
                            rect.Height = pNow.Y - rect.Top;
                            rect.Width = pNow.X - rect.Left;
                            break;
                        case EnumMousePointPosition.MouseSizeRight:
                            rect.Width = pNow.X - rect.Left;
                            break;
                        case EnumMousePointPosition.MouseSizeTop:
                            rect.Height = rect.Height - pNow.Y + rect.Y;
                            rect.Y = pNow.Y;
                            break;
                        case EnumMousePointPosition.MouseSizeLeft:
                            rect.Width = rect.Width - pNow.X + rect.X;
                            rect.X = pNow.X;
                            break;
                        case EnumMousePointPosition.MouseSizeBottomLeft:
                            rect.Height = pNow.Y - rect.Top;
                            rect.Width = rect.Width + pNow.X - rect.X;
                            rect.X = pNow.X;
                            break;
                        case EnumMousePointPosition.MouseSizeTopRight:
                            rect.Height = rect.Height - pNow.Y + rect.Y;
                            rect.Width = pNow.X - rect.X;
                            rect.Y = pNow.Y;
                            break;
                        case EnumMousePointPosition.MouseSizeTopLeft:
                            rect.Width = rect.Width - pNow.X + rect.X;
                            rect.Height = rect.Height - pNow.Y + rect.Y;
                            rect.X = pNow.X;
                            rect.Y = pNow.Y;
                            break;
                        default:
                            break;
                    }
                    statusunit.PositionSize = rect.Left + "," + rect.Top + "," + rect.Width + "," + rect.Height;

                    //_selectedStatusUnit = statusunit;
                    //var su = SelectedWorkstation.statusunits.ToList().Find(t => t.name == su.StatusName);
                    //su.positionsize = su.PositionSize;
                    #endregion

                }
                //箭头曲线中间点移动
                else if (_selectedCondition != null)
                {
                    _selectedCondition.MiddlePisiton = pNow.X + "," + pNow.Y;           
                }
                //刷新
                panelMain_Paint(null, null);
            }
            #endregion

            #region 右键事件

            else if (e.Button == MouseButtons.Right)
            {
                //画箭头曲线，如果画到终点状态框中，则选中当前曲线
                if (_bBeginDraw)
                {
                    //画曲线时的实时终点坐标
                    _pEnd.X = e.X;
                    _pEnd.Y = e.Y;
                    //如果鼠标在其他状态框中
                    var suEnd= _statusUnits.Find(t => GetRectOfSu(t).Contains(pNow) && t != _selectedStatusUnit);
                    if(suEnd != null)
                    {
                        var conditionName = _selectedStatusUnit.Name + "_" + suEnd.Name;
                        var condition = _conditions.Find(t => t.Name == conditionName);
                        if(condition != null)
                        {
                            MessageBox.Show(conditionName + @"条件已经存在");
                        }
                        else
                        {
                            StatusMachine.DeviceConfigCondition dccondition = new StatusMachine.DeviceConfigCondition();
                            
                            dccondition.SourceSuName = _selectedStatusUnit.Name;
                            dccondition.TargetSuName = suEnd.Name;
                            dccondition.Name = _selectedStatusUnit.Name+"_"+ suEnd.Name;
                            Point PStart = FindEndPoint(GetRectOfSu(suEnd), GetRectOfSu(_selectedStatusUnit));
                            Point PEnd = FindEndPoint(GetRectOfSu(_selectedStatusUnit), GetRectOfSu(suEnd));
                            dccondition.MiddlePisiton = ((PEnd.X - PStart.X) / 2 + PStart.X) + "," + ((PEnd.Y-PStart.Y)/2+PStart.Y);
                            dccondition.WorkStationName = SelectedWorkstation.Name;

                            _conditions.Add(dccondition);
                            //存储到xml文件中
                            var lstCondition = ClassComm.DeviceConfig.Conditions.ToList().FindAll(t => t.WorkStationName != toolStripComboBoxWorkstation.Text);
                            lstCondition.AddRange(_conditions);
                            ClassComm.DeviceConfig.Conditions = lstCondition.ToArray();
                            ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, _filePath, Encoding.UTF8);

                            _selectedCondition = dccondition;
                            _selectedStatusUnit = null;
                        }
                        _bBeginDraw = false;
                    }
                 
                    panelMain_Paint(null, null); //刷新
                }




            }

            #endregion

            #region 移动鼠标事件


            else  //只移动鼠标,显示鼠标形状
            {

                _mousePointPosition = EnumMousePointPosition.MouseSizeNone;
                //鼠标在矩形状态框中
                foreach (var statusunit in _statusUnits)
                {
                    Rectangle rect = GetRectOfSu(statusunit);
                    if (rect.Contains(pNow.X, pNow.Y))
                    {
                        _nDeltaX = pNow.X - rect.X;
                        _nDeltaY = pNow.Y - rect.Y;
                        _mousePointPosition = MousePointPosition(rect, e); //'判断光标的位置状态    
                        switch (_mousePointPosition) //'改变光标    
                        {
                            case EnumMousePointPosition.MouseSizeNone:
                                this.Cursor = Cursors.Arrow; //'箭头    
                                break;
                            case EnumMousePointPosition.MouseDrag:
                                this.Cursor = Cursors.SizeAll; //'四方向    
                                break;
                            case EnumMousePointPosition.MouseSizeBottom:
                                this.Cursor = Cursors.SizeNS; //'南北    
                                break;
                            case EnumMousePointPosition.MouseSizeTop:
                                this.Cursor = Cursors.SizeNS; //'南北    
                                break;
                            case EnumMousePointPosition.MouseSizeLeft:
                                this.Cursor = Cursors.SizeWE; //'东西    
                                break;
                            case EnumMousePointPosition.MouseSizeRight:
                                this.Cursor = Cursors.SizeWE; //'东西    
                                break;
                            case EnumMousePointPosition.MouseSizeBottomLeft:
                                this.Cursor = Cursors.SizeNESW; //'东北到南西    
                                break;
                            case EnumMousePointPosition.MouseSizeBottomRight:
                                this.Cursor = Cursors.SizeNWSE; //'东南到西北    
                                break;
                            case EnumMousePointPosition.MouseSizeTopLeft:
                                this.Cursor = Cursors.SizeNWSE; //'东南到西北    
                                break;
                            case EnumMousePointPosition.MouseSizeTopRight:
                                this.Cursor = Cursors.SizeNESW; //'东北到南西    
                                break;
                            default:
                                break;
                        }
                        break;
                    }

                    this.Cursor = Cursors.Default;
                }


                var rectPoint = new Rectangle(pNow.X - Band, pNow.Y - Band, Band * 2, Band * 2);
                //曲线中间点
                foreach(var condition in _conditions)
                {
                    Point pMiddle = GetMiddlePoint(condition);

                    if(rectPoint.Contains(pMiddle))
                    {
                        this.Cursor = Cursors.SizeAll;
                        //_clMoveOn = condition;
                        break;
                    }
                }                
            }


            #endregion

        }

        private void panelMain_MouseDown(object sender, MouseEventArgs e)
        {
            Size scrollOffset = new Size(panel1.AutoScrollPosition);
            var pNow = new Point(e.X - scrollOffset.Width, e.Y - scrollOffset.Height);

            if (e.Button == MouseButtons.Left)
            {
                //当鼠标位于曲线中间点的方框内，则选中当前曲线
                Rectangle rectPoint = new Rectangle(pNow.X - Band, pNow.Y - Band, Band * 2, Band * 2);

                var condition = _conditions.Find(t => rectPoint.Contains(GetMiddlePoint(t)));
                if(condition !=null)
                {
                    _selectedStatusUnit = null;
                    _selectedCondition = condition;
                }

                var statusunit = _statusUnits.Find(t => GetRectOfSu(t).Contains(pNow));
                if(statusunit !=null)
                {
                    _selectedCondition = null;
                    _selectedStatusUnit = statusunit;
                    _nDeltaX = pNow.X - GetRectOfSu(statusunit).X;//记录鼠标位置和状态框location的位置偏差
                    _nDeltaY = pNow.Y - GetRectOfSu(statusunit).Y;
                }             
            }
            else if (e.Button == MouseButtons.Right)
            {
                var statusunit = _statusUnits.Find(t => GetRectOfSu(t).Contains(pNow));
                if(statusunit !=null)
                {
                    _selectedCondition = null;
                    _selectedStatusUnit = statusunit;
                    _bBeginDraw = true;

                    Rectangle rect = GetRectOfSu(statusunit);
                    _pStart.X = rect.Left + rect.Width / 2 + scrollOffset.Width;
                    _pStart.Y = rect.Top + rect.Height / 2 + scrollOffset.Height;

                }              
            }
            panelMain_Paint(null, null);
        }

        private void panelMain_MouseUp(object sender, MouseEventArgs e)
        {
            if (_bBeginDraw)
            {
                _bBeginDraw = false;
            }

            //_selectedStatusUnit = null;
            //_selectedCondition = null;

            panelMain_Paint(null, null);
        }

        private void panelMain_DoubleClick(object sender, EventArgs e)
        {
            if (_selectedStatusUnit != null)
            {
                //把选择的状态框传递给新的窗体进行操作
                 FormStatusUnit frmStatusUnit = new FormStatusUnit(SelectedWorkstation.Name, _selectedStatusUnit.Name);
                frmStatusUnit.Show();
            }
            else if (_selectedCondition != null)
            {
                FormConditions frmCondition = new FormConditions(SelectedWorkstation.Name,_selectedCondition.Name);
                frmCondition.Show();
            }
            panelMain_Paint(null, null);
        }

        private void panelMain_Paint(object sender, PaintEventArgs e)
        {
            //panel1用于承载panelMain，从而使用scrollbar

            if(SelectedWorkstation == null)
            {
                return;
            }
            Size scrollOffset = new Size(panel1.AutoScrollPosition);

            #region bmp的graphics上画图

            Bitmap localBitmap = new Bitmap(panelMain.Width, panelMain.Height);

            Graphics bmpGraphics = Graphics.FromImage(localBitmap);
            bmpGraphics.Clear(panelMain.BackColor);
            bmpGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            bmpGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            bmpGraphics.CompositingQuality = CompositingQuality.HighQuality;
            // bmpGraphics.TranslateTransform(panelMain.AutoScrollPosition.X, panelMain.AutoScrollPosition.Y);

            #endregion

            #region 把状态框画在bmp上

            foreach(var statusunit in _statusUnits)
            {
                Rectangle rect = GetRectOfSu(statusunit);
                Rectangle rectScroll = new Rectangle(rect.X + scrollOffset.Width, rect.Y + scrollOffset.Height, rect.Width, rect.Height);
                bmpGraphics.DrawRectangle(statusunit== _selectedStatusUnit?_penSelectSu: _penDefault, rectScroll);
                bmpGraphics.FillRectangle(_theBrush, rectScroll);
                bmpGraphics.DrawString(statusunit.Name, new Font("微软雅黑", 14, FontStyle.Regular),
                    new SolidBrush(Color.Blue), rectScroll.X + 20, rectScroll.Y + 20);

                if (_bDisplayStatusUnitNote)
                {
                    string strDisplay = "";
                    if (statusunit.EnterNote?.Length > 0)
                        strDisplay = "进入函数：\r" + statusunit.EnterNote;
                    if (statusunit.DuringFunction?.Length > 0)
                        strDisplay += "执行函数：\r" + statusunit.DuringNote;
                    bmpGraphics.DrawString(strDisplay, new Font("微软雅黑", 10, FontStyle.Regular),
                        new SolidBrush(Color.Blue), rectScroll.Right, rectScroll.Bottom);
                }
            }

           
            #endregion

            #region 把条件线画在bmp上

            foreach(var condition in _conditions)
            {
                StatusMachine.DeviceConfigStatusUnit suSource = _statusUnits.Find(t => t.Name == condition.SourceSuName);
                StatusMachine.DeviceConfigStatusUnit suTarget = _statusUnits.Find(t => t.Name == condition.TargetSuName);
                Point pstart = FindEndPoint(GetRectOfSu(suTarget), GetRectOfSu(suSource));
                Point pend = FindEndPoint(GetRectOfSu(suSource), GetRectOfSu( suTarget));
                Point pmiddle = GetMiddlePoint(condition);
                pstart.X += scrollOffset.Width;
                pend.X += scrollOffset.Width;
                pmiddle.X += scrollOffset.Width;
                pstart.Y += scrollOffset.Height;
                pend.Y += scrollOffset.Height;
                pmiddle.Y += scrollOffset.Height;
                bmpGraphics.DrawCurve(condition == _selectedCondition?_penSelectCondition: _penDefault, new PointF[] { pstart, pmiddle, pend });

                if (_bDisplayConditionNote)
                {
                    string strDisplay = "";
                    if (condition.ConditionNote?.ToString().Length > 0)
                        strDisplay = "退出条件：\r" + condition.ConditionNote;
                    if (condition.ExitNote?.ToString().Length > 0)
                        strDisplay += "退出函数：\r" + condition.ExitNote;
                    bmpGraphics.DrawString(strDisplay, new Font("微软雅黑", 10, FontStyle.Regular),
                        new SolidBrush(Color.Brown),
                        new Rectangle(pmiddle.X,  pmiddle.Y , 400, 300));
                }

                //画曲线中间点
                Rectangle rectMidPoint = new Rectangle(pmiddle.X - 2, pmiddle.Y - 2, 4, 4);
                bmpGraphics.DrawRectangle(_penDefault, rectMidPoint);
                bmpGraphics.FillRectangle(new SolidBrush(Color.Blue), rectMidPoint);

            }
           

            if (_bBeginDraw)
            {
                bmpGraphics.DrawLine(_penDefault, _pStart, _pEnd);
            }
            #endregion

            #region 显示浮动提示文字

            //if (_surMoveOn != null)
            //{
            //    Rectangle rectScroll = new Rectangle(_surMoveOn.Rect.X + scrollOffset.Width,
            //        _surMoveOn.Rect.Y + scrollOffset.Height, _surMoveOn.Rect.Width, _surMoveOn.Rect.Height);

            //    string strDisplay = "";
            //    if (_surMoveOn.EnterNote?.Length > 0)
            //        strDisplay = "进入函数：\r" + _surMoveOn.EnterNote ;
            //    if (_surMoveOn.DuringFunction?.Length > 0)
            //        strDisplay += "执行函数：\r" + _surMoveOn.DuringNote;
            //    bmpGraphics.DrawString(strDisplay, new Font("微软雅黑", 10, FontStyle.Regular),
            //        new SolidBrush(Color.Blue), rectScroll.Right, rectScroll.Bottom);

            //}
            //else if (_clMoveOn != null)
            //{

            //    string strDisplay = "";
            //    if (_clMoveOn.ConditionNote?.Length > 0)
            //        strDisplay = "退出条件：\r" + _clMoveOn.ConditionNote;
            //    if (_clMoveOn.ExitNote?.Length > 0)
            //        strDisplay += "退出函数：\r" + _clMoveOn.ExitNote;
            //    bmpGraphics.DrawString(strDisplay, new Font("微软雅黑", 10, FontStyle.Regular),
            //        new SolidBrush(Color.Brown), new Rectangle(_clMoveOn.PMiddle.X+scrollOffset.Width , _clMoveOn.PMiddle.Y+scrollOffset.Height ,300,300));
            //}


            #endregion

            #region 把bmpgraphics画到panelMain上

            Graphics g = panelMain.CreateGraphics();
            g.DrawImage(localBitmap, 0, 0);

     
            #endregion

            #region 清空对象

            localBitmap.Dispose();
            bmpGraphics.Dispose();
            g.Dispose();

            #endregion
        }

        #endregion

        #region Paint子函数

        private void SetPenStyle()
        {

            System.Drawing.Drawing2D.AdjustableArrowCap lineArrow =
                new System.Drawing.Drawing2D.AdjustableArrowCap(6, 12, true);
            _penDefault.CustomEndCap = lineArrow;
            _penSelectCondition.CustomEndCap = lineArrow;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.ResizeRedraw |
                          ControlStyles.AllPaintingInWmPaint, true);
        }
        private Point FindEndPoint(Rectangle rectBegin, Rectangle rectEnd)
        {
            Point pFind = new Point(0, 0);
            Point pBegin = rectBegin.Location;
            Point pEnd = rectEnd.Location;
            if ((pBegin.X >= pEnd.X) && (pBegin.Y <= pEnd.Y))
            {
                if (Math.Abs(pBegin.X - pEnd.X) > Math.Abs(pBegin.Y - pEnd.Y))
                {
                    pFind.X = pEnd.X + rectEnd.Width;
                    pFind.Y = pEnd.Y + rectEnd.Height / 2;
                }
                else
                {
                    pFind.X = pEnd.X + rectEnd.Width / 2;
                    pFind.Y = pEnd.Y;
                }
            }
            else if ((pBegin.X >= pEnd.X) && (pBegin.Y > pEnd.Y))
            {
                if (Math.Abs(pBegin.X - pEnd.X) > Math.Abs(pBegin.Y - pEnd.Y))
                {
                    pFind.X = pEnd.X + rectEnd.Width;
                    pFind.Y = pEnd.Y + rectEnd.Height / 2;
                }
                else
                {
                    pFind.X = pEnd.X + rectEnd.Width / 2;
                    pFind.Y = pEnd.Y + rectEnd.Height;
                }
            }
            else if ((pBegin.X < pEnd.X) && (pBegin.Y <= pEnd.Y))
            {
                if (Math.Abs(pBegin.X - pEnd.X) > Math.Abs(pBegin.Y - pEnd.Y))
                {
                    pFind.X = pEnd.X;
                    pFind.Y = pEnd.Y + rectEnd.Height / 2;
                }
                else
                {
                    pFind.X = pEnd.X + rectEnd.Width / 2;
                    pFind.Y = pEnd.Y;
                }
            }
            else if ((pBegin.X < pEnd.X) && (pBegin.Y > pEnd.Y))
            {
                if (Math.Abs(pBegin.X - pEnd.X) > Math.Abs(pBegin.Y - pEnd.Y))
                {
                    pFind.X = pEnd.X;
                    pFind.Y = pEnd.Y + rectEnd.Height / 2;
                }
                else
                {
                    pFind.X = pEnd.X + rectEnd.Width / 2;
                    pFind.Y = pEnd.Y + rectEnd.Height;
                }
            }
            else
            {
                pFind.X = pEnd.X;
                pFind.Y = pEnd.Y;
            }

            return pFind;
        }

        private EnumMousePointPosition MousePointPosition(Point pLeftTop, Size size, System.Windows.Forms.MouseEventArgs e)
        {
            Point p = new Point(e.X, e.Y);
            p.X -= pLeftTop.X;
            p.Y -= pLeftTop.Y;

            if ((p.X >= -1 * Band) | (p.X <= size.Width) | (p.Y >= -1 * Band) | (p.Y <= size.Height))
            {
                if (p.X < Band)
                {
                    if (p.Y < Band) { return EnumMousePointPosition.MouseSizeTopLeft; }
                    else
                    {
                        if (p.Y > -1 * Band + size.Height)
                        { return EnumMousePointPosition.MouseSizeBottomLeft; }
                        else
                        { return EnumMousePointPosition.MouseSizeLeft; }
                    }
                }
                else
                {
                    if (p.X > -1 * Band + size.Width)
                    {
                        if (p.Y < Band)
                        { return EnumMousePointPosition.MouseSizeTopRight; }
                        else
                        {
                            if (p.Y > -1 * Band + size.Height)
                            { return EnumMousePointPosition.MouseSizeBottomRight; }
                            else
                            { return EnumMousePointPosition.MouseSizeRight; }
                        }
                    }
                    else
                    {
                        if (p.Y < Band)
                        { return EnumMousePointPosition.MouseSizeTop; }
                        else
                        {
                            if (p.Y > -1 * Band + size.Height)
                            { return EnumMousePointPosition.MouseSizeBottom; }
                            else
                            { return EnumMousePointPosition.MouseDrag; }
                        }
                    }
                }
            }
            else
            { return EnumMousePointPosition.MouseSizeNone; }
        }
        private EnumMousePointPosition MousePointPosition(Rectangle rect, System.Windows.Forms.MouseEventArgs e)
        {
            //   Point p = new Point((int)((float)e.X*_scale),(int)((float) e.Y*_scale));//鼠标位置
            Size scrollOffset = new Size(panel1.AutoScrollPosition);
            var p = new Point(e.X - scrollOffset.Width, e.Y - scrollOffset.Height);

           // var p = new Point(e.X, e.Y);

            Point pTopLeft = new Point(rect.X, rect.Y);
            Point pTopRight = new Point(rect.X + rect.Width, rect.Y);
            Point pBottomLeft = new Point(rect.X, rect.Y + rect.Height);
            Point pBottomRight = new Point(rect.X + rect.Width, rect.Y + rect.Height);

            if (GetRectByBand(pTopLeft, Band).Contains(p))
            {
                return EnumMousePointPosition.MouseSizeTopLeft;
            }
            if (GetRectByBand(pTopRight, Band).Contains(p))
            {
                return EnumMousePointPosition.MouseSizeTopRight;
            }
            if (GetRectByBand(pBottomRight, Band).Contains(p))
            {
                return EnumMousePointPosition.MouseSizeBottomRight;
            }
            if (GetRectByBand(pBottomLeft, Band).Contains(p))
            {
                return EnumMousePointPosition.MouseSizeBottomLeft;
            }
            Rectangle r = new Rectangle();
            r.X = rect.X - Band;
            r.Y = rect.Y;
            r.Width = Band * 2;
            r.Height = rect.Height;
            if (r.Contains(p))
            {
                return EnumMousePointPosition.MouseSizeLeft;
            }

            r.X = rect.X;
            r.Y = rect.Y - Band;
            r.Width = rect.Width;
            r.Height = Band * 2;
            if (r.Contains(p))
            {
                return EnumMousePointPosition.MouseSizeTop;
            }

            r.X = rect.X + rect.Width - Band;
            r.Y = rect.Y;
            r.Width = Band * 2;
            r.Height = rect.Height;
            if (r.Contains(p))
            {
                return EnumMousePointPosition.MouseSizeRight;
            }

            r.X = rect.X;
            r.Y = rect.Y + rect.Height - Band;
            r.Width = rect.Width;
            r.Height = Band * 2;
            if (r.Contains(p))
            {
                return EnumMousePointPosition.MouseSizeBottom;
            }

            if (rect.Contains(p))
            {
                return EnumMousePointPosition.MouseDrag;
            }

            return EnumMousePointPosition.MouseSizeNone;
        }
        private Rectangle GetRectByBand(Point point, int band)
        {
            Rectangle rect = new Rectangle();
            rect.X = point.X - band > 0 ? point.X - band : 0;
            rect.Y = point.Y - band > 0 ? point.Y - band : 0;
            rect.Width = band * 2;
            rect.Height = band * 2;
            return rect;
        }

        #endregion

        private void toolStripButtonSave_Click(object sender, EventArgs e)
        {
            var lstCondition =ClassComm.DeviceConfig.Conditions.ToList().FindAll(t => t.WorkStationName != toolStripComboBoxWorkstation.Text);
            var lstSu = ClassComm.DeviceConfig.StatusUnits.ToList().FindAll(t => t.WorkStationName != toolStripComboBoxWorkstation.Text);
            lstCondition.AddRange(_conditions);
            lstSu.AddRange(_statusUnits);

            ClassComm.DeviceConfig.Conditions = lstCondition.ToArray();
            ClassComm.DeviceConfig.StatusUnits = lstSu.ToArray();

           ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig,_filePath,Encoding.UTF8);
        }


        private void ToolStripComboBoxWorkstation_SelectedIndexChanged(object sender, EventArgs e)
        {
            //根据选择的工作站配置文件更新当前状态单元列表
            var workstationName = toolStripComboBoxWorkstation.Text;

            SelectedWorkstation = ClassComm.DeviceConfig.WorkStations.ToList().Find(t => t.Name == workstationName);

          //  toolStripComboBoxWorkstation.Enabled = false;

            _conditions = ClassComm.DeviceConfig.Conditions.ToList().FindAll(t => t.WorkStationName == workstationName);
            _statusUnits = ClassComm.DeviceConfig.StatusUnits.ToList().FindAll(t => t.WorkStationName == workstationName);
  
            panelMain_Paint(null, null);

        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            if(_selectedCondition !=null)
            {
                if (MessageBox.Show(@"确定删除选中的条件单元？", @"删除条件", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    var lstCondition = ClassComm.DeviceConfig.Conditions.ToList().FindAll(t => t.WorkStationName != toolStripComboBoxWorkstation.Text);
                    _conditions.Remove(_selectedCondition);
                    lstCondition.AddRange(_conditions);

                    ClassComm.DeviceConfig.Conditions = lstCondition.ToArray();
                    ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, _filePath, Encoding.UTF8);
                }
            }
            else if(_selectedStatusUnit !=null)
            {
                if (MessageBox.Show(@"确定删除选中的状态单元和状态单元连接的条件单元？", @"删除状态", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    var lstSu = ClassComm.DeviceConfig.StatusUnits.ToList().FindAll(t => t.WorkStationName != toolStripComboBoxWorkstation.Text);
                    _statusUnits.Remove(_selectedStatusUnit);
                    lstSu.AddRange(_statusUnits);
                    ClassComm.DeviceConfig.StatusUnits = lstSu.ToArray();
                    ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, _filePath, Encoding.UTF8);

                    var lstConditions = ClassComm.DeviceConfig.Conditions.ToList().FindAll(t => t.WorkStationName != toolStripComboBoxWorkstation.Text);
                    _conditions.RemoveAll(t => t.SourceSuName == _selectedStatusUnit.Name || t.TargetSuName == _selectedStatusUnit.Name);
                    lstConditions.AddRange(_conditions);
                    ClassComm.DeviceConfig.Conditions = lstConditions.ToArray();
                    ClassComm.SaveDeviceConfigToFile(ClassComm.DeviceConfig, _filePath, Encoding.UTF8);
                }
            }
            panelMain_Paint(null, null);         

        }

        private void toolStripButtonAddStatusUnit_Click(object sender, EventArgs e)
        {
            if (toolStripComboBoxWorkstation.SelectedIndex < 0)
            {
                MessageBox.Show(@"请先选择工作站！");
                return;
            }
            FormStatusUnit frmStatusUnit = new FormStatusUnit(SelectedWorkstation.Name,"");
            frmStatusUnit.ShowDialog();

            _statusUnits = ClassComm.DeviceConfig.StatusUnits.ToList().FindAll(t => t.WorkStationName == SelectedWorkstation.Name);


            panelMain_Paint(null, null);

        }
       private void toolStripButtonAddCondition_Click(object sender, EventArgs e)
        {
            if (toolStripComboBoxWorkstation.SelectedIndex < 0)
            {
                MessageBox.Show(@"请先选择工作站！");
                return;
            }

            FormConditions frmCondition = new FormConditions(SelectedWorkstation.Name,"");
            frmCondition.ShowDialog();

            panelMain_Paint(null, null);

        }
   
        private void toolStripButtonStatusNote_Click(object sender, EventArgs e)
        {
            if (toolStripButtonStatusNote.Text.Equals("显示状态"))
            {
                toolStripButtonStatusNote.Text = @"隐藏状态";
                _bDisplayStatusUnitNote = true;
            }
            else
            {
                toolStripButtonStatusNote.Text = @"显示状态";
                _bDisplayStatusUnitNote = false;
            }
        }

        private void toolStripButtonConditionNote_Click(object sender, EventArgs e)
        {
            if (toolStripButtonConditionNote.Text.Equals("显示条件"))
            {
                toolStripButtonConditionNote.Text = @"隐藏条件";
                _bDisplayConditionNote = true;
            }
            else
            {
                toolStripButtonConditionNote.Text = @"显示条件";
                _bDisplayConditionNote = false;
            }
        }

        private void toolStripButtonSaveImage_Click(object sender, EventArgs e)
        {

            Size scrollOffset = new Size(0,0);

            #region bmp的graphics上画图

            Bitmap localBitmap = new Bitmap(panelMain.Width, panelMain.Height);

            Graphics bmpGraphics = Graphics.FromImage(localBitmap);
            bmpGraphics.Clear(panelMain.BackColor);
            bmpGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            bmpGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            bmpGraphics.CompositingQuality = CompositingQuality.HighQuality;
            // bmpGraphics.TranslateTransform(panelMain.AutoScrollPosition.X, panelMain.AutoScrollPosition.Y);

            #endregion

            #region 把状态框画在bmp上

            foreach (var statusunit in _statusUnits)
            {
                Rectangle rect = GetRectOfSu(statusunit);
                Rectangle rectScroll = new Rectangle(rect.X + scrollOffset.Width, rect.Y + scrollOffset.Height, rect.Width, rect.Height);
                bmpGraphics.DrawRectangle(statusunit == _selectedStatusUnit ? _penSelectSu : _penDefault, rectScroll);
                bmpGraphics.FillRectangle(_theBrush, rectScroll);
                bmpGraphics.DrawString(statusunit.Name, new Font("微软雅黑", 14, FontStyle.Regular),
                    new SolidBrush(Color.Blue), rectScroll.X + 20, rectScroll.Y + 20);

                if (_bDisplayStatusUnitNote)
                {
                    string strDisplay = "";
                    if (statusunit.EnterNote?.Length > 0)
                        strDisplay = "进入函数：\r" + statusunit.EnterNote;
                    if (statusunit.DuringFunction?.Length > 0)
                        strDisplay += "执行函数：\r" + statusunit.DuringNote;
                    bmpGraphics.DrawString(strDisplay, new Font("微软雅黑", 10, FontStyle.Regular),
                        new SolidBrush(Color.Blue), rectScroll.Right, rectScroll.Bottom);
                }
            }


            #endregion

            #region 把条件线画在bmp上

            foreach (var condition in _conditions)
            {
                StatusMachine.DeviceConfigStatusUnit suSource = _statusUnits.Find(t => t.Name == condition.SourceSuName);
                StatusMachine.DeviceConfigStatusUnit suTarget = _statusUnits.Find(t => t.Name == condition.TargetSuName);
                Point pstart = FindEndPoint(GetRectOfSu(suTarget), GetRectOfSu(suSource));
                Point pend = FindEndPoint(GetRectOfSu(suSource), GetRectOfSu(suTarget));
                Point pmiddle = GetMiddlePoint(condition);
                pstart.X += scrollOffset.Width;
                pend.X += scrollOffset.Width;
                pmiddle.X += scrollOffset.Width;
                pstart.Y += scrollOffset.Height;
                pend.Y += scrollOffset.Height;
                pmiddle.Y += scrollOffset.Height;
                bmpGraphics.DrawCurve(condition == _selectedCondition ? _penSelectCondition : _penDefault, new PointF[] { pstart, pmiddle, pend });

                if (_bDisplayConditionNote)
                {
                    string strDisplay = "";
                    if (condition.ConditionNote?.ToString().Length > 0)
                        strDisplay = "退出条件：\r" + condition.ConditionNote;
                    if (condition.ExitNote?.ToString().Length > 0)
                        strDisplay += "退出函数：\r" + condition.ExitNote;
                    bmpGraphics.DrawString(strDisplay, new Font("微软雅黑", 10, FontStyle.Regular),
                        new SolidBrush(Color.Brown),
                        new Rectangle(pmiddle.X , pmiddle.Y , 400, 300));
                }

                //画曲线中间点
                Rectangle rectMidPoint = new Rectangle(pmiddle.X - 2, pmiddle.Y - 2, 4, 4);
                bmpGraphics.DrawRectangle(_penDefault, rectMidPoint);
                bmpGraphics.FillRectangle(new SolidBrush(Color.Blue), rectMidPoint);

            }


            if (_bBeginDraw)
            {
                bmpGraphics.DrawLine(_penDefault, _pStart, _pEnd);
            }
            #endregion


            #region save bmp

            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                localBitmap.Save(dlg.FileName);
            }

            #endregion

            #region 清空对象

            localBitmap.Dispose();
            bmpGraphics.Dispose();

            #endregion

        }

 

    }

    public class StatusUnitRect
    {
        public string StatusName;
        public string StatusCnName;
        public string EnterFunction;
        public string DuringFunction;
        public string EnterNote;
        public string DuringNote;
        public string No;
        public Rectangle Rect;
        public string Text;
        public string PositionSize;
        public string WorkstationName;

        public StatusUnitRect()
        {

        }

        public StatusUnitRect(Rectangle r, string statusName, string statusCnName)
        {
            Rect = r;
            StatusCnName = statusCnName;
            StatusName = statusName;
            Text = StatusName + "\r\n------------\r\n" + StatusCnName;
        }
    }
    public class ConditionLine
    {
        public string ConditionName;
        public string TargetName;
        public string SourceName;
        public string ConditionFunction;
        public string ExitFunction;
        public string MiddlePosition;
        public string ConditionNote;
        public string ExitNote;
        public Point PStart;
        public Point PMiddle;
        public Point PEnd;
        public string WorkstationName;

        public ConditionLine()
        {

        }
        public ConditionLine(string name)
        {
            ConditionName = name;
        }

    }
}
