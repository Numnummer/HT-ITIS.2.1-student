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

        public bool TryParseValues(string value1, string value2,
            out double number1,
            out double number2)
        {
            if (double.TryParse(value1, out number1)
                && double.TryParse(value2, out number2))
            {
                return true;
            }
            number1=number2=0;
            return false;
        }
    }
}
