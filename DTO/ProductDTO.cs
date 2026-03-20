using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    ////Read DTO
    //public record ProductReadDTO(Guid Id, [Required, MaxLength(30)] string? Brand, [Required, MaxLength(40)] string? Model,
    //    string? Description, [MinLength(0)] decimal? Price, [MinLength(0)] int Quanity, string? Currency,
    //    [Required] CharacteristicsDTO? Characteristics, string? ImageCode, [Required] DateTime? CreatedAt);
    ////Create DTO
    //public record ProductCreateDTO( [Required, MaxLength(30)] string? Brand, [Required, MaxLength(40)] string? Model,
    //    string? Description, [MinLength(0)] decimal? Price, [MinLength(0)] int Quanity, string? Currency,
    //    [Required] CharacteristicsDTO? Characteristics, string? ImageCode, [Required] DateTime? CreatedAt);
    ////Update DTO
    //public record ProductUpdateDTO( [Required, MaxLength(30)] string? Brand, [Required, MaxLength(40)] string? Model,
    //    string? Description, [MinLength(0)] decimal? Price, [MinLength(0)] int Quanity, string? Currency,
    //    [Required] CharacteristicsDTO? Characteristics, string? ImageCode, [Required] DateTime? CreatedAt);

    //Product Query
    //public record ProductQuery([Required, MaxLength(30)] string? Brand, [Required] string? State,
    //    [Required] decimal? PriceFrom, [Required] decimal? PriceTo, string? SortBy = "Brand", string? SortDir = "asc",
    //    int Page, [Required] DateTime? CreatedAt)
    //{
    //    private int _pageSize = 10;
    //    public int PageSize
    //    {
    //        get => _pageSize;
    //        set => _pageSize = value < 1 ? 10 : (value > 50 ? 50 : value);
    //    }
    //}

    //Object from Model to DTO
    public record ProductDTO(Guid Id, [Required, MaxLength(30)] string? Brand, [Required, MaxLength(40)] string? Model,
        string? Description, [Range(0, double.MaxValue)] decimal? Price, [Range(0, int.MaxValue)] int Quanity, string? Currency,
        [Required] CharacteristicsDTO? Characteristics, string? ImageCode, [Required] DateTime? CreatedAt);


    
}
