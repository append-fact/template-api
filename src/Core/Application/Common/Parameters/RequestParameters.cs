namespace Application.Common.Parameters
{
    public class RequestParameters
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; } 

        public RequestParameters()
        {
            PageNumber = 1;
            PageSize = 10;
        }

        public RequestParameters(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;
            PageSize = pageSize > 10 ? 10 : pageSize;
        }

        public bool IsValidOrderByColumn<T>()
        {
            return string.IsNullOrEmpty(SortColumn) || typeof(T).GetProperty(SortColumn) != null;
        }
    }
}