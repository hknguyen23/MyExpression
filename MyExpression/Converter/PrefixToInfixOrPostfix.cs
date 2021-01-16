using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.Converter
{
    class PrefixToInfixOrPostfix : Converter
    {
        protected override string convertToPostfix(string inputExpression)
        {
            try
            {
                return prefixToPostfix(inputExpression);
            }
            catch
            {
                return "";
            }
        }

        private string prefixToPostfix(string exp)
        {
            string[] tokens = exp.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Stack<string> stack = new Stack<string>();
            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                if (Helper.isOperator(tokens[i]))
                {
                    string op1 = stack.Pop();
                    string op2 = stack.Pop();
                    stack.Push(op1 + " " + op2 + " " + tokens[i]);
                }
                else
                {
                    stack.Push(tokens[i]);
                }
            }
            return stack.Pop();
        }

    }
}
