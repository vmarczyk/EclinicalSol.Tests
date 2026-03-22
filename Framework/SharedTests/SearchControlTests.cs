using EclinicalSol.Tests.Framework.Controls;

namespace EclinicalSol.Tests.Framework.SharedTests;

public static class SearchControlTests
{
    public static void SearchIsVisible(SearchControl search)
    {
        Assert.That(search.IsVisible, Is.True, "Search control is not visible on the page.");
    }

    public static void TestSearch_SetsInputValue(SearchControl search, string query)
    {
        search.Search(query);

        Assert.That(search.Value, Is.EqualTo(query),
            $"Search input value should be '{query}' after searching, but was '{search.Value}'.");
    }

    public static void TestClear_EmptiesInput(SearchControl search, string query)
    {
        search.Search(query);
        search.Clear();

        Assert.That(search.Value, Is.Empty,
            "Search input should be empty after Clear(), but still contains text.");
    }
}
