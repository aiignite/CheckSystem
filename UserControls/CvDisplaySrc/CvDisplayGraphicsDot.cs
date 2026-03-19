using OpenCvSharp;
using System.Drawing;
using System.Windows.Forms;

namespace UserControls.CvDisplaySrc
{
    public class CvDisplayGraphicsDot : CvDisplayGraphicsShape
    {
        public Point2d _Location;

        public virtual Point2d Location
        {
            get
            {
                return _Location;
            }
            set
            {
                _Location = value;
                DisplayOrigin = TranslateToScreenPos(new Point2d(value.X, value.Y));
              
            }
        }
        

        public double X
        {
            get
            {
                return Location.X;
            }
            set
            {
                Location = new Point2d(value, Y); 
            }
        }


        public double Y
        {
            get
            {
                return Location.Y;
            }
            set
            {
                Location = new Point2d(X,value);
            }
        }

        protected double _focusRange = 5;


        public override Point2d DisplayOrigin
        {
            get
            {
                return TranslateToScreenPos(Location);
            }
            set
            {
                base.DisplayOrigin = value;
            }
        }

        public CvDisplayGraphicsDot():this(-1,-1)
        {

        }

        public CvDisplayGraphicsDot(double x,double y)
        {
           
            Location = new Point2d(x, y);
        }

        public CvDisplayGraphicsDot(Point2d p)
        {
            Location = p;
        }

        protected override void OnDisplayOriginChanged(Point2d oldPos, Point2d newPos)
        {
            _Location = TranslateToMatPos(newPos);
            base.OnDisplayOriginChanged(oldPos, newPos);
        }

        public override bool IsMouseIn(PointF pos)
        {
            return new Rect2d(DisplayOrigin.X - _focusRange, DisplayOrigin.Y - _focusRange,
                _focusRange * 2, _focusRange * 2).Contains(pos.X,pos.Y);
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if(Selected && IsLeftMouseDown)
            {
                DisplayOrigin += new Point2d( e.X - MouseDownPos.X,
                    e.Y - MouseDownPos.Y);
                MouseDownPos = e.Location;

            }
        }
        public override void OnPaint(PaintEventArgs e)
        {
            Size2d pixelSize = PixelSize;

            double dispX =  MatDisplayOrigin.X + X * pixelSize.Width;
            double dispY = MatDisplayOrigin.Y + Y * pixelSize.Height;
            if (new Rect2d(0,0,e.ClipRectangle.Width,e.ClipRectangle.Height).Contains(dispX, dispY))
            {
                DrawCross(e.Graphics, new PointF((float)dispX, (float)dispY),
                  IsFocused || Selected? FocusedColor : Color);
            }
        }


        
    }
}
