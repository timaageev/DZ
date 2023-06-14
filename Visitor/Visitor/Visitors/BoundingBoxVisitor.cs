using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
    public class BoundingBoxVisitor : IVisitor
    {
        public RectangularCuboid BoundingBox { get; private set; }

        public void Visit(Ball ball)
        {
            var size = 2 * ball.Radius;
            var position = ball.Position - new Vector3D(ball.Radius, ball.Radius, ball.Radius);
            BoundingBox = new RectangularCuboid(position, size, size, size);
        }

        public void Visit(Cylinder cylinder)
        {
            var sizeX = 2 * cylinder.Radius;
            var sizeY = 2 * cylinder.Radius;
            var sizeZ = cylinder.SizeZ;
            var position = cylinder.Position - new Vector3D(cylinder.Radius, cylinder.Radius, 0);
            BoundingBox = new RectangularCuboid(position, sizeX, sizeY, sizeZ);
        }

        public void Visit(RectangularCuboid cuboid)
        {
            BoundingBox = cuboid;
        }

        public void Visit(CompoundBody compoundBody)
        {
            double minX = double.MaxValue;
            double minY = double.MaxValue;
            double minZ = double.MaxValue;
            double maxX = double.MinValue;
            double maxY = double.MinValue;
            double maxZ = double.MinValue;

            foreach (var part in compoundBody.Parts)
            {
                var partVisitor = new BoundingBoxVisitor();
                part.Accept(partVisitor);
                var partBox = partVisitor.BoundingBox;

                minX = Math.Min(minX, partBox.Position.X);
                minY = Math.Min(minY, partBox.Position.Y);
                minZ = Math.Min(minZ, partBox.Position.Z);
                maxX = Math.Max(maxX, partBox.Position.X + partBox.SizeX);
                maxY = Math.Max(maxY, partBox.Position.Y + partBox.SizeY);
                maxZ = Math.Max(maxZ, partBox.Position.Z + partBox.SizeZ);
            }

            var sizeX = maxX - minX;
            var sizeY = maxY - minY;
            var sizeZ = maxZ - minZ;
            var position = new Vector3D(minX, minY, minZ);
            BoundingBox = new RectangularCuboid(position, sizeX, sizeY, sizeZ);
        }
    }
}
