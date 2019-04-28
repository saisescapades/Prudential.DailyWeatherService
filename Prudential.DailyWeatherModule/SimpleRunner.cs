using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Prudential.DailyWeatherModule.Logic;
using Prudential.DailyWeatherModule.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Prudential.DailyWeatherModule
{
    public class SimpleRunner : IRunner
    {        
        private readonly IDataFetcher _dataFetcher;
        private readonly ILogger _logger;
        private readonly IFileCreator _fileCreator;
        private readonly AppSettings _mySettings;

        public SimpleRunner(IDataFetcher dataFetcher, IFileCreator fileCreator, ILogger<SimpleRunner> logger, IOptions<AppSettings> settings)
        {
            _dataFetcher = dataFetcher;
            _fileCreator = fileCreator;
            _mySettings = settings.Value;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("Execution started at " + DateTime.Now);

            try
            {
                string inputFilePath = Path.Combine(_mySettings.InputFolderPath,
                                                    DateTime.Now.ToString(_mySettings.InputFileNameDateFormat).ToString() + "_" +
                                                    _mySettings.InputFileName);

                using (StreamReader r = new StreamReader(inputFilePath))
                using (var jsonReader = new JsonTextReader(r))
                {
                    var js = new JsonSerializer();

                    var cities = js.Deserialize<List<City>>(jsonReader);

                    if (cities.Count > 0)
                    {
                        string fileName = String.Empty, weatherInfo = String.Empty;
                        foreach (var city in cities)
                        {
                            try
                            {
                                weatherInfo = _dataFetcher.GetDataByCityID(city.id);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(String.Format("Error encountered fetching weather information. Error encountered was: {0}",
                                                                ex.Message));
                            }

                            try
                            {
                                fileName = String.Format("{0}[{1}]_{2}.json",
                                                                city.name.Replace(" ", ""),
                                                                city.id,
                                                                DateTime.Now.ToString(_mySettings.OutputFileNameDateFormat).ToString());

                                if (_fileCreator.Create(fileName, weatherInfo))
                                    _logger.LogInformation("Information for City '{0}'[{1}] successfully saved as a file with name {2}", 
                                                            city.name, 
                                                            city.id,
                                                            fileName);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(String.Format("Error encountered while creating file for City '{0}'[{1}]. Error encountered was: {1}",
                                                                city.name,
                                                                city.id,
                                                                ex.Message));
                            }
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Input file does not contain any City IDs");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            _logger.LogInformation("Execution completed at " + DateTime.Now);
        }
    }
}
