using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace BlazorApp2.Services
{
    public class LoqatePostcodeSearchService : IPostcodeSearch
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private LoqatePostcodeSearchResponse _loqatePostcodeSearchResponse;
        private const string _urlConstantParams = "&IsMiddleware=false&Countries=GBR&Language=en-gb";

        public LoqatePostcodeSearchService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _loqatePostcodeSearchResponse = new LoqatePostcodeSearchResponse();
        }

        public System.Data.DataSet CaptureInteractiveFindv110(string key, string text, bool ismiddleware, string container, string origin, string countries, int limit, string language, bool bias, string filters, string geofence)
        {
            //Build the url
            var url = "http://api.addressy.com/Capture/Interactive/Find/v1.10/dataset.ws?";
            url += "&Key=" + System.Web.HttpUtility.UrlEncode(key);
            url += "&Text=" + System.Web.HttpUtility.UrlEncode(text);
            url += "&IsMiddleware=" + System.Web.HttpUtility.UrlEncode(ismiddleware.ToString(CultureInfo.InvariantCulture));
            url += "&Container=" + System.Web.HttpUtility.UrlEncode(container);
            url += "&Origin=" + System.Web.HttpUtility.UrlEncode(origin);
            url += "&Countries=" + System.Web.HttpUtility.UrlEncode(countries);
            url += "&Limit=" + System.Web.HttpUtility.UrlEncode(limit.ToString(CultureInfo.InvariantCulture));
            url += "&Language=" + System.Web.HttpUtility.UrlEncode(language);
            url += "&Bias=" + System.Web.HttpUtility.UrlEncode(bias.ToString(CultureInfo.InvariantCulture));
            url += "&Filters=" + System.Web.HttpUtility.UrlEncode(filters);
            url += "&GeoFence=" + System.Web.HttpUtility.UrlEncode(geofence);

            //Create the dataset
            var dataSet = new System.Data.DataSet();
            dataSet.ReadXml(url);

            //Check for an error
            if (dataSet.Tables.Count == 1 && dataSet.Tables[0].Columns.Count == 4 && dataSet.Tables[0].Columns[0].ColumnName == "Error")
                throw new Exception(dataSet.Tables[0].Rows[0].ItemArray[1].ToString());

            //Return the dataset
            return dataSet;

            //FYI: The dataset contains the following columns:
            //Id
            //Type
            //Text
            //Highlight
            //Description
        }

        public async Task<IEnumerable<string>> GetDataAsync(string searchText, int limit)
        {
            using var httpClient = _httpClientFactory.CreateClient("PostcodeSearch");

            var url = $"{httpClient.BaseAddress}&Text={searchText}&Limit={limit}{_urlConstantParams}";

            var apiContent = await DoGetDataAsync(httpClient, url);

            _loqatePostcodeSearchResponse = JsonSerializer.Deserialize<LoqatePostcodeSearchResponse>(apiContent)
                ?? new LoqatePostcodeSearchResponse();

            return DisplayData();
        }

        public async Task<IEnumerable<string>> GetAddressAsync(string rawAddress)
        {
            if (string.IsNullOrWhiteSpace(rawAddress))
            {
                throw new ArgumentNullException(nameof(rawAddress));
            }

            var model = GetModelFromDisplayData(rawAddress);

            if (model.Type.Equals("address", StringComparison.OrdinalIgnoreCase))
            {
                return await GetFormattedAddressAsync(model);
            }

            var httpClient = _httpClientFactory.CreateClient("PostcodeSearch");
            var url = $"{httpClient.BaseAddress}&Container={model.Id}&{_urlConstantParams}";

            var apiContent = await DoGetDataAsync(httpClient, url);

            _loqatePostcodeSearchResponse = JsonSerializer.Deserialize<LoqatePostcodeSearchResponse>(apiContent)
                ?? new LoqatePostcodeSearchResponse();

            return DisplayData();
        }

        private async Task<IEnumerable<string>> GetFormattedAddressAsync(LoqatePostcodeSearchItem model)
        {
            var httpClient = _httpClientFactory.CreateClient("RetrieveAddress");
            var url = $"{httpClient.BaseAddress}&Id={model.Id}&Text={model.Description}&{_urlConstantParams}";

            var apiContent = await DoGetDataAsync(httpClient, url);

            var address = JsonSerializer.Deserialize<LoqateFormattedAddressResponse>(apiContent) ?? new LoqateFormattedAddressResponse();

            return DisplayFormattedAddress(address);
        }

        private IEnumerable<string> DisplayFormattedAddress(LoqateFormattedAddressResponse addressResponse)
        {
            var result = new List<string>();
            
            foreach (var item in addressResponse.Items)
            {
                result.Add(item.Label);
            }
            return result;
        }

        private async Task<string> DoGetDataAsync(HttpClient httpClient, string url)
        {
            var apiResult = await httpClient.GetAsync(url);

            apiResult.EnsureSuccessStatusCode();

            var content = await apiResult.Content.ReadAsStringAsync();

            return content;
        }

        private IEnumerable<string> DisplayData()
        {
            return _loqatePostcodeSearchResponse.Items.Select(m => $"{m.Text}, {m.Description}");
        }

        private LoqatePostcodeSearchItem GetModelFromDisplayData(string displayData)
        {
            var displayDataArray = displayData.Split(",");
            var text = displayDataArray[0];
            var description = displayDataArray[1];

            var model = _loqatePostcodeSearchResponse.Items.Single(ls => ls.Text.Equals(text));
            //&& ls.Description.Equals(description));

            return model;
        }
    }
}
