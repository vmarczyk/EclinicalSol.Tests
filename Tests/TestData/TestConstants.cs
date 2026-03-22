namespace EclinicalSol.Tests.Tests.TestData;

public static class TestConstants
{
    // The job title we are going to search for and apply to in the test
    public const string JobTitle = "Senior AI-Enabled Software Engineer in Test";

    // Non-existing job title to test the "no results" scenario
    public const string NonExistingJobTitle = "This Job Does Not Exist";

    // Exact substring expected in the resume validation message on submit
    public const string ResumeRequiredMessage = "Resume/CV is required.";
}
