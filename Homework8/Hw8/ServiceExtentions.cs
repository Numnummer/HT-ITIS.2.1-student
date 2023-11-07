using Hw8.Calculator;
using System.Runtime.CompilerServices;

namespace Hw8
{
    public static class ServiceExtentions
    {
        public static void AddAllSingletonDependences(this IServiceCollection services)
        {
            services.AddTransient<ICalculator, Calculator.Calculator>();
            services.AddTransient<IParser, Parser>();
        }
    }
}
