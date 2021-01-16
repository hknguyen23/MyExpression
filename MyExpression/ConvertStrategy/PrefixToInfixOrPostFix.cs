using MyExpression.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.ConvertStrategy
{
    class PrefixToInfixOrPostFix : ConvertStrategy
    {
        public override string convert(string textBoxExpression, ref Expression exp, Notation notation, List<string> varibles)
        {
            string postfix;
            string[] tokens;
            Stack<Expressions.Expression> stack = new Stack<Expressions.Expression>();
            string prefix = textBoxExpression;

            try
            {
                postfix = prefixToPostfix(prefix);
            }
            catch
            {
                postfix = "";
            }

            tokens = postfix.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < tokens.Length; i++)
            {
                if (Helper.isLetter(tokens[i]) && !varibles.Contains(tokens[i]))
                {
                    varibles.Add(tokens[i]);
                }
            }

            //variblesListBox.Items.Clear(); nhớ làm lại bên main
            //setVariblesList(varibles);

            try
            {
                for (int i = 0; i < tokens.Length; i++)
                {
                    if (Helper.isOperator(tokens[i]))
                    {
                        BinaryExpression binaryExpression = new BinaryExpression(tokens[i]);
                        binaryExpression.addExpression2(stack.Pop());
                        binaryExpression.addExpression1(stack.Pop());
                        stack.Push(binaryExpression);
                    }
                    else if (Helper.isLetter(tokens[i]))
                    {
                        stack.Push(new ConstExpression(tokens[i]));
                    }
                    // tokens[i] is a number
                    else
                    {
                        stack.Push(new ConstExpression(Double.Parse(tokens[i])));
                    }
                }

                // final tree is top of nodes stack
                exp = new BinaryExpression(stack.Pop());

                return exp.toString(notation);
            }
            catch
            {
                return "Invalid input!!!";
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
