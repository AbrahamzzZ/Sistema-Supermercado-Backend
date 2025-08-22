namespace Utilities.Shared
{
    public class Paginacion<P>
    {
        public List<P> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
