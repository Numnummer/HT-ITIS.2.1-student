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
        var isValuesCorrect = parser.TryParseValues(val1, val2, out var number1, out var number2);
        if (!isValuesCorrect)
        {
            return Content(Messages.InvalidNumberMessage);
        }
        switch (parser.ParseOperation(operation))
        {
            case Operation.Plus:
                return calculator.Plus(number1, number2);
            case Operation.Minus:
                return calculator.Minus(number1, number2);
            case Operation.Multiply:
                return calculator.Multiply(number1, number2);
            case Operation.Divide:
                {
                    if (number2==0)
                    {
                        return Content(Messages.DivisionByZeroMessage);
                    }
                    return calculator.Divide(number1, number2);
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