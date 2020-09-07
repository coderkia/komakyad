using System;
using System.Collections.Generic;
using System.Text;

namespace Kia.KomakYad.Domain.Dtos
{
    public class UserDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool EmailConfirmed { get; set; }

    }
}
