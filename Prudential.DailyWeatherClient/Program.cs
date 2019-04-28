using Prudential.DailyWeatherModule;
using Prudential.DailyWeatherModule.App_Start;
using System;

namespace Prudential.DailyWeatherClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Execution started.");

                ContainerConfig.Init();

                var runner = ContainerConfig.GetInstance<IRunner>();
                runner.Run();

                Console.WriteLine("Execution completed.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception occured: " + e.Message);
                Console.ReadKey();
            }
        }
    }
}
