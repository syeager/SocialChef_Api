using Microsoft.AspNetCore.Server.IISIntegration;

namespace IdentityServer4.Quickstart.UI
{
    public static class AccountOptions
    {
        public const bool AllowLocalLogin = true;
        public const bool AllowRememberLogin = true;
        public const bool ShowLogoutPrompt = true;
        public const bool AutomaticRedirectAfterSignOut = false;
        public const string WindowsAuthenticationSchemeName = IISDefaults.AuthenticationScheme;
        public const string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}