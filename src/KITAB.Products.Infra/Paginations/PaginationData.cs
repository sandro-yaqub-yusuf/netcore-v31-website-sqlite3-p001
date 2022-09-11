namespace KITAB.Products.Infra.Paginations
{
    public class PaginationData
    {
        public int CurrentPage { get; set; }
        public string Filter { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }
        public string Order { get; set; }
        public int PageSize { get; set; }
        public string SortField { get; set; }
        public string SortOrder { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
