namespace Hw8.Calculator
{
    public class Parser : IParser
    {
        public Operation ParseOperation(string operation)
        {
            return operation switch
            {
                "Plus" => Operation.Plus,
                "Minus" => Operation.Minus,
                "Multiply" => Operation.Multiply,
                "Divide" => Operation.Divide,
                _ => Operation.Invalid
            };
        }

        public (double, double, bool) ParseValues(string value1, string value2)
        {
            if (double.TryParse(value1, out var number1)
                && double.TryParse(value2, out var number2))
            {
                return (number1, number2, true);
            }
            else return (0, 0, false);
        }
    }
}
