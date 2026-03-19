using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace DeviceDesign
{
    public partial class FormDeviceControllerDesign : FormBase
    {

        private  double _panelMainScale = 1.0;
        private  double _imageScale = 1.0;
        private  int _panelMainWidth = 893;  //A4 width
        private  int _panelMainHeight = 630; //A4 height

        private Pen _penRect = new Pen(Color.Red, 2);
        private Pen _penLine = new Pen(Color.Blue, 2);
        private Font _fontName = new Font("微软雅黑", 14, FontStyle.Regular);
        private SolidBrush _brushName = new SolidBrush(Color.Blue);

        private DeviceHardware _deviceHardware = new DeviceHardware();

        private List<ControllerImage> _lstControllerImages = new List<ControllerImage>();
        private List<ConnectorLine> _lstConnectorLines = new List<ConnectorLine>();

        private List<Point> _lstPointTemp = new List<Point>();
        private bool _bPointCatched = false;
        private Point _pointCatched;
        private Point _pointNowInPanel = new Point() ;
        private Point _pointMouseDown = new Point();
        private Line _lineCatch;

        private ControllerImage _controllerSelected ;  //引用类型
        private Point _pointControllerSelectedOld = new Point();
        private Point _pointConnectLineCross ;//值类型
        private Point _pointConnectLineCrossOld = new Point();
        private ConnectorLine _connectLineMovePoint;

        private bool _isDrawConnectLine = false;
        private bool _isMoveConnectPoint = false;
        private bool _isMoveController = false;
        private bool _isMovePanel = false;

        public FormDeviceControllerDesign()
        {
            InitializeComponent();

            string filePath = "";
            string path = Directory.GetCurrentDirectory();
            DirectoryInfo info = new DirectoryInfo(path);
            //Debug.Assert(info.Parent != null, "info.Parent != null");
            //Debug.Assert(info.Parent.Parent != null, "info.Parent.Parent != null");
            filePath = info.Parent.Parent.FullName + @"\DeviceHardware.xml";
            _deviceHardware = ClassComm.Deserialize<DeviceHardware>(filePath);

            UpdateControllerImages();

            panelMain.MouseWheel += PanelMain_MouseWheel;
            panelMain.MouseDown += PanelMain_MouseDown;
            panelMain.MouseUp += PanelMain_MouseUp;
            panelMain.MouseMove += PanelMain_MouseMove;
            toolStripButtonZoomIn.Click += ToolStripButtonZoomIn_Click;
            toolStripButtonZoomOut.Click += ToolStripButtonZoomOut_Click;
            panelMain.Paint += PanelMain_Paint;
        }

        private void PanelMain_Paint(object sender, PaintEventArgs e)
        {
      
            #region bmp的graphics上画图

            Bitmap localBitmap = new Bitmap(panelMain.Width, panelMain.Height);

            Graphics bmpGraphics = Graphics.FromImage(localBitmap);
            bmpGraphics.Clear(panelMain.BackColor);
            bmpGraphics.SmoothingMode = SmoothingMode.AntiAlias;
            // bmpGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            //bmpGraphics.CompositingQuality = CompositingQuality.HighQuality;
            // bmpGraphics.TranslateTransform(panelMain.AutoScrollPosition.X, panelMain.AutoScrollPosition.Y);
            //用于字体缩放
            Matrix matrix = new Matrix();
            matrix.Scale((float)_imageScale, (float)_imageScale);
            bmpGraphics.Transform = matrix;
            #endregion

            #region 把状态框画在bmp上

            foreach(var controller in _deviceHardware.Controllers)
            {
                try
                {
                    foreach(var image in _lstControllerImages)
                    {
                        bmpGraphics.DrawRectangle(_penRect,image.Rect);
                        bmpGraphics.DrawString(image.Name, _fontName, _brushName, image.Rect.Left + 20, image.Rect.Top - 40);
                        foreach(var line in image.LstLines)
                        {
                            
                            bmpGraphics.DrawLine(_penRect, line.pStart,line.pEnd);
                            bmpGraphics.DrawString(line.Text, _fontName, _brushName,line.pStart.X, line.pStart.Y - 25);
                        }
                    } 
                    
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }               

            }

            //已经完成的连接线
            foreach(var line in _lstConnectorLines)
            {
                bmpGraphics.DrawLines(_penLine, line.LstPoints.ToArray());
            }
            //进行中的连接线
            if (_lstPointTemp.Count > 1)
            {
                bmpGraphics.DrawLines(_penLine, _lstPointTemp.ToArray());
                bmpGraphics.DrawLine(_penLine, _lstPointTemp.Last(), _pointNowInPanel);
            }
            else if(_lstPointTemp.Count > 0)
            {
                bmpGraphics.DrawLine(_penLine, _lstPointTemp.Last(), _pointNowInPanel);
            }

            #endregion



            #region 把bmpgraphics画到panelMain上

            Graphics g = panelMain.CreateGraphics();
            g.DrawImage(localBitmap, 0, 0);


            #endregion

            #region 清空对象

            localBitmap.Dispose();
            bmpGraphics.Dispose();
          //  g.Dispose();

            #endregion


            Thread.Sleep(50);
            
        }

        private void UpdateControllerImages()
        {
            foreach (var controller in _deviceHardware.Controllers)
            {
                try
                {
                    var image = _lstControllerImages.Find(t => t.Name == controller.Name);
                    if(image == null)
                    {
                        image = new ControllerImage();
                        _lstControllerImages.Add(image);
                    }                  
                     
                    image.Name = controller.Name;
                    image.Type = controller.Type;
                    var lst = controller.Rectangle.Split(',');
                    image.Rect = new Rectangle(int.Parse(lst[0]), int.Parse(lst[1]), int.Parse(lst[2]), int.Parse(lst[3]));
                    var lstVisibleLines = controller.Lines.ToList().FindAll(t => t.IsVisible.ToUpper().Contains("TRUE"));
                    var lineCount = lstVisibleLines.Count;
                    var lineCountRight = lineCount / 2 + lineCount % 2;
                    var lineDistance =image.Rect.Height / (lineCountRight + 1);
                    var lineLong = image.Rect.Width/3;

                    for (int i = 0; i < lineCount; i++)
                    {
                            
                        if (i < lineCountRight)
                        {
                            var lineRightY = image.Rect.Top + (i + 1) * lineDistance;
                            var left = image.Rect.Right;
                            image.LstLines.Add(new Line() { Text = lstVisibleLines[i].Name,pStart = new Point(left,lineRightY),pEnd=new Point(left+lineLong,lineRightY)  });
                            }
                        else
                        {
                            var lineLeftY = image.Rect.Top + (i + 1 - lineCountRight) * lineDistance;
                            var left = image.Rect.Left-lineLong;
                            image.LstLines.Add(new Line() { Text = lstVisibleLines[i].Name, pStart = new Point(left + lineLong, lineLeftY) , pEnd = new Point(left, lineLeftY) });
                        }
                    }           
      
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }


            }
        }


        #region panelMain大小位置控制

        private Rectangle RectOfPoint(Point p,int r)
        {
            return new Rectangle(p.X-r,p.Y-r,r*2,r*2);
        }

        //当鼠标位于控件矩形上时，鼠标变为手型，可以移动控件
        //当鼠标位于控件直线终点上时，鼠标变为十字，开始画直线，
        //鼠标单击左键，产生一条直线，当直线终点落在另外一条控件直线终点时，直线完成
        //当鼠标位于其他空白区域时，可以拖动画图区域移动
        private void PanelMain_MouseMove(object sender, MouseEventArgs e)
        {
            Point cursorPanelMain = panelMain.PointToClient(Cursor.Position);
            _pointNowInPanel.X = (int)(cursorPanelMain.X / _imageScale);
            _pointNowInPanel.Y =  (int)(cursorPanelMain.Y / _imageScale);
           // Rectangle rectPointNow = new Rectangle(_pointNowInPanel.X - 3, _pointNowInPanel.Y - 3, 6, 6);


      



            if (e.Button == MouseButtons.Left)
            {
                if(_isMovePanel)
                {
                    this.panelMain.Left = this.panelMain.Left + (Cursor.Position.X - _pointMouseDown.X);
                    this.panelMain.Top = this.panelMain.Top + (Cursor.Position.Y - _pointMouseDown.Y);
                    _pointMouseDown.X = Cursor.Position.X;
                    _pointMouseDown.Y = Cursor.Position.Y;
                }
                else if(_isMoveController)
                {
                   
                    foreach(var line in _controllerSelected.LstLines)
                    {
                        Point pstart = line.pStart;
                        Point pend = line.pEnd;
                         line.pStart.Offset(new Point(_pointNowInPanel.X - _controllerSelected.Rect.X, _pointNowInPanel.Y - _controllerSelected.Rect.Y));
                        line.pEnd.Offset(new Point(_pointNowInPanel.X - _controllerSelected.Rect.X, _pointNowInPanel.Y - _controllerSelected.Rect.Y));

                        foreach(var conline in _lstConnectorLines)
                        {
                             if (conline.LstPoints.First() == pend)
                            {
                                conline.LstPoints[0] = line.pEnd;
                            }                           
                            else if (conline.LstPoints.Last() == pend)
                            {
                                conline.LstPoints[conline.LstPoints.Count - 1] = line.pEnd;
                            }
                        }
                    }
                    _controllerSelected.Rect.X = _pointNowInPanel.X;
                    _controllerSelected.Rect.Y = _pointNowInPanel.Y;


                    //connector移动
                }            
                else if(_isMoveConnectPoint) //connector line point on left button
                {
                    foreach (var connector in _lstConnectorLines)
                    {
                        for (int i = 0; i < connector.LstPoints.Count; i++)
                        {
                            if (connector.LstPoints[i] == _pointConnectLineCross)
                            {
                                connector.LstPoints[i] = _pointNowInPanel;
                                _pointConnectLineCross = connector.LstPoints[i];
                                break;
                            }
                         //   break;
                        }
                    }
                }
                //更新界面
                PanelMain_Paint(null, null);
            }
            else
            {
                foreach (var c in _lstControllerImages)
                {
                    var line = c.LstLines.Find(t => RectOfPoint(_pointNowInPanel, 3).Contains(t.pEnd));
                    if (line != null)
                    {
                        Cursor = Cursors.Cross;
                        break;
                    }
                    else
                    {
                        if (!_isDrawConnectLine)
                            Cursor = Cursors.Default;
                    }
                }

                foreach (var connector in _lstConnectorLines)
                {
                    foreach (var point in connector.LstPoints)
                    {
                        Rectangle rect = new Rectangle(point.X - 2, point.Y - 2, 4, 4);
                        if (RectOfPoint(point, 3).Contains(_pointNowInPanel))
                        {
                            Cursor = Cursors.Hand;
                            break;
                        }
                        else
                        {
                            if (!_isMoveConnectPoint)
                            {
                                Cursor = Cursors.Default; ;
                            }
                        }
                    }

                }
            }

      //      PanelMain_Paint(null, null);

            Thread.Sleep(50);

        }

        private void ToolStripButtonZoomOut_Click(object sender, EventArgs e)
        {
            _panelMainScale /= 1.1;
            _panelMainWidth = (int)((double)_panelMainWidth * _panelMainScale);
            _panelMainHeight = (int)((double)_panelMainHeight * _panelMainScale);
            panelMain.Width = (int)((double)_panelMainWidth * _imageScale);
            panelMain.Height = (int)((double)_panelMainHeight * _imageScale);

        }

        private void ToolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            _panelMainScale *= 1.1;
            _panelMainWidth = (int)((double)_panelMainWidth * _panelMainScale);
            _panelMainHeight = (int)((double)_panelMainHeight * _panelMainScale);
            panelMain.Width = (int)((double)_panelMainWidth * _imageScale);
            panelMain.Height = (int)((double)_panelMainHeight * _imageScale);

        }

        private void PanelMain_MouseUp(object sender, MouseEventArgs e)
        {
            _isMovePanel = false;
            _isMoveController = false;
            _isMoveConnectPoint = false;
        }
        private void PanelMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if(!_isDrawConnectLine)
                {
                    foreach (var c in _lstControllerImages)
                    {
                        var line = c.LstLines.Find(t => RectOfPoint(_pointNowInPanel, 3).Contains(t.pEnd));
                        if (line != null)
                        {
                            _isDrawConnectLine = true;
                            _lstPointTemp.Add(line.pEnd);
                            break;
                        }
                    }
                    //如果不是在划线过程中，并且也不是点击在端部连接点
                    if(!_isDrawConnectLine)
                    {
                        // isMoveController
                        _isMoveController = false;
                        _isMovePanel = false;
                        _isMoveConnectPoint = false;
                        _controllerSelected = null;
                        //点击控制器框
                        foreach (var controller in _lstControllerImages)
                        {
                            if (controller.Rect.Contains(_pointNowInPanel))
                            {
                                _controllerSelected = controller;
                                _pointControllerSelectedOld.X = controller.Rect.Left;
                                _pointControllerSelectedOld.Y = controller.Rect.Top;
                                _isMoveController = true;
                                break;
                            }
                        }
                        //点击连接线连接点
                        foreach (var connector in _lstConnectorLines)
                        {
                            foreach (var point in connector.LstPoints)
                            {                                
                                Rectangle rect = new Rectangle(point.X - 2, point.Y - 2, 4, 4);
                                if (RectOfPoint(point, 3).Contains(_pointNowInPanel))
                                {
                                    _connectLineMovePoint = connector;
                                    _pointConnectLineCross = point;
                                    _pointConnectLineCrossOld = point;
                                    _isMoveConnectPoint = true;
                                    break;
                                }
                            }
                            if (_isMoveConnectPoint)
                            {
                                break;
                            }
                        }

                        if (!_isMoveConnectPoint && !_isMoveController)
                        {
                            _isMovePanel = true;
                            _pointMouseDown = Cursor.Position;
                        }

                    }
                }
                //如果在划线过程中，鼠标左键单击则把当前点增加到列表中
                else
                {
                    bool bFind = false;
                    foreach (var c in _lstControllerImages)
                    {
                        var line = c.LstLines.Find(t => RectOfPoint(_pointNowInPanel, 3).Contains(t.pEnd));
                        if (line != null)
                        {
                            _lstPointTemp.Add(line.pEnd);
                            bFind = true;
                            break;
                        }
                    }
                    if(!bFind)
                        _lstPointTemp.Add(_pointNowInPanel);
                }
             
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (_isDrawConnectLine)
                {
                    _isDrawConnectLine = false;

                    var conLast = _lstControllerImages.Find(t => t.LstLines.Find(tt => tt.pEnd == _lstPointTemp.Last()) != null);
                    if (conLast != null)
                    {
                        var conFirst = _lstControllerImages.Find(t => t.LstLines.Find(tt => tt.pEnd == _lstPointTemp.First()) != null);
                        if (conFirst != null)
                        {
                            ConnectorLine cline = new ConnectorLine();
                            cline.SourceLineName = conFirst.Name + "." + conFirst.LstLines.Find(t => t.pEnd == _lstPointTemp.First()).Text;
                            cline.TargetLineName = conLast.Name + "." + conLast.LstLines.Find(t => t.pEnd == _lstPointTemp.Last()).Text;
                            cline.Name = cline.SourceLineName + "_" + cline.TargetLineName;
                            cline.LstPoints.AddRange(_lstPointTemp);
                            _lstConnectorLines.Add(cline);
                        }
                    }
                    _lstPointTemp.Clear();



                    //    //add
                    //    ConnectorLine cLine = new ConnectorLine();
                    //    foreach (var c in _lstControllerImages)
                    //    {
                    //        var line = c.LstLines.Find(t => t.pEnd == _lstPointTemp[0]);
                    //        if (line != null)
                    //        {
                    //            cLine.SourceLineName = c.Name + "." + line.Text;
                    //        }
                    //        line = c.LstLines.Find(t => t.pEnd == _lstPointTemp[_lstPointTemp.Count - 1]);
                    //        if (line != null)
                    //        {
                    //            cLine.TargetLineName = c.Name + "." + line.Text;
                    //        }
                    //    }
                    //    if (cLine.SourceLineName != null && cLine.TargetLineName != null)
                    //    {
                    //        cLine.Name = cLine.SourceLineName + "_" + cLine.TargetLineName;
                    //        cLine.LstPoints.AddRange(_lstPointTemp);
                    //        _lstConnectorLines.Add(cLine);
                    //    }
                    //    _lstPointTemp.Clear();
                    //}
                }
            }

            PanelMain_Paint(null, null);

        }

        private void PanelMain_MouseDown1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _pointMouseDown.X = Cursor.Position.X;  //注：全局变量mouseDownPoint前面已定义为Point类型
                _pointMouseDown.Y = Cursor.Position.Y;
                //       isSelected = true;

                if(!_isDrawConnectLine)
                {
                    if(_bPointCatched)
                    {
                        _isDrawConnectLine = true;
                        _lstPointTemp.Add(_pointCatched);       
                    }
                }
                else
                {
                    if (_bPointCatched)
                    {
                        _lstPointTemp.Add(_pointCatched);
                    }
                    else
                    {
                        Point p = panelMain.PointToClient(Cursor.Position);
                        p.X = (int)((double)p.X/ _imageScale);
                        p.Y = (int)((double)p.Y / _imageScale);
                        _lstPointTemp.Add(p);
                    }
                }
      
            }
            else if(e.Button == MouseButtons.Right)
            {
                if (_isDrawConnectLine)
                {
                    _isDrawConnectLine = false;
                    //add
                    ConnectorLine cLine = new ConnectorLine();
                    foreach (var c in _lstControllerImages)
                    {
                        var line = c.LstLines.Find(t =>t.pEnd==_lstPointTemp[0]);
                        if (line != null)
                        {
                            cLine.SourceLineName = c.Name + "." + line.Text;
                        }
                        line = c.LstLines.Find(t => t.pEnd == _lstPointTemp[_lstPointTemp.Count-1]);
                        if(line != null)
                        {
                            cLine.TargetLineName = c.Name + "." + line.Text;
                        }
                    }
                    if(cLine.SourceLineName !=null && cLine.TargetLineName != null)
                    {
                        cLine.Name = cLine.SourceLineName + "_" + cLine.TargetLineName;
                        cLine.LstPoints.AddRange(_lstPointTemp);
                        _lstConnectorLines.Add(cLine);
                    }
                    _lstPointTemp.Clear();
                }

            }

            PanelMain_Paint(null, null);

        }

        private void PanelMain_MouseWheel(object sender, MouseEventArgs e)
        {
            // MessageBox.Show("滚动事件已被捕捉");


            System.Drawing.Size t = panelMain.Size;
            if (e.Delta > 0)
            {
                if (_imageScale > 4) return;
                _imageScale *= 1.1;
                this.panelMain.Left = this.panelMain.Left - t.Width / 20;
                this.panelMain.Top = this.panelMain.Top - t.Height / 20;
            }
            else
            {
                if (_imageScale < 0.25) return;
                _imageScale /= 1.1;
                this.panelMain.Left = this.panelMain.Left + t.Width / 20;
                this.panelMain.Top = this.panelMain.Top + t.Height / 20;
            }
            panelMain.Width = (int)((double)_panelMainWidth * _imageScale);
            panelMain.Height = (int)((double)_panelMainHeight * _imageScale);

            PanelMain_Paint(null, null);




        }

        private bool IsMouseInPanel()
        {
            if (this.panelMain.Left < PointToClient(Cursor.Position).X
            && PointToClient(Cursor.Position).X < this.panelMain.Left + this.panelMain.Width
            && this.panelMain.Top < PointToClient(Cursor.Position).Y
            && PointToClient(Cursor.Position).Y < this.panelMain.Top + this.panelMain.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        private void addStatusUnitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }

    public class Line
    {
        public Point pStart;
        public Point pEnd;
        public string Text;
    }

    public class ControllerImage
    {
        public string Name;
        public string Type;
        public Rectangle Rect;
        public List<Line> LstLines;
       // public List<Point> LstEndPoints;

        public ControllerImage()
        {
            Rect = new Rectangle(100, 100, 100, 200);
            LstLines = new List<Line>();
         //   LstEndPoints = new List<Point>();
        }
    }

    public class ConnectorLine
    {
        public string Name;
        public string SourceLineName;
        public string TargetLineName;
        public List<Point> LstPoints;

        public ConnectorLine()
        {
            LstPoints = new List<Point>();
        }
    }

}
