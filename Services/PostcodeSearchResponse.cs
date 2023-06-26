namespace BlazorApp2.Services
{
    public class PostcodeSearchResponse
    {
        public List<Item>? Items { get; set; }
    }

    public class Item
    {
        public string? Id { get; set; }
        public string? Type { get; set; }
        public string? Text { get; set; }
        public string? Highlight { get; set; }
        public string? Description { get; set; }
    }
}
