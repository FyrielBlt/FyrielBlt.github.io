using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BackPfe.Paginate
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable,
           Paginations pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * pagination.QuantityPage)
                .Take(pagination.QuantityPage);
        }
    }
}
