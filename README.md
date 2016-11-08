# SPBypassLoginPage
This farm solution works with SharePoint 2013 and SharePoint 2016. It adds a page that allows SharePoint administrators to bypass provider selection page when multiple authentication modes are enabled in a zone of a web application.

## Installation
The package depends on SharePoint version: For SharePoint 2013 use "SPBypassLoginPage SP15.wsp" and for SharePoint 2016 use "SPBypassLoginPage SP16.wsp".
- Download [latest release](https://github.com/Yvand/SPBypassLoginPage/releases/latest)
- Install and deploy farm solution:
```powershell
Add-SPSolution "C:\Data\Dev\SPBypassLoginPage SP15.wsp"
Install-SPSolution -Identity "SPBypassLoginPage SP15.wsp" -GACDeployment
```
- Enable the custom login page, either through central administration or with PowerShell:
```powershell
Set-SPWebApplication "http://sp/" -Zone "Default" -SignInRedirectUrl "/_login/Bypass/BypassLogin.aspx"
```

## Configuration
By default it redirects to the 1st trusted authentication mode it finds in the current zone (most common use-case), but administrators can customize behavior through custom farm property "CustomBypassLogin":
- "prompt": displays the same experience as out of the box: users must choose the authentication mode to use
- "Windows": redirects to windows authentication mode
- "Forms": redirects to FBA authentication mode
- "Trusted:TrustName": redirects to the [trusted provider](https://technet.microsoft.com/en-us/library/ff607829.aspx) specified by "TrustName".
- "Trusted": redirects to any [trusted provider](https://technet.microsoft.com/en-us/library/ff607829.aspx) enabled on the zone.

This property can be managed easily with PowerShell:
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
- Revert to default provider selection page:
```powershell
Set-SPWebApplication "http://sp/" -Zone "Default" -SignInRedirectUrl ""
```
- Remove solution:
```powershell
$farm = Get-SPFarm
$farm.Properties.Remove("CustomBypassLogin")
$farm.Update()
Uninstall-SPSolution -Identity "SPBypassLoginPage SP15.wsp"
Remove-SPSolution -Identity "SPBypassLoginPage SP15.wsp"
```
