using CS_APIServerProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class OrderItemUpdateDTO
    {
        public Guid OrderId { get; set; }
        public OrderUpdateDTO? Order { get; set; }
        public Guid ProductId { get; set; }
        public ProductUpdateDTO? Product { get; set; }
        [Required]
        public int Quanity { get; set; }
        [Required]
        public decimal Price { get; set; }
    }
}
