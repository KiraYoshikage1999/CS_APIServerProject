namespace CS_APIServerProject.DTO
{
    public class OrderQuery
    {
        public int Number {  get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedAt { get; set; }

        //Sorting
        public string SortBy { get; set; } = "Number";
        public string SortDir { get; set; } = "asc";

        public int Page { get; set; } = 1;
        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 1 ? 10 : (value > 50 ? 50 : value);
        }

    }
}
