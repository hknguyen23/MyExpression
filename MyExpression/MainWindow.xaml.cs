﻿using System;
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
using MyExpression.ConvertStrategy;
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
        NotationManager notationManager = NotationManager.getInstance();
        ConvertStrategy.ConvertStrategy strategy;

        public MainWindow()
        {
            InitializeComponent();
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
            string[] tokens = textBoxExpression.Text.Split(new string[] { "+", "-", "*", "/", "^", "(", ")", " " }, 
                StringSplitOptions.RemoveEmptyEntries);
            string input = textBoxExpression.Text.Replace(" ", "");

            try
            {
                double temp = Double.Parse(variableValue);
                int j = 0;
                for (int i = 0; i < tokens.Length; i++)
                {
                    while (j < input.Length && (Helper.isOperator(input[j] + "") 
                        || input[j] == '(' || input[j] == ')'))
                    {
                        j++;
                    }

                    if (tokens[i] == variableName)
                    {
                        int startIndex = input.IndexOf(tokens[i], j, variableName.Length);
                        input = input.Remove(startIndex, tokens[i].Length);
                        input = input.Insert(startIndex, variableValue);
                        textBoxExpression.Text = input;
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

        private Notation getNotation(string notationName) // factory
        {
            return notationManager.getNotation(notationName);
        }

        private void cmbNotations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedIndex = cmbNotations.SelectedIndex;
            notation = getNotation(NotationManager.notationArray[selectedIndex]);
            lbNotationName.Content = NotationManager.notationArray[selectedIndex] + ": ";
            if (selectedIndex == 0 || selectedIndex == 1)
            {
                strategy = new InfixToPrefixOrPostfix();
            }
            else if (selectedIndex == 1 || selectedIndex == 3)
            {
                strategy = new PrefixToInfixOrPostFix();
            }
            else
            {
                strategy = new PostfixToInfixOrPrefix();
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
            variblesListBox.Items.Clear();

            string result = strategy.convert(textBoxExpression.Text,ref exp, notation, varibles);
            setVariblesList(varibles);
            lbToStringResult.Content = result;
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
