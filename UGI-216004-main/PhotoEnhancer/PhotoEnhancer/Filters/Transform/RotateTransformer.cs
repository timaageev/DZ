using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoEnhancer
{
    public class RotateTransformer : ITransformer<RotationParameters>
    {
        Size oldSize { get; set; }
        double angleInRadians { get; set; }

        public Size ResultSize { get; private set; }

        public void Initialize(Size size, RotationParameters parameters)
        {
            oldSize = size;
            angleInRadians = parameters.AngleInDegrees * Math.PI / 180;
            ResultSize = new Size(
                    (int)(size.Width * Math.Abs(Math.Cos(angleInRadians)) +
                        size.Height * Math.Abs(Math.Sin(angleInRadians))),
                    (int)(size.Width * Math.Abs(Math.Sin(angleInRadians)) +
                        size.Height * Math.Abs(Math.Cos(angleInRadians))));
        }

        public Point? MapPoint(Point point)
        {
            point = new Point(point.X - ResultSize.Width / 2, point.Y - ResultSize.Height / 2);
            var x = (int)(point.X * Math.Cos(angleInRadians) - point.Y * Math.Sin(angleInRadians) + oldSize.Width / 2);
            var y = (int)(point.X * Math.Sin(angleInRadians) + point.Y * Math.Cos(angleInRadians) + oldSize.Height / 2);

            if (x < 0 || x >= oldSize.Width || y < 0 || y >= oldSize.Height)
                return null;

            return new Point(x, y);
        }
    }
}
