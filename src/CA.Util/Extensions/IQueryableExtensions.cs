using Microsoft.EntityFrameworkCore;

namespace CA.Util.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<IEnumerable<TSource>> ToIListAsync<TSource>(this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            var list = new List<TSource>();

            await foreach (var element in source.AsAsyncEnumerable().WithCancellation(cancellationToken))
            {
                list.Add(element);
            }

            return list;
        }
    }
}
