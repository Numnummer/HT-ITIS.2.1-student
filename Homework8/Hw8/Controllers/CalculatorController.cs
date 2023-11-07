using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        [FromServices] IParser parser,
        string val1,
        string operation,
        string val2)
    {
        var numbers = parser.ParseValues(val1, val2);
        if (numbers.Item3==false)
        {
            return Content(Messages.InvalidNumberMessage);
        }
        switch (parser.ParseOperation(operation))
        {
            case Operation.Plus:
                return calculator.Plus(numbers.Item1, numbers.Item2);
            case Operation.Minus:
                return calculator.Minus(numbers.Item1, numbers.Item2);
            case Operation.Multiply:
                return calculator.Multiply(numbers.Item1, numbers.Item2);
            case Operation.Divide:
                {
                    if (numbers.Item2==0)
                    {
                        return Content(Messages.DivisionByZeroMessage);
                    }
                    return calculator.Divide(numbers.Item1, numbers.Item2);
                }
        }
        return Content(Messages.InvalidOperationMessage);
    }

    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}