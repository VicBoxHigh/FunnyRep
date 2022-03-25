using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace DumyReportes.Filters
{
    public class VicAuth : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //actionContext.Request.Properties.
             
            
            KeyValuePair<string, IEnumerable<string>> keyValuePair = actionContext.Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
            if (actionContext.Request.Headers.Authorization == null) //No Auh method
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                return;
            }
            if(String.IsNullOrEmpty(actionContext.Request.Headers.Authorization.Parameter))
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
           
            string paramsAuth = actionContext.Request.Headers.Authorization.Parameter;
            string parsed = Encoding.UTF8.GetString(Convert.FromBase64String(paramsAuth));

            string usrename = parsed.Split(':')[0];
            string password = parsed.Split(':')[1];
            base.OnAuthorization(actionContext);
            actionContext.prin
            //actionContext.Response.StatusCode = System.Net.HttpStatusCode.Unauthorized;

            //OAuthAuthorizationServerOptions



        }



    }
}