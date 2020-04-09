using Kia.KomakYad.DataAccess.Models;

namespace Kia.KomakYad.Api.Dtos
{
    public class ReadCollectionToReturnDto
    {
        public int Id { get; set; }
        public bool IsReversed { get; set; }
        public int Priority { get; set; }
        public Collection Collection { get; set; }
        public byte ReadPerDay { get; set; }
        public bool? Deleted { get; set; }
    }
}
