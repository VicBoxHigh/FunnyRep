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
using DumyReportes.Flags;

namespace DumyReportes.Filters
{
    public class IdentityBasicAuthenticationAttribute : BaseAuthenticationAttr
    {



        //credentials are got on super, here just need to validate them against DB or your storage to know the 
        public override async Task<Dictionary<string, object>> AuthenticateAsync(HttpAuthenticationContext context, string userName, string pass, CancellationToken cancellationToken)
        {
            LoginValidatorHelper loginValidatorHelper = new LoginValidatorHelper(userName, pass);
            GenericPrincipal genericPrincipal = null;
            Dictionary<string, object> result = new Dictionary<string, object>();

            try
            {
                ErrorFlag resultValidate = loginValidatorHelper.Validate(out User user, out string error);
                if (resultValidate == ErrorFlag.ERROR_OK_RESULT)//false? entonces no access a user, 
                {
                    genericPrincipal = createPrincipal(user, "Basic");
                    string token = TokenHelper.GenerateToken(user);
                    user.CurrentToke = token;

                }
                result.Add("IPrincipal", genericPrincipal);
                result.Add("error", error);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }




            return result;

        }

        //identifica las propiedades del principal para así asignar su rol o algo más
        private GenericPrincipal createPrincipal(User user, string authType)
        {

            var identity = new UserIdentiy(user, authType);

            GenericPrincipal genericPrincipal = new GenericPrincipal(identity, new string[] { user.AccessLevel.ToString() });

            return genericPrincipal;
        }

        delegate int MyDelegatte(int a, int b);

        public override async Task<Dictionary<string, object>> AuthenticateAsync(HttpAuthenticationContext context, string token, CancellationToken cancellationToken)
        {

            ErrorFlag resultValidate = TokenHelper.ValidateToke(token, out User user, out string resultError);

            Dictionary<string, object> result = new Dictionary<string, object>();

            GenericPrincipal genericPrincipal = null;

            Dictionary<string, MyDelegatte> mudo;


            if (resultValidate != ErrorFlag.ERROR_OK_RESULT || user == null)
            {

                result.Add("error", resultError);
                result.Add("IPricipal", genericPrincipal);//principal explicit null

                return result;//null 
            }
            //si hay error parse, o expiration del token,  retornará Unauthorized de igual manera..

            if (user != null)
            {

                //genericPrincipal = createPrincipal(user, "Bearer");  //Solo usa bearer porque está función solo es llamada cuando hay token
                result.Add("IPrincipal", createPrincipal(user, "Bearer"));
                result.Add("error", "");
            }


            return result;
        }


    }
}