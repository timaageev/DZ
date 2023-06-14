using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
    public class Ball : Body
    {
        public double Radius { get; }

        public Ball(Vector3D position, double radius) : base(position)
        {
            Radius = radius;
        }
        public override void Accept(IVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
