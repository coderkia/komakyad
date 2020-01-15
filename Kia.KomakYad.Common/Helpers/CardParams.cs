namespace Kia.KomakYad.Common.Helpers
{
    public class CardParams
    {
        private const int MaxPageSize = 50;

        private int pageSize = 10;
        public int? CollectionId { get; set; }
        public int? Deck { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }
        public string OrderBy { get; set; }
    }
}
