using Microsoft.Extensions.Options;
using NUnit.Framework;
using Prudential.DailyWeatherModule.Logic;
using Prudential.DailyWeatherModule.Models;

namespace Tests
{
    [TestFixture]
    public class StreamWriterCreatorTests
    {
        private StreamWriterCreator _fileCreator;
        private AppSettings appSettings;
        private IOptions<AppSettings> options;

        [SetUp]
        public void Setup()
        {
            appSettings = new AppSettings();
            //Although below values have been harcoded, the same may be fetched from a test configuration file
            //MOQ package may also be used for mocking dependencies.
            appSettings.WeatherAPIURL = "http://samples.openweathermap.org/data/2.5/weather?appid=aa69195559bd4f88d79f9aadeb77a8f6&id={cityID}";

            options = Options.Create(appSettings);

            _fileCreator = new StreamWriterCreator(options);
        }

        /// <summary>
        /// Checks if test file has been successfully created
        /// </summary>
        [TestCase("TestFileName.csv", "Test data.", true)]
        public void IsFileCreatedSuccessfully_StreamWriter_ReturnsExpectedResult(string fileName, string weatherInfo, bool expectedResult)
        {
            bool isFileCreatedSuccessfully = false;

            try
            {
                if (_fileCreator.Create(fileName, weatherInfo))
                    isFileCreatedSuccessfully = true;
            }
            catch
            {
                isFileCreatedSuccessfully = false;//While this statement in not needed, it helps improve readability
            }            

            Assert.AreEqual(expectedResult, isFileCreatedSuccessfully);
        }
    }
}