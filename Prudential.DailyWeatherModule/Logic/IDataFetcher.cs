namespace Prudential.DailyWeatherModule.Logic
{
    public interface IDataFetcher
    {
        string GetDataByCityID(string place);
    }
}
