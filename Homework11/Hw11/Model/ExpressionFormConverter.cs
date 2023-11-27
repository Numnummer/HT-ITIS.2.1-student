using System.Text;

namespace Hw11.Model
{
    public static class ExpressionFormConverter
    {
        public static string ToPostfixForm(string standartExpression)
        {
            string[] tokens = standartExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            StringBuilder output = new();
            Stack<string> operators = new Stack<string>();

            Dictionary<string, int> precedence = new Dictionary<string, int>
            {
                {"(", 0},
                {"+", 1},
                {"-", 1},
                {"*", 2},
                {"/", 2}
            };

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out _))
                {
                    output.Append(token+' ');
                }
                else if (token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    while (operators.Peek() != "(")
                    {
                        output.Append(operators.Pop()+' ');
                    }
                    operators.Pop();
                }
                else if (precedence.ContainsKey(token))
                {
                    while (operators.Count > 0 && precedence[token] <= precedence[operators.Peek()])
                    {
                        output.Append(operators.Pop() + ' ');
                    }
                    operators.Push(token);
                }
            }

            while (operators.Count > 0)
            {
                output.Append(operators.Pop() + ' ');
            }
            return output.ToString();
        }
    }
}
