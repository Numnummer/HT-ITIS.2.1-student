using Hw9.ErrorMessages;
using System.Text;

namespace Hw9.Model
{
    public class ExpressionValidator
    {
        private const string mathOperationSymbols = "+-*/";
        private const string otherSymbols = "().";
        private static string message;
        public static string IsValid(string? expression)
        {
            if (expression==null || string.IsNullOrWhiteSpace(expression))
            {
                return MathErrorMessager.EmptyString;
            }
            var rawExpression = expression.Replace(" ", "");
            var normalExpression = MakeNormalExpression(rawExpression);

            message=IsValidBrackets(rawExpression);
            if (message!=string.Empty)
            {
                return message;
            }
            message=IsValidBounds(rawExpression);
            if (message!=string.Empty)
            {
                return message;
            }

            for (int i = 1; i < rawExpression.Length; i++)
            {
                if (mathOperationSymbols.Contains(rawExpression[i]))
                {
                    if (rawExpression[i-1]=='(' && rawExpression[i]!='-')
                    {
                        return MathErrorMessager.InvalidOperatorAfterParenthesisMessage(rawExpression[i].ToString());
                    }
                    if (mathOperationSymbols.Contains(rawExpression[i-1]))
                    {
                        return MathErrorMessager.TwoOperationInRowMessage(rawExpression[i-1].ToString(), rawExpression[i].ToString());
                    }
                    continue;
                }
                if (rawExpression[i]==')' &&
                        mathOperationSymbols.Contains(rawExpression[i-1]))
                {
                    return MathErrorMessager.OperationBeforeParenthesisMessage(rawExpression[i-1].ToString());
                }
                if (!double.TryParse(rawExpression[i].ToString(), out _)
                    && !otherSymbols.Contains(rawExpression[i]))
                {
                    return MathErrorMessager.UnknownCharacterMessage(rawExpression[i]);
                }
            }
            message = IsNumbersCorrect(rawExpression);
            if (message!=string.Empty)
            {
                return message;
            }
            return string.Empty;
        }

        public static string MakeNormalExpression(string rawExpression)
        {
            var result = new StringBuilder();
            for (int i = 0; i < rawExpression.Length; i++)
            {
                if (i-1>=0 && rawExpression[i] == '-' && rawExpression[i-1]=='(')
                {
                    result.Append($"-");
                    continue;
                }
                if (mathOperationSymbols.Contains(rawExpression[i]))
                {
                    result.Append($" {rawExpression[i]} ");
                    continue;
                }
                if (rawExpression[i]=='(')
                {
                    result.Append($"{rawExpression[i]} ");
                    continue;
                }
                if (rawExpression[i]==')')
                {
                    result.Append($" {rawExpression[i]}");
                    continue;
                }
                if (char.IsDigit(rawExpression[i]))
                {
                    result.Append(rawExpression[i]);
                    continue;
                }
            }
            return result.ToString();
        }

        private static string IsValidBounds(string rawExpression)
        {
            if (mathOperationSymbols.Contains(rawExpression[0]))
            {
                return MathErrorMessager.StartingWithOperation;
            }
            if (mathOperationSymbols.Contains(rawExpression[^1]))
            {
                return MathErrorMessager.EndingWithOperation;
            }
            return string.Empty;
        }

        private static string IsNumbersCorrect(string rawExpression)
        {
            var splitBy = new char[] { '(', ')', '+', '-', '*', '/' };
            var numbers = rawExpression.Split(splitBy, StringSplitOptions.RemoveEmptyEntries);
            foreach (var number in numbers)
            {
                if (number.Count(symbol => symbol=='.')>1)
                {
                    return MathErrorMessager.NotNumberMessage(number);
                }
            }
            return string.Empty;
        }

        private static string IsValidBrackets(string rawExpression)
        {
            var counter = 0;
            foreach (var symbol in rawExpression)
            {
                if (symbol=='(')
                {
                    counter++;
                }
                if (symbol==')')
                {
                    counter--;
                }
                if (counter<0)
                {
                    return MathErrorMessager.IncorrectBracketsNumber;
                }
            }
            if (counter!=0)
            {
                return MathErrorMessager.IncorrectBracketsNumber;
            }
            return string.Empty;
        }
    }
}
