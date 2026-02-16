using CS_APIServerProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class OrderUpdateDTO
    {
        [Required]
        public string? CustomerId { get; set; }
        [Required]
        public string? Status { get; set; }

        [Required]
        public int Number { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public List<OrderItemUpdateDTO>? Items { get; set; }
    }
}
