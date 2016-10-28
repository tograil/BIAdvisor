using BIAdvisor.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BIAdvisor.Web.Helpers
{
    public class ClaimsTransformationModule : ClaimsAuthenticationManager
    {
        private UserMethods _userMethods;

        public ClaimsTransformationModule()
        {
            _userMethods = new UserMethods();
        }
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal != null && incomingPrincipal.Identity.IsAuthenticated == true)
            {
                var name = ((ClaimsIdentity)incomingPrincipal.Identity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (name != null && !string.IsNullOrWhiteSpace(name.Value))
                {
                    //Get the first role from incoming request
                    //var role = ((ClaimsIdentity)incomingPrincipal.Identity).Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                    //if (role != null && !string.IsNullOrWhiteSpace(role.Value))
                    //{
                    //    return incomingPrincipal;
                    //}

                    //If role not found, get name from request and check the db for the role
                    //Check user in the db.
                    var dr = _userMethods.GetUser(name.Value);
                    if (dr != null && !string.IsNullOrWhiteSpace((dr["SecurityLevel"] ?? "").ToString()))
                    {
                        ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(new Claim(ClaimTypes.Role, dr["SecurityLevel"].ToString()));
                    }
                }
            }

            return incomingPrincipal;
        }
    }
}
