namespace ComvoyaAPI.Domain.Entities
{
    public class Interests
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<User> Users { get; set; }
    }
}
