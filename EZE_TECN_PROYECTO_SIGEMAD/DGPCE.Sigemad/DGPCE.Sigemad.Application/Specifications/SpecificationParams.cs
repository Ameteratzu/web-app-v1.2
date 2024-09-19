namespace DGPCE.Sigemad.Application.Specifications
{
    public abstract class SpecificationParams
    {
        public string? Sort { get; set; }

        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = (value < 1) ? 1 : value;
        }

        private const int MaxPageSize = 50;
        private int _pageSize = 3;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }

        public string? Search { get; set; }
    }
}
