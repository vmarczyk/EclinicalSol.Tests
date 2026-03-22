using EclinicalSol.Tests.Framework.Controls;

namespace EclinicalSol.Tests.Framework.SharedTests;

public static class InputControlTests
{
    public static void TestIsVisible(InputControl input)
    {
        Assert.That(input.IsVisible, Is.True, "Input control is not visible on the page.");
    }

    public static void TestType_SetsValue(InputControl input, string text)
    {
        input.Type(text);

        Assert.That(input.Value, Is.EqualTo(text),
            $"Input value should be '{text}' after typing, but was '{input.Value}'.");
    }
    
}
