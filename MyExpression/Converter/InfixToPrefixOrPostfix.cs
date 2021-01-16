using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.Converter
{
    class InfixToPrefixOrPostfix : Converter
    {
        protected override string convertToPostfix(string inputExpression)
        {
            try
            {
                return infixToPostfix(inputExpression);
            }
            catch
            {
                return "";
            }
        }

        protected override void normalizeIfNeeded(ref string inputExpression)
        {
            inputExpression = inputExpression.Replace(" ", "");
            for (int i = 0; i < inputExpression.Length - 1; i++)
            {
                if (inputExpression[i] == '-' && inputExpression[i + 1] == '-')
                {
                    if (i == 0)
                    {
                        // "--a-b"      --> "a-b"
                        // "---a--b"    --> "-a--b"
                        inputExpression = inputExpression.Remove(0, 2);
                        i--;
                    }
                    else if (Helper.isOperator(inputExpression[i - 1] + "") || inputExpression[i - 1] == '(')
                    {
                        // "a----b"         --> "a--b"
                        // "a---b"          --> "a-b"
                        // "a/(--b*(c+d))   --> "a/(b*(c+d))
                        inputExpression = inputExpression.Remove(i, 2);
                        i--;
                    }
                }
            }
        }

        private string infixToPostfix(string exp)
        {
            Stack<char> ops = new Stack<char>();
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < exp.Length; i++)
            {
                char c = exp[i];
                if (c == '(')
                {
                    ops.Push(c);
                }
                else if (Helper.isLetter(c + ""))
                {
                    StringBuilder sb = new StringBuilder();
                    while (i < exp.Length && (Helper.isNumeric(exp[i]) || Helper.isLetter(exp[i] + "")))
                    {
                        c = exp[i];
                        sb.Append(c);
                        i++;
                    }
                    i--;
                    int j = i - sb.Length;
                    if (j >= 0)
                    {
                        if (exp[j] == '-')
                        {
                            if (j == 0)
                            {
                                result.Append("-" + sb.ToString() + " ");
                                ops.Pop();
                            }
                            else if (j == 1)
                            {
                                if (exp[j - 1] == '(')
                                {
                                    result.Append("-" + sb.ToString() + " ");
                                    ops.Pop();
                                }
                                else result.Append(sb.ToString() + " ");
                            }
                            else
                            {
                                if (Helper.isOperator(exp[j - 1] + "") || exp[j - 1] == '(')
                                {
                                    if (result.ToString().Split(new string[] { " " },
                                        StringSplitOptions.RemoveEmptyEntries).Length >= 2)
                                    {
                                        result.Remove(result.Length - 3, 2);
                                    }
                                    result.Append("-" + sb.ToString() + " ");
                                    ops.Pop();
                                    if (Helper.isOperator(exp[j - 1] + ""))
                                        ops.Push(exp[j - 1]);
                                }
                                else result.Append(sb.ToString() + " ");
                            }
                        }
                        else result.Append(sb.ToString() + " ");
                    }
                    else result.Append(sb.ToString() + " ");
                }
                else if (Helper.isNumeric(c) || c == '.')
                {
                    StringBuilder sb = new StringBuilder();
                    while (i < exp.Length && (Helper.isNumeric(exp[i]) || exp[i] == '.'))
                    {
                        c = exp[i];
                        sb.Append(c);
                        i++;
                    }
                    i--;
                    int j = i - sb.Length;
                    if (j >= 0)
                    {
                        if (exp[j] == '-')
                        {
                            if (j == 0)
                            {
                                result.Append((-1 * Double.Parse(sb.ToString())) + " ");
                                ops.Pop();
                            }
                            else if (j == 1)
                            {
                                if (exp[j - 1] == '(')
                                {
                                    result.Append((-1 * Double.Parse(sb.ToString())) + " ");
                                    ops.Pop();
                                }
                                else result.Append(Double.Parse(sb.ToString()) + " ");
                            }
                            else
                            {
                                if (Helper.isOperator(exp[j - 1] + "") || exp[j - 1] == '(')
                                {
                                    if (result.ToString().Split(new string[] { " " },
                                        StringSplitOptions.RemoveEmptyEntries).Length >= 2)
                                    {
                                        result.Remove(result.Length - 3, 2);
                                    }
                                    result.Append((-1 * Double.Parse(sb.ToString())) + " ");
                                    ops.Pop();
                                    if (Helper.isOperator(exp[j - 1] + ""))
                                        ops.Push(exp[j - 1]);
                                }
                                else result.Append(Double.Parse(sb.ToString()) + " ");
                            }
                        }
                        else result.Append(Double.Parse(sb.ToString()) + " ");
                    }
                    else result.Append(Double.Parse(sb.ToString()) + " ");
                }
                else if (Helper.isOperator(c + ""))
                {
                    if (c == '-' && i < exp.Length - 1)
                    {
                        if (i == 0 && exp[i + 1] == '(')
                        {
                            // -(1+1)
                            result.Append(0 + " ");
                        }
                        else
                        {
                            // -(-(1+1)) or 10/-(2+3)
                            if ((exp[i + 1] == '(' && exp[i - 1] == '(') ||
                                (exp[i + 1] == '(' && Helper.isOperator(exp[i - 1] + "")))
                            {
                                result.Append(0 + " ");
                            }
                        }
                    }
                    while (ops.Count != 0 && ops.Peek() != '(')
                    {
                        char temp = ops.Peek();
                        if (Helper.isLargerOrEqual(temp, c))
                        {
                            temp = ops.Pop();
                            result.Append(temp + " ");
                        }
                        else break;
                    }
                    ops.Push(c);
                }
                else if (c == ')')
                {
                    while (ops.Count != 0 && ops.Peek() != '(')
                    {
                        char temp = ops.Pop();
                        result.Append(temp + " ");
                    }
                    ops.Pop();
                }
                else continue;
            }

            while (ops.Count != 0)
            {
                result.Append(ops.Pop() + " ");
            }

            return result.ToString();
        }
    }
}
