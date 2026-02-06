using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class UserUpdateDTO
    {
        public Guid Id { get; set; }

        [Required]
        public string? DateOfBirth { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required,MinLength(3),MaxLength(10)]
        public string? Name { get; set; }
        [Required, MinLength(5), MaxLength(20)]
        public string? LastName { get; set; }
    }
}
