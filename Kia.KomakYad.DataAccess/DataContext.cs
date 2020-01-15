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
        public DbSet<ReadCollection> ReadCollections { get; set; }
        public DbSet<ReadCard> ReadCards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        
    }
}
