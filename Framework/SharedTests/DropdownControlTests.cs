using EclinicalSol.Tests.Framework.Controls;

namespace EclinicalSol.Tests.Framework.SharedTests;

public static class DropdownControlTests
{
    public static void TestIsVisible(DropdownControl dropdown)
    {
        Assert.That(dropdown.IsVisible, Is.True, "Dropdown control is not visible on the page.");
    }

    public static void TestSelectOption_SetsSelectedValue(DropdownControl dropdown, string option)
    {
        dropdown.SelectOption(option);

        Assert.That(dropdown.SelectedValue, Is.EqualTo(option),
            $"Dropdown selected value should be '{option}' after selection, but was '{dropdown.SelectedValue}'.");
    }

    public static void TestSelectOption_ReplacesExistingSelection(DropdownControl dropdown, string initial, string replacement)
    {
        dropdown.SelectOption(initial);
        dropdown.SelectOption(replacement);

        Assert.That(dropdown.SelectedValue, Is.EqualTo(replacement),
            $"Dropdown should show '{replacement}' after reselection, but was '{dropdown.SelectedValue}'.");
    }
}
