using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Api.Models
{
    public class MoveCardModel
    {
        [Range(0, 6)]
        public int DestinationDeck { get; set; }

        [Range(0, 6)]
        public int? TargetDeck { get; set; }
    }
}
