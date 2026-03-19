using System.Collections.Generic;
using System.Linq;

namespace UserControls.CvDisplaySrc
{
    public class CvDisplayGraphicsShapeCollection : List<CvDisplayGraphicsShape>
    {
        public CvDisplayGraphicsShapeCollection()
            : this(null) { }

        public CvDisplayGraphicsShapeCollection(CvDisplayGraphicsMat gMat)
        {
            ParentMat = gMat;
        }

        private CvDisplayGraphicsMat _parentMat;

        public CvDisplayGraphicsMat ParentMat
        {
            get
            {
                return _parentMat;
            }
            set
            {
                _parentMat = value;
                foreach (var shape in this)
                {
                    shape.ParentMat = ParentMat;
                }
            }
        }

        public new void Add(CvDisplayGraphicsShape shape)
        {
            base.Add(shape);
            shape.ParentMat = ParentMat;
        }

        public new void AddRange(IEnumerable<CvDisplayGraphicsShape> range)
        {
            var cvDisplayGraphicsShapes = range as CvDisplayGraphicsShape[] ?? range.ToArray();
            base.AddRange(cvDisplayGraphicsShapes);
            foreach (var shape in cvDisplayGraphicsShapes)
                shape.ParentMat = ParentMat;
        }

        public new void Insert(int index, CvDisplayGraphicsShape shape)
        {
            base.Insert(index, shape);
            shape.ParentMat = ParentMat;
        }

        public new void InsertRange(int index, IEnumerable<CvDisplayGraphicsShape> range)
        {
            var cvDisplayGraphicsShapes = range as CvDisplayGraphicsShape[] ?? range.ToArray();
            base.InsertRange(index, cvDisplayGraphicsShapes);
            foreach (var shape in cvDisplayGraphicsShapes)
                shape.ParentMat = ParentMat;
        }

        public new void Clear()
        {
            foreach (var obj in this)
            {
                obj.Dispose();
            }
            base.Clear();
        }
    }
}
