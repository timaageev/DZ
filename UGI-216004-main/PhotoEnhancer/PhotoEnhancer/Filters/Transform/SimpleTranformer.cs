using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEnhancer
{
    public class SimpleTranformer : ITransformer<EmptyParameters>
    {
        Func<Size, Size> sizeTransform;
        Func<Point, Size, Point> pointTransform;

        public SimpleTranformer(Func<Size, Size> sizeTransform, 
            Func<Point, Size, Point> pointTransform)
        {
            this.sizeTransform = sizeTransform;
            this.pointTransform = pointTransform;
        }

        public Size ResultSize { get; private set; }

        Size oldSize;

        public void Initialize(Size size, EmptyParameters parameters)
        {
            oldSize = size;
            ResultSize = sizeTransform(oldSize);
        }

        public Point? MapPoint(Point newPoint)
        {
            return pointTransform(newPoint, oldSize);
        }
    }
}
