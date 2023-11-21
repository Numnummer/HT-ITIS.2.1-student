using Hw10.Dto;
using Hw10.ErrorMessages;
using System.Linq.Expressions;

namespace Hw10.Model
{
    public class MyExpressionVisitor : ExpressionVisitor
    {
        private readonly object _lock = new object();

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var leftExpression = Task.Run(() => ProcessExpression(node.Left));
            var rightExpression = Task.Run(() => ProcessExpression(node.Right));
            var expressions = Task.WhenAll(leftExpression, rightExpression).Result;
            Thread.Sleep(1000);

            lock (_lock)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        DtoHelper.Dto.Result=expressions[0]+expressions[1];
                        break;
                    case ExpressionType.Divide:
                        if (expressions[1]==0)
                        {
                            DtoHelper.Dto.ErrorMessage=MathErrorMessager.DivisionByZero;
                            throw new Exception(DtoHelper.Dto.ErrorMessage);
                        }
                        DtoHelper.Dto.Result=expressions[0]/expressions[1];
                        break;
                    case ExpressionType.Multiply:
                        DtoHelper.Dto.Result=expressions[0]*expressions[1];
                        break;
                    case ExpressionType.Subtract:
                        DtoHelper.Dto.Result=expressions[0]-expressions[1];
                        break;
                }
            }
            return Expression.Constant(DtoHelper.Dto.Result);
        }
        private double ProcessExpression(Expression expression)
        {
            var result = Visit(expression);
            if (result is ConstantExpression constant)
            {
                return (double)constant.Value;
            }
            return Expression.Lambda<Func<double>>(result).Compile().Invoke();
        }
    }
}
