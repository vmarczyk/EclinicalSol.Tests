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

    public static void TestType_ReplacesExistingValue(InputControl input, string initial, string replacement)
    {
        input.Type(initial);
        input.Type(replacement);

        Assert.That(input.Value, Is.EqualTo(replacement),
            $"Input should contain '{replacement}' after retyping, but was '{input.Value}'.");
        Assert.That(input.Value, Is.Not.EqualTo(initial),
            $"Input should no longer contain the initial value '{initial}'.");
    }
}
