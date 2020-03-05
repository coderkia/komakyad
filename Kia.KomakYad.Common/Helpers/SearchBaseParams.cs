using System;
using System.Collections.Generic;
using System.Text;

namespace Kia.KomakYad.Common.Helpers
{
    public abstract class SearchBaseParams
    {
        protected const int MaxPageSize = 50;
        public virtual string OrderBy { get; set; }

        protected int pageSize = 10;
        public virtual int PageNumber { get; set; } = 1;
        public virtual int PageSize
        {
            get { return pageSize; }
            set
            {
                pageSize = value > MaxPageSize ? MaxPageSize : value;
            }
        }
    }
}
