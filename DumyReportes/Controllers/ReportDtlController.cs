using DumyReportes.Data;
using DumyReportes.Models;
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
        readonly ReportDtlContext _ReportDtlContext = new ReportDtlContext();

        public IHttpActionResult Get()
        {
            return StatusCode(HttpStatusCode.NotImplemented);

            Flags.ErrorFlag resultGet = _ReportDtlContext.GetAll(out List<IReportObject> reportObjects, out string error);

            if (resultGet != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.Conflict, resultGet.ToString());

            return Ok(
                    new
                    {
                        reportsDtl = reportObjects
                    }

                );

        }

        //Todas las entries de un ReportHeader, es independiente del usuario, ya que es gobernado por el IdReport (Header)
        // GET: api/ReportDtl
        [Route("~/api/ReportDtl/{idRH}")]
        public IHttpActionResult Get(int idRH)
        {
            if (idRH < 1) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString());
            Flags.ErrorFlag resultGet = _ReportDtlContext.GetAll(idRH, out List<IReportObject> reportObjects, out string error);

            if (resultGet != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.Conflict, resultGet.ToString());

            return Ok(
                    new
                    {
                        reportsDtl = reportObjects
                    }

                );

        }




        //Crea un ReportDtlEntry
        // POST: api/ReportDtl
        [HttpPost]
        public IHttpActionResult Post([FromBody] ReportDtlEntry reportDtlEntry)
        {

            if (!reportDtlEntry.Validate()) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_OBJECT.ToString());

            Flags.ErrorFlag resultCreate = _ReportDtlContext.Create(reportDtlEntry, out string error);

            if (resultCreate != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.Conflict, resultCreate.ToString());


            return StatusCode(HttpStatusCode.Created);

        }

        // PUT: api/ReportDtl/5
        public IHttpActionResult Put(int id, [FromBody] string value)
        {
            return StatusCode(HttpStatusCode.NotImplemented);

        }

        // DELETE: api/ReportDtl/5
        public IHttpActionResult Delete(int id)
        {
            return StatusCode(HttpStatusCode.NotImplemented);
        }
    }
}
