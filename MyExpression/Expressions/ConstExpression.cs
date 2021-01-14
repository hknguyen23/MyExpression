using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.Expressions
{
    public class ConstExpression : Expression
    {
        private double value;
        private string variableName;

        public double Value { get; set; }
        public string VariableName { get; set; }
        public ConstExpression()
        {
            this.VariableName = "";
        }

        public ConstExpression(double value)
        {
            this.Value = value;
            this.VariableName = "";
        }

        public ConstExpression(string variableName)
        {
            this.VariableName = variableName;
        }

        public override double evaluate()
        {
            return this.Value;
        }

        public override string toString(Notation notation)
        {
            if (this.VariableName == "")
                return this.Value.ToString();
            return this.VariableName;
        }
    }
}
