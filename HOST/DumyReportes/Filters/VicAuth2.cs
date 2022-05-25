﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Security.Principal;
using System.Web.Http.Filters;

namespace DumyReportes.Filters
{
    public class VicAuth2 : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //actionContext.Request.Properties.
    //        base.OnAuthorization(actionContext);
            if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                actionContext.Response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden)
                {
                    ReasonPhrase = "HTTPS Required for this call"
                };
            }
            else
            {
                base.OnAuthorization(actionContext);
            }
            /*Thread.CurrentPrincipal.IsAuthenticated;

            KeyValuePair<string, IEnumerable<string>> keyValuePair = actionContext.Request.Headers.FirstOrDefault(h => h.Key.Equals("Authorization"));
            if (actionContext.Request.Headers.Authorization == null) //No Auh method
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                return;
            }
            if(String.IsNullOrEmpty(actionContext.Request.Headers.Authorization.Parameter))
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
                return;
            }
           
            string paramsAuth = actionContext.Request.Headers.Authorization.Parameter;
            string parsed = Encoding.UTF8.GetString(Convert.FromBase64String(paramsAuth));

            string usrename = parsed.Split(':')[0];
            string password = parsed.Split(':')[1];*/





        }



    }
}