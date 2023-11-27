using System.Linq.Expressions;

namespace Hw11.Model
{
    public static class ExpressionBuilder
    {
        public static Expression BuildExpression(string expression)
        {
            var postfixExpression = ExpressionFormConverter.ToPostfixForm(expression);
            var expressionMembers = postfixExpression.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var expressionStack = new Stack<Expression>();
            foreach (var member in expressionMembers)
            {
                if (double.TryParse(member, out var number))
                {
                    expressionStack.Push(Expression.Constant(number));
                    continue;
                }
                var second = expressionStack.Pop();
                var first = expressionStack.Pop();
                switch (member)
                {
                    case "+":
                        expressionStack.Push(Expression.Add(first, second));
                        break;
                    case "-":
                        expressionStack.Push(Expression.Subtract(first, second));
                        break;
                    case "*":
                        expressionStack.Push(Expression.Multiply(first, second));
                        break;
                    case "/":
                        expressionStack.Push(Expression.Divide(first, second));
                        break;
                }
            }
            return expressionStack.Pop();
        }
    }
}
