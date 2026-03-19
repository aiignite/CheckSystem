using OpenCvSharp;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Cursors = System.Windows.Forms.Cursors;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;
using Point = OpenCvSharp.Point;

namespace UserControls.CvDisplaySrc
{
    internal sealed class CvDisplay : PictureBox
    {
        #region 内部操作数据

        /// <summary>
        /// Mat绘制类
        /// </summary>
        private readonly CvDisplayGraphicsMat _cdgMat;

        /// <summary>
        /// 鼠标是否允许移动
        /// </summary>
        private bool _isMouseMoving;

        ///// <summary>
        ///// 鼠标按下类型
        ///// 0=非左，非右
        ///// 1=左
        ///// 2=右
        ///// </summary>
        //private int _mouseDownType;

        /// <summary>
        /// 鼠标点下的坐标
        /// </summary>
        private Point _mouseDownLocation;

        /// <summary>
        /// 鼠标实时位置
        /// </summary>
        private System.Drawing.Point _mouseLocation;

        /// <summary>
        /// 鼠标放置位置的像素实际坐标
        /// </summary>
        private Point _mousePixcelLocation;

        #endregion

        #region 事件

        /// <summary>
        /// 当前像元位置变化
        /// </summary>
        public event EventHandler<PosChangedEventArgs> PositionChanged;

        public event EventHandler<MouseActionEventArgs> MouseAction;

        #endregion

        #region 公开属性

        public CvDisplayGraphicsMat GraphicsMat
        {
            get
            {
                return _cdgMat;
            }
        }

        public enum AutoDisplayMode
        {
            Original,
            Fit,
            Full
        }

        /// <summary>
        /// 绘图元素集合
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public CvDisplayGraphicsShapeCollection GraphicsShapes
        {
            get
            {
                return GraphicsMat.GraphicsShapes;
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("CvDisplay"), Description("自动显示图片模式")]
        public AutoDisplayMode AutoDisplay
        {
            get;
            set;
        }

        [EditorBrowsable(EditorBrowsableState.Always)]
        [Category("CvDisplay"), Description("OpenCv2 Mat图片数据类")]
        public new Mat Image
        {
            get
            {
                return _cdgMat.Image;
            }
            set
            {
                _cdgMat.Image = value;
                ImageResize();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Image BackgroundImage
        {
            get
            {
                return base.BackgroundImage;
            }
            set
            {
                base.BackgroundImage = null;
            }
        }

        #endregion

        public CvDisplay()
        {
            _cdgMat = new CvDisplayGraphicsMat();
            DoubleBuffered = true;

            AutoDisplay = AutoDisplayMode.Original;

            ContextMenuStrip = new ContextMenuStrip();
            //ContextMenuStrip.Items.Add("Fit image", null, OnFitImageClick);
            //ContextMenuStrip.Items.Add("Original image", null, OnOriginalImageClick);
            //ContextMenuStrip.Items.Add("Full image", null, OnFullImageClick);
            //ContextMenuStrip.Items.Add("Save as", null, OnSaveAsClick);
        }

        #region 事件处理

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            Fit();
        }

        public void OnFitImageClick(object sender, EventArgs e)
        {
            Fit();
        }

        public void OnOriginalImageClick(object sender, EventArgs e)
        {
            OriginalSize();
        }

        public void OnFullImageClick(object sender, EventArgs e)
        {
            Full();
        }

        public void OnSaveAsClick(object sender, EventArgs e)
        {
            if (Image == null) return;
            using (var ofd = new SaveFileDialog())
            {
                ofd.Filter = @"Bitmap|*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    SaveAs(ofd.FileName);
                }
            }
        }

        #endregion

        #region 父类重载
        protected override void OnMouseDown(MouseEventArgs e)
        {
            var shapeMove = false;
            foreach (var shape in GraphicsShapes)
            {
                shape.OnMouseDown(e);
                shapeMove |= shape.Selected;
            }

            if (e.Button == MouseButtons.Right && !shapeMove)
            {
                Cursor = Cursors.SizeAll;
                _isMouseMoving = true;
                _mouseDownLocation = new Point(e.Location.X, e.Location.Y);
            }

            if (_cdgMat.IsMouseIn(e.Location))
            {
                if (MouseAction != null)
                    MouseAction(this, new MouseActionEventArgs(_cdgMat.TransformPixelPostion(e.Location), e.Button));
            }
            else
            {
                if (MouseAction != null)
                    MouseAction(this, new MouseActionEventArgs(new Point(-1, -1), MouseButtons.None));
            }

            Refresh();
            base.OnMouseDown(e);
        }

        private void ImageResize()
        {
            switch (AutoDisplay)
            {
                case AutoDisplayMode.Original:
                    OriginalSize();
                    break;
                case AutoDisplayMode.Fit:
                    Fit();
                    break;
                case AutoDisplayMode.Full:
                    Full();
                    break;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            if (Width != 0 && Height != 0)
            {
                ImageResize();
            }

            base.OnResize(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            foreach (var shape in GraphicsShapes)
                shape.OnMouseUp(e);
            Cursor = Cursors.Default;
            _isMouseMoving = false;
            base.OnMouseUp(e);
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                Zoom(2, 2, new PointF(e.X, e.Y));
            }
            else
            {
                Zoom(0.5, 0.5, new PointF(e.X, e.Y));
            }
            base.OnMouseWheel(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            _mouseLocation = e.Location;

            foreach (var shape in GraphicsShapes)
                shape.OnMouseMove(e);

            if (_isMouseMoving && Image != null)
            {
                //移动图片
                var nowLocation = new Point(e.X, e.Y);
                var move = nowLocation - _mouseDownLocation;

                SyncUpdateOrigin(_cdgMat.DisplayOrigin + move);

                //Refresh();
                _mouseDownLocation = nowLocation;
            }
            else if (_cdgMat.IsMouseIn(e.Location))
            {
                //坐标在绘图区域内
                //记录实际像素点和颜色 ，提示在tooltip上
                Cursor = Cursors.Cross;

                var p = _cdgMat.TransformPixelPostion(e.Location);
                if (!p.Equals(_mousePixcelLocation))
                {
                    var tip = string.Format("({0},{1})", p.X, p.Y);
                    object[] res;
                    MatHelper.GetMatChannelValues(Image, p.X, p.Y, out res);
                    tip += " [";
                    tip = res.Aggregate(tip, (current, obj) => current + obj + ",");
                    tip = tip.Substring(0, tip.Length - 1) + ']';

                    //Console.WriteLine(tip);

                    if (PositionChanged != null)
                        PositionChanged(this, new PosChangedEventArgs(p, res));
                }

                _mousePixcelLocation = p;
            }
            else
            {
                //坐标不在绘图区域内
                _mousePixcelLocation = new Point(-1, -1);
            }

            Refresh();
            base.OnMouseMove(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            var gh = e.Graphics;
            gh.Clear(BackColor);
            if (Image != null)
            {
                _cdgMat.OnPaint(e);
            }
        }

        #endregion

        #region 内部使用函数

        /// <summary>
        /// 同步更新所有绘图的原点
        /// </summary>
        /// <param name="p"></param>
        private void SyncUpdateOrigin(Point2d p)
        {
            _cdgMat.DisplayOrigin = p;
        }

        private static System.Drawing.Point ConvertCvPoint2DrawingPoint(Point p)
        {
            return new System.Drawing.Point(p.X, p.Y);
        }

        private static Point ConvertDrawingPoint2CvPoint(System.Drawing.Point p)
        {
            return new Point(p.X, p.Y);
        }

        #endregion

        #region 对外接口

        /// <summary>
        /// 图片缩放
        /// </summary>
        /// <param name="scale">x,y等比例缩放参数</param>
        public void Zoom(double scale)
        {
            Zoom(scale, scale);
        }

        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="filepath"></param>
        public void SaveAs(string filepath)
        {
            if (Image == null) return;
            Cv2.ImWrite(filepath, Image);
        }

        /// <summary>
        /// 图片缩放
        /// </summary>
        /// <param name="xScale">x缩放参数</param>
        /// <param name="yScale">y缩放参数</param>
        public void Zoom(double xScale, double yScale)
        {
            Zoom(xScale, yScale, new PointF(0, 0));
        }

        /// <summary>
        /// 根据某个原点进行缩放
        /// </summary>
        /// <param name="xScale">x缩放参数</param>
        /// <param name="yScale">y缩放参数</param>
        /// <param name="zoomOrign">缩放参考点</param>
        public void Zoom(double xScale, double yScale, PointF zoomOrign)
        {
            if (Image == null) return;

            var newXPixelSize = Math.Abs(xScale) * _cdgMat.PixelSize.Width;
            var newYPixelSize = Math.Abs(yScale) * _cdgMat.PixelSize.Height;
            if (newXPixelSize > 0 && newYPixelSize > 0)
            {
                int dispPixelX = (int)(Width / newXPixelSize),
                    dispPixelY = (int)(Height / newYPixelSize);
                if (dispPixelX < 1 || dispPixelY < 1) //最少显示一个像素点
                    return;

                if (_cdgMat.IsMouseIn(zoomOrign)) //如果在聚焦在图片某点放大
                {
                    //变换前 图片绘制坐标原点距离 当前鼠标鼠标的距离
                    double disX = zoomOrign.X - _cdgMat.DisplayOrigin.X,
                        disY = zoomOrign.Y - _cdgMat.DisplayOrigin.Y;

                    //缩放后的距离
                    disX *= xScale;
                    disY *= yScale;

                    //同步更新所有需要绘图的元素的原点
                    SyncUpdateOrigin(new Point2d(zoomOrign.X - disX, zoomOrign.Y - disY));
                }
                _cdgMat.PixelSize = new Size2d(newXPixelSize, newYPixelSize);
                Refresh();
            }
        }

        /// <summary>
        /// 整个图片充满控件
        /// </summary>
        public void Full()
        {
            if (Image == null) return;
            //换算单个像素尺寸
            _cdgMat.PixelSize = new Size2d(Width / (double)Image.Width, Height / (double)Image.Height);

            _cdgMat.DisplayOrigin = new Point2d(0, 0);

            Refresh();
        }

        /// <summary>
        /// 自适应图片的横纵比最大化
        /// </summary>
        public void Fit()
        {
            if (Image == null) return;
            var newsize = new Size2d();
            double hvScale1 = Width / (double)Height,//控件横纵比
            hvScale2 = Image.Width / (double)Image.Height;//图片横纵比

            //根据横纵比算出实际上画图的大小
            if (hvScale1 > hvScale2)
            {
                newsize.Height = Height;
                newsize.Width = Image.Width * (newsize.Height / Image.Height);
            }
            else
            {
                newsize.Width = Width;
                newsize.Height = Image.Height * (newsize.Width / Image.Width);
            }

            //计算单像素尺寸
            _cdgMat.PixelSize = new Size2d(newsize.Width / Image.Width, newsize.Height / Image.Height);

            SyncUpdateOrigin(new Point2d((Width - _cdgMat.DispRect.Width) / 2,
                (Height - _cdgMat.DispRect.Height) / 2));

            Refresh();
        }

        /// <summary>
        /// 恢复图片原始比例
        /// </summary>
        public void OriginalSize()
        {
            if (Image == null) return;
            _cdgMat.PixelSize = new Size2d(1, 1);
            SyncUpdateOrigin(new Point2d(0, 0));

            Refresh();
        }

        public void Clear()
        {
            Image = null;
            GraphicsShapes.Clear();
            Refresh();
        }
        #endregion
    }
}
