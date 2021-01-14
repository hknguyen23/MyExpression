using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.Notations
{
    public class PostfixNotation : Notation
    {
        public override string arrange(string expression1, string op, string expression2)
        {
            return expression1 + " " + expression2 + " " + op;
        }
    }
}
