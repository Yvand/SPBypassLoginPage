# SPBypassLoginPage
This page allows SharePoint administrators to bypass provider selection page when multiple authentication modes are enabled in a zone of a web application.

By default it redirects to the 1st trusted authentication mode it finds in the current zone (most common use-case), but administrators can customize behavior through custom farm property "CustomBypassLogin" (see below for more details).

## Installation
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases)
- Install solution:
`Add-SPSolution "C:\Data\Dev\SPBypassLoginPage.wsp"`
- Deploy it: 
`Install-SPSolution -Identity "SPBypassLoginPage.wsp" -GACDeployment`
- Go to central administration site > Select your web application > "Authentication Providers" > Select the zone:
In "Sign In Page URL" section: check "Custom Sign In Page" > type URL below and validate:
/_login/Bypass/BypassLogin.aspx

## How to Update
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases)
- Update solution:

```powershell
Update-SPSolution -GACDeployment -Identity "SPBypassLoginPage.wsp" -LiteralPath "C:\Data\Dev\SPBypassLoginPage.wsp"
```

## How to configure
Custom farm property "CustomBypassLogin" can have following values:
- "prompt": displays the same experience to users as out of the box (they must choose the authentication mode to use)
- "Windows": redirects to windows authentication mode
- "Forms": redirects to FBA authentication mode
- "Trusted:TrustName": Redirects to trusted provider specified in "TrustName". Note that you can just specify "Trusted", whihch will redirect to any trusted provider enabled on the zone.

Sample to set this property:
```powershell
$farm = Get-SPFarm
# To create property:
$farm.Properties.Add("CustomBypassLogin", "Trusted:TrustName")
$farm.Update()
# To update it:
$farm.Properties["CustomBypassLogin"] = "prompt"
$farm.Update()
# To remove it:
$farm.Properties.Remove("CustomBypassLogin")
$farm.Update()
```
