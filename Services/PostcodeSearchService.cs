using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.Json;

namespace BlazorApp2.Services
{
    public class PostcodeSearchService : IPostcodeSearch
    {
        private readonly HttpClient _httpClient;

        public PostcodeSearchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<string> JsonFormat(string key, string text, bool ismiddleware, string container, string origin, string countries, int limit, string language, bool bias, string filters, string geofence)
        {
            var param = $"{_httpClient.BaseAddress}&Text={text}&IsMiddleware={ismiddleware}&Countries=GBR&Limit={limit}&Language=en-gb";

            var request = new HttpRequestMessage
            {
                RequestUri = new Uri(param)
            };

            var t = await _httpClient.GetAsync(param);

            t.EnsureSuccessStatusCode();

            var content = await t.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<PostcodeSearchResponse>(content);

            return content;
        }
    }
}
