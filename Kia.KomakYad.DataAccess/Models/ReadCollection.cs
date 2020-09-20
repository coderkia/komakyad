using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kia.KomakYad.DataAccess.Models
{
    [Table("ReadCollections", Schema = "dbo")]
    public class ReadCollection
    {
        public int Id { get; set; }
        public virtual User Owner { get; set; }
        public int OwnerId { get; set; }
        public bool IsReversed { get; set; }
        public int Priority { get; set; }
        public int CollectionId { get; set; }
        public virtual Collection Collection { get; set; }

        [Range(1, 100)]
        public byte ReadPerDay { get; set; }
        public bool Deleted { get; set; }
        public DateTime LastUpdateCheck { get; set; }
    }
}
