using Hw11.Dto;
using Hw11.Exceptions;
using Hw11.Model;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    [ExcludeFromCodeCoverage]
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        var message = ExpressionValidator.IsValid(expression);
        if (message!=string.Empty)
        {
            throw new Exception(message);
        }
        var resultExpression = await Task.Run(() =>
        {
            expression=ExpressionValidator.MakeNormalExpression(expression);
            return ExpressionBuilder.BuildExpression(expression);
        });

        var visitor = new MyExpressionVisitor();
        return await visitor.VisitAsync((dynamic)resultExpression);
    }
}