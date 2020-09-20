using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kia.KomakYad.DataAccess.Models
{
    [Table("Cards", Schema = "dbo")]
    public class Card
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(2000)]
        public string Answer { get; set; }

        [Required]
        [StringLength(2000)]
        public string Question { get; set; }

        [StringLength(2000)]
        public string Example { get; set; }

        public string JsonData { get; set; }

        public virtual Collection Collection { get; set; }
        public int CollectionId { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; set; }

       
    }
}
