using CS_APIServerProject.DTO;
using System.ComponentModel.DataAnnotations;
namespace CS_APIServerProject.Models
{
    public class Order
    {
        public Guid Id { get; set; }

        public string? CustomerId { get; set; }
        public string? Status { get; set; }
    
        [Required]
        public int Number { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public List<OrderItem>? Items { get; set; }


    }
}
