using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Domain.Dtos
{
    public class UserForUpdateDto
    {
        [StringLength(250)]
        public string FirstName { get; set; }

        [StringLength(250)]
        public string LastName { get; set; }
    }
}
