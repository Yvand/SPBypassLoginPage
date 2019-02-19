using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.IdentityModel.Pages;
using Microsoft.SharePoint.Utilities;
using System;
using System.Web;
using System.Web.UI.WebControls;

namespace Yvand.SPBypassLoginPage
{
    public class Utilities
    {
        public const string AuthModeWindows = "Windows";
        public const string AuthModeForms = "Forms";
        public const string AuthModeTrusted = "Trusted";
        public const string DisplayAllAuthNModes = "prompt";

        public const string WSFedHomeRealm = "whr";
        public const string WSFedWAuth = "wauth";


        public static string GetSubString(string value, char separator, int index)
        {
            int stop = value.IndexOf(separator);
            if (stop == -1) return String.Empty;
            string[] array = value.Split(separator);
            if (array.Length < index + 1) return String.Empty;
            return array[index];
        }
    }

    public partial class BypassLogin : IdentityModelSignInPageBase
    {
        const string CustomLoginProperty = "CustomBypassLogin";
        public string LoginMode
        {
            get
            {
                if (!SPFarm.Local.Properties.ContainsKey(CustomLoginProperty))
                {
                    //    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    //    {
                    //        base.Web.AllowUnsafeUpdates = true;
                    SPFarm.Local.Properties.Add(CustomLoginProperty, "Trusted");
                    //        //SPFarm.Local.Update();
                    //        base.Web.AllowUnsafeUpdates = false;
                    //    });
                }
                return SPFarm.Local.Properties[CustomLoginProperty].ToString();
            }
        }

        /// <summary>
        /// Override OnPreInit to prevent call to base.OnPreInit(e) to avoid redirection for mobile devices
        /// which results in an endless loop
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            //base.OnPreInit(e);
        }

#if SP2013
        /// <summary>
        /// Property UnsecuredLayoutsPageBase.IisSettings was introduced in April 2014 CU v15.0.4605.1004 (1st CU post SP1).
        /// Because of this, it must be overridden here (without override keyword) to handle plain SP1 farms (15.0.4569.1000 and 15.0.4571.1502).
        /// </summary>
        public SPIisSettings IisSettings
        {
            get
            {
                return SPContext.Current.Site.WebApplication.GetIisSettingsWithFallback(SPContext.Current.Site.Zone);
            }
        }
#endif

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack) return;

            ClaimsLogonPageTitle.Text =
                SPHttpUtility.NoEncode((string)HttpContext.GetGlobalResourceObject("wss", "login_pagetitle", System.Threading.Thread.CurrentThread.CurrentUICulture));
            ClaimsLogonPageTitleInTitleArea.Text =
                SPHttpUtility.NoEncode((string)HttpContext.GetGlobalResourceObject("wss", "login_pagetitle", System.Threading.Thread.CurrentThread.CurrentUICulture));
            ClaimsLogonPageMessage.Text = SPHttpUtility.NoEncode(SPResource.GetString(Strings.SelectAuthenticationMethod));

            if (ClientQueryString.Contains(Utilities.DisplayAllAuthNModes) ||
                String.Equals(LoginMode, Utilities.DisplayAllAuthNModes, StringComparison.InvariantCultureIgnoreCase))
                LetUserChoose();
            else
                HandleRedirect(LoginMode);
        }

        /// <summary>
        /// Redirect user to the authentication mode configured by the administrator or selected by the user
        /// </summary>
        /// <param name="value">authentication mode to use</param>
        private void HandleRedirect(string value)
        {
            Type typeSelected = null;
            string trustedProviderName = String.Empty;
            GetAuthModeAndProviderName(value, out typeSelected, out trustedProviderName);

            string redirectUrl = String.Empty;
            foreach (SPAuthenticationProvider provider in IisSettings.ClaimsAuthenticationProviders)
            {
                if (provider.GetType() != typeSelected) continue;
                redirectUrl = provider.AuthenticationRedirectionUrl.OriginalString;
                if (provider.GetType() != typeof(SPTrustedAuthenticationProvider))
                    redirectUrl += "?";
                else if (String.Equals(provider.DisplayName, trustedProviderName, StringComparison.InvariantCultureIgnoreCase))
                    break;
            }

            // admin sets an authentication mode that is not enabled on the zone, redirectUrl will be empty
            // In this scenario, fallback to let user choose which authentication mod he wants to use
            if (String.IsNullOrEmpty(redirectUrl))
            {
                LetUserChoose();
                return;
            }

            // Get all original query string parameters.
            System.Text.StringBuilder additionalParameters = new System.Text.StringBuilder(2048);
            if (!redirectUrl.EndsWith("&") && !redirectUrl.EndsWith("?")) additionalParameters.Append("&");
            foreach (string key in this.Request.QueryString.Keys)
            {
                additionalParameters.Append(key + "=" + Server.UrlEncode(this.Request.QueryString[key]) + "&");
            }
            //additionalParameters.Append(HomeRealm + "=" + "testrealm" + "&");
            //additionalParameters.Append(WAuth + "=" + "urn:oasis:names:tc:SAML:1.0:am:password" + "&");

            string fullUrl = redirectUrl + additionalParameters.ToString();
#if DEBUG
            this.Response.Write(fullUrl);
#else
            this.Response.Redirect(fullUrl);
#endif
        }

        private void LetUserChoose()
        {
            ClaimsLogonSelector.AutoPostBack = true;
            ClaimsLogonSelector.Focus();
            ClaimsLogonSelector.Items.Add(new ListItem("", "none"));
            foreach (SPAuthenticationProvider provider in IisSettings.ClaimsAuthenticationProviders)
            {
                string value = String.Empty;
                if (provider.GetType() == typeof(SPWindowsAuthenticationProvider))
                    value = Utilities.AuthModeWindows;
                else if (provider.GetType() == typeof(SPFormsAuthenticationProvider))
                    value = Utilities.AuthModeForms;
                else if (provider.GetType() == typeof(SPTrustedAuthenticationProvider))
                    value = Utilities.AuthModeTrusted + String.Format(":{0}", provider.DisplayName);
                ClaimsLogonSelector.Items.Add(new ListItem(provider.DisplayName, value));
            }

            if (ClaimsLogonSelector.Items.Count == 1)
            {
                ClaimsLogonSelector.Style.Add("display", "none");
            }
        }

        protected void ClaimsLogonSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            string redirectUrl = String.Empty;
            HandleRedirect(ClaimsLogonSelector.SelectedValue);
        }

        private void GetAuthModeAndProviderName(string value, out Type type, out string providerName)
        {
            type = null;
            providerName = String.Empty;
            if (value == Utilities.AuthModeWindows) type = typeof(Microsoft.SharePoint.Administration.SPWindowsAuthenticationProvider);
            else if (value == Utilities.AuthModeForms) type = typeof(Microsoft.SharePoint.Administration.SPFormsAuthenticationProvider);
            else if (value.StartsWith(Utilities.AuthModeTrusted))
            {
                type = typeof(Microsoft.SharePoint.Administration.SPTrustedAuthenticationProvider);
                providerName = Utilities.GetSubString(value, ':', 1);
            }
        }
    }
}
