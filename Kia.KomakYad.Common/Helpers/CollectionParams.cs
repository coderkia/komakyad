namespace Kia.KomakYad.Common.Helpers
{
    public class CollectionParams : SearchBaseParams
    {
        public int? AuthorId { get; set; }
        public string Title { get; set; }
        public bool IncludePrivateCollections { get; set; }
    }
}
