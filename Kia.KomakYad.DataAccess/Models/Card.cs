using System;
using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.DataAccess.Models
{
    public class Card
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; }

        [Required]
        public string JsonData { get; set; }
        public Collection Collection { get; set; }
        public int CollectionId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
