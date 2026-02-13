using System.ComponentModel.DataAnnotations;

namespace CS_APIServerProject.DTO
{
    public class ProductQuery
    {
        //Filtering
        [Required]
        public string? Brand { get; set; }
        [Required]
        public string? State { get; set; }
        [Required]
        public decimal PriceFrom { get; set; }
        [Required]
        public decimal PriceTo { get; set; }

        //Sorting
        public string SortBy { get; set; } = "Brand";
        public string SortDir { get; set; } = "asc";

        //Pagination
        public int Page { get; set; }
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : (value > 50 ? 50 : value);
        }

        public DateTime? CreatedAt { get; set; }
    }
}
