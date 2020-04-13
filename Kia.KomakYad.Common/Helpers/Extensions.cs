using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;

namespace Kia.KomakYad.Common.Helpers
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

        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static void AddPagination<T>(this HttpResponse response, PagedList<T> pagedList) where T : class
        {
            AddPagination(response,pagedList.CurrentPage, pagedList.PageSize, pagedList.TotalCount, pagedList.TotalPages);
        }


        public static void AddPagination(this HttpResponse response, int currentPage, int itemsPerPage, int totalItems, int totalPages)
        {
            var paginationHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);

            var camelCaseFormatter = new JsonSerializerSettings();
            camelCaseFormatter.ContractResolver = new CamelCasePropertyNamesContractResolver();
            response.Headers.Add("Pagination",
                JsonConvert.SerializeObject(paginationHeader, camelCaseFormatter));
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }

        public static int CalculateAge(this DateTime date)
        {
            var age = DateTime.Today.Year - date.Year;
            if (date.AddYears(age) > DateTime.Today)
                age--;

            return age;
        }

        public static DateTime GetDue(this byte deck)
        {
            switch (deck)
            {
                case 0:
                    return DateTime.Now;
                case 1:
                    return DateTime.Now.AddHours(8);
                case 2:
                    return DateTime.Now.AddDays(1);
                case 3:
                    return DateTime.Now.AddDays(3);
                case 4:
                    return DateTime.Now.AddDays(7);
                case 5:
                    return DateTime.Now.AddDays(14);
                case 6:
                    return DateTime.Now.AddDays(28);
                default:
                    throw new NotImplementedException($"Deck {deck} is not defined.");

            }
        }
    }
}
