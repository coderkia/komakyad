using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Domain.Dtos
{
    public class UserForUpdateDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
    }
}
