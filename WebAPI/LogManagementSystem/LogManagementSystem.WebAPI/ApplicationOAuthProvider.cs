using LogManagementSystem.BLL;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LogManagementSystem.WebAPI
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {
        UserBUS userBUS = new UserBUS();
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            int statusLoggin = userBUS.CheckLogin(context.UserName, context.Password);
            if (statusLoggin == 0)
            {
                var role = userBUS.GetPermission(context.UserName);
                var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                identity.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
                identity.AddClaim(new Claim("Permission", role.Permission + ""));
                var additionalData = new AuthenticationProperties(new Dictionary<string, string>{
            {
                "role", Newtonsoft.Json.JsonConvert.SerializeObject(role.RoleName)
            },
            {
                "userID", Newtonsoft.Json.JsonConvert.SerializeObject(role.UserID)
            },
            {
                "permission", Newtonsoft.Json.JsonConvert.SerializeObject(role.Permission+"")
            }
        });
                var token = new AuthenticationTicket(identity, additionalData);
                context.Validated(token);
            }
            else
            {
                return;
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}