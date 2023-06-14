using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
    public class BoxifyVisitor : IVisitor
    {
        public Body BoxifiedBody { get; private set; }

        public void Visit(Ball ball)
        {
            var boundingBoxVisitor = new BoundingBoxVisitor();
            ball.Accept(boundingBoxVisitor);
            BoxifiedBody = boundingBoxVisitor.BoundingBox;
        }

        public void Visit(Cylinder cylinder)
        {
            var boundingBoxVisitor = new BoundingBoxVisitor();
            cylinder.Accept(boundingBoxVisitor);
            BoxifiedBody = boundingBoxVisitor.BoundingBox;
        }

        public void Visit(RectangularCuboid cuboid)
        {
            BoxifiedBody = cuboid;
        }

        public void Visit(CompoundBody compoundBody)
        {
            var boxifiedParts = new List<Body>();
            foreach (var part in compoundBody.Parts)
            {
                var partVisitor = new BoxifyVisitor();
                part.Accept(partVisitor);
                boxifiedParts.Add(partVisitor.BoxifiedBody);
            }
            BoxifiedBody = new CompoundBody(boxifiedParts);
        }
    }
}