namespace ComvoyaAPI.Application.Models
{
    public class InterestsDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public List<UserDto> Users { get; set; }
    }
}
