using BasicAuthentication.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace DumyReportes.Filters
{
    public abstract class BaseAuthenticationAttr : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => throw new NotImplementedException();

        //para indicar usuario valido, ErrorResult remains null? 
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            System.Net.Http.HttpRequestMessage request = context.Request;
            System.Net.Http.Headers.AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null) return;

            IPrincipal principal = null;

            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Solicitud sin credenciales", request);
                return;
            }

            if (!authorization.Scheme.Equals("Basic") && !authorization.Scheme.Equals("Bearer"))
            {

                return;
            }

            if (authorization.Scheme.Equals("Basic"))//Login
            {
                Tuple<string, string> userPass = ExtractUserNameAndPassword(authorization.Parameter);

                if (userPass == null)
                {
                    context.ErrorResult = new AuthenticationFailureResult("Credenciales nos válidas", request);
                    return;
                }
                var user = userPass.Item1;
                var pass = userPass.Item2;

                Task<Dictionary<string, object>> task = AuthenticateAsync(context, user, pass, cancellationToken);

                //principal = await task;
                Dictionary<string, object> resultAuth = await task;
                object resultError = "";
                resultAuth.TryGetValue("error", out resultError);
                if (!String.IsNullOrEmpty(resultError as string))
                {
                    context.ErrorResult = new AuthenticationFailureResult(resultError as string, request);
                    return;
                }

                resultAuth.TryGetValue("IPrincipal", out object principalObj);
                principal = principalObj as IPrincipal;




            }
            else if (authorization.Scheme.Equals("Bearer"))//Any othe actiona
            {
                //string token = Convert.FromBase64String(authorization.Parameter);

                //principal
                Dictionary<string, object> result = await AuthenticateAsync(context, authorization.Parameter, cancellationToken);
                object resultError = "";
                result.TryGetValue("error", out resultError);
                if (!String.IsNullOrEmpty(resultError as string))
                {
                    context.ErrorResult = new AuthenticationFailureResult(resultError as string, request);
                    return;
                }

                result.TryGetValue("IPrincipal", out object principalObj);
                principal = principalObj as IPrincipal;


            }
            else //cualquier otra
            {
                return;
            }







            //https://racineennis.ca/2018/07/02/how-to-basic-authentication-filter-aspnet-web-api
            //https://www.youtube.com/watch?v=BZnmhyZzKgs
            //call specific implementation of this class

            //Task myTask = AuthenticateAsync(context, user, pass, cancellationToken);

            if (principal == null)
            {
                //Si la autenticación fue correcta, se tendrá un obj principal;
                context.ErrorResult = new AuthenticationFailureResult("Credenciales inválidas.", request);


            }
            else
            {
                /*Thread.CurrentPrincipal = principal;
                context.Principal = principal;*/
                HttpContext.Current.User = principal;
            }



        }

        public abstract Task<Dictionary<string, object>> AuthenticateAsync(HttpAuthenticationContext context, string user, string pass, CancellationToken cancellationToken);

        public abstract Task<Dictionary<string, object>> AuthenticateAsync(HttpAuthenticationContext context, string token, CancellationToken cancellationToken);





        private static Tuple<string, string> ExtractUserNameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            // The currently approved HTTP 1.1 specification says characters here are ISO-8859-1.
            // However, the current draft updated specification for HTTP 1.1 indicates this encoding is infrequently
            // used in practice and defines behavior only for ASCII.
            Encoding encoding = Encoding.ASCII;
            // Make a writable copy of the encoding to enable setting a decoder fallback.
            encoding = (Encoding)encoding.Clone();
            // Fail on invalid bytes rather than silently replacing and continuing.
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;
            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials))
            {
                return null;
            }

            int colonIndex = decodedCredentials.IndexOf(':');

            if (colonIndex == -1)
            {
                return null;
            }

            string userName = decodedCredentials.Substring(0, colonIndex);
            string password = decodedCredentials.Substring(colonIndex + 1);
            return new Tuple<string, string>(userName, password);
        }
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }
        public string Realm { get; set; }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter;

            if (String.IsNullOrEmpty(Realm))
            {
                parameter = null;
            }
            else
            {
                // A correct implementation should verify that Realm does not contain a quote character unless properly
                // escaped (precededed by a backslash that is not itself escaped).
                parameter = "realm=\"" + Realm + "\"";
            }
            //hace challenge con auth basic,
            context.ChallengeWith("Basic", parameter);
        }
 
    }
}