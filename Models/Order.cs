using System.ComponentModel.DataAnnotations;
namespace CS_APIServerProject.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public DateTime CreatedAt { get; set; }

        public List<Product>? FK_Products { get; set; }


    }
}
