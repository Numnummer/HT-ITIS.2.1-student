using Hw10.Dto;
using Hw10.ErrorMessages;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw10.Model
{
    public class MyExpressionVisitor : ExpressionVisitor
    {
        private readonly object _locker = new object();

        public async Task<double> VisitAsync(BinaryExpression node)
        {
            var leftExpression = Task.Run(async () => await ProcessExpression(node.Left));
            var rightExpression = Task.Run(async () => await ProcessExpression(node.Right));
            var expressions = await Task.WhenAll(leftExpression, rightExpression);
            await Task.Delay(1000);
            lock (_locker)
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
            return DtoHelper.Dto.Result;
        }

        private async Task<double> ProcessExpression(Expression expression)
        {
            if (expression is ConstantExpression constant)
            {
                return (double)constant.Value;
            }
            return await VisitAsync(expression as BinaryExpression);
        }
    }
}