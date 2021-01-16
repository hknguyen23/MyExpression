using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression
{
    public class Helper
    {
        public static bool isNumeric(char c)
        {
            return c >= '0' && c <= '9';
        }

        public static bool isLetter(string s)
        {
            if (s[0] == '-' && s.Length > 1)
            {
                return (s[1] >= 'a' && s[1] <= 'z') || (s[1] >= 'A' && s[1] <= 'Z');
            }
            return (s[0] >= 'a' && s[0] <= 'z') || (s[0] >= 'A' && s[0] <= 'Z');
        }

        public static bool isOperator(string s)
        {
            return s == "+" || s == "-" || s == "*" || s == "/" || s == "^";
        }

        public static bool isLargerOrEqual(char x, char y)
        {
            switch (x)
            {
                case '+':
                case '-':
                    return y == '+' || y == '-';
                case '*':
                case '/':
                    return y == '+' || y == '-' || y == '*' || y == '/';
            }
            return true;
        }

    }
}
