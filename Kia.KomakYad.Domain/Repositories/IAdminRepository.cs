﻿using Kia.KomakYad.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kia.KomakYad.Domain.Repositories
{
    public interface IAdminRepository
    {
        Task SetCollectionLimit(User user, int limit);
        Task SetCardLimit(User user, int limit);
    }
}
