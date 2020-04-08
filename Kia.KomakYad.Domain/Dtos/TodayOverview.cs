using System.Collections.Generic;

namespace Kia.KomakYad.Domain.Dtos
{
    public class TodayOverview
    {
        public int CollectionId { get; set; }
        public int OwnerId { get; set; }
        public byte? Deck { get; set; }
        public int DueCount { get; set; }
        public int UpCount { get; set; }
        public int DownCount { get; set; }
    }
}
