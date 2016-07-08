using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.IdentityModel.Pages;
using System;
using System.Diagnostics;

namespace CustomRedirect
{
    public partial class BypassLogin : IdentityModelSignInPageBase
    {
        const string CustomLoginProperty = "CustomLoginProperty";
        
        const string WSFedHomeRealm = "whr";
        const string WSFedWAuth = "wauth";
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            HandleRedirect();
        }

        private void HandleRedirect()
        {
            try
            {
                var webApp = SPContext.Current.Site.WebApplication;
                if (webApp == null) return;

                // Get redirection URL of SAML trust in current zone of current web app
                string redirectUrl = String.Empty;
                SPIisSettings iisSettings = webApp.GetIisSettingsWithFallback(SPContext.Current.Site.Zone);
                if (!iisSettings.UseTrustedClaimsAuthenticationProvider)
                    return;

                foreach (SPAuthenticationProvider prov in iisSettings.ClaimsAuthenticationProviders)
                {
                    if (prov.GetType() == typeof(SPTrustedAuthenticationProvider))
                    {
                        redirectUrl = prov.AuthenticationRedirectionUrl.ToString();
                        break;
                    }
                }

                // Get all original query string parameters.
                System.Text.StringBuilder additionalParameters = new System.Text.StringBuilder(2048);
                additionalParameters.Append("&");
                foreach (string key in this.Request.QueryString.Keys)
                {
                    additionalParameters.Append(key + "=" + Server.UrlEncode(this.Request.QueryString[key]) + "&");
                }
                //additionalParameters.Append(HomeRealm + "=" + "testrealm" + "&");
                //additionalParameters.Append(WAuth + "=" + "urn:oasis:names:tc:SAML:1.0:am:password" + "&");

                // Perform redirection
                string fullUrl = redirectUrl + additionalParameters.ToString();
                this.Response.Redirect(fullUrl);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
