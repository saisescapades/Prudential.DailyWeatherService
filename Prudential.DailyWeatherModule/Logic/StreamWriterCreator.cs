using Microsoft.Extensions.Options;
using Prudential.DailyWeatherModule.Models;
using System;
using System.IO;

namespace Prudential.DailyWeatherModule.Logic
{
    public class StreamWriterCreator : IFileCreator
    {
        private readonly AppSettings _mySettings;

        public StreamWriterCreator(IOptions<AppSettings> settings)
        {
            _mySettings = settings.Value;
        }

        /// <summary>
        /// Creates a new file with city's weather information as it's content
        /// </summary>
        /// <param name="fileName">File Name as string</param>
        /// <param name="weatherInfo">Weather Information to be saved in a file</param>
        /// <returns>Boolean response to indicate that the file was successfully created.</returns>
        public bool Create(string fileName, string weatherInfo)
        {
            string newFileName = DateTime.Now.ToString(_mySettings.OutputFolderNameFormat).ToString();
            string completeteDirectoryName = Path.Combine(_mySettings.OutputFolderPath, newFileName);

            if (!Directory.Exists(completeteDirectoryName))
            { 
                Directory.CreateDirectory(completeteDirectoryName);
            }
       
            using (var writer = File.CreateText(Path.Combine(completeteDirectoryName, fileName)))
            {
                writer.WriteLine(weatherInfo); //or .Write(), if you wish
            }

            return true;
        }
    }
}
