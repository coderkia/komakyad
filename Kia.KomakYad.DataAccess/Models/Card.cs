using System;
using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.DataAccess.Models
{
    public class Card
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Required]
        public string Answer { get; set; }

        [Required]
        public string Question { get; set; }

        public string Example { get; set; }
        public string ExtraData { get; set; }
        public Collection Collection { get; set; }
        public int CollectionId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

       
    }
}
