namespace BlazorApp2.Services
{
    public interface IPostcodeSearch
    {
        Task<string> JsonFormat(string key, string text, bool ismiddleware, string container, string origin, string countries, int limit, string language, bool bias, string filters, string geofence);
    }
}
