using MyExpression.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.ConvertStrategy
{
    public class ConvertStrategy
    {

        public virtual string convert(string textBoxExpression,ref Expression exp, Notation notation, List<string> varibles)
        {
            return "";
        }
        
    }
}
