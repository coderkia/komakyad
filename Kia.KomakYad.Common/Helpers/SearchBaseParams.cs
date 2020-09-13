namespace Kia.KomakYad.Common.Helpers
{
    public abstract class SearchBaseParams
    {
        protected const int MaxPageSize = 50;
        public virtual string OrderBy { get; set; }
        public virtual string OrderByDesc { get; set; }

        protected int _pageSize = 10;
        private int _pageNumber = 1;

        public virtual int PageNumber { get => _pageNumber; set => _pageNumber = value > 0 ? value : 1; }
        public virtual int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value < 1)
                {
                    return;
                }
                _pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }

        public void BypassMaxPageSize(int pageSize)
        {
            _pageSize = pageSize;
        }
    }
}
