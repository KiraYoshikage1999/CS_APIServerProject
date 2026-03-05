using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    //User Read
    public record UserReadDTO( Guid Id,
        [Required] DateTime? DateOfBirth, [Required,Phone] string? PhoneNumber
        , [Required, EmailAddress] string? Email , [Required, MinLength(3), MaxLength(10)] string? Name 
        , [Required, MinLength(5), MaxLength(20)] string? LastName);
    //User Update
    public record UserUpdateDTO(
        [Required] DateTime? DateOfBirth, [Required, Phone] string? PhoneNumber
        , [Required, EmailAddress] string? Email, [Required, MinLength(3), MaxLength(10)] string? Name
        , [Required, MinLength(5), MaxLength(20)] string? LastName);
    //User Create
    public record UserCreateDTO(
        [Required] DateTime? DateOfBirth, [Required, Phone] string? PhoneNumber
        , [Required, EmailAddress] string? Email, [Required, MinLength(3), MaxLength(10)] string? Name
        , [Required, MinLength(5), MaxLength(20)] string? LastName);

    //User DTO copy of Model DTO. For... something?
    public record UserDTO( Guid Id,
        [Required] DateTime? DateOfBirth, [Required, Phone] string? PhoneNumber
        , [Required, EmailAddress] string? Email, [Required, MinLength(3), MaxLength(10)] string? Name
        , [Required, MinLength(5), MaxLength(20)] string? LastName);
}
