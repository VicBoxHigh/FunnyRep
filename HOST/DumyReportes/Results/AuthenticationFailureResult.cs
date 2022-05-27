using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace BasicAuthentication.Results
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            ReasonPhrase = reasonPhrase +" AuthFailedResult block";
            Request = request;
        }

        public string ReasonPhrase { get; private set; }

        public HttpRequestMessage Request { get; private set; }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            //Todos los reponses de estad clase son "No autorizdos"
            //y se entrega la razón de la no autorización, debe ser accesible desde ajjajx
            
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            ///response.Content = new StringContent(ReasonPhrase + )  response.Content + " " + this.ReasonPhrase;
           // response.Content.Headers.Add("Error", ReasonPhrase);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            response.Content = new StringContent(ReasonPhrase);//responseText in xhr result
 
            return response;
        }
    }
}