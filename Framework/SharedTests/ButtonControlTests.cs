using EclinicalSol.Tests.Framework.Controls;

namespace EclinicalSol.Tests.Framework.SharedTests;

public static class ButtonControlTests
{
    public static void TestIsVisible(ButtonControl button)
    {
        Assert.That(button.IsVisible, Is.True, "Button is not visible on the page.");
    }

    public static void TestIsEnabled(ButtonControl button)
    {
        Assert.That(button.IsEnabled, Is.True, "Button is not enabled.");
    }

    public static void TestHasText(ButtonControl button, string expectedText)
    {
        Assert.That(button.Text, Is.EqualTo(expectedText),
            $"Expected button text '{expectedText}' but was '{button.Text}'.");
    }
}
