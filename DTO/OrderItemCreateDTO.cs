using CS_APIServerProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class OrderItemCreateDTO
    {
        public Guid OrderId { get; set; }
        public OrderCreateDTO? Order { get; set; }
        public Guid ProductId { get; set; }
        public ProductCreateDTO? Product { get; set; }
        [Required]
        public int Quanity { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
