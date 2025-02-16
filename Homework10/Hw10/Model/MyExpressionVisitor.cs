﻿using Hw10.Dto;
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
            var leftExpression = Task.Run(() => ProcessExpression(node.Left));
            var rightExpression = Task.Run(() => ProcessExpression(node.Right));
            var expressions = await Task.WhenAll(leftExpression, rightExpression);
            await Task.Delay(3000);
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
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var leftExpression = ProcessExpression(node.Left);
            var rightExpression = ProcessExpression(node.Right);

            lock (_locker)
            {
                switch (node.NodeType)
                {
                    case ExpressionType.Add:
                        DtoHelper.Dto.Result=leftExpression+rightExpression;
                        break;
                    case ExpressionType.Divide:
                        DtoHelper.Dto.Result=leftExpression/rightExpression;
                        break;
                    case ExpressionType.Multiply:
                        DtoHelper.Dto.Result=leftExpression*rightExpression;
                        break;
                    case ExpressionType.Subtract:
                        DtoHelper.Dto.Result=leftExpression-rightExpression;
                        break;
                }
            }
            return Expression.Constant(DtoHelper.Dto.Result);
        }

        private double ProcessExpression(Expression expression)
        {
            var result = Visit(expression) as ConstantExpression;
            return (double)result.Value;
        }
    }
}