using Kia.KomakYad.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kia.KomakYad.DataAccess
{
    public interface IDataContext
    {
        DbSet<Card> Cards { get; set; }
        DbSet<Collection> Collections { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<ReadCollection> ReadCollections { get; set; }
        DbSet<ReadCard> ReadCards { get; set; }
    }
}
