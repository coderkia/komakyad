using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.DataAccess.Models
{
    public class ReadCollection
    {
        public int Id { get; set; }
        public User Owner { get; set; }
        public int OwnerId { get; set; }
        public bool IsReversed { get; set; }
        public int Priority { get; set; }
        public int CollectionId { get; set; }
        public Collection Collection { get; set; }

        [Range(1, 100)]
        public byte ReadPerDay { get; set; }
        public bool Deleted { get; set; }
    }
}
