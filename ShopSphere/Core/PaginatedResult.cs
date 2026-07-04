namespace ShopSphere.Core
{
    public class PaginatedResult<T>
    {
        public List<T> Items { get; set; } = [];
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public PaginatedResult() { }

        public PaginatedResult(List<T> items, int totalRecords, int pageNumber, int pageSize)
        {
            Items = items;
            TotalRecords = totalRecords;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}
