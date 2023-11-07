namespace Hw8.Calculator
{
    public interface IParser
    {
        Operation ParseOperation(string operation);
        bool TryParseValues(string value1, string value2, out double number1, out double number2);
    }
}