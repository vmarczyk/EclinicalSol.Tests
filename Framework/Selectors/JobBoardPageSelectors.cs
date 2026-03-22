using OpenQA.Selenium;

namespace EclinicalSol.Tests.Framework.Selectors;

public static class JobBoardPageSelectors
{
    public static readonly By SearchInput = By.Id("keyword-filter");
    public static readonly By JobCountHeader = By.CssSelector("[data-testid='job-count-header']");
    public static By JobLink(string jobTitle) =>
        By.XPath($"//tr[contains(@class,'job-post')]//p[contains(@class,'body--medium') and normalize-space()='{jobTitle}']");
}
