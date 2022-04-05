﻿using System;
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



        //credentials are got on super, here just need to validate them against DB or your storage to know the 
        public override async Task<IPrincipal> AuthenticateAsync(HttpAuthenticationContext context, string userName, string pass, CancellationToken cancellationToken)
        {
            LoginValidatorHelper loginValidatorHelper = new LoginValidatorHelper(userName, pass);
            GenericPrincipal genericPrincipal = null;
            try
            {

            if (loginValidatorHelper.Validate(out User user))//false? entonces no access a user, 
            {
                genericPrincipal = createPrincipal(user, "Basic");
                string token = TokenHelper.GenerateToken(user);
                user.CurrentToke = token;

            }
            }
            catch( Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }


            return genericPrincipal;

        }

        //identifica las propiedades del principal para así asignar su rol o algo más
        private GenericPrincipal createPrincipal(User user, string authType)
        {

            var identity = new UserIdentiy(user, authType);
            
            GenericPrincipal genericPrincipal = new GenericPrincipal(identity, new string[] { user.AccessLevel.ToString() });

            return genericPrincipal;
        }

        public override async Task<IPrincipal> AuthenticateAsync(HttpAuthenticationContext context, string token, CancellationToken cancellationToken)
        {

            User user = TokenHelper.ValidateToke(token);

            GenericPrincipal genericPrincipal = null;

            if (user != null)
            {
                genericPrincipal = createPrincipal(user,"Bearer");
            }

            return genericPrincipal;
        }


    }
}