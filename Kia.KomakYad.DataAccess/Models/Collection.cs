using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kia.KomakYad.DataAccess.Models
{
    [Table("Collections", Schema = "dbo")]
    public class Collection
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; } = Guid.NewGuid();
        public int AuthorId { get; set; }
        public virtual User Author { get; set; }

        [Required]
        [StringLength(450)]
        public string Title { get; set; }

        [Required]
        [StringLength(2000)]
        public string Description { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public DateTime? ModifiedOn { get; set; }
        public virtual ICollection<Card> Cards { get; set; }
        public bool IsPrivate { get; set; }

    }
}
