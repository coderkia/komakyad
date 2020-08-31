using System;
using System.Collections.Generic;
using System.Text;

namespace Kia.KomakYad.Tools.Migrations.DestinationModels
{
    class UserCard
    {
        public int UserCardId { get; set; }
        public System.Guid UserCardKey { get; set; }
        public int CardId { get; set; }
        public int UserFlashId { get; set; }
        public byte CurrentDeck { get; set; }
        public int ReviewCount { get; set; }
        public int CreatedBy { get; set; }
        public string Hint { get; set; }
        public System.DateTime Due { get; set; }
        public System.DateTime? ModifiedOn { get; set; }
    }
}
