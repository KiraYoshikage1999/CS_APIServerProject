using CS_APIServerProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class OrderReadDTO
    {
        public Guid Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public List<Product>? FK_Products { get; set; }
    }
}
