using System;
using System.Collections.Generic;
using System.Text;

namespace Kia.KomakYad.Tools.Migrations.DestinationModels
{
    class UserFlash
    {
        public int UserFlashId { get; set; }
        public int UserId { get; set; }
        public int FlashId { get; set; }
        public System.DateTime ExpirationDate { get; set; }
        public Nullable<int> CardCount { get; set; }
        public bool SwitchQuestionAndAnswer { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<byte> ReadPerDay { get; set; }
        public bool Deleted { get; set; }
    }
}
