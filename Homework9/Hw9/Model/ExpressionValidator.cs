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
            var members = normalExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < members.Length; i++)
            {
                if (mathOperationSymbols.Contains(rawExpression[i]))
                {
                    if (members[i-1]=="(")
                    {
                        return MathErrorMessager.InvalidOperatorAfterParenthesisMessage(rawExpression[i].ToString());
                    }
                    if (mathOperationSymbols.Contains(members[i-1]))
                    {
                        return MathErrorMessager.TwoOperationInRowMessage(members[i-1], members[i]);
                    }
                    continue;
                }
                if (members[i]==")" &&
                        mathOperationSymbols.Contains(members[i-1]))
                {
                    return MathErrorMessager.OperationBeforeParenthesisMessage(members[i-1]);
                }
                if (!double.TryParse(members[i], out _)
                    && !otherSymbols.Contains(rawExpression[i]))
                {
                    return MathErrorMessager.UnknownCharacterMessage(members[i][0]);
                }
            }
            message = IsNumbersCorrect(rawExpression);
            if (message!=string.Empty)
            {
                return message;
            }
            return string.Empty;
        }

        private static string MakeNormalExpression(string rawExpression)
        {
            var result = new StringBuilder();
            foreach (var symbol in rawExpression)
            {
                if (mathOperationSymbols.Contains(symbol))
                {
                    result.Append($" {symbol} ");
                }
                if (symbol=='(')
                {
                    result.Append($"{symbol} ");
                }
                if (symbol==')')
                {
                    result.Append($" {symbol}");
                }
                if (char.IsDigit(symbol))
                {
                    result.Append(symbol);
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
                if (number[0]=='.' || number[^1]=='.'
                    || number.Count(symbol => symbol=='.')>1)
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
