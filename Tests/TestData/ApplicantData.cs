namespace EclinicalSol.Tests.Tests.TestData;

// Holding all the applicant data for the test. 
public record ApplicantData(
    string FirstName,
    string LastName,
    string Email,
    string Country,
    string Phone,
    string LinkedInProfile,
    string City,
    string DesiredCompensation,
    string VisaSponsorship,
    string WillingToTravel)
{
    // All required fields filled except resume.
    public static ApplicantData Valid() => new(
        FirstName:           "Vera",
        LastName:            "Kaltovich",
        Email:               "vera.kaltovich.test@example.com",
        Country:             "+46",
        Phone:               "0760063701",
        LinkedInProfile:     "https://www.linkedin.com/in/verakaltovich",
        City:                "Stockholm",
        DesiredCompensation: "150000",
        VisaSponsorship:     "No",
        WillingToTravel:     "Yes"
    );
}
