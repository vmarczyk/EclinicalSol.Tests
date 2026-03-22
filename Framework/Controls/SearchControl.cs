using EclinicalSol.Tests.Framework.Base;
using OpenQA.Selenium;

namespace EclinicalSol.Tests.Framework.Controls;

// wrapper for Search input field, with common properties and methods.
public class SearchControl(IWebDriver driver, By locator) : ControlBase(driver, locator)
{
    public string Value => Element.GetAttribute("value") ?? string.Empty;

    public void Search(string query)
    {
        Element.Clear();
        Element.SendKeys(query);
        Element.SendKeys(Keys.Enter);
    }

    public void Clear() => Element.Clear();
}
