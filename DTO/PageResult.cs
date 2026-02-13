namespace CS_APIServerProject.DTO
{
    public class PageResult<T>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / Page);

        public List<T> Items { get; set; } = new();
    }
}
