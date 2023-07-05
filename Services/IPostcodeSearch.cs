namespace BlazorApp2.Services
{
    public interface IPostcodeSearch
    {
        Task<IEnumerable<string>> GetDataAsync(string? serachText, int? limit);
        bool IsValueAnAddress(string rawAddress);
        Task<IEnumerable<string>> GetAddressAsync(string? rawAddress);
    }
}
