using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MyExpression.Expressions;
using MyExpression.Notations;

namespace MyExpression
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Expressions.Expression exp;
        Notation notation;
        int selectedIndex = -1;
        List<string> varibles = new List<string>();
        string[] notationArray = new string[] { "Prefix", "Postfix", "Infix", "Postfix", "Infix", "Prefix" };
        NotationManager notationManager = NotationManager.getInstance();

        public MainWindow()
        {
            InitializeComponent();
        }

        private bool isNumeric(char c)
        {
            return c >= '0' && c <= '9';
        }

        private bool isLetter(string s)
        {
            if (s[0] == '-' && s.Length > 1)
            {
                return (s[1] >= 'a' && s[1] <= 'z') || (s[1] >= 'A' && s[1] <= 'Z');
            }
            return (s[0] >= 'a' && s[0] <= 'z') || (s[0] >= 'A' && s[0] <= 'Z');
        }

        private bool isOperator(string s)
        {
            return s == "+" || s == "-" || s == "*" || s == "/" || s == "^";
        }

        private bool isLargerOrEqual(char x, char y)
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
                else if (isLetter(c + ""))
                {
                    StringBuilder sb = new StringBuilder();
                    while (i < exp.Length && (isNumeric(exp[i]) || isLetter(exp[i] + ""))) {
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
                                if (isOperator(exp[j - 1] + "") || exp[j - 1] == '(')
                                {
                                    if (result.ToString().Split(new string[] { " " },
                                        StringSplitOptions.RemoveEmptyEntries).Length >= 2)
                                    {
                                        result.Remove(result.Length - 3, 2);
                                    }
                                    result.Append("-" + sb.ToString() + " ");
                                    ops.Pop();
                                    if (isOperator(exp[j - 1] + ""))
                                        ops.Push(exp[j - 1]);
                                }
                                else result.Append(sb.ToString() + " ");
                            }
                        }
                        else result.Append(sb.ToString() + " ");
                    }
                    else result.Append(sb.ToString() + " ");
                }
                else if (isNumeric(c) || c == '.')
                {
                    StringBuilder sb = new StringBuilder();
                    while (i < exp.Length && (isNumeric(exp[i]) || exp[i] == '.'))
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
                                if (isOperator(exp[j - 1] + "") || exp[j - 1] == '(')
                                {
                                    if (result.ToString().Split(new string[] { " " }, 
                                        StringSplitOptions.RemoveEmptyEntries).Length >= 2)
                                    {
                                        result.Remove(result.Length - 3, 2);
                                    }
                                    result.Append((-1 * Double.Parse(sb.ToString())) + " ");
                                    ops.Pop();
                                    if (isOperator(exp[j - 1] + ""))
                                        ops.Push(exp[j - 1]);
                                }
                                else result.Append(Double.Parse(sb.ToString()) + " ");
                            }
                        }
                        else result.Append(Double.Parse(sb.ToString()) + " ");
                    }
                    else result.Append(Double.Parse(sb.ToString()) + " ");
                }
                else if (isOperator(c + ""))
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
                            // -(-(1+1))
                            if (exp[i + 1] == '(' && exp[i - 1] == '(')
                            {
                                result.Append(0 + " ");
                            }
                        }
                    }
                    while (ops.Count != 0 && ops.Peek() != '(')
                    {
                        char temp = ops.Peek();
                        if (isLargerOrEqual(temp, c))
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

        private string prefixToPostfix(string exp)
        {
            string[] tokens = exp.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            Stack<string> stack = new Stack<string>();
            for (int i = tokens.Length - 1; i >= 0; i--)
            {
                if (isOperator(tokens[i]))
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

        private void setVariblesList(List<string> varibles) 
        {
            if (varibles.Count == 0)
            {
                Label label = new Label();
                label.Height = 25;
                label.Width = 250;
                label.Content = "No varibles found!!!";
                variblesListBox.Items.Add(label);
                return;
            }

            for (int i = 0; i < varibles.Count; i++)
            {
                Label label = new Label();
                label.Height = 25;
                label.Width = 100;
                label.Margin = new Thickness(0, 0, 5, 0);
                label.Content = varibles[i].Replace("-", "");
                TextBox textBox = new TextBox();
                textBox.Width = 100;
                textBox.Height = 25;
                textBox.Margin = new Thickness(0, 0, 5, 0);
                textBox.Tag = varibles[i].Replace("-", "");
                Button button = new Button();
                button.Width = 80;
                button.Height = 25;
                button.Margin = new Thickness(0, 0, 5, 0);
                button.Tag = i;
                button.Content = "OK";
                button.Click += btnSetValue_Click;
                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.Children.Add(label);
                stackPanel.Children.Add(textBox);
                stackPanel.Children.Add(button);
                variblesListBox.Items.Add(stackPanel);
            }
        }

        private void btnSetValue_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int index = Int32.Parse(button.Tag.ToString());
            StackPanel stackPanel = (StackPanel)variblesListBox.Items[index];
            TextBox textBox = (TextBox)stackPanel.Children[1];
            string variableValue = textBox.Text;
            string variableName = textBox.Tag.ToString();
            string[] tokens = textBoxExpression.Text.Split(new string[] { "+", "-", "*", "/", "^", "(", ")" }, 
                StringSplitOptions.RemoveEmptyEntries);
            int len = textBoxExpression.Text.Length;

            try
            {
                double temp = Double.Parse(variableValue);
                int j = 0;
                for (int i = 0; i < tokens.Length; i++)
                {
                    while (j < textBoxExpression.Text.Length && (isOperator(textBoxExpression.Text[j] + "") 
                        || textBoxExpression.Text[j] == '(' || textBoxExpression.Text[j] == ')'))
                    {
                        j++;
                    }

                    if (tokens[i] == variableName)
                    {
                        int startIndex = textBoxExpression.Text.IndexOf(tokens[i], j, variableName.Length);
                        textBoxExpression.Text = textBoxExpression.Text.Remove(startIndex, tokens[i].Length);
                        textBoxExpression.Text = textBoxExpression.Text.Insert(startIndex, variableValue);
                        j += variableValue.Length + 1;
                    }
                    else j += tokens[i].Length + 1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid value!!!");
                Console.WriteLine(ex);
            }
        }

        private Notation getNotation(string notationName)
        {
            return notationManager.getNotation(notationName);
        }

        private void cmbNotations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = cmbNotations.SelectedIndex;
            if (selectedIndex == 0 || selectedIndex == 5)
            {
                lbNotationName.Content = "Prefix: ";
            }
            else if (selectedIndex == 1 || selectedIndex == 3)
            {
                lbNotationName.Content = "Postfix: ";
            }
            else
            {
                lbNotationName.Content = "Infix: ";
            }
        }

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            if (selectedIndex == -1)
            {
                MessageBox.Show("Please select an option from the box");
                return;
            }

            varibles.Clear();
            string inputExpression;
            string postfix;
            string[] tokens;
            Stack<Expressions.Expression> stack = new Stack<Expressions.Expression>();

            switch (selectedIndex)
            {
                #region Infix to Prefix or Postfix
                // Infix to Prefix or Postfix
                case 0:
                case 1:
                    inputExpression = textBoxExpression.Text.Replace(" ", "");

                    // remove "--"
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
                            else if (isOperator(inputExpression[i - 1] + "") || inputExpression[i - 1] == '(')
                            {
                                // "a----b"         --> "a--b"
                                // "a---b"          --> "a-b"
                                // "a/(--b*(c+d))   --> "a/(b*(c+d))
                                inputExpression = inputExpression.Remove(i, 2);
                                i--;
                            }
                        }
                    }

                    try
                    {
                        postfix = infixToPostfix(inputExpression);
                    }
                    catch
                    {
                        postfix = "";
                    }
                    
                    tokens = postfix.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (isLetter(tokens[i]) && !varibles.Contains(tokens[i]))
                        {
                            varibles.Add(tokens[i]);
                        }
                    }

                    variblesListBox.Items.Clear();
                    setVariblesList(varibles);

                    try
                    {
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            if (isOperator(tokens[i]))
                            {
                                BinaryExpression binaryExpression = new BinaryExpression(tokens[i]);
                                binaryExpression.addExpression2(stack.Pop());
                                binaryExpression.addExpression1(stack.Pop());
                                stack.Push(binaryExpression);
                            }
                            else if (isLetter(tokens[i]))
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


                        notation = getNotation(notationArray[selectedIndex]);
                        lbToStringResult.Content = exp.toString(notation);
                    }
                    catch
                    {
                        lbToStringResult.Content = "Invalid input!!!";
                    }

                    break;
                #endregion

                #region Prefix to Infix or Postfix
                // Prefix to Infix or Postfix
                case 2:
                case 3:
                    string prefix = textBoxExpression.Text;

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
                        if (isLetter(tokens[i]) && !varibles.Contains(tokens[i]))
                        {
                            varibles.Add(tokens[i]);
                        }
                    }

                    variblesListBox.Items.Clear();
                    setVariblesList(varibles);

                    try
                    {
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            if (isOperator(tokens[i]))
                            {
                                BinaryExpression binaryExpression = new BinaryExpression(tokens[i]);
                                binaryExpression.addExpression2(stack.Pop());
                                binaryExpression.addExpression1(stack.Pop());
                                stack.Push(binaryExpression);
                            }
                            else if (isLetter(tokens[i]))
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

                        notation = getNotation(notationArray[selectedIndex]);
                        lbToStringResult.Content = exp.toString(notation);
                    }
                    catch
                    {
                        lbToStringResult.Content = "Invalid input!!!";
                    }

                    break;
                #endregion

                #region Postfix to Infix or Prefix
                // Postfix to Infix or Prefix
                case 4:
                case 5:
                    postfix = textBoxExpression.Text;
                    tokens = postfix.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < tokens.Length; i++)
                    {
                        if (isLetter(tokens[i]) && !varibles.Contains(tokens[i]))
                        {
                            varibles.Add(tokens[i]);
                        }
                    }

                    variblesListBox.Items.Clear();
                    setVariblesList(varibles);

                    try
                    {
                        for (int i = 0; i < tokens.Length; i++)
                        {
                            if (isOperator(tokens[i]))
                            {
                                BinaryExpression binaryExpression = new BinaryExpression(tokens[i]);
                                binaryExpression.addExpression2(stack.Pop());
                                binaryExpression.addExpression1(stack.Pop());
                                stack.Push(binaryExpression);
                            }
                            else if (isLetter(tokens[i]))
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

                        notation = getNotation(notationArray[selectedIndex]);
                        lbToStringResult.Content = exp.toString(notation);
                    }
                    catch
                    {
                        lbToStringResult.Content = "Invalid input!!!";
                    }

                    break;
                #endregion

                default:
                    return;
            }
            
        }

        private void btnEvaluate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (varibles.Count == 0)
                {
                    double result = exp.evaluate();
                    lbEvaluateResult.Content = "Result: " + result;
                }
                else
                {
                    // if there're varibles, display the result with variables
                    lbEvaluateResult.Content = "Result: " + exp.toString(new InfixNotation());
                }
            }
            catch
            {
                lbEvaluateResult.Content = "Result: Invalid input!!!";
            }
        }
    }
}
