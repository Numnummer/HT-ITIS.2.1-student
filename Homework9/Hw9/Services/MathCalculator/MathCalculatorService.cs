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