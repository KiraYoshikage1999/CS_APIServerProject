using System.ComponentModel.DataAnnotations;
namespace CS_APIServerProject.Models
{
    public class User
    {
        public Guid Id { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        [Required, Phone]
        public string? PhoneNumber { get; set; }
        [Required, EmailAddress]
        public string? Email { get; set; }
        [Required, MinLength(3), MaxLength(10)]
        public string? Name { get; set; }
        [Required, MinLength(5), MaxLength(20)]
        public string? LastName { get; set; }
    }
}
