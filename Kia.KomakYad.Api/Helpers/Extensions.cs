using Kia.KomakYad.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kia.KomakYad.Api.Helpers
{
    public static class Extensions
    {
        public static IEnumerable<T> Map<T>(this IEnumerable<T> source, Action<T> action) 
        { 
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }
    }
}
