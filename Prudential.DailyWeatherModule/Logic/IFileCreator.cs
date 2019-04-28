namespace Prudential.DailyWeatherModule.Logic
{
    public interface IFileCreator
    {
        bool Create(string fileName, string weatherInfo);
    }
}
