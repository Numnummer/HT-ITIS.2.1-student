using Hw10.DbModels;
using Hw10.Dto;
using Hw10.ErrorMessages;
using Hw10.Services.MathCalculator;
using Microsoft.EntityFrameworkCore;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
    private readonly ApplicationContext _dbContext;
    private readonly IMathCalculatorService _simpleCalculator;

    public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
    {
        _dbContext = dbContext;
        _simpleCalculator = simpleCalculator;
    }

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
        {
            return new CalculationMathExpressionResultDto(MathErrorMessager.EmptyString);
        }

        var solved = await _dbContext.SolvingExpressions.FirstOrDefaultAsync(e => e.Expression == expression);

        if (solved != null)
        {
            return new CalculationMathExpressionResultDto(solved.Result);
        }

        var result = await _simpleCalculator.CalculateMathExpressionAsync(expression);

        _dbContext.SolvingExpressions.Add(new SolvingExpression()
        {
            Expression = expression,
            Result = result.Result
        });

        await _dbContext.SaveChangesAsync();

        return result;
    }

}