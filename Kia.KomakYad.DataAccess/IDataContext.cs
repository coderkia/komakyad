﻿using Kia.KomakYad.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Kia.KomakYad.DataAccess
{
    public interface IDataContext
    {
        DbSet<Card> Cards { get; set; }
        DbSet<Collection> Collections { get; set; }
        DbSet<ReadCollection> ReadCollections { get; set; }
        DbSet<ReadCard> ReadCards { get; set; }
    }
}
