using EclinicalSol.Tests.Framework.Base;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace EclinicalSol.Tests.Framework.Controls;

// This is a simple wrapper around IWebElement for buttons, having common properties and actions
public class ButtonControl(IWebDriver driver, By locator) : ControlBase(driver, locator)
{
    public bool IsEnabled => Element.Enabled;
    public string Text     => Element.Text;

    public void ScrollAndClick() =>
        new Actions(Driver).ScrollToElement(Element).MoveToElement(Element).Click().Perform();
}
