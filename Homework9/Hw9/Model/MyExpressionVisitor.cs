using Hw9.Dto;
using Hw9.ErrorMessages;
using System.Linq.Expressions;

namespace Hw9.Model
{
    public class MyExpressionVisitor : ExpressionVisitor
    {
        private readonly object _lock = new object();

        protected override Expression VisitBinary(BinaryExpression node)
        {
            var leftExpression = Task.Run(() => Expression.Lambda<Func<double>>(Visit(node.Left)).Compile().Invoke());
            var rightExpression = Task.Run(() => Expression.Lambda<Func<double>>(Visit(node.Right)).Compile().Invoke());
            var expressions = Task.WhenAll(leftExpression, rightExpression).Result;
            Thread.Sleep(1000);
            lock (_lock)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        DtoHelper.Dto.Result=expressions[0]+expressions[1];
                        return Expression.Constant(DtoHelper.Dto.Result);
                    case ExpressionType.Divide:
                        if (expressions[1]==0)
                        {
                            DtoHelper.Dto.ErrorMessage=MathErrorMessager.DivisionByZero;
                            throw new Exception(DtoHelper.Dto.ErrorMessage);
                        }
                        DtoHelper.Dto.Result=expressions[0]/expressions[1];
                        return Expression.Constant(DtoHelper.Dto.Result);
                    case ExpressionType.Multiply:
                        DtoHelper.Dto.Result=expressions[0]*expressions[1];
                        return Expression.Constant(DtoHelper.Dto.Result);
                    case ExpressionType.Subtract:
                        DtoHelper.Dto.Result=expressions[0]-expressions[1];
                        return Expression.Constant(DtoHelper.Dto.Result);
                    default:
                        break;
                }
            }
            return node.Update(node.Left, node.Conversion, node.Right);
        }
    }
}
