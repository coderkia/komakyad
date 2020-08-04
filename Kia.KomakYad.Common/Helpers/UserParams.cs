namespace Kia.KomakYad.Common.Helpers
{
    public class UserParams: SearchBaseParams
    {

        public int? UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
    }
}
