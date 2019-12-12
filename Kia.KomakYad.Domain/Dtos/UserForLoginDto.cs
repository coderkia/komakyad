using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Domain.Dtos
{
    public class UserForLoginDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
