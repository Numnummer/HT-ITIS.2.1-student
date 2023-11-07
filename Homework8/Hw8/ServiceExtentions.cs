using Hw8.Calculator;
using System.Runtime.CompilerServices;

namespace Hw8
{
    public static class ServiceExtentions
    {
        public static void AddCalculatorGroupDependences(this IServiceCollection services)
        {
            services.AddSingleton<ICalculator, Calculator.Calculator>();
            services.AddSingleton<IParser, Parser>();
        }
    }
}
