using Kia.KomakYad.DataAccess;
using Kia.KomakYad.DataAccess.Models;
using Kia.KomakYad.Tools.Migrations.DestinationModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Kia.KomakYad.Tools.Migrations
{
    class Program
    {
        private static Dictionary<int, Collection> CollectionMapper = new Dictionary<int, Collection>();
        private static Dictionary<int, ReadCollection> ReadCollectionMapper = new Dictionary<int, ReadCollection>();
        private static Dictionary<int, DataAccess.Models.Card> CardMapper = new Dictionary<int, DataAccess.Models.Card>();
        private static Dictionary<int, int> UserMapper = new Dictionary<int, int>();
        private static DataContext DbContext;
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.Write(new string('/', 50));
            Console.Write(new string('\\', 50));
            Console.WriteLine();
            Console.Write(new string('*', 40));
            Console.Write(" Data Migration Tool ");
            Console.Write(new string('*', 39));
            Console.WriteLine();
            Console.Write(new string('\\', 50));
            Console.Write(new string('/', 50));
            Console.ResetColor();
            Console.WriteLine();
            Console.WriteLine();

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            optionsBuilder.UseSqlServer("Server=.; Database=komakyad; Trusted_Connection=True;");
            DbContext = new DataContext(optionsBuilder.Options);
            Console.WriteLine("Press Enter to migrate data");
            Console.ReadLine();

            if (MoveUsers())
            {
                Console.WriteLine(new string('-', 50));
                MoveFlashes();
                Console.WriteLine(new string('-', 50));
                MoveCards();
                Console.WriteLine(new string('-', 50));
                MoveUserFlashes();
                Console.WriteLine(new string('-', 50));
                MoveUserCards();
            }


            Console.WriteLine();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(new string('/', 50));
            Console.Write(new string('\\', 50));
            Console.WriteLine();
            Console.Write(new string('*', 34));
            Console.Write(" Migration successfully finished ");
            Console.Write(new string('*', 33));
            Console.WriteLine();
            Console.Write(new string('\\', 50));
            Console.Write(new string('/', 50));
            Console.ResetColor();
            Console.ReadLine();
        }

        static bool MoveUsers()
        {
            var users = Reader.GetUsers();
            Console.WriteLine("Moving users");
            Console.WriteLine();
            foreach (var user in users)
            {
                user.Password = "PaSh$$Komak1234Great";
                user.Username = user.FirstName.ToLower();
                var message = $"{user.Email.PadRight(30)}\t{user.FirstName.PadRight(15)}\t{user.LastName.PadRight(15)}".PadRight(100, '-');
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write("\tRegistering ...");
                var registeredUser = AuthClient.Register(user);
                if (registeredUser == null)
                {
                    Console.ResetColor();
                    return false;
                }
                UserMapper.Add(user.Id, registeredUser.Id);
                Console.CursorLeft = message.Length;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\tRegistered      ");
                Console.WriteLine();
                Console.ResetColor();
            }
            Console.WriteLine();
            Console.ResetColor();
            return true;
        }

        static void MoveFlashes()
        {
            List<Flash> flashes = Reader.GetFlashes();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Moving Flashes");
            Console.WriteLine();
            foreach (var flash in flashes)
            {
                var message = $"Flash ID:{flash.FlashId}\tTitle: {flash.Title.PadRight(40)}".PadRight(80);
                Console.Write(message);
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.CursorLeft = 85;
                Console.Write("Moving ...");
                var collection = new Collection
                {
                    AuthorId = UserMapper[flash.CreatedBy],
                    CreatedOn = flash.CreatedOn,
                    Description = flash.Description ?? string.Empty,
                    IsPrivate = false,
                    Title = flash.Title,
                    UniqueId = flash.FlashKey ?? Guid.NewGuid()
                };

                DbContext.Add(collection);
                CollectionMapper.Add(flash.FlashId, collection);
                Console.CursorLeft = 85;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Moved      ");
                Console.ResetColor();
                Console.WriteLine();
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Saving Flashes, Please wait...");
            DbContext.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Flashes are saved successfully");
            Console.ResetColor();
        }

        static void MoveCards()
        {
            List<DestinationModels.Card> cards = Reader.GetCards();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Moving Cards");
            Console.WriteLine();
            int count = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var item in cards)
            {
                count++;
                Console.CursorLeft = 0;
                Console.Write("Cards Count: " + count);
                var card = new DataAccess.Models.Card
                {
                    Answer = item.Answer ?? string.Empty,
                    Question = item.Question ?? string.Empty,
                    CollectionId = CollectionMapper[item.FlashId].Id,
                    Example = item.Example ?? string.Empty,
                    CreatedOn = item.CreatedOn,
                    UniqueId = item.CardKey
                };
                if (card.Answer.Length > 2000)
                {
                    card.Example = "- Answer =>" + card.Answer.Substring(2000);
                    card.Answer = card.Answer.Substring(0, 2000);
                }
                DbContext.Add(card);
                CardMapper.Add(item.CardId, card);
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Saving {count} Cards, Please wait...");
            DbContext.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{count} Cards are saved successfully");
            Console.ResetColor();
        }

        static void MoveUserFlashes()
        {
            List<DestinationModels.UserFlash> userFlashes = Reader.GetUserFlashes();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Moving User Flashes");
            Console.WriteLine();
            int count = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var item in userFlashes)
            {
                count++;
                Console.CursorLeft = 0;
                Console.Write("UserFlash Count: " + count);
                var readCollection = new ReadCollection
                {
                    IsReversed = item.SwitchQuestionAndAnswer,
                    OwnerId = UserMapper[item.UserId],
                    ReadPerDay = item.ReadPerDay ?? 10,
                    CollectionId = CollectionMapper[item.FlashId].Id,
                };
                DbContext.Add(readCollection);
                ReadCollectionMapper.Add(item.UserFlashId, readCollection);
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Saving {count} UserFlashes, Please wait...");
            DbContext.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{count} UserFlashes are saved successfully");
            Console.ResetColor();
        }

        static void MoveUserCards()
        {
            List<UserCard> userCards = Reader.GetUserCards();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Moving User Flashes");
            Console.WriteLine();
            int count = 0;
            Console.ForegroundColor = ConsoleColor.Yellow;
            foreach (var item in userCards)
            {
                count++;
                Console.CursorLeft = 0;
                Console.Write("UserCards Count: " + count);
                var readCard = new ReadCard
                {
                    Due = item.Due,
                    CurrentDeck = item.CurrentDeck,
                    CardId = CardMapper[item.CardId].Id,
                    LastChanged = item.ModifiedOn ?? DateTime.Now.AddDays(-2),
                    OwnerId = UserMapper[item.CreatedBy],
                    PreviousDeck = 0,
                    ReadCollectionId = ReadCollectionMapper[item.UserFlashId].Id
                };
                DbContext.Add(readCard);
            }
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Saving {count} UserCards, Please wait...");
            DbContext.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{count} UserCards are saved successfully");
            Console.ResetColor();
        }
    }
}
