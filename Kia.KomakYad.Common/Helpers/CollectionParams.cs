namespace Kia.KomakYad.Common.Helpers
{
    public class CollectionParams
    {
        private const int MaxPageSize = 50;
        public int PageNumber { get; set; }

        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }
        public string OrderBy { get; set; }
        public int UserId { get; set; }
    }
}
