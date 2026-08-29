using MapleNecrocer;
using Xunit;

namespace MapleNecrocer.Tests;

public sealed class ChineseSearchQueryTests
{
    [Fact]
    public void CreateCandidates_AddsWindowsSimplifiedChineseConversion()
    {
        IReadOnlyList<string> candidates = ChineseSearchQuery.CreateCandidates("劍士披風");

        Assert.Equal(new[] { "劍士披風", "剑士披风" }, candidates);
    }

    [Fact]
    public void CreateCandidates_DoesNotDuplicateAlreadySimplifiedQuery()
    {
        IReadOnlyList<string> candidates = ChineseSearchQuery.CreateCandidates("剑士披风");

        Assert.Equal(new[] { "剑士披风" }, candidates);
    }

    [Theory]
    [InlineData("Hair", "Hair")]
    [InlineData("01040001", "01040001")]
    [InlineData("Hair 01040001", "Hair 01040001")]
    public void CreateCandidates_PreservesNonChineseText(string query, string expected)
    {
        Assert.Equal(new[] { expected }, ChineseSearchQuery.CreateCandidates(query));
    }

    [Fact]
    public void CreateCandidates_TrimsSurroundingWhitespaceOnce()
    {
        IReadOnlyList<string> candidates = ChineseSearchQuery.CreateCandidates("  劍士  ", _ => "剑士");

        Assert.Equal(new[] { "劍士", "剑士" }, candidates);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void CreateCandidates_EmptyInputReturnsNoCandidates(string query)
    {
        Assert.Empty(ChineseSearchQuery.CreateCandidates(query));
    }

    [Fact]
    public void CreateCandidates_NullConversionFallsBackToOriginalQuery()
    {
        IReadOnlyList<string> candidates = ChineseSearchQuery.CreateCandidates("劍士", _ => null);

        Assert.Equal(new[] { "劍士" }, candidates);
    }

    [Fact]
    public void CreateCandidates_ConversionExceptionFallsBackToOriginalQuery()
    {
        IReadOnlyList<string> candidates = ChineseSearchQuery.CreateCandidates(
            "劍士",
            _ => throw new InvalidOperationException("conversion failed"));

        Assert.Equal(new[] { "劍士" }, candidates);
    }

    [Fact]
    public void ContainsAny_MatchesOriginalOrSimplifiedCandidate()
    {
        IReadOnlyList<string> candidates = ChineseSearchQuery.CreateCandidates("劍", _ => "剑");

        Assert.True(ChineseSearchQuery.ContainsAny("傳統劍", candidates));
        Assert.True(ChineseSearchQuery.ContainsAny("剑士披风", candidates));
        Assert.False(ChineseSearchQuery.ContainsAny("帽子", candidates));
    }
}
