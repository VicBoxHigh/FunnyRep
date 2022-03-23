using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DumyReportes.Controllers
{
    public class ReportDtlController : ApiController
    {
        //Todas las entries de un ReportHeader, es independiente del usuario, ya que es gobernado por el IdReport (Header)
        // GET: api/ReportDtl
        [Route("~/api/ReportDtl/all/{idReportHeader}")]
        public IHttpActionResult Get(int idReportHeader)
        {
            
        }
        //Una ReportDtlEntry en especifico //no sería de uso publico
        // GET: api/ReportDtl/5
        public IHttpActionResult Get(int id)
        {  

        }

        //Crea un ReportDtlEntry
        // POST: api/ReportDtl
        public IHttpActionResult Post([FromBody] value)
        {
        }

        // PUT: api/ReportDtl/5
        public IHttpActionResult Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ReportDtl/5
        public IHttpActionResult Delete(int id)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
