namespace Kia.KomakYad.Common.Helpers
{
    public class ReadCollectionParams: SearchBaseParams
    {
        public int? OwnerId { get; set; }
        public bool IncludingDeleted { get; set; }
    }
}
