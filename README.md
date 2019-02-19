# SPBypassLoginPage for SharePoint 2019 / 2016 / 2013

This farm solution adds a page that replaces standard provider selection page when multiple authentication modes are enabled in a zone of a web application.  
SharePoint administrators can configure it to set what authentication mode should be used.

[![Build status](https://dev.azure.com/Yvand/SPBypassLoginPage/_apis/build/status/SPBypassLoginPage-CI)](https://dev.azure.com/Yvand/SPBypassLoginPage/_build/latest?definitionId=4)
[Total downloads](https://img.shields.io/github/downloads/Yvand/SPBypassLoginPage/total.svg?style=flat)
[Downloads of latest release](https://img.shields.io/github/downloads/Yvand/SPBypassLoginPage/latest/total.svg?style=flat

## Installation

The package to install depends on SharePoint version, but besides the name of the package, the steps are the same for all SharePoint versions:

* Download the [latest release](https://github.com/Yvand/SPBypassLoginPage/releases/latest)
* Install and deploy farm solution:

```powershell
Add-SPSolution "C:\Data\Dev\SPBypassLoginPage SP2019.wsp"
Install-SPSolution -Identity "SPBypassLoginPage SP2019.wsp" -GACDeployment
```

* Enable the custom login page, either through central administration or with PowerShell:

```powershell
Set-SPWebApplication "http://spsites/" -Zone "Default" -SignInRedirectUrl "/_login/Bypass/BypassLogin.aspx"
```

## Configuration

By default it redirects to the 1st trusted authentication mode it finds in the current zone (most common use-case), but administrators can customize behavior through custom farm property "CustomBypassLogin":

* "prompt": displays the same experience as out of the box: users must choose the authentication mode to use
* "Windows": redirects to windows authentication mode
* "Forms": redirects to FBA authentication mode
* "Trusted:SPTrustedIdentityTokenIssuerName": redirects to the [SPTrustedLoginProvider](https://technet.microsoft.com/en-us/library/ff607829.aspx) specified by "SPTrustedIdentityTokenIssuerName".
* "Trusted": redirects to any [trusted provider](https://technet.microsoft.com/en-us/library/ff607829.aspx) enabled on the zone.

This property can be set with PowerShell:

```powershell
$farm = Get-SPFarm
# To create property:
$farm.Properties.Add("CustomBypassLogin", "Trusted:SPTrustedIdentityTokenIssuerName")
$farm.Update()
# To update it:
$farm.Properties["CustomBypassLogin"] = "prompt"
$farm.Update()
# To remove it:
$farm.Properties.Remove("CustomBypassLogin")
$farm.Update()
```

## How to remove the package from the farm

* Revert to the default provider selection page:

```powershell
Set-SPWebApplication "http://spsites/" -Zone "Default" -SignInRedirectUrl ""
```

* Remove

```powershell
$farm = Get-SPFarm
$farm.Properties.Remove("CustomBypassLogin")
$farm.Update()
Uninstall-SPSolution -Identity "SPBypassLoginPage SP2019.wsp"
Remove-SPSolution -Identity "SPBypassLoginPage SP2019.wsp"
```

* Remove

Due to breaking changes in the latest version, cmdlet Update-SPSolution won't work. Instead, you need to remove and reinstall the solution.
