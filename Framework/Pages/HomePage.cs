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
        // Hover over "Resources" to expand the submenu, then move onto "Careers" and click.
        var resourcesMenu = WaitForVisible(HomePageSelectors.ResourcesMenuLink);
        new Actions(Driver).MoveToElement(resourcesMenu).Perform();

        var careersLink = WaitForVisible(HomePageSelectors.CareersSubMenuLink);
        new Actions(Driver).MoveToElement(careersLink).Click().Perform();

        return new CareersPage(Driver);
    }
}
