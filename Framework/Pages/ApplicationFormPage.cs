using OpenQA.Selenium;
using EclinicalSol.Tests.Framework.Base;
using EclinicalSol.Tests.Framework.Controls;
using EclinicalSol.Tests.Framework.Selectors;
using EclinicalSol.Tests.Framework.SharedTests;

namespace EclinicalSol.Tests.Framework.Pages;

// This is a Page Object Model class representing the Job Application Form page.
// It contains properties for all relevant controls on the page and methods to interact with them.

public class ApplicationFormPage(IWebDriver driver) : PageBase(driver)
{
    public readonly InputControl    FirstNameInput          = new(driver, ApplicationFormSelectors.FirstName);
    public readonly InputControl    LastNameInput           = new(driver, ApplicationFormSelectors.LastName);
    public readonly InputControl    EmailInput              = new(driver, ApplicationFormSelectors.Email);
    public readonly InputControl    PhoneInput              = new(driver, ApplicationFormSelectors.Phone);
    public readonly InputControl    LinkedInInput           = new(driver, ApplicationFormSelectors.LinkedInProfile);
    public readonly InputControl    CityInput               = new(driver, ApplicationFormSelectors.City);
    public readonly InputControl    CompensationInput       = new(driver, ApplicationFormSelectors.DesiredCompensation);
    public readonly DropdownControl CountryDropdown         = new(driver, ApplicationFormSelectors.CountryDropdown);
    public readonly DropdownControl VisaSponsorshipDropdown = new(driver, ApplicationFormSelectors.VisaSponsorshipDropdown);
    public readonly DropdownControl WillingToTravelDropdown = new(driver, ApplicationFormSelectors.WillingToTravelDropdown);
    public readonly ButtonControl   SubmitButton            = new(driver, ApplicationFormSelectors.SubmitButton);

    public ApplicationFormPage TypeFirstName(string value)           { FirstNameInput.Type(value);                    return this; }
    public ApplicationFormPage TypeLastName(string value)            { LastNameInput.Type(value);                     return this; }
    public ApplicationFormPage TypeEmail(string value)               { EmailInput.Type(value);                        return this; }
    public ApplicationFormPage SelectCountry(string value)           { CountryDropdown.TypeAndSelect(value);          return this; }
    public ApplicationFormPage TypePhone(string value)               { PhoneInput.Type(value);                        return this; }
    public ApplicationFormPage TypeLinkedIn(string value)            { LinkedInInput.Type(value);                     return this; }
    public ApplicationFormPage TypeCity(string value)                { CityInput.Type(value);                         return this; }
    public ApplicationFormPage TypeDesiredCompensation(string value) { CompensationInput.Type(value);                 return this; }
    public ApplicationFormPage SelectVisaSponsorship(string value)   { VisaSponsorshipDropdown.SelectOption(value);   return this; }
    public ApplicationFormPage SelectWillingToTravel(string value)   { WillingToTravelDropdown.SelectOption(value);   return this; }

    // Smoke-check key controls before filling the form (reuses shared tests)
    public ApplicationFormPage AssertRequiredControlsVisible()
    {
        InputControlTests.TestIsVisible(FirstNameInput);
        InputControlTests.TestIsVisible(LastNameInput);
        InputControlTests.TestIsVisible(EmailInput);
        InputControlTests.TestIsVisible(PhoneInput);
        InputControlTests.TestIsVisible(LinkedInInput);
        InputControlTests.TestIsVisible(CityInput);
        InputControlTests.TestIsVisible(CompensationInput);
        DropdownControlTests.TestIsVisible(CountryDropdown);
        DropdownControlTests.TestIsVisible(VisaSponsorshipDropdown);
        DropdownControlTests.TestIsVisible(WillingToTravelDropdown);
        ButtonControlTests.TestIsVisible(SubmitButton);
        return this;
    }

    // Submit the form without filling the resume
    public ApplicationFormPage Submit()
    {
        SubmitButton.ScrollAndClick();
        return this;
    }

    // Verify that the expected error message is displayed for the resume field
    public ApplicationFormPage VerifyResumeErrorMessage(string expectedMessage)
    {
        var error = WaitForVisible(ApplicationFormSelectors.ResumeErrorMessage);
        Assert.That(error.Text.Trim(), Does.Contain(expectedMessage.Trim()),
            $"Expected resume error message '{expectedMessage}' was not displayed.");
        return this;
    }

    // Verify that the Resume/CV label has error styling (red).
    public ApplicationFormPage VerifyResumeLabelIsRed()
    {
        // Locator already requires #upload-label-resume + .upload-label--error (error styling).
        WaitForVisible(ApplicationFormSelectors.ResumeCvLabelError);
        return this;
    }
}
