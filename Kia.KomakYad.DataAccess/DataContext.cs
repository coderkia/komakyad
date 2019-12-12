using Kia.KomakYad.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Kia.KomakYad.DataAccess
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
           : base(options)
        {

        }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserCollection> UserCollections { get; set; }
        public DbSet<CustomizedCard> CustomizedCards { get; set; }
        public DbSet<DueCard> DueCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCollection>()
                .HasKey(k => new { k.CollectionId, k.UserId });

            modelBuilder.Entity<DueCard>()
                .HasKey(k => new { k.OwnerId, k.CardId });

            base.OnModelCreating(modelBuilder);
        }
        
    }
}
