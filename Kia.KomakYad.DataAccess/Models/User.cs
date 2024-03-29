﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kia.KomakYad.DataAccess.Models
{
    [Table("Users", Schema = "dbo")]
    public class User : IdentityUser<int>
    {
        public Guid UniqueId { get; set; } = Guid.NewGuid();

        [StringLength(250)]
        public string FirstName { get; set; }

        [StringLength(250)]
        public string LastName { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public int? CollectionLimit { get; set; }
        public int? CardLimit { get; set; }
        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
