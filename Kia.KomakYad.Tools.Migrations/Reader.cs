using Dapper;
using Kia.KomakYad.Tools.Migrations.DestinationModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Kia.KomakYad.Tools.Migrations
{
    class Reader
    {
        public static List<Flash> GetFlashes(int userId)
        {
            using (IDbConnection db = new SqlConnection(ConnectionString.Old))
            {
                var flashes = db.Query<Flash>($"SELECT * FROM Flash WHERE CreatedBy = {userId}");
                return flashes.ToList();
            }
        }
    }
}
