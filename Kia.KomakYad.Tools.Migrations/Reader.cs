using Dapper;
using Kia.KomakYad.Tools.Migrations.DestinationModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Kia.KomakYad.Tools.Migrations
{
    class Reader
    {
        public static List<Flash> GetFlashes()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString.Old))
            {
                var flashes = db.Query<Flash>($"SELECT * FROM Flash");
                return flashes.ToList();
            }
        }

        public static List<Card> GetCards()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString.Old))
            {
                return db.Query<DestinationModels.Card>($"SELECT * FROM Card").ToList();
            }
        }
        public static List<UserCard> GetUserCards()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString.Old))
            {
                return db.Query<UserCard>($"SELECT * FROM UserCard").ToList();
            }
        }

        public static List<UserFromDb> GetUsers()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString.Old))
            {
                return db.Query<UserFromDb>($"SELECT * FROM [asp].[User]").ToList();
            }
        }

        internal static List<UserFlash> GetUserFlashes()
        {
            using (IDbConnection db = new SqlConnection(ConnectionString.Old))
            {
                return db.Query<UserFlash>($"SELECT * FROM UserFlash").ToList();
            }
        }
    }
}
