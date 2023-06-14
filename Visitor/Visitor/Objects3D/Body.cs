using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
    public abstract class Body
    {
        public Vector3D Position { get; }

        protected Body(Vector3D position)
        {
            Position = position;
        }

        public abstract void Accept(IVisitor visitor);
    }

}
