namespace CMS.Application.DTOs
{
    public class PaginationDto
    {
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<object> Items { get; set; }
    }
}
