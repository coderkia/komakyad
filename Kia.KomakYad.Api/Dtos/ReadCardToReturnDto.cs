using Kia.KomakYad.Api.Models;

namespace Kia.KomakYad.Api.Dtos
{
    public class ReadCardToReturnDto
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int CardId { get; set; }
        public CardToReturnDto Card { get; set; }
        public ReadCardJsonData JsonData { get; set; }
        public byte CurrentDeck { get; set; }
    }
}
