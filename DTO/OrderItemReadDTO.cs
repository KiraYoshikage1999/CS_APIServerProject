using CS_APIServerProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class OrderItemReadDTO
    {
        public Guid OrderId { get; set; }
        public OrderReadDTO? Order { get; set; } 
        public Guid ProductId { get; set; }
        public ProductReadDTO? Product { get; set; }
        [Required]
        public int Quanity { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
