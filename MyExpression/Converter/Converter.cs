using MyExpression.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.Converter
{
    public class Converter
    {
        public string convert(string textBoxExpression, ref Expression exp, Notation notation, List<string> varibles)
        {
            string postfix;
            string[] tokens = new string[] { };
            string inputExpression = textBoxExpression;
            Stack<Expressions.Expression> stack = new Stack<Expressions.Expression>();

            normalizeIfNeeded(ref inputExpression);

            postfix = convertToPostfix(inputExpression);

            tokens = postfix.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            addVariables(tokens, varibles);

            exp = createRootExpression(tokens, stack);

            if (exp == null)
            {
                return "";
            }

            return exp.toString(notation);
        }

        // bước 1
        protected virtual void normalizeIfNeeded(ref string inputExpression)
        {
            return;
        }

        // bước 2
        protected virtual string convertToPostfix(string inputExpression)
        {
            return inputExpression;
        }

        // bước 3
        protected void addVariables(string[] tokens, List<string> varibles)
        {
            for (int i = 0; i < tokens.Length; i++)
            {
                if (Helper.isLetter(tokens[i]) && !varibles.Contains(tokens[i]))
                {
                    varibles.Add(tokens[i]);
                }
            }
        }

        // bước 4
        protected Expression createRootExpression(string[] tokens, Stack<Expression> stack)
        {
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
                return new BinaryExpression(stack.Pop());
            }
            catch
            {
                return null;
            }
        }

    }
}
