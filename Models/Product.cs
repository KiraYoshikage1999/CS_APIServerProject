using CS_APIServerProject.DTO;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        [Required, MaxLength(30)]
        public string? Brand { get; set; }
        [Required, MaxLength(40)]
        public string? Model { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int Quanity { get; set; }
        public string? FK_Salesman { get; set; }
        public string? Currency { get; set; }

        public Characteristics? Characteristics { get; set; } 

        //public string? ImagePath { get; set; }
        //public IFormFile? Image { get; set; }
        
        public string? imageCode { get; set; }

        public DateTime? CreatedAt { get; set; }

        public static implicit operator Product(Task<ActionResult<ProductReadDTO>> v)
        {
            throw new NotImplementedException();
        }
    }
}
