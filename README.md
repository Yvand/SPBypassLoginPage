# SPBypassLoginPage
This farm solution works with SharePoint 2013 and SharePoint 2016. It adds a page that allows SharePoint administrators to bypass provider selection page when multiple authentication modes are enabled in a zone of a web application.

By default it redirects to the 1st trusted authentication mode it finds in the current zone (most common use-case), but administrators can customize behavior through custom farm property "CustomBypassLogin" (see below for more details).

## Installation
There is a different package for each SharePoint version: For SharePoint 2013 use "SPBypassLoginPage SP15.wsp" and for SharePoint 2016 use "SPBypassLoginPage SP16.wsp".
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases/latest)
- Install and deploy solution:
```powershell
Add-SPSolution "C:\Data\Dev\SPBypassLoginPage SP15.wsp"
Install-SPSolution -Identity "SPBypassLoginPage SP15.wsp" -GACDeployment
```
- Go to central administration > select your web application > "Authentication Providers" > Select the zone:<br>
In "Sign In Page URL" section: check "Custom Sign In Page", type URL below and validate:<br>
/_login/Bypass/BypassLogin.aspx

## Configuration
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

## How to update solution
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases/latest)
- Update solution:
```powershell
Update-SPSolution -GACDeployment -Identity "SPBypassLoginPage SP15.wsp" -LiteralPath "C:\Data\Dev\SPBypassLoginPage SP15.wsp"
```

## How to remove solution
- Revert sign-in page to the default one in web application settings
- Remove solution
```powershell
$farm = Get-SPFarm
$farm.Properties.Remove("CustomBypassLogin")
$farm.Update()
Uninstall-SPSolution -Identity "SPBypassLoginPage SP15.wsp"
Remove-SPSolution -Identity "SPBypassLoginPage SP15.wsp"
```
