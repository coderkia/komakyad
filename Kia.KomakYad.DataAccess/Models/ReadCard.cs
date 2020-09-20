using System;
using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.DataAccess.Models
{
    public class ReadCard
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public virtual User Owner { get; set; }
        public int CardId { get; set; }
        public virtual Card Card { get; set; }
        public int ReadCollectionId { get; set; }
        public virtual ReadCollection ReadCollection { get; set; }
        public string JsonData { get; set; }
        public DateTime Due { get; set; } = DateTime.Now;

        [Required]
        [Range(0, 6)]
        public byte CurrentDeck { get; set; }

        [Required]
        [Range(0, 6)]
        public byte PreviousDeck { get; set; }
        public DateTime LastChanged { get; set; } = DateTime.Now;

    }
}
