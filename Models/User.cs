namespace CS_APIServerProject.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
        public string? LastName { get; set; }
    }
}
