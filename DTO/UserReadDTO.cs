using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class UserReadDTO
    {
        public Guid Id { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required,Phone]
        public string? PhoneNumber { get; set; }
        [Required,EmailAddress]
        public string? Email { get; set; }
        [Required,]
        public string? Name { get; set; }
        [Required]
        public string? LastName { get; set; }
    }
}
