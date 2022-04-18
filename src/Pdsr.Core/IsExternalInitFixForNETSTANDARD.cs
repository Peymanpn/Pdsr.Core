/*
 * init-only properties doesn't work in NETSTANDARD2.0 and below
 * therefor this is a work around the issue
 */
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit{ }
}
