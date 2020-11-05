namespace Kia.KomakYad.Common.Helpers
{
    public class ReadCollectionParams: SearchBaseParams
    {
        public int? OwnerId { get; set; }
        public bool IncludeDeleted { get; set; }
    }
}
