using System;
using System.Collections.Generic;
using System.Linq;

namespace Pdsr.Data.Extensions
{
    public static partial class IEnumerableExtensions
    {
        public static IEnumerable<TSource> Page<TSource>(this IEnumerable<TSource> source, int pageNumber, int pageSize = 100)
        {
            if (source is null) throw new ArgumentNullException($"{nameof(source)} cannot be null.");
            if (pageNumber < 1) throw new ArgumentOutOfRangeException($"{nameof(source)} must be greater than 0.");
            if (pageSize < 1) throw new ArgumentOutOfRangeException($"{nameof(pageSize)} must be greater than 0.");

            int offset = (pageNumber - 1) * pageSize;
            int limit = pageSize;
            var items = source.Skip(offset).Take(limit);

            return items;
        }

    }
}
