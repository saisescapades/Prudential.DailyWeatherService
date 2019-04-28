using System;
using System.Collections.Generic;
using System.Text;

namespace Prudential.DailyWeatherModule.Models
{
    public class AppSettings
    {
        public string InputFolderPath { get; set; }
        public string InputFileName { get; set; }
        public string InputFileNameDateFormat { get; set; }
        public string OutputFolderPath { get; set; }
        public string OutputFolderNameFormat { get; set; }
        public string OutputFileNameDateFormat { get; set; }
        public string WeatherAPIURL { get; set; }
    }
}
