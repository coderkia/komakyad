namespace Kia.KomakYad.Common.Helpers
{
    public class ReadCardParams : SearchBaseParams
    {
        public string Answer { get; set; }
        public string Question { get; set; }
        public string Example { get; set; }
        public byte? Deck { get; set; }
        public bool OnlyDued { get; set; } = true;

    }
}
