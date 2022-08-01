using System;

namespace coop2._0.Model
{
    public class PaginationResponse
    {
        public int PageNumber { get; private set; } = 1;

        public int PageSize { get; private set; } = 10;

        public int TotalRecords { get; set; }

        public PaginationResponse(int pageNumber, int pageSize, int totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
        }
    }
}