using DumyReportes.Data;
using DumyReportes.Filters;
using DumyReportes.Flags;
using DumyReportes.Models;
using DumyReportes.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DumyReportes.Controllers
{


    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [IdentityBasicAuthenticationAttribute]
    public class ReportDtlController : ApiController
    {
        readonly ReportDtlContext _ReportDtlContext = new ReportDtlContext();


        //Todas las entries de un ReportHeader (RH), es independiente del usuario, ya que es gobernado por el IdReport (Header)
        // GET: api/ReportDtl
        [Route("~/api/ReportDtl/{idRH}")]
        public IHttpActionResult Get([FromUri] int idRH)
        {


            UserIdentiy user = HttpContext.Current.User.Identity as UserIdentiy;

            if (user == null || !user.IsAuthenticated) return Unauthorized();

            if (idRH < 1) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString());
            Flags.ErrorFlag resultGet = _ReportDtlContext.GetAll(idRH, out List<IReportObject> reportObjects, out string error);

            if (resultGet == ErrorFlag.ERROR_NOT_EXISTS) return Content(HttpStatusCode.Conflict, "El reporte proporcionado no existe");
            if (resultGet != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.Conflict, error);

            return Ok(
                    new
                    {
                        reportDtlEntries = reportObjects
                    }

                );

        }




        //Crea un ReportDtlEntry
        // POST: api/ReportDtl
        [HttpPost]
        public IHttpActionResult Post([FromBody] ReportDtlEntry reportDtlEntry)
        {
            UserIdentiy user = HttpContext.Current.User.Identity as UserIdentiy;

            if (user == null || !user.IsAuthenticated) return Unauthorized();

            new ReportDataContext().Get(reportDtlEntry.IdReport, out IReportObject report, out string error1);
            //ErrorFlag resultGetRep = _ReportDtlContext.Get();
            Report headFromDtl = report as Report;

            if (headFromDtl.IdStatus == ReportStatus.STATUS_COMPLETADA)
            {
                return Content(HttpStatusCode.Forbidden, "No es posible enviar una entrada a un reporte COMPLETADO.");
            }

            if (!reportDtlEntry.Validate()) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_OBJECT.ToString());

            //Pasa el usuario que está creando el entry
            Flags.ErrorFlag resultCreate = _ReportDtlContext.Create2(reportDtlEntry, user.user, out string error);

            if (resultCreate != Flags.ErrorFlag.ERROR_OK_RESULT) return ValidateResult(resultCreate, error);

            return StatusCode(HttpStatusCode.Created);

        }

        private IHttpActionResult ValidateResult(ErrorFlag resultCreateEvidence, string error = "")
        {
            IHttpActionResult result;
            switch (resultCreateEvidence)
            {
                case ErrorFlag.ERROR_NO_FILE_TO_WRITE:

                    result = InternalServerError(new Exception("No es posible escribir el archivo. Operación abortada."));
                    break;
                case ErrorFlag.ERROR_NOT_EXISTS:
                    result = NotFound();
                    break;
                case ErrorFlag.ERROR_NO_AFECTED_RECORDS:
                    result = InternalServerError(new Exception("Error en base de datos"));
                    break;
                case ErrorFlag.ERROR_UNAUTHORIZED_ACCESS:
                    result = Unauthorized();
                    break;
                case ErrorFlag.ERROR_CONNECTION_DB:
                    result = InternalServerError(new Exception("Error al conectar la base de datos"));
                    break;
                case ErrorFlag.ERROR_CREATION_ENITITY:
                    result = Content(HttpStatusCode.InternalServerError, error);
                    break;
                case ErrorFlag.ERROR_PARSE:
                    result = Content(HttpStatusCode.BadRequest, error);
                    break;
                default:
                    result = BadRequest("Error desconocido");
                    break;
            }

            return result;

        }


    }
}
