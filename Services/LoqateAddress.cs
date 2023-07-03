namespace BlazorApp2.Services
{
    public class LoqateFormattedAddress
    {
        public string? Id { get; set; }
        public string? DomesticId { get; set; }
        public string? Language { get; set; }
        public string? LanguageAlternatives { get; set; }
        public string? Department { get; set; }
        public string? Company { get; set; }
        public string? SubBuilding { get; set; }
        public string? BuildingNumber { get; set; }
        public string? BuildingName { get; set; }
        public string? SecondaryStreet { get; set; }
        public string? Street { get; set; }
        public string? Block { get; set; }
        public string? Neighbourhood { get; set; }
        public string? District { get; set; }
        public string? City { get; set; }
        public string? Line1 { get; set; }
        public string? Line2 { get; set; }
        public string? Line3 { get; set; }
        public string? Line4 { get; set; }
        public string? Line5 { get; set; }
        public string? AdminAreaName { get; set; }
        public string? AdminAreaCode { get; set; }
        public string? Province { get; set; }
        public string? ProvinceName { get; set; }
        public string? ProvinceCode { get; set; }
        public string? PostalCode { get; set; }
        public string? CountryName { get; set; }
        public string? CountryIso2 { get; set; }
        public string? CountryIso3 { get; set; }
        public string? CountryIsoNumber { get; set; }
        public string? SortingNumber1 { get; set; }
        public string? SortingNumber2 { get; set; }
        public string? Barcode { get; set; }
        public string? POBoxNumber { get; set; }
        public string? Label { get; set; }
        public string? Type { get; set; }
        public string? DataLevel { get; set; }
        public string? Field1 { get; set; }
        public string? Field2 { get; set; }
        public string? Field3 { get; set; }
        public string? Field4 { get; set; }
        public string? Field5 { get; set; }
        public string? Field6 { get; set; }
        public string? Field7 { get; set; }
        public string? Field8 { get; set; }
        public string? Field9 { get; set; }
        public string? Field10 { get; set; }
        public string? Field11 { get; set; }
        public string? Field12 { get; set; }
        public string? Field13 { get; set; }
        public string? Field14 { get; set; }
        public string? Field15 { get; set; }
        public string? Field16 { get; set; }
        public string? Field17 { get; set; }
        public string? Field18 { get; set; }
        public string? Field19 { get; set; }
        public string? Field20 { get; set; }
    }

    public class LoqateFormattedAddressResponse
    {
        public LoqateFormattedAddressResponse()
        {
            Items = new List<LoqateFormattedAddress>();
        }

        public IEnumerable<LoqateFormattedAddress> Items { get; set; }
    }
}
