# Change log for SPBypassLoginPage

## Unreleased

* Breaking change: Remove SharePoint 2013
* Breaking change: Packages for SharePoint 2019 and SharePoint 2016 are built using .NET Framework 4.7.2 (over .NET Framework 4.6.2)
* Use preferentially HTTP header HTTP_X_FORWARDED_FOR to get the IP address of the client, and fallback to REMOTE_ADDR if it does not exist
* Improve the readability of the code
* Make authentication mode checks case-insensitive

## SPBypassLoginPage 2019.12.16.9

* Add property CustomBypassLogin_WindowsAuthIPs to redirect specified client IPs to Windows authentication endpoint
* Handle scenario where value of farm properties is null

## SPBypassLoginPage 2019.02.19.10

* Use Azure DevOps to build and release SPBypassLoginPage
* Add version for SharePoint 2019 - SPBypassLoginPage SP2019.wsp
* Rename WSP package for SharePoint 2016 to SPBypassLoginPage SP2016.wsp
* Rename WSP package for SharePoint 2013 to SPBypassLoginPage SP2013.wsp
