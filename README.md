# SPBypassLoginPage
This farm solution works with SharePoint 2013. It adds a page that allows SharePoint administrators to bypass provider selection page when multiple authentication modes are enabled in a zone of a web application.

By default it redirects to the 1st trusted authentication mode it finds in the current zone (most common use-case), but administrators can customize behavior through custom farm property "CustomBypassLogin" (see below for more details).

## Installation
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases/latest)
- Install solution:
`Add-SPSolution "C:\Data\Dev\SPBypassLoginPage.wsp"`
- Deploy it: 
`Install-SPSolution -Identity "SPBypassLoginPage.wsp" -GACDeployment`
- Go to central administration site > Select your web application > "Authentication Providers" > Select the zone:
In "Sign In Page URL" section: check "Custom Sign In Page" > type URL below and validate:<br>
/_login/Bypass/BypassLogin.aspx

## How to configure
Custom farm property "CustomBypassLogin" can have following values:
- "prompt": displays the same experience to users as out of the box (they must choose the authentication mode to use)
- "Windows": redirects to windows authentication mode
- "Forms": redirects to FBA authentication mode
- "Trusted:TrustName": Redirects to the trusted identity provider (SPTrustedIdentityTokenIssuer) specified by "TrustName". If you just type "Trusted", it will redirect to any trusted provider enabled on the zone.

Sample to create, set and remove this property in PowerShell:
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

## How to Update
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases/latest)
- Update solution:

```powershell
Update-SPSolution -GACDeployment -Identity "SPBypassLoginPage.wsp" -LiteralPath "C:\Data\Dev\SPBypassLoginPage.wsp"
```

## Limitations
Currently it does not work with SharePoint 2016 because page references Microsoft.SharePoint.IdentityModel strongly typed to v15, which generates an assembly load error.
<br>I'm looking for a good solution to fix this.
