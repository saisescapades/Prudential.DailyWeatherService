using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prudential.DailyWeatherModule.Logic;
using Prudential.DailyWeatherModule.Models;
using Serilog;

namespace Prudential.DailyWeatherModule.App_Start
{
    public static class ContainerConfig
    {        
        private static ServiceCollection collection;
        private static ServiceProvider provider;

        public static void Init()
        {

            var configuration = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.json")            
                                    .Build();

            Log.Logger = new LoggerConfiguration()
                             .ReadFrom.Configuration(configuration)
                             .CreateLogger();

            //Setup DI
            collection = new ServiceCollection();
            collection.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            collection.AddSingleton<IRunner, SimpleRunner>();
            collection.AddSingleton<IDataFetcher, WeatherAPIFetcher>();
            collection.AddSingleton<IFileCreator, StreamWriterCreator>();
            collection.Configure<AppSettings>(configuration.GetSection("AppSettings"));
            provider = collection.BuildServiceProvider();     
        }

        public static TService GetInstance<TService>() where TService : class
        {          
            return provider.GetService<TService>();
        }      
    }
}
