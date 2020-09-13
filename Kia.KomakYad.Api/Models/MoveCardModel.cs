using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Api.Models
{
    public class MoveCardModel
    {
        [Range(0, 6)]
        public byte DestinationDeck { get; set; }

        [Range(0, 6)]
        public byte? TargetDeck { get; set; }
    }
}
