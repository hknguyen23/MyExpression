using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.Expressions
{
    public abstract class Expression
    {
        public abstract double evaluate();
        public abstract string toString(Notation notation);
    }
}
