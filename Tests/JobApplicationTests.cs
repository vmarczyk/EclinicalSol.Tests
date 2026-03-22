using EclinicalSol.Tests.Framework.Base;
using EclinicalSol.Tests.Framework.Pages;
using EclinicalSol.Tests.Framework.SharedTests;
using EclinicalSol.Tests.Tests.TestData;

namespace EclinicalSol.Tests.Tests;

[TestFixture]
public class JobApplicationTests : TestBase
{
    [Test]
    public void ApplyToJob_WithoutResume_Submit()
    {
        var applicant = ApplicantData.Valid();
        var jobTitle = TestConstants.JobTitle;

        var jobBoard = new HomePage(Driver)
            .GoToCareers()
            .OpenJobBoard();

        SearchControlTests.SearchIsVisible(jobBoard.JobSearch);
        jobBoard.JobSearch.Search(jobTitle);

        var form = jobBoard.OpenJob(jobTitle);
        form.AssertRequiredControlsVisible();

        // Fill in all required fields except resume and submit
        form.TypeFirstName(applicant.FirstName)
            .TypeLastName(applicant.LastName)
            .TypeEmail(applicant.Email)
            .SelectCountry(applicant.Country)
            .TypePhone(applicant.Phone)
            .TypeLinkedIn(applicant.LinkedInProfile)
            .TypeCity(applicant.City)
            .TypeDesiredCompensation(applicant.DesiredCompensation)
            .SelectVisaSponsorship(applicant.VisaSponsorship)
            .SelectWillingToTravel(applicant.WillingToTravel)
            //.AttachResume("path/to/resume.pdf")
            .Submit();

        form.VerifyResumeErrorMessage(TestConstants.ResumeRequiredMessage)
            .VerifyResumeLabelIsRed();
    }
}
