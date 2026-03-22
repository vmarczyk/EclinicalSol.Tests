using OpenQA.Selenium;
using EclinicalSol.Tests.Framework.Utils;

namespace EclinicalSol.Tests.Framework.Base;

// This is a BaseClass for all controls with common logic.
public abstract class ControlBase(IWebDriver driver, By locator)
{
    protected readonly IWebDriver Driver = driver;
    protected readonly By Locator = locator;
    protected IWebElement Element => WaitHelper.WaitForVisible(Driver, Locator);

    public virtual bool IsVisible => Element.Displayed;
}
