using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.Expressions
{
    public class BinaryExpression : Expression
    {
        private string op;
        private Expression exp1;
        private Expression exp2;

        public string Op { get; set; }
        public Expression Expression1 { get; set; }
        public Expression Expression2 { get; set; }

        public BinaryExpression() { }

        public BinaryExpression(string op)
        {
            this.Op = op;
        }

        public BinaryExpression(Expression expression)
        {
            this.Op = ((BinaryExpression)expression).Op;
            this.Expression1 = ((BinaryExpression)expression).Expression1;
            this.Expression2 = ((BinaryExpression)expression).Expression2;
        }

        public void addExpression1(Expression expression)
        {
            if (expression is BinaryExpression) {
                this.Expression1 = new BinaryExpression((BinaryExpression)expression);
            }
            else if (expression is ConstExpression) {
                ConstExpression temp = (ConstExpression)expression;
                if (temp.VariableName == "")
                    this.Expression1 = new ConstExpression(temp.Value);
                else this.Expression1 = new ConstExpression(temp.VariableName);
            }
        }

        public void addExpression2(Expression expression)
        {
            if (expression is BinaryExpression)
            {
                this.Expression2 = new BinaryExpression((BinaryExpression)expression);
            }
            else if (expression is ConstExpression)
            {
                ConstExpression temp = (ConstExpression)expression;
                if (temp.VariableName == "")
                    this.Expression2 = new ConstExpression(temp.Value);
                else this.Expression2 = new ConstExpression(temp.VariableName);
            }
        }

        private double calculate(Expression expression1, string op, Expression expression2)
        {
            switch (op)
            {
                case "+":
                    return expression1.evaluate() + expression2.evaluate();
                case "-":
                    return expression1.evaluate() - expression2.evaluate();
                case "*":
                    return expression1.evaluate() * expression2.evaluate();
                case "/":
                    return expression1.evaluate() / expression2.evaluate();
                case "^":
                    return Math.Pow(expression1.evaluate(), expression2.evaluate());
            }
            return -1;
        }

        public override double evaluate()
        {
            return calculate(this.Expression1, this.Op, this.Expression2);
        }

        public override string toString(Notation notation)
        {
            return notation.arrange(this.Expression1.toString(notation), this.Op, this.Expression2.toString(notation));
        }
    }
}
