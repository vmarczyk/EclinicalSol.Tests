# EclinicalSol.Tests — Selenium UI Test Framework

End-to-end test suite for [eclinicalsol.com](https://www.eclinicalsol.com)
Stack: **C# · .NET 9 · NUnit 4 · Selenium WebDriver 4**

---

## The task

> Using Selenium, write a test to:
> 1. Find this job on the https://www.eclinicalsol.com/ website.
> 2. Fill in all the required fields to apply **except for resume**.
> 3. Click **Submit application** button.
> 4. Verify **"Resume/CV is required."** message displays.
> 5. Verify **Resume/CV label is red**.
>
> Keep in mind:
> - How do you handle **data setup**?
> - How do you **organize your test methods**?

### How each requirement is addressed

| # | Requirement | Implementation |
|---|---|---|
| 1 | Find the job on the website | `HomePage → GoToCareers() → OpenJobBoard() → OpenJob(jobTitle)` — full navigation chain via Page Objects. Job title is a named constant in `TestConstants.JobTitle`. |
| 2 | Fill all required fields except resume | `ApplicationFormPage` exposes a form (`TypeFirstName`, `SelectCountry`, …). Form data comes from `ApplicantData.Valid()` — a typed record, not strings in the test. |
| 3 | Click Submit | `form.Submit()` calls `SubmitButton.ScrollAndClick()` — scroll ensures the button is in view before clicking. |
| 4 | Verify error message | `form.VerifyResumeErrorMessage(TestConstants.ResumeRequiredMessage)` waits for `#resume-error.helper-text--error` and asserts the text contains the expected substring. |
| 5 | Verify label is red | `form.VerifyResumeLabelIsRed()` waits for `#upload-label-resume.upload-label--error` — the error CSS class is the source of truth for the red styling, making the check robust across theme changes. |


---

## What the tests cover

Home → Resources → Careers → View Open Positions → search for a job → open the application form → fill all required fields (except resume) → submit → assert resume-required error and red label.

---

## Project structure

```
EclinicalSol.Tests/
│
├── Framework/                    # Reusable framework layer (no test logic here)
│   ├── Base/
│   │   ├── TestBase.cs           # NUnit SetUp/TearDown — driver lifecycle
│   │   ├── PageBase.cs           # Base for all Page Objects
│   │   └── ControlBase.cs        # Base for all UI Controls
│   │
│   ├── Driver/
│   │   └── DriverFactory.cs      # Creates IWebDriver from config
│   │
│   ├── Pages/                    # Page Object Model (POM) classes
│   │   ├── HomePage.cs
│   │   ├── CareersPage.cs
│   │   ├── JobBoardPage.cs
│   │   └── ApplicationFormPage.cs
│   │
│   ├── Controls/                 # UI control wrappers
│   │   ├── ButtonControl.cs
│   │   ├── InputControl.cs
│   │   ├── DropdownControl.cs
│   │   └── SearchControl.cs
│   │
│   ├── Selectors/                # All locators — one static class per page
│   │   ├── HomePageSelectors.cs
│   │   ├── CareersPageSelectors.cs
│   │   ├── JobBoardPageSelectors.cs
│   │   └── ApplicationFormSelectors.cs
│   │
│   ├── SharedTests/              # Reusable assertion helpers per control type
│   │   ├── ButtonControlTests.cs
│   │   ├── InputControlTests.cs
│   │   ├── DropdownControlTests.cs
│   │   └── SearchControlTests.cs
│   │
│   └── Utils/
│       ├── WaitHelper.cs         # Centralised explicit-wait helpers
│       └── ConfigReader.cs       # Reads appsettings.json
│
├── Tests/                        # Actual test classes
│   ├── JobApplicationTests.cs    # Good-style test (uses POM)
│   ├── BadStyleNoPomJobApplicationTest.cs  # All in one class
│   └── TestData/
│       ├── ApplicantData.cs      # Test data record
│       └── TestConstants.cs      # Job title, error messages
│
├── appsettings.json              # Environment + browser config
├── selector-suggestions.txt      # Notes on selector fragility + improvement plan
└── EclinicalSol.Tests.csproj     # Auto-template generated
```

---

## Architecture

### Layer diagram

```
┌─────────────────────────────────────────────────┐
│                   Test Layer                    │
│   JobApplicationTests (inherits TestBase)       │
│   • Reads TestConstants / ApplicantData         │
│   • Calls Page Objects & SharedTests            │
└────────────────────┬────────────────────────────┘
                     │ uses
┌────────────────────▼────────────────────────────┐
│              Page Object Layer                  │
│  HomePage → CareersPage → JobBoardPage          │
│                        → ApplicationFormPage    │
│  • Each page returns the next page on navigation│
│  • Pages own Controls as public fields          │
│  • Pages use Selectors                          │
└──────────┬──────────────────┬───────────────────┘
           │ inherits         │ uses
┌──────────▼──────┐  ┌────────▼────────────────────┐
│   PageBase      │  │       Control Layer          │
│  WaitForVisible │  │  ButtonControl               │
│  (delegates to  │  │  InputControl                │
│   WaitHelper)   │  │  DropdownControl             │
└─────────────────┘  │  SearchControl               │
                     │  (all extend ControlBase)    │
                     └────────────────────────────--┘
                                  │ uses
                     ┌────────────▼───────────────────┐
                     │  Infrastructure Layer          │
                     │  DriverFactory  (browser)      │
                     │  WaitHelper     (explicit wait)│
                     │  ConfigReader   (appsettings)  │
                     └────────────────────────────────┘
```

### Design decisions

#### 1. Page Object Model (POM)
Each page of the application is represented by a dedicated class that:
- encapsulates all selectors and interactions for that page
- returns the **next** Page Object on navigation, enabling a clean fluent call-chain in tests
- keeps no assertions — pages are interaction-only

```csharp
// Test reads like a user story
var form = new HomePage(Driver)
    .GoToCareers()       // → CareersPage
    .OpenJobBoard()      // → JobBoardPage  (also handles new-tab switch)
    .OpenJob(jobTitle);  // → ApplicationFormPage
```

#### 2. Typed Control abstractions
Rather than letting pages call `driver.FindElement(...)` directly, each logical UI widget has its own class (`InputControl`, `DropdownControl`, `ButtonControl`, `SearchControl`) that all extend `ControlBase`.

Benefits:
- `ControlBase.Element` resolves via `WaitHelper.WaitForVisible` on every access, so stale-element exceptions are avoided automatically
- **Reusable assertions** — `SharedTests/*` helpers assert control behaviour in one place and are called from both page methods and test methods without duplication

```csharp
// ApplicationFormPage exposes controls as typed public fields. Examples:
public readonly InputControl    FirstNameInput = new(driver, ApplicationFormSelectors.FirstName);
public readonly DropdownControl CountryDropdown = new(driver, ApplicationFormSelectors.CountryDropdown);
```

#### 3. Selectors separated from pages
All `By` locators live in static `*Selectors` classes, one per page. Pages and controls never contain raw CSS/XPath strings. This means:
- selector changes require editing exactly one file
- it is easy to review locator quality independently of page logic

#### 4. Centralised waits
`WaitHelper` is the single place that touches `WebDriverWait`. No `Thread.Sleep`, no per-test waits. No implicit waits.

#### 5. Configuration-driven environment
`appsettings.json` defines `environment` + `browser`. `ConfigReader` resolves `baseUrl` from the active environment at runtime. Switching to UAT or prod means changing one JSON key — no code change needed.

```json
{
  "environment": "test",
  "browser":     "chrome",
  "environments": {
    "test": { "baseUrl": "https://www.eclinicalsol.com" },
    "uat":  { "baseUrl": "" }
  }
}
```

#### 6. Builder pattern on ApplicationFormPage
Every `Type*` / `Select*` method returns `this`, so form-fill reads as a single readable chain:

```csharp
form.TypeFirstName(applicant.FirstName)
    .TypeLastName(applicant.LastName)
    .TypeEmail(applicant.Email)
    .SelectCountry(applicant.Country)
    ...
    .Submit();
```

#### 7. SharedTests — reusable control assertions
`Framework/SharedTests/` holds static helpers that assert generic control behaviour (visibility, typing, dropdown selection). Pages call these for checking their own controls before proceeding:

```csharp
// Inside ApplicationFormPage.AssertRequiredControlsVisible()
InputControlTests.TestIsVisible(FirstNameInput);
DropdownControlTests.TestIsVisible(CountryDropdown);
ButtonControlTests.TestIsVisible(SubmitButton);
```


---

## Good vs Bad Test architecture

The repo shows both styles of good and bad-architecture.

| Concern | `JobApplicationTests` | `BadStyleNoPomJobApplicationTest` |
|---|---|---|
| Driver creation | `TestBase.SetUp` (one place) | Duplicated inside `SetUp` |
| Config reading | `ConfigReader` | Inline `ConfigurationBuilder` (will be repeated for the other tests) |
| Page navigation | Page Objects chain | Just `driver.FindElement` calls |
| Selectors | `*Selectors` classes | Hardcoded strings inline (not easy to manage) |
| Waits | `WaitHelper` | Inline `WebDriverWait` |
| Test data | `ApplicantData.Valid()` | Data in method body |
| React-Select logic | `DropdownControl.SelectOption` | Private helper duplicated in test file |
| Assertions | Descriptive failure messages | `Assert.That` on raw elements |

---

## How to run

### Prerequisites
- .NET 9 SDK
- Google Chrome

### Run all tests
```bash
dotnet test
```

### Run a specific test
```bash
dotnet test --filter "FullyQualifiedName~JobApplicationTests"
```


### CI
`.github/workflows/ci.yml` runs pipeline on every commit or PR to main. 

---

## Known selector risks

See `selector-suggestions.txt` for a full review. Summary:

| Priority | Selector | Risk |
|---|---|---|
| P0 | `#menu-item-170`, `#menu-item-5597` | Auto-generated IDs — break on menu changes |
| P0 | `LinkText("View Open Positions")` | Breaks on changes |
| P1 | `question_11353177007`-style IDs | Question record IDs — volatile across board duplicates |
| P1 | Job row XPath by title + CSS class | Breaks if developer changes their DOM structure |

Recommended fix: ask the front-end team to add `data-testid` attributes on the nav items and CTA buttons.

---

