using System;

namespace xxENSONOxx.xxSTACKSxx.Infrastructure.IntegrationTests.CosmosDb.Extensions;

internal static class StringExtensions
{
    internal static bool Contains(this string source, string toCheck, StringComparison comp)
    {
        return source?.IndexOf(toCheck, comp) >= 0;
    }
}
