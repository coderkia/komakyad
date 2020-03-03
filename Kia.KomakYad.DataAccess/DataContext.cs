using Kia.KomakYad.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Kia.KomakYad.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
           : base(options)
        {

        }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ReadCollection> ReadCollections { get; set; }
        public DbSet<ReadCard> ReadCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReadCollection>()
               .HasOne<User>(c => c.Owner)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReadCollection>()
               .HasOne<Collection>(c => c.Collection)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReadCard>()
               .HasOne<User>(c => c.Owner)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ReadCard>()
               .HasOne<Card>(c => c.Card)
               .WithMany()
               .OnDelete(DeleteBehavior.NoAction);

            base.OnModelCreating(modelBuilder);
        }

    }
}
