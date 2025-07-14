namespace Application.Common.Wrappers
{
    public class PagedResponse<T> : Response<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);

        public PagedResponse(T data, int pageNumer, int pageSize, int totalRecords)
        {
            PageNumber = pageNumer;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            Data = data;
            Message = null;
            Succeeded = true;
            Errors = null;
        }
    }
}