using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace UserControls.CvDisplaySrc
{
    sealed class CvDisplayGraphicsRectangle : CvDisplayGraphicsDot
    {

        private readonly CvDisplayGraphicsLineSegment _ab;
        private readonly CvDisplayGraphicsLineSegment _bc;
        private readonly CvDisplayGraphicsLineSegment _cd;
        private readonly CvDisplayGraphicsLineSegment _ad;

        public override CvDisplayGraphicsMat ParentMat
        {
            get { return base.ParentMat; }
            set
            {
                base.ParentMat = value;
                if (_ab != null)
                    _ab.ParentMat = value;
                if (_bc != null)
                    _bc.ParentMat = value;
                if (_cd != null)
                    _cd.ParentMat = value;
                if (_ad != null)
                    _ad.ParentMat = value;
            }
        }

        public CvDisplayGraphicsRectangle(Point2d center, Size2d size, double angle)
        {
            Location = center;
            RectSize = size;
            Angle = angle;

            _ab = new CvDisplayGraphicsLineSegment();
            _bc = new CvDisplayGraphicsLineSegment();
            _cd = new CvDisplayGraphicsLineSegment();
            _ad = new CvDisplayGraphicsLineSegment();
        }

        private double _Angle;

        public double Angle
        {
            get
            {
                return _Angle;
            }
            set
            {
                _Angle = value;
            }
        }

        public Size2d RectSize;

        private enum SelectedType
        {
            None,
            Center,
            WidthEdge,
            HeightEdge,
            Rotate
        }

        private SelectedType _SelectedType = SelectedType.None;

        public double Width
        {
            get
            {
                return RectSize.Width;
            }
            set
            {
                RectSize.Width = value;
            }
        }

        public double Height
        {
            get
            {
                return RectSize.Height;
            }
            set
            {
                RectSize.Height = value;
            }
        }

        public override bool IsMouseIn(PointF pos)
        {
            var bCenter = base.IsMouseIn(pos);

            _SelectedType = SelectedType.None;

            if (bCenter)
            {
                _SelectedType = SelectedType.Center;
            }
            else
            {
                var points = GetPoints(false);

                _ab.Start = points[0];
                _ab.End = points[1];

                _bc.Start = points[1];
                _bc.End = points[2];

                _cd.Start = points[2];
                _cd.End = points[3];

                _ad.Start = points[3];
                _ad.End = points[0];

                if (new CvDisplayGraphicsDot(_bc.Mid) { ParentMat = ParentMat }.IsMouseIn(pos))
                {
                    _SelectedType = SelectedType.Rotate;
                }
                else if (_ab.IsMouseIn(pos) || _cd.IsMouseIn(pos))
                {
                    _SelectedType = SelectedType.WidthEdge;
                }
                else if (_bc.IsMouseIn(pos) || _ad.IsMouseIn(pos))
                {
                    _SelectedType = SelectedType.HeightEdge;
                }
            }

            return _SelectedType != SelectedType.None;
        }


        public override void OnMouseMove(MouseEventArgs e)
        {
            if (IsLeftMouseDown)
            {
                double xmove = e.X - MouseDownPos.X, ymove = e.Y - MouseDownPos.Y;
                if (_SelectedType == SelectedType.HeightEdge)
                {
                    Height += ymove / PixelSize.Height;
                    MouseDownPos = e.Location;
                }
                else if (_SelectedType == SelectedType.WidthEdge)
                {
                    Width += xmove / PixelSize.Width;
                    MouseDownPos = e.Location;
                }
                else if (_SelectedType == SelectedType.Rotate)
                {
                    var center = TranslateToScreenPos(Location);
                    CvDisplayGraphicsLineSegment line1 = new CvDisplayGraphicsLineSegment(center, new Point2d(MouseDownPos.X, MouseDownPos.Y)),
                        line2 = new CvDisplayGraphicsLineSegment(center, new Point2d(e.X, e.Y));
                    Angle += Calculate.Angle(line1.K, line2.K);
                    Console.WriteLine(Angle);
                    MouseDownPos = e.Location;
                }
                else
                    base.OnMouseMove(e);
            }

        }

        Point2d[] GetPoints(bool isSreen)
        {
            var ps = new List<Point2d>();
            foreach (var p in new RotatedRect(new Point2f((float)X, (float)Y),
                new Size2f((float)Width, (float)Height), (float)Angle).Points())
            {
                var sp = isSreen ? TranslateToScreenPos(new Point2d(p.X, p.Y)) :
                    new Point2d(p.X, p.Y);
                ps.Add(sp);
            }
            return ps.ToArray();
        }

        public override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var ps = new List<PointF>();
            foreach (var p in new RotatedRect(new Point2f((float)X, (float)Y),
                new Size2f((float)Width, (float)Height), (float)Angle).Points())
            {
                var sp = TranslateToScreenPos(new Point2d(p.X, p.Y));
                ps.Add(new PointF((float)sp.X, (float)sp.Y));
            }
            var pen = new Pen(GetDrawColor(), GetDrawSize());
            e.Graphics.DrawPolygon(pen, ps.ToArray());

            if (_SelectedType != SelectedType.None)
            {
                DrawCircle(e.Graphics, new Pen(Color.Green, 3), new PointF((ps[2].X + ps[1].X) / 2, (ps[2].Y + ps[1].Y) / 2), 5);
            }
        }
    }
}
