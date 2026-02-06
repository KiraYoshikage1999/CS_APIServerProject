using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class UserCreateDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string? DateOfBirth { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? LastName { get; set; }
    }
}
