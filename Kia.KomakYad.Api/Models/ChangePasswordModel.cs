using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Api.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 8, ErrorMessage = "You must specify password between 8 and 250 character.")]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
