namespace Kia.KomakYad.Domain.Extensions
{
    public static class DeckExtensions
    {
        public static bool IsAllDeckNeeded(this byte deck)
        {
            return deck == byte.MaxValue;
        }

    }
}
