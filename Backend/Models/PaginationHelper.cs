namespace StreamerApi.Models
{
    public class PaginationHelper
    {
        public static Pager<List<T>> CreatePagedReponse<T>(List<T> pagedData, PaginationFilter validFilter, int totalRecords)
        {
            var respose = new Pager<List<T>>(pagedData, validFilter.PageNumber, validFilter.PageSize, totalRecords);
            var totalPages = ((double)totalRecords / (double)validFilter.PageSize);
            int roundedTotalPages = Convert.ToInt32(Math.Ceiling(totalPages));

            respose.TotalPages = roundedTotalPages;
            return respose;
        }
    }
}
