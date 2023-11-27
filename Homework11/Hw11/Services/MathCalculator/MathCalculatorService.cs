using Hw11.Dto;
using Hw11.Exceptions;
using Hw11.Model;
using System.Linq.Expressions;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
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
        var result = await visitor.VisitAsync((dynamic)resultExpression);

        return result;
    }
}