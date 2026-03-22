using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using EclinicalSol.Tests.Framework.Base;
using EclinicalSol.Tests.Framework.Selectors;

namespace EclinicalSol.Tests.Framework.Pages;

// This is a Page Object Model class representing the Home page.
// It is very simple for now, it only navigates to the Careers page.
public class HomePage(IWebDriver driver) : PageBase(driver)
{
    public CareersPage GoToCareers()
    {
        // Hover over "Resources" to expand the submenu, then click "Careers"
        var resourcesMenu = WaitForVisible(HomePageSelectors.ResourcesMenuLink);
        new Actions(Driver).MoveToElement(resourcesMenu).Perform();

        WaitForVisible(HomePageSelectors.CareersSubMenuLink).Click();

        return new CareersPage(Driver);
    }
}
