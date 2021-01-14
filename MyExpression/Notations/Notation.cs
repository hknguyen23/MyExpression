using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyExpression.Expressions;

namespace MyExpression
{
    public abstract class Notation
    {
        public abstract string arrange(string expression1, string op, string expression2);
    }
}
