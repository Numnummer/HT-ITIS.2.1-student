namespace Hw8.Calculator
{
    public interface IParser
    {
        Operation ParseOperation(string operation);
        (double, double, bool) ParseValues(string value1, string value2);
    }
}