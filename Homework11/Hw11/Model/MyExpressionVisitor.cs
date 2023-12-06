using Hw11.Dto;
using Hw11.ErrorMessages;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Hw11.Model
{
    public class MyExpressionVisitor
    {
        public async Task<double> VisitAsync(BinaryExpression node)
        {
            var leftExpression = Process(node.Left);
            var rightExpression = Process(node.Right);

            var expressions = await Task.WhenAll(leftExpression, rightExpression);
            await Task.Delay(1000);
            double result = default;
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    result = expressions[0] + expressions[1];
                    break;
                case ExpressionType.Divide:
                    if (expressions[1] == 0)
                    {
                        throw new DivideByZeroException("Division by zero");
                    }
                    result = expressions[0] / expressions[1];
                    break;
                case ExpressionType.Multiply:
                    result = expressions[0] * expressions[1];
                    break;
                case ExpressionType.Subtract:
                    result = expressions[0] - expressions[1];
                    break;
            }
            return result;
        }

        public async Task<double> VisitAsync(ConstantExpression node) =>
            await Task.FromResult((double)node.Value);

        private Task<double> Process(Expression expression)
        {
            return VisitAsync((dynamic)expression);
        }
    }

}