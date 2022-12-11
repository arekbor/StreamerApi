namespace StreamerApi.Models
{
    public class Pager<T> : Response<T>
    {
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public Pager(T data, int pageNumber, int pageSize)
        {
            this.Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
        }
    }
}
