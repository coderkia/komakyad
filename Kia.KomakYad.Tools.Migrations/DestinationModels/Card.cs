using System;
using System.Collections.Generic;
using System.Text;

namespace Kia.KomakYad.Tools.Migrations.DestinationModels
{
    public class Card
    {
        public int CardId { get; set; }
        public System.Guid CardKey { get; set; }
        public int FlashId { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Example { get; set; }
        public string Hint { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public byte AnswerCount { get; set; }
    }
}
