using System;
using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.DataAccess.Models
{
    public class CustomizedCard
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [Required]
        public string JsonData { get; set; }
        public Card OriginalCard { get; set; }
        public int OriginalCardId { get; set; }
        public int Owner { get; set; }
    }
}
