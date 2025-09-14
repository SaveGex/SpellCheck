using MR.AspNetCore.Pagination;

namespace DomainData.Records
{
    public record KeysetPaginationAfterResult<T> : KeysetPaginationResult<T>
    {
        public string After;
        public KeysetPaginationAfterResult(string After, IReadOnlyList<T> Data, int TotalCount, int PageSize, bool HasPrevious, bool HasNext) : base(Data, TotalCount, PageSize, HasPrevious, HasNext)
        {
            this.After = After;
        }

        public KeysetPaginationAfterResult(string After, KeysetPaginationResult<T> result)
            : base(result)
        {
            this.After = After;
        }
    }
}
