using OpenQA.Selenium;

namespace EclinicalSol.Tests.Framework.Selectors;

public static class ApplicationFormSelectors
{
    public static readonly By FirstName = By.Id("first_name");
    public static readonly By LastName  = By.Id("last_name");
    public static readonly By Email     = By.Id("email");
    public static readonly By Phone     = By.Id("phone");
    public static readonly By LinkedInProfile      = By.Id("question_11353177007");
    public static readonly By City                 = By.Id("question_11353179007");
    public static readonly By DesiredCompensation  = By.Id("question_11353180007");
    public static readonly By CountryDropdown = By.Id("country");
    public static readonly By VisaSponsorshipDropdown = By.Id("question_11353181007");
    public static readonly By WillingToTravelDropdown = By.Id("question_11353182007");
    public static readonly By SubmitButton = By.CssSelector("button[type='submit']");
    public static readonly By ResumeErrorMessage =
        By.CssSelector("#resume-error.helper-text--error");
    public static readonly By ResumeCvLabelError =
        By.CssSelector("#upload-label-resume.upload-label--error");
}
