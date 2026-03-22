using Microsoft.Extensions.Configuration;
using NUnit.Framework.Legacy;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace EclinicalSol.Tests.Tests;

// Example of BAD TEST: no Page Object Model, all logic in test method
[TestFixture]
public class BadStyleNoPomJobApplicationTest
{
    // IWebDriver instance for the test, initialized in SetUp and cleaned up in TearDown. 
    private IWebDriver? _driver;

    [SetUp]
    public void SetUp()
    {
        // Driver creation duplicated here 
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        _driver = new ChromeDriver(options);
        _driver.Manage().Window.Size = new System.Drawing.Size(1440, 900);

        // Config read duplicated inline 
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
        var env = config["environment"]
            ?? throw new InvalidOperationException("'environment' is missing in appsettings.json.");
        var baseUrl = config[$"environments:{env}:baseUrl"]
            ?? throw new InvalidOperationException($"Base URL for '{env}' is not configured.");
        _driver.Navigate().GoToUrl(baseUrl);
    }

    [TearDown]
    public void TearDown()
    {
        _driver?.Quit();
        _driver?.Dispose();
    }

    [Test]
    public void ApplyWithoutResume_SubmitShowsResumeErrorAndRedLabel()
    {
        var d = _driver!;
        // Good: we have waiter, not fixed sleep. But the wait logic is duplicated inline and not abstracted
        var wait = new WebDriverWait(d, TimeSpan.FromSeconds(15));

        // “Data setup”: everything is literals in the method
        const string jobTitle = "Senior AI-Enabled Software Engineer in Test";
        const string expectedResumeMessage = "Resume/CV is required.";

        // --- 1. Find the job (Resources → Careers → Greenhouse board → search → open) ---

        var resourcesMenu = wait.Until(drv =>
            Visible(drv, By.CssSelector("#menu-item-170 > a.main-menu-link")));
        Assert.That(resourcesMenu.Displayed, Is.True, "Resources menu should be visible.");
        new Actions(d).MoveToElement(resourcesMenu).Perform();

        var careersLink = wait.Until(drv =>
            Visible(drv, By.CssSelector("#menu-item-5597 > a.sub-menu-link")));
        Assert.That(careersLink.Displayed, Is.True, "Careers link should be visible.");
        careersLink.Click();

        var viewOpenPositions = wait.Until(drv => Visible(drv, By.LinkText("View Open Positions")));
        viewOpenPositions.Click();

        wait.Until(drv => drv.WindowHandles.Count >= 2);
        d.SwitchTo().Window(d.WindowHandles.Last());

        var keyword = wait.Until(drv => Visible(drv, By.Id("keyword-filter")));
        keyword.Clear();
        keyword.SendKeys(jobTitle);
        keyword.SendKeys(Keys.Enter);

        wait.Until(drv =>
            Visible(drv, By.CssSelector("[data-testid='job-count-header']")));

        var jobLink = wait.Until(drv => Visible(drv, By.XPath(
            $"//td[contains(@class,'cell')]//a[.//p[contains(@class,'body--medium') and normalize-space(.)='{jobTitle}']]")));
        Assert.That(jobLink.Displayed, Is.True, "Job row link should be visible.");
        new Actions(d).ScrollToElement(jobLink).MoveToElement(jobLink).Click().Perform();

        // --- 2–3. Fill required fields except resume, submit ---

        wait.Until(drv => Visible(drv, By.Id("first_name"))).SendKeys("Test");
        Assert.That(d.FindElement(By.Id("first_name")).GetAttribute("value"), Is.EqualTo("Test"));

        d.FindElement(By.Id("last_name")).SendKeys("Applicant");
        Assert.That(d.FindElement(By.Id("last_name")).GetAttribute("value"), Does.Contain("Applicant"));

        d.FindElement(By.Id("email")).SendKeys("e2e.applicant@example.com");

        // Phone country (React Select) — logic copied inline (bad: no abstraction).
        var country = d.FindElement(By.Id("country"));
        new Actions(d).ScrollToElement(country).Perform();
        country.Click();
        country.SendKeys("+46");
        country.SendKeys(Keys.Enter);

        d.FindElement(By.Id("phone")).SendKeys("0760063701");
        d.FindElement(By.Id("question_11353177007")).SendKeys("https://www.linkedin.com/in/example");
        d.FindElement(By.Id("question_11353179007")).SendKeys("Stockholm");
        d.FindElement(By.Id("question_11353180007")).SendKeys("150000");

        ClickReactSelectOption(d, wait, By.Id("question_11353181007"), "No");
        ClickReactSelectOption(d, wait, By.Id("question_11353182007"), "Yes");

        d.FindElement(By.CssSelector("button[type='submit']")).Click();

        // --- 4. Resume/CV required message ---

        var resumeError = wait.Until(drv => Visible(drv, By.CssSelector("#resume-error.helper-text--error")));
        Assert.That(resumeError.Text.Trim(), Does.Contain(expectedResumeMessage.Trim()));

        // --- 5. Resume/CV label in error (red) state — we assert class, not computed color (cheap check). ---

        var resumeLabel = wait.Until(drv => Visible(drv, By.CssSelector("#upload-label-resume.upload-label--error")));
        Assert.That(resumeLabel.Displayed, Is.True);
        StringAssert.Contains("upload-label--error", resumeLabel.GetAttribute("class") ?? string.Empty);

        var color = resumeLabel.GetCssValue("color");
        Assert.That(color.StartsWith("rgb", StringComparison.OrdinalIgnoreCase), Is.True);
    }

   // This method encapsulates the logic to select an option from a Select dropdown. 
    private static void ClickReactSelectOption(IWebDriver d, WebDriverWait wait, By inputId, string optionText)
    {
        var input = d.FindElement(inputId);
        var control = input.FindElement(By.XPath("ancestor::div[contains(@class,'select__control')]"));
        new Actions(d).ScrollToElement(control).MoveToElement(control).Click().Perform();
        var option = wait.Until(drv => drv.FindElement(
            By.XPath($"//div[contains(@class,'select__option') and normalize-space(text())='{optionText}']")));
        new Actions(d).MoveToElement(option).Click().Perform();
    }

    // This method tries to find an element and checks if it is visible.
    private static IWebElement? Visible(IWebDriver drv, By locator)
    {
        try
        {
            var el = drv.FindElement(locator);
            return el.Displayed ? el : null;
        }
        catch (NoSuchElementException)
        {
            return null;
        }
    }
}
