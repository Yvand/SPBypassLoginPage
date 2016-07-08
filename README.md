# SPBypassLoginPage
This page allows SharePoint administrators to bypass provider selection page when multiple authentication modes are enabled in a zone of a web application.

Current version redirects to 1st trusted authentication mode it finds on the current zone (most common use case for this page) and will generate an error otherwise, but it will change soon to give administrators more control.

# Installation
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases)
- Install solution:
`Add-SPSolution "C:\Data\Dev\SPBypassLoginPage.wsp"`
- Deploy it: 
`Install-SPSolution -Identity "SPBypassLoginPage.wsp" -GACDeployment`
- Go to central administration site > Select your web application > "Authentication Providers" > Select the zone:
In "Sign In Page URL" section: check "Custom Sign In Page" > type URL below and validate:
/_login/Bypass/BypassLogin.aspx

# How to Update
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases)
- Update solution:

```powershell
Update-SPSolution -GACDeployment -Identity "SPBypassLoginPage.wsp" -LiteralPath "C:\Data\Dev\SPBypassLoginPage.wsp"
```
