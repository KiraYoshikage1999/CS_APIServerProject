using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class ProductCreateDTO
    {
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
    }
}
