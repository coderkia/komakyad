using System.Collections.Generic;

namespace Kia.KomakYad.Domain.Dtos
{
    public class UserWithRolesDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public List<string> Roles { get; set; }

    }
}
