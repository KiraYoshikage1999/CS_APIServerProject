using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CS_APIServerProject.DTO
{
    public class ProductCreateDTO
    {
        [JsonIgnore]
        public Guid Id { get; set; } 
        [Required, MinLength(3)]   
        public string? Brand { get; set; }
        [Required, MinLength(3)]
        public string? Model { get; set; }
        public string? Description { get; set; }
        [Range(0,9999999)]
        public decimal? Price { get; set; }
        [Range(0,int.MaxValue)]
        public int Quanity { get; set; }
        public string? FK_Salesman { get; set; }
        public string? Currency { get; set; }

        public CharacteristicsDTO? Characteristics { get; set; }

        //public string? ImagePath { get; set; }
        public IFormFile Image { get; set; }

        
    }
}
