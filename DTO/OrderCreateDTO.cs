using CS_APIServerProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class OrderCreateDTO
    {
        [Required]
        public string? CustomerId { get; set; }
        [Required]
        public string? Status { get; set; }

        [Required, MinLength(0)]
        public int Number { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public List<OrderItemCreateDTO>? Items { get; set; }
    }
}
