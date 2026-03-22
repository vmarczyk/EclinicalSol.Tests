using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EclinicalSol.Tests.Framework.Driver;

// Factory class to create WebDriver instances based on configuration
public static class DriverFactory
{
    public static IWebDriver Create(string browser = "chrome", bool headless = false)
    {
        IWebDriver driver = browser.ToLower() switch
        {
            "chrome" => CreateChromeDriver(headless),
            _ => throw new ArgumentException($"Browser '{browser}' is not supported.")
        };

        driver.Manage().Window.Size = new System.Drawing.Size(1440, 900);
        return driver;
    }

    private static IWebDriver CreateChromeDriver(bool headless)
    {
        var options = new ChromeOptions();
        options.AddArgument("--no-sandbox");
        options.AddArgument("--disable-dev-shm-usage");
        options.AddArgument("--disable-gpu");
        if (headless) options.AddArgument("--headless=new");
        return new ChromeDriver(options);
    }
}
