using Hw10.Dto;
using Hw10.Model;
using System.Linq.Expressions;

namespace Hw10.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        var message = ExpressionValidator.IsValid(expression);
        if (message!=string.Empty)
        {
            return new CalculationMathExpressionResultDto(message);
        }
        var resultExpression = await Task.Run(() =>
        {
            expression=ExpressionValidator.MakeNormalExpression(expression);
            return ExpressionBuilder.BuildExpression(expression);
        });
        if (resultExpression is ConstantExpression constant)
        {
            return new CalculationMathExpressionResultDto((double)constant.Value);
        }
        try
        {
            var visitor = new MyExpressionVisitor();
            visitor.Visit(resultExpression);
        }
        catch (Exception exception)
        {
            return new CalculationMathExpressionResultDto(DtoHelper.Dto.ErrorMessage);
        }

        return new CalculationMathExpressionResultDto(DtoHelper.Dto.Result);
    }
}