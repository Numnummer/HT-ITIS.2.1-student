using Hw9.Dto;
using Hw9.Model;
using Microsoft.VisualBasic;
using System.Linq.Expressions;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var message = ExpressionValidator.IsValid(expression);
        if (message!=string.Empty)
        {
            return new CalculationMathExpressionResultDto(message);
        }
        var result = (Func<double>)await Task.Run(() =>
        {
            return ExpressionBuilder.BuildExpression(expression).Compile();
        });
        return new CalculationMathExpressionResultDto(result.Invoke());
    }
}