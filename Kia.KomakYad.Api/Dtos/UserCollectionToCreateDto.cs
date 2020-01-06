using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Api.Dtos
{
    public class UserCollectionToCreateDto
    {
        [Range(1, 255)]
        public int ReadPerDay { get; set; } = 10;

        public bool IsReversed { get; set; }
    }
}
