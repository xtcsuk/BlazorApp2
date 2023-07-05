namespace BlazorApp2.Services
{
    public interface IPostcodeSearch
    {
        Task<IEnumerable<string>> GetAddressFirstAttemptAsync(string? serachText, int? limit);
        Task<IEnumerable<string>> GetAddressFurtherAttemptsAsync(string? rawAddress);
        bool IsValueAnAddress(string? rawAddress);
        Task<(IEnumerable<string> addressContainer, string Address)> GetAddressAsync(string? rawAddress); 
    }
}
