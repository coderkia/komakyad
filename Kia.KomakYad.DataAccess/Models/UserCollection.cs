namespace Kia.KomakYad.DataAccess.Models
{
    public class UserCollection
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public int CollectionId { get; set; }
        public Collection Collection { get; set; }
        public byte ReadPerDay { get; set; }
    }
}
