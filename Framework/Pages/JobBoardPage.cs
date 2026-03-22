using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using EclinicalSol.Tests.Framework.Base;
using EclinicalSol.Tests.Framework.Utils;
using EclinicalSol.Tests.Framework.Controls;
using EclinicalSol.Tests.Framework.Selectors;

namespace EclinicalSol.Tests.Framework.Pages;

// This is a Page Object Model class representing the Job Board page.
// It is very simple for now, it only has a search control and a method to open a job application form by job title.
public class JobBoardPage(IWebDriver driver) : PageBase(driver)
{
    public readonly SearchControl JobSearch = new(driver, JobBoardPageSelectors.SearchInput);

    public ApplicationFormPage OpenJob(string jobTitle)
    {
        // Wait until exactly 1 job row is in the DOM — this is the only state
        // has 28 jobs, so this wait correctly blocks until React finishes re-rendering.
        WaitHelper.WaitForElementCount(Driver, By.CssSelector("tr.job-post"), expectedCount: 1);

        var link = WaitForVisible(JobBoardPageSelectors.JobLink(jobTitle));
        new Actions(Driver).ScrollToElement(link).MoveToElement(link).Click().Perform();
        return new ApplicationFormPage(Driver);
    }
}
