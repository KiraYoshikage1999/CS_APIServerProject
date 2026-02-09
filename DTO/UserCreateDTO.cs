using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class UserCreateDTO
    {

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
