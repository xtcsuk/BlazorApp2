namespace BlazorApp2.Services
{
    public class LoqatePostcodeSearchResponse
    {
        public LoqatePostcodeSearchResponse()
        {
            Items = new List<LoqatePostcodeSearchItem>();
          
        }

        public IEnumerable<LoqatePostcodeSearchItem> Items { get; set; }
    }

    public class LoqatePostcodeSearchItem
    {
        public string Id { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Highlight { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
