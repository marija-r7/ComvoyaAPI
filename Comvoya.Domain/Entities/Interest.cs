using System.ComponentModel.DataAnnotations;

namespace ComvoyaAPI.Domain.Entities
{
    public class Interest
    {
        public int Id { get; set; }
        [Required, MaxLength(30)]
        public required string Name { get; set; }
        public ICollection<UserInterest> UserInterests { get; set; } = new List<UserInterest>();
    }
}
