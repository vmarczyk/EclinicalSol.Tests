using OpenQA.Selenium;
using EclinicalSol.Tests.Framework.Utils;

namespace EclinicalSol.Tests.Framework.Base;

// This is a base class for all pages
// currently, it is only has a common helper method. 
public abstract class PageBase(IWebDriver driver)
{
    protected readonly IWebDriver Driver = driver;

    protected IWebElement WaitForVisible(By locator) =>
        WaitHelper.WaitForVisible(Driver, locator);
}
