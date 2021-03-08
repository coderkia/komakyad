using System;
using Kia.KomakYad.Api.Models;

namespace Kia.KomakYad.Api.Dtos
{
    public class CardToSearchReturnDto
    {
        public int Id { get; set; }

        public Guid UniqueId { get; set; } = Guid.NewGuid();

        public string Answer { get; set; }

        public string Question { get; set; }

        public string Example { get; set; }

        public ReadCardJsonData JsonData { get; set; }
    }
}
