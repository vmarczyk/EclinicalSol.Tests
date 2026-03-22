using EclinicalSol.Tests.Framework.Base;
using OpenQA.Selenium;

namespace EclinicalSol.Tests.Framework.Controls;

// wrapper for input fields
// currently, supports only typing and getting value.
public class InputControl(IWebDriver driver, By locator) : ControlBase(driver, locator)
{
    public string Value => Element.GetAttribute("value") ?? string.Empty;

    public void Type(string text)
    {
        Element.Clear();
        Element.SendKeys(text);
    }
}
