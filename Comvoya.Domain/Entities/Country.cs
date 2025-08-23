namespace ComvoyaAPI.Domain.Entities
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<City> Cities { get; set; } = new List<City>();
    }
}
