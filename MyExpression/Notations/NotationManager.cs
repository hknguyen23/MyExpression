using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyExpression.Notations
{
    public class NotationManager
    {
        private Dictionary<string, Notation> notations = new Dictionary<string, Notation>();
        private static NotationManager instance = null;
        private NotationManager()
        {
            notations.Add("Infix", new InfixNotation());
            notations.Add("Prefix", new PrefixNotation());
            notations.Add("Postfix", new PostfixNotation());
        }

        public Notation getNotation(string notationName)
        {
            if (notations.ContainsKey(notationName))
            {
                return notations[notationName];
            }
            else return null;
        }

        public static NotationManager getInstance()
        {
            if (instance == null)
            {
                instance = new NotationManager();
            }
            return instance;
        }
    }
}
