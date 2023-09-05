using Microsoft.EntityFrameworkCore;
using Pdsr.Core.Domain;

namespace Pdsr.Data.Extensions
{
    public static class ISubjectOwnableDbContextExtensions
    {
        #region Utilities
        private static void ThrowErrorIfSubjectIdIsNull(string subjectId) { if (string.IsNullOrEmpty(subjectId)) throw new ArgumentNullException("SubjectId cannot be null"); }

        private static void ThrowExceptionIfSourceIsNull(object source) { if (source is null) throw new ArgumentNullException("Source cannot be null"); }

        #endregion
        public static Task<bool> AnyAsync<TSource>(this IQueryable<TSource> source, string subjectId, CancellationToken cancellationToken = default)
            where TSource : ISubjectOwnable
        {
            ThrowExceptionIfSourceIsNull(source);
            ThrowErrorIfSubjectIdIsNull(subjectId);
            return source.AnyAsync(u => u.SubjectId == subjectId, cancellationToken);
        }

        public static Task<TSource> SingleAsync<TSource>(this IQueryable<TSource> source, string subjectId, CancellationToken cancellationToken = default)
            where TSource : ISubjectOwnable
        {
            ThrowExceptionIfSourceIsNull(source);
            ThrowErrorIfSubjectIdIsNull(subjectId);
            return source.SingleAsync(u => u.SubjectId == subjectId, cancellationToken);
        }
    }
}
