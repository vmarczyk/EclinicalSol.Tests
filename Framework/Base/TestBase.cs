using OpenQA.Selenium;
using EclinicalSol.Tests.Framework.Driver;
using EclinicalSol.Tests.Framework.Utils;

namespace EclinicalSol.Tests.Framework.Base;

// this is a base class for tests
// currently, it only has common setup/teardown logic.
public class TestBase
{
    protected IWebDriver Driver { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        Driver = DriverFactory.Create(ConfigReader.GetBrowser(), ConfigReader.IsHeadless());
        Driver.Navigate().GoToUrl(ConfigReader.GetBaseUrl());
    }

    [TearDown]
    public void TearDown()
    {
        Driver?.Quit();
        Driver?.Dispose();
    }
}
