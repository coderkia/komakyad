using System;
using System.Collections.Generic;
using System.Text;

namespace Kia.KomakYad.Tools.Migrations.DestinationModels
{
    class Flash
    {
        public int FlashId { get; set; }
        public Nullable<System.Guid> FlashKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Nullable<int> CardCount { get; set; }
        public Nullable<long> Price { get; set; }
        public Nullable<byte> AuthorPercentage { get; set; }
        public byte AccessType { get; set; }
        public long ReadRate { get; set; }
    }
}
