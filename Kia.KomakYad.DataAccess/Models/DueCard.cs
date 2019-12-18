using System;
using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.DataAccess.Models
{
    public class DueCard
    {
        public int OwnerId { get; set; }
        public int CardId { get; set; }
        public Card Card { get; set; }
        public User Owner { get; set; }
        public DateTime Due { get; set; }

        [Required]
        [Range(0, 6)]
        public byte CurrentDeck { get; set; }

        [Required]
        [Range(0, 6)]
        public byte PreviousDeck { get; set; }
        public DateTime LastChanged { get; set; }

    }
}
