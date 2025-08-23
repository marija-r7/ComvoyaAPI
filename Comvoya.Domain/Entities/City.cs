namespace ComvoyaAPI.Domain.Entities
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int CountryId { get; set; }

        public Country Country { get; set; }
    }
}