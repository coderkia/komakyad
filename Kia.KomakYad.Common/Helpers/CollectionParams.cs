namespace Kia.KomakYad.Common.Helpers
{
    public class CollectionParams : SearchBaseParams
    {
        public override int PageSize
        {
            get => base.PageSize;
            set => base.PageSize = value < 10 ? value : 10;
        }
        public int? AuthorId { get; set; }
        public string Title { get; set; }
        public bool IncludePrivateCollections { get; set; }
    }
}
