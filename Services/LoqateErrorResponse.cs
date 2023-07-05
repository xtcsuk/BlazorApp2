namespace BlazorApp2.Services
{
    public class LoqateError
    {
        public string? Error { get; set; }
        public string? Description { get; set; }
        public string? Cause { get; set; }
        public string? Resolution { get; set; }
    }

    public class LoqateErrorResponse
    {
        public LoqateErrorResponse()
        {
            Items = new List<LoqateError>();            
        }

        public IEnumerable<LoqateError>? Items { get; set; }
    }
}
