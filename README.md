# SPBypassLoginPage
This page allows SharePoint administrators to bypass provider selection page when multiple authentication modes are enabled in a zone in a web application.
Current version redirects to 1st federated authentication mode it finds on the current zone (most common use case for this page) and will generate an error message otherwise, but it will change soon to give administrators more control on the behavior

# Installation
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases)
- Run this cmdlet to install solution:
`Add-SPSolution "C:\Data\Dev\SPBypassLoginPage.wsp"`
- Go to central administration site > Manage web applications > Select your web application > "Authentication Providers" > Select the zone:
In "Sign In Page URL" section: check "Custom Sign In Page" > type URL below and validate:
/_login/Bypass/BypassLogin.aspx

# How to Update
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases)
- Run this cmdlet to update solution:
`Update-SPSolution -GACDeployment -Identity "SPBypassLoginPage.wsp" -LiteralPath "C:\Data\Dev\SPBypassLoginPage.wsp"`
