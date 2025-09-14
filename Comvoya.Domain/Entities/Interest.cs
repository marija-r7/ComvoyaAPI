namespace ComvoyaAPI.Domain.Entities
{
    public class Interest
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<User>? Users { get; set; }
    }
}
