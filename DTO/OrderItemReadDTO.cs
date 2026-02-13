using CS_APIServerProject.Models;
using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class OrderItemReadDTO
    {
        public Order? Order { get; set; }
        public Product? Product { get; set; }
        [Required,MinLength(0)]
        public int Quanity { get; set; }
        [Required,MinLength(0)]
        public decimal Price { get; set; }
    }
}
