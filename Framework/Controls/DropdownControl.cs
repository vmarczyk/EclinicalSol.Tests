using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using EclinicalSol.Tests.Framework.Utils;
using EclinicalSol.Tests.Framework.Base;

namespace EclinicalSol.Tests.Framework.Controls;

// common wrapper for dropdowns.
public class DropdownControl(IWebDriver driver, By locator) : ControlBase(driver, locator)
{
    public override bool IsVisible =>
        Element.FindElement(By.XPath("ancestor::div[contains(@class,'select__control')]")).Displayed;

    public string SelectedValue =>
        Element.FindElement(By.XPath("ancestor::div[contains(@class,'select-shell')]"))
            .FindElement(By.CssSelector(".select__single-value"))
            .Text;

    public void TypeAndSelect(string query)
    {
        new Actions(Driver).ScrollToElement(Element).Perform();
        Element.Click();
        Element.SendKeys(query);
        Element.SendKeys(Keys.Enter);
    }

    public void SelectOption(string optionText)
    {
        var control = Element.FindElement(By.XPath("ancestor::div[contains(@class,'select__control')]"));
        new Actions(Driver).ScrollToElement(control).MoveToElement(control).Click().Perform();

        var option = WaitHelper.WaitForVisible(Driver,
            By.XPath($"//div[contains(@class,'select__option') and normalize-space(text())='{optionText}']"));
        new Actions(Driver).MoveToElement(option).Click().Perform();
    }
}
