using Kia.KomakYad.Domain.Dtos;

namespace Kia.KomakYad.Tools.Migrations.DestinationModels
{
    class UserFromDb : UserForRegisterDto
    {
        public int Id { get; set; }
        public string Surname
        {
            set
            {
                LastName = value;
            }
        }
        public string Forename
        {
            set
            {
                FirstName = value;
            }
        }
    }
}
