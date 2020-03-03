using System;
using System.ComponentModel.DataAnnotations;

namespace Kia.KomakYad.DataAccess.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [StringLength(250)]
        public string FirstName { get; set; }

        [StringLength(250)]
        public string LastName { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
