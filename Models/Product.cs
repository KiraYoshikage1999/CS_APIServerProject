namespace CS_APIServerProject.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int quanity { get; set; }
        public string? FK_Salesman { get; set; }
        public string? currency { get; set; }

        public Characteristics? Characteristics { get; set; } 




    }
}
