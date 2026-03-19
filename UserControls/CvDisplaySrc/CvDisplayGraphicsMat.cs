using OpenCvSharp;
using System;
using System.Drawing;
using System.Windows.Forms;
using CvPoint = OpenCvSharp.Point;
using CvSize = OpenCvSharp.Size;
using SdPoint = System.Drawing.Point;

namespace UserControls.CvDisplaySrc
{
    /// <summary>
    /// 需要绘制的Mat对象
    /// </summary>
    public class CvDisplayGraphicsMat : CvDisplayGraphicsObject
    {
        protected Mat _Image = null;
        public Mat Image
        {
            get
            {
                return _Image;
            }
            set
            {
                if (_Image != null)
                {
                    _Image.Dispose();
                    _Image = null;
                }
                if (value != null)
                    _Image = new Mat(value, new Rect(0, 0, value.Width, value.Height));
                Reset();
            }
        }

        /// <summary>
        /// 实际显示在屏幕区域内的ROI
        /// </summary>
        public Rect2d DispRect
        {
            get
            {
                return new Rect2d(DisplayOrigin, _displaySize);
            }
        }

        /// <summary>
        /// 实际显示在屏幕可见区域内的图片ROI
        /// </summary>
        public Rect ShowMatRect { get; protected set; }

        /// <summary>
        /// 单像素在屏幕中显示的大小
        /// </summary>
        public override Size2d PixelSize
        {
            get
            {
                return _pixelSize;

            }
            set
            {
                _pixelSize = value;
                if (Image == null)
                    _displaySize = new Size2d(0, 0);
                else
                    _displaySize = new Size2d(
                        Image.Width * _pixelSize.Width, Image.Height * _pixelSize.Height
                        );

            }
        }

        protected Size2d _displaySize;

        /// <summary>
        /// 整张图片需要显示在屏幕中的大小
        /// </summary>
        public Size2d DisplaySize
        {
            get
            {
                return _displaySize;
            }
        }


        public CvDisplayGraphicsShapeCollection GraphicsShapes
        {
            get; protected set;
        }


        public CvDisplayGraphicsMat()
        {
            PixelSize = new Size2d(1, 1);

            GraphicsShapes = new CvDisplayGraphicsShapeCollection(this);
        }



        #region override


        public override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }
        public override void Reset()
        {
            base.Reset();
            PixelSize = new Size2d(1, 1);

        }
        public override void Dispose()
        {
            if (_Image != null)
            {
                _Image.Dispose();
            }
            base.Dispose();

        }

        public override void OnPaint(PaintEventArgs e)
        {
            if (Image != null)
            {
                try
                {
                    var showMatRect = new Rect(); //需要裁减的图片范围
                    var drawImageStartPos = new PointF(); //绘制showMatRect的起始点
                    if (DispRect.X < 0)
                    {
                        //显示区域的起始点X不在屏幕内
                        showMatRect.X = (int)(Math.Abs(DispRect.X) / PixelSize.Width);
                        drawImageStartPos.X = (float)(showMatRect.X * PixelSize.Width + DispRect.X);
                    }
                    else
                    {
                        showMatRect.X = 0;
                        drawImageStartPos.X = (float)DispRect.X;
                    }
                    showMatRect.Width = (int)((e.ClipRectangle.Width - drawImageStartPos.X) / PixelSize.Width) + 1;

                    if (DispRect.Y < 0)
                    {
                        //显示区域的起始点Y不在屏幕内
                        showMatRect.Y = (int)(Math.Abs(DispRect.Y) / PixelSize.Height);
                        drawImageStartPos.Y = (float)(showMatRect.Y * PixelSize.Height + DispRect.Y);
                    }
                    else
                    {
                        showMatRect.Y = 0;
                        drawImageStartPos.Y = (float)DispRect.Y;
                    }
                    showMatRect.Height = (int)((e.ClipRectangle.Height - drawImageStartPos.Y) / PixelSize.Height) + 1;


                    AdjustMatRect(Image, ref showMatRect);//调整需要显示Mat区域，以免截取的区域超出图片范围

                    using (var displayMat = new Mat(Image, showMatRect))
                    {
                        //计算截取区域需要显示在屏幕中的大小
                        var drawSize = new CvSize((int)(displayMat.Width * PixelSize.Width), (int)(displayMat.Height * PixelSize.Height));

                        if (drawSize.Width < 1) drawSize.Width = 1;
                        if (drawSize.Height < 1) drawSize.Height = 1;

                        using (var resizeMat = new Mat())
                        {
                            //以Nearest的方式缩放图片尺寸
                            Cv2.Resize(displayMat, resizeMat, drawSize, 0, 0, InterpolationFlags.Nearest);

                            //缩放完的图片直接画在控件上
                            using (Image drawImage = OpenCvSharp.Extensions.BitmapConverter.ToBitmap(resizeMat))
                            {
                                e.Graphics.DrawImage(drawImage, drawImageStartPos);
                                drawImage.Dispose();
                            }

                            resizeMat.Dispose();
                        }

                        displayMat.Dispose();
                    }
                    ShowMatRect = showMatRect;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            foreach (var shape in GraphicsShapes)
            {
                shape.OnPaint(e);
            }
        }

        public override bool IsMouseIn(PointF pos)
        {
            return DispRect.Contains(pos.X, pos.Y);
        }

        #endregion

        #region public method


        /// <summary>
        /// 转换屏幕坐标为图片中的像素坐标
        /// </summary>
        /// <param name="pos">屏幕坐标</param>
        /// <returns></returns>
        public CvPoint TransformPixelPostion(SdPoint pos)
        {
            var res = new CvPoint(-1, -1);
            if (IsMouseIn(pos))
            {
                res.X = (int)((pos.X - DispRect.X) / PixelSize.Width);
                res.Y = (int)((pos.Y - DispRect.Y) / PixelSize.Height);
            }
            return res;
        }
        #endregion

        #region protected method

        /// <summary>
        /// 调整显示的图片区域，以免截取的mat越界
        /// </summary>
        /// <param name="mt"></param>
        /// <param name="rect"></param>
        protected void AdjustMatRect(Mat mt, ref Rect rect)
        {
            //调整XY坐标
            if (rect.X < 0)
                rect.X = 0;
            if (rect.X >= mt.Width)
                rect.X = mt.Width - 1;
            if (rect.Y < 0)
                rect.Y = 0;
            if (rect.Y >= mt.Height)
                rect.Y = mt.Height - 1;

            //调整长宽
            if (rect.Width + rect.X > mt.Width)
                rect.Width = mt.Width - rect.X;
            if (rect.Height + rect.Y > mt.Height)
                rect.Height = mt.Height - rect.Y;
        }
        #endregion
    }
}
