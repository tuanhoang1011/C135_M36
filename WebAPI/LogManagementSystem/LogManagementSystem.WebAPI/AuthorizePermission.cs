using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace LogManagementSystem.WebAPI
{
    public class AuthorizePermission : AuthorizeAttribute
    {
        public string Permission;

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var user = actionContext.RequestContext.Principal as ClaimsPrincipal;

            if (user == null || !user.Identity.IsAuthenticated)
                return false;

            Debug.Print(Permission);
            if (user.Claims.Any(claim => claim.Type.Equals("Permission") && Permission.Contains(claim.Value)))
                return true;

            return false;

        }
    }
}