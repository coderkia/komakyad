namespace Kia.KomakYad.Domain.Extensions
{
    public static class DeckExtensions
    {
        public static bool AllDeck(this byte deck)
        {
            return deck == byte.MaxValue;
        }

    }
}
