using System.Runtime.InteropServices;
using System.Text;

namespace MapleNecrocer;

internal static class ChineseSearchQuery
{
    private const uint SimplifiedChinese = 0x02000000;

    internal static IReadOnlyList<string> CreateCandidates(string query)
    {
        return CreateCandidates(query, ConvertToSimplifiedChinese);
    }

    internal static IReadOnlyList<string> CreateCandidates(
        string query,
        Func<string, string?> converter)
    {
        string original = query.Trim();
        if (original.Length == 0)
        {
            return Array.Empty<string>();
        }

        var candidates = new List<string> { original };
        try
        {
            string? simplified = converter(original);
            if (!string.IsNullOrEmpty(simplified) &&
                !candidates.Contains(simplified, StringComparer.Ordinal))
            {
                candidates.Add(simplified);
            }
        }
        catch (Exception)
        {
            // The original candidate keeps Search usable if Windows conversion fails.
        }

        return candidates;
    }

    internal static bool ContainsAny(string value, IReadOnlyList<string> candidates)
    {
        return candidates.Any(candidate =>
            value.IndexOf(candidate, StringComparison.OrdinalIgnoreCase) >= 0);
    }

    internal static bool MatchesTerms(IEnumerable<string> values, string query)
    {
        string[] searchableValues = values.ToArray();
        foreach (string rawTerm in query.Split(
                     new[] { ' ', '\t', '\r', '\n' },
                     StringSplitOptions.RemoveEmptyEntries))
        {
            bool isExcluded = rawTerm.StartsWith('-');
            string term = isExcluded ? rawTerm[1..] : rawTerm;
            if (term.Length == 0)
            {
                continue;
            }

            bool matches = searchableValues.Any(value =>
                ContainsAny(value, CreateCandidates(term)));
            if (isExcluded ? matches : !matches)
            {
                return false;
            }
        }

        return true;
    }

    private static string? ConvertToSimplifiedChinese(string source)
    {
        int requiredLength = LCMapStringEx(
            "zh-CN",
            SimplifiedChinese,
            source,
            source.Length,
            null,
            0,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero);
        if (requiredLength == 0)
        {
            return null;
        }

        var destination = new StringBuilder(requiredLength);
        int convertedLength = LCMapStringEx(
            "zh-CN",
            SimplifiedChinese,
            source,
            source.Length,
            destination,
            destination.Capacity,
            IntPtr.Zero,
            IntPtr.Zero,
            IntPtr.Zero);

        return convertedLength == 0
            ? null
            : destination.ToString(0, convertedLength);
    }

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int LCMapStringEx(
        string? localeName,
        uint mapFlags,
        string source,
        int sourceLength,
        StringBuilder? destination,
        int destinationLength,
        IntPtr versionInformation,
        IntPtr reserved,
        IntPtr sortHandle);
}
