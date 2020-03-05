using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Common.Helpers
{
    public class CardParams : SearchBaseParams
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int CollectionId { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }
        public string Example { get; set; }
        public int UserId { get; set; }
    }
}
