using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace EclinicalSol.Tests.Framework.Utils;

// This is a utility class for explicit waits
// It provides common wait methods for elements to be visible, contain specific text, or for new tabs to open.
public static class WaitHelper
{
    private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

    public static IWebElement WaitForVisible(IWebDriver driver, By locator, TimeSpan? timeout = null)
    {
        var wait = CreateWait(driver, timeout);
        return wait.Until(d => TryFindVisible(d, locator))!;
    }

    // Waits until the number of matching elements equals the expected count.
    // Use this to confirm a dynamic list has finished re-rendering after a filter or search.
    public static void WaitForElementCount(IWebDriver driver, By locator, int expectedCount, TimeSpan? timeout = null)
    {
        var wait = CreateWait(driver, timeout);
        wait.Until(d => d.FindElements(locator).Count == expectedCount);
    }

    public static void WaitForNewTab(IWebDriver driver, int expectedTabCount, TimeSpan? timeout = null)
    {
        var wait = CreateWait(driver, timeout);
        wait.Until(d => d.WindowHandles.Count >= expectedTabCount);
    }

    private static WebDriverWait CreateWait(IWebDriver driver, TimeSpan? timeout) =>
        new(driver, timeout ?? DefaultTimeout);
        
    // This method tries to find an element and checks if it is visible. If the element is not found or not visible, it returns null.
    private static IWebElement? TryFindVisible(IWebDriver driver, By locator)
    {
        try
        {
            var element = driver.FindElement(locator);
            return element.Displayed ? element : null;
        }
        catch (NoSuchElementException)
        {
            return null;
        }
    }

}
