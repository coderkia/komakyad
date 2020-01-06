using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.DataAccess.Models
{
    public class UserCollection
    {
        public User User { get; set; }
        public int UserId { get; set; }
        public bool IsReversed { get; set; }
        public int Priority { get; set; }
        public int CollectionId { get; set; }
        public Collection Collection { get; set; }
        [Range(1, 100)]
        public byte ReadPerDay { get; set; }
    }
}
