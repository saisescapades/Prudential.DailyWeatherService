using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using Prudential.DailyWeatherModule.Logic;
using Prudential.DailyWeatherModule.Models;
using System;

namespace Tests
{
    public class WeatherAPIFetcherTests
    {
        private AppSettings appSettings;
        private IOptions<AppSettings> options;
        private WeatherAPIFetcher _dataFetcher;

        [SetUp]
        public void Setup()
        {
            appSettings = new AppSettings();
            //Although below values have been harcoded, the same may be fetched from a test configuration file
            //MOQ package may also be used for mocking dependencies.            
            appSettings.WeatherAPIURL = "http://samples.openweathermap.org/data/2.5/weather?appid=aa69195559bd4f88d79f9aadeb77a8f6&id={cityID}";

            options = Options.Create(appSettings);

            _dataFetcher = new WeatherAPIFetcher(options);
        }

        /// <summary>
        /// Checks if weather information returned is in valid JSON format
        /// </summary>       
        [TestCase("123456", true)]
        [TestCase("654321", true)]
        public void IsResponseInJsonFormat_DataFetcher_ReturnsExpectedResult(string cityID, bool expectedResult)
        {
            string response = String.Empty;

            bool reponseIsValidJSON = false;

            try
            {
                response = _dataFetcher.GetDataByCityID(cityID);

                try
                {
                    JObject joResponse = null;
                    joResponse = JObject.Parse(response);//Will raise an exception if string returned is not a valid JSON

                    reponseIsValidJSON = true;
                }
                catch
                {
                    reponseIsValidJSON = false;//While this statement in not needed, it helps improve readability
                }
            }
            catch
            {
                reponseIsValidJSON = false;//While this statement in not needed, it helps improve readability
            }

            Assert.AreEqual(expectedResult, reponseIsValidJSON);
        }

        /// <summary>
        /// Checks if weather information returned contains "weather" attribute
        /// </summary>     
        [TestCase("123456", true)]
        [TestCase("654321", true)]
        public void ResponseContainsWeatherAttribute_DataFetcher_ReturnsExpectedResult(string cityID, bool expectedResult)
        {
            string response = String.Empty;

            try
            {
                response = _dataFetcher.GetDataByCityID(cityID);
            }
            catch
            { }

            Assert.AreEqual(expectedResult, response.Contains("\"weather\""));
        }
    }
}