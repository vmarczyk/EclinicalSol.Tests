using OpenQA.Selenium;
using EclinicalSol.Tests.Framework.Base;
using EclinicalSol.Tests.Framework.Selectors;
using EclinicalSol.Tests.Framework.Utils;

namespace EclinicalSol.Tests.Framework.Pages;

// This is a Page Object Model class representing the Careers page.
// It is very simple for now, needed only to open the Job Board in a new tab for now.
public class CareersPage(IWebDriver driver) : PageBase(driver)
{
    public JobBoardPage OpenJobBoard()
    {
        WaitForVisible(CareersPageSelectors.ViewOpenPositionsButton).Click();
        WaitHelper.WaitForNewTab(Driver, expectedTabCount: 2);
        Driver.SwitchTo().Window(Driver.WindowHandles.Last());
        return new JobBoardPage(Driver);
    }
}
