using Pdsr.Core.Domain;

namespace Pdsr.Core.Extensions;

public static class ISubjectOwnerExtensions
{
    public static bool IsOwner<TSource>(this ISubjectOwnable source, string subjectId) => source.SubjectId == subjectId;
}
