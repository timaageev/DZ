using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
        public interface IVisitor
        {
            void Visit(Ball ball);
            void Visit(Cylinder cylinder);
            void Visit(RectangularCuboid cuboid);
            void Visit(CompoundBody compoundBody);
        }
}
