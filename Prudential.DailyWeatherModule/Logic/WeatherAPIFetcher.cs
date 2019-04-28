using Microsoft.Extensions.Options;
using Prudential.DailyWeatherModule.Models;
using System;
using System.Net.Http;

namespace Prudential.DailyWeatherModule.Logic
{
    public class WeatherAPIFetcher : IDataFetcher
    {
        private readonly AppSettings _mySettings;

        public WeatherAPIFetcher(IOptions<AppSettings> settings)
        {
            _mySettings = settings.Value;
        }

        /// <summary>
        /// Retrieves weather information of city based on City ID passed
        /// </summary>
        /// <param name="cityID">CityID as string, for example 123456</param>
        /// <returns>String response comprising weather information returned by Open Weather Map API</returns>
        public string GetDataByCityID(string cityID)
        {            
            string responseString = String.Empty;

            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, _mySettings.WeatherAPIURL.Replace("{cityID}", cityID)))
            using (var response = client.SendAsync(request).Result)
            {
                response.EnsureSuccessStatusCode();//This statement will raise an expection if HTTP status code returned is not 200.

                var responseContent = response.Content;
                responseString = responseContent.ReadAsStringAsync().Result;              
            }

            return responseString;
        }       
    }
}
