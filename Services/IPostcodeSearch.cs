namespace BlazorApp2.Services
{
    public interface IPostcodeSearch
    {
        Task<IEnumerable<string>> GetDataAsync(string serachText, int limit);
        Task<IEnumerable<string>> GetAddressAsync(string rawAddress);
    }
}
