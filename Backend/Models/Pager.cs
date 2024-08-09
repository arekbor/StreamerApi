namespace StreamerApi.Models
{
    public class Pager<T> : Response<T>
    {
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public Pager(T items, int pageNumber, int pageSize, int totalRecords)
        {
            this.Items = items;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
        }
    }
}
