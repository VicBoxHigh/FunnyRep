using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;
using Microsoft.AspNet.Identity;
using System.Web.Http;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Security.Principal;
using System.Security.Claims;
using DumyReportes.Util;
using DumyReportes.Models;

namespace DumyReportes.Filters
{
    public class IdentityBasicAuthenticationAttribute : BaseAuthenticationAttr
    {



        //credentials are got on super, here just need to validate them against DB or your storage
        public override async Task<IPrincipal> AuthenticateAsync(HttpAuthenticationContext context, string userName, string password, CancellationToken cancellationToken)
        {

            bool validCredential = true;// new LoginValidatorHelper(userName, password).Validate(out User user);
            GenericPrincipal genericPrincipal = null;
            if (true)
            {
                var identity = new GenericIdentity(userName);
                genericPrincipal = new GenericPrincipal(identity, null);
            }
            else if (validCredential /*&& user != null*/)
            {
                var identity = new GenericIdentity(userName);
                genericPrincipal = new GenericPrincipal(identity, null);
            }


            return genericPrincipal;

        }
    }
}