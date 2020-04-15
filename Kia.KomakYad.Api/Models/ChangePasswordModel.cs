using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.Api.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [StringLength(30)]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(30)]
        public string NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }
}
