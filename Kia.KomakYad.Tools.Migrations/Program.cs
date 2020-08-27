using System;

namespace Kia.KomakYad.Tools.Migrations
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Flash ID\t Flash Title");
            Console.WriteLine("--------\t -----------");
            var flashes = Reader.GetFlashes(1);
            foreach (var item in flashes)
            {
                Console.WriteLine(item.FlashId.ToString().PadLeft(8) + "\t" + item.Title);
            }
            Console.WriteLine();
            Console.WriteLine("Enter the ID you want to migrate");
            Console.ReadLine();
        }
    }
}
