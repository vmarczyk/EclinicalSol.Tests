using OpenQA.Selenium;

namespace EclinicalSol.Tests.Framework.Selectors;

public static class HomePageSelectors
{
    public static readonly By ResourcesMenuLink = By.CssSelector("#menu-item-170 > a.main-menu-link");
    public static readonly By CareersSubMenuLink = By.CssSelector("#menu-item-5597 > a.sub-menu-link");
}
