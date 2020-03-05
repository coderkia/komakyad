namespace Kia.KomakYad.Common.Helpers
{
    public class UserParams: SearchBaseParams
    {

        public int UserId { get; set; }
        public string Gender { get; set; }
        public int MinAge { get; set; } = 18;
        public int MaxAge { get; set; } = 99;


        public bool Likees { get; set; }
        public bool Likers { get; set; }
    }
}
