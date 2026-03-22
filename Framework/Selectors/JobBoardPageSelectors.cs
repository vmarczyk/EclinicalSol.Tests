using OpenQA.Selenium;

namespace EclinicalSol.Tests.Framework.Selectors;

public static class JobBoardPageSelectors
{
    public static readonly By SearchInput = By.Id("keyword-filter");
    public static By JobLink(string jobTitle) =>
        By.XPath(
            $"//td[contains(@class,'cell')]//a[.//p[contains(@class,'body--medium') and normalize-space(.)='{jobTitle}']]");
}
