using System;

namespace Kia.KomakYad.Api.Dtos
{
    public class CardToReturnDto
    {
        public int Id { get; set; }
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        public string Answer { get; set; }

        public string Question { get; set; }

        public string Example { get; set; }
        public string ExtraData { get; set; }
        public CollectionToReturnDto Collection { get; set; }
    }
}
