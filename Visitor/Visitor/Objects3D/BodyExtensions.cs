using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Visitor
{
    public static class BodyExtensions
    {
        // Это расширение нужно, чтобы компилятор не возражал 
        // против вызова ещё не добавленного метода Accept
        public static TResult TryAcceptVisitor<TResult>(this Body body, dynamic visitor)
        {
            dynamic dynamicBody = body;
            return (TResult)dynamicBody.Accept(visitor);
        }
    }
}
