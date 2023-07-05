using AntDesign.Internal;
using System.Globalization;
using System.Text;
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

        public async Task<IEnumerable<string>> GetDataAsync(string? searchText, int? limit)
        {
            // response format from provider for a none address (in this instance postcode LU32NX) search
            // {"Items":[{"Id":"GB|RM|ENG|2NX-LU3","Type":"Postcode",
            // "Text":"LU3 2NX","Highlight":"0-3,4-7","Description":"Norton Road, Luton - 62 Addresses"}]}

            if (string.IsNullOrWhiteSpace(searchText))
            {
                throw new ArgumentNullException(nameof(searchText));
            }

            var resultLimit = limit ?? 10;

            using var httpClient = _httpClientFactory.CreateClient("PostcodeSearch");

            var url = $"{httpClient.BaseAddress}&Text={searchText}&Limit={resultLimit}{_urlConstantParams}";

            var apiContent = await DoGetDataAsync(httpClient, url);

            _loqatePostcodeSearchResponse = JsonSerializer.Deserialize<LoqatePostcodeSearchResponse>(apiContent)
                ?? new LoqatePostcodeSearchResponse();

            return DisplayData();
        }

        public bool IsValueAnAddress(string rawAddress)
        {
            if (string.IsNullOrWhiteSpace(rawAddress))
            {
                throw new ArgumentNullException(nameof(rawAddress));
            }

            var model = GetModelFromDisplayData(rawAddress);

            return model.Type.Equals("address", StringComparison.OrdinalIgnoreCase);
        }

        public async Task<IEnumerable<string>> GetAddressAsync(string? rawAddress)
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
            // provider response for a formatted address
            // {"Items":[{"Id":"GB|RM|A|56664311","DomesticId":"56664311",
            // "Language":"ENG","LanguageAlternatives":"ENG","Department":"",
            // "Company":"","SubBuilding":"","BuildingNumber":"","BuildingName":"57A",
            // "SecondaryStreet":"","Street":"Norton Road","Block":"",
            // "Neighbourhood":"","District":"","City":"Luton","Line1":"57A Norton Road",
            // "Line2":"","Line3":"","Line4":"","Line5":"","AdminAreaName":"Luton","AdminAreaCode":"",
            // "Province":"Bedfordshire","ProvinceName":"Bedfordshire","ProvinceCode":"","PostalCode":"LU3 2NX",
            // "CountryName":"United Kingdom","CountryIso2":"GB","CountryIso3":"GBR",
            // "CountryIsoNumber":"826","SortingNumber1":"60123","SortingNumber2":"",
            // "Barcode":"(LU32NX4G7)","POBoxNumber":"","Label":"57A Norton Road\nLUTON\nLU3 2NX\nUNITED KINGDOM",
            // "Type":"Residential","DataLevel":"Premise","Field1":"","Field2":"","Field3":"",
            // "Field4":"","Field5":"","Field6":"","Field7":"","Field8":"","Field9":"",
            // "Field10":"","Field11":"","Field12":"","Field13":"","Field14":"","Field15":"",
            // "Field16":"","Field17":"","Field18":"","Field19":"","Field20":""}]}

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
                if (string.IsNullOrWhiteSpace(item.Label))
                {
                    throw new NullReferenceException(nameof(item.Label));
                }

                var label = item.Label.Replace("\n", " ");
                result.Add(label);

            }
            return result;
        }

        private async Task<string> DoGetDataAsync(HttpClient httpClient, string url)
        {
            var apiResult = await httpClient.GetAsync(url);

            apiResult.EnsureSuccessStatusCode();

            var content = await apiResult.Content.ReadAsStringAsync();

            CheckForErrorInApiResponse(content);

            return content;
        }

        private void CheckForErrorInApiResponse(string content)
        {
            if (!content.Contains("Error", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // error format returned from the provider
            // {"Items":[{"Error":"1001","Description":"Id Invalid",
            // "Cause":"The Id parameter supplied was invalid.",
            // "Resolution":"You should only pass an ID where the type was returned as Address from the Find service.
            // Passing IDs of type Postcode will not result in a retrieve - instead,
            // these should be passed back into the Find service as Container until an Address type is returned."}]}

            var result = JsonSerializer.Deserialize<LoqateErrorResponse>(content);
            if (result != null && result.Items != null)
            {
                var message = new StringBuilder();

                // prepare the message
                foreach (var item in result.Items)
                {
                    message.AppendLine($@"Description: {item.Description} 
                                       Cause: {item.Cause} Resolution: {item.Resolution}");
                }

                throw new BadHttpRequestException($"{message}");
            }
        }

        private IEnumerable<string> DisplayData()
        {
            //var result = new List<string>();
            //var listResponse = _loqatePostcodeSearchResponse.Items.ToList();
            //var upperRange = listResponse.Count;

            //for (var i = 0; i < upperRange; i++)
            //{
            //    result.Add($"{listResponse[i].Text}, {listResponse[i].Description}: seqId{i}");
            //}

            //return result;
            return _loqatePostcodeSearchResponse.Items.Select(m => $"{m.Text}, {m.Description}");
        }

        private LoqatePostcodeSearchItem GetModelFromDisplayData(string displayData)
        {
            var displayDataArray = displayData.Split(",");
            var text = displayDataArray[0];
            var description = displayDataArray[1];

            var model = _loqatePostcodeSearchResponse.Items.Single(m => m.Text.StartsWith(text));

            //var model = _loqatePostcodeSearchResponse.Items.Single(ls => ls.Text.Equals(text));

            return model;
        }
    }
}
