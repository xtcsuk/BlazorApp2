namespace BlazorApp2.Models
{
    public class SelectModel
    {
        public string Id { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Disabled { get; set; } = false;
        public bool Divider { get; set; } = false;
    }
}
