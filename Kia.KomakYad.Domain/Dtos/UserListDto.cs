namespace Kia.KomakYad.Domain.Dtos
{
    public class UserListDto
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string Email { get; set; }
        public string CreatedOn { get; set; }
        public bool Locked { get; set; }
        public int? CollectionLimit { get; set; }
        public int? CardLimit { get; set; }
        public bool EmailConfirmed { get; set; }

    }
}
