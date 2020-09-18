using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Api.Models
{
    public class ResetPasswordModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 8, ErrorMessage = "You must specify password between 8 and 250 character.")]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
