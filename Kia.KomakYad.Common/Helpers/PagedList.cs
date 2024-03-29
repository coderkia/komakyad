﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kia.KomakYad.Common.Helpers
{
    public class PagedList<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList()
        {

        }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, SearchBaseParams searchParams)
        {
            var count = await source.CountAsync();
           
            if(!string.IsNullOrWhiteSpace(searchParams.OrderBy))
                source = source.OrderBy(searchParams.OrderBy);

            if (!string.IsNullOrWhiteSpace(searchParams.OrderByDesc))
                source = source.OrderByDescending(searchParams.OrderByDesc);

            var items = await source.Skip((searchParams.PageNumber - 1) * searchParams.PageSize).Take(searchParams.PageSize).ToListAsync();
            return new PagedList<T>(items, count, searchParams.PageNumber, searchParams.PageSize);
        }


    }
}
