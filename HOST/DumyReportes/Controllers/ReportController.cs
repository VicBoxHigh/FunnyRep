using DumyReportes.Data;
using DumyReportes.Filters;
using DumyReportes.Flags;
using DumyReportes.Models;
using DumyReportes.Util;
using System;
using System.Collections.Generic;
using System.IO;
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
    public class ReportController : ApiController
    {

        //WhoNotifiedReports and OwnerReports

        private readonly ReportDataContext _ReportDataContext = new ReportDataContext();




        //isowner TRUE FALSE
        //idUser  3088 3088
        //__________________
        //  Assigned | Created
        //So, the function can get all the header reports , by owner or not, depending on the IdUser

        ///Como se implementó autenticación, ya es posible hacerlo si estos parametros.

        //Los datos a entregar dependerán del nivel de usuario que hizo login.

        [HttpGet]
        public IHttpActionResult Get(/*bool isOwner, int idUser*/)
        {
            //los usuarios se dividen en niveles 0/10/20, 
            //todo empleado de Marves ya es 0, 
            //10 y 20 son asignados manualmente, y son independientes al usuario nivel 0.
            //Si el usuario es > 0, entonces se traerán los ReportesHeads asignados a ese user
            //en caso contrario, se traerán los creados por ese usuario/empleado
            UserIdentiy genericIdentity = HttpContext.Current.User.Identity as UserIdentiy;

            if (genericIdentity == null || !genericIdentity.IsAuthenticated) return Unauthorized();

            User user = genericIdentity.user;

            //Cualquier tipo de request podría traer reportes asignados a un agente y no asignados así que se tendrán 2 listas;

            List<Report> reportesAsignados = new List<Report>();
            List<Report> reportesNoAsignados = new List<Report>();
            string error = "";
            ErrorFlag flagResultGet = ErrorFlag.ERROR_OK_RESULT;

            switch (user.AccessLevel)
            {
                case AccessLevel.PUBLIC:
                    flagResultGet = _ReportDataContext.GetReportsByWhoNotified(user, out reportesAsignados, out reportesNoAsignados, out error);
                    break;
                case AccessLevel.AGENT://los AGENT solo aparecerán reportes asignados.
                    flagResultGet = _ReportDataContext.GetReportsByOwner(owner: user, out reportesAsignados, out error);
                    break;
                case AccessLevel.ADMIN:
                case AccessLevel.TI:
                    flagResultGet = _ReportDataContext.GetAllReports(reportAsigned: out reportesAsignados, reportsNoAsigned: out reportesNoAsignados, out error);

                    break;
            }

            if (reportesAsignados.Count == 0 && reportesNoAsignados.Count == 0) return Content(HttpStatusCode.NoContent, "La consulta no regresó reportes.");

            if (flagResultGet != ErrorFlag.ERROR_OK_RESULT) return Content(EvaluateErrorFlag(flagResultGet), error);

            return Ok(
                new
                {
                    reportesAsignados,
                    reportesNoAsignados
                }
            );


        }



        public HttpStatusCode EvaluateErrorFlag(Flags.ErrorFlag errorFlag)
        {
            HttpStatusCode resultCode;
            switch (errorFlag)
            {

                case Flags.ErrorFlag.ERROR_NO_UPDATED_RECORDS:
                    resultCode = HttpStatusCode.NotModified;
                    break;
                case ErrorFlag.ERROR_PARSE:
                    resultCode = HttpStatusCode.Conflict;
                    break;
                case Flags.ErrorFlag.ERROR_OK_RESULT:
                    resultCode = HttpStatusCode.OK;
                    break;
                case Flags.ErrorFlag.ERROR_RECORD_EXISTS://ya existe la entidad                    
                case Flags.ErrorFlag.ERROR_CONFLICT_CANT_DELETE:
                    resultCode = HttpStatusCode.Conflict;
                    break;
                case Flags.ErrorFlag.ERROR_DATABASE:
                    resultCode = HttpStatusCode.NotModified;
                    break;
                case ErrorFlag.UNKNOWN:
                    resultCode = HttpStatusCode.InternalServerError;
                    break;
                default:
                    resultCode = HttpStatusCode.InternalServerError;
                    break;


            }

            return resultCode;

        }


        //Get reports ByOwnerId
        //Get reports ByWhoNotifier


        //Crea ReportHeader
        // POST: api/Report
        public IHttpActionResult Post([FromBody] Report report)
        {
            if (report == null) return Content(HttpStatusCode.BadRequest, "Objeto nulo");
            if (!report.Validate()) return Content(HttpStatusCode.BadRequest, "Datos de la solicitud son inválidos.");

            UserIdentiy user = HttpContext.Current.User.Identity as UserIdentiy;
            if (user == null || !user.IsAuthenticated) return Content(HttpStatusCode.Unauthorized, "No está autorizado para realizar esta operación.");

            report.IdUserWhoNotified = user.user.IdUser;

            ErrorFlag resultCreateEvidence = EvidenceHelper.CreateEvidenceImg(report);

            if (resultCreateEvidence != ErrorFlag.ERROR_OK_RESULT) return ValidateResult(resultCreateEvidence);

            ErrorFlag resultCreation = _ReportDataContext.Create(report, out string error);

            if (resultCreation != Flags.ErrorFlag.ERROR_OK_RESULT)
            {
                return ValidateResult(resultCreation);
            }

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
                case ErrorFlag.ERROR_CREATION_ENITITY:
                    result = Content(HttpStatusCode.Conflict, error);
                    break;
                case ErrorFlag.ERROR_NOT_EXISTS:
                    result = Content(HttpStatusCode.NotFound, "El elemento no existe.");
                    break;
                case ErrorFlag.ERROR_NO_AFECTED_RECORDS:
                    result = Content(HttpStatusCode.NotModified, "Sin cambios");
                    break;
                case ErrorFlag.ERROR_CONNECTION_DB:
                    result = InternalServerError(new Exception("Error en base de datos"));
                    break;
                case ErrorFlag.ERROR_UNAUTHORIZED_ACCESS:
                    result = Content(HttpStatusCode.Forbidden, "No tiene permisos para acceder a la ruta de almacenamiento.");
                    break;
                case ErrorFlag.CANT_FIND_DIRECTORY:
                    result = Content(HttpStatusCode.NotFound, "No es posible encontrar la ruta de alacenamiento. ");
                    break;
                default:
                    result = BadRequest("No es posible procesar la solicitud.");
                    break;
            }

            return result;

        }



        //Los únicos cambios que puede haber en un reporte son la clasificación y el estado
        public IHttpActionResult Put([FromUri] int id, [FromUri] int newClasif, [FromUri] int newStatus)
        {
            UserIdentiy user = HttpContext.Current.User.Identity as UserIdentiy;
            if (user == null || !user.IsAuthenticated || user.user.AccessLevel < AccessLevel.AGENT) return Unauthorized();

            if (newClasif > -1 || newStatus > -1)
            {

                Flags.ErrorFlag resulUpdate = _ReportDataContext.Update(id, newClasif, newStatus, out string error);

                if (resulUpdate != Flags.ErrorFlag.ERROR_OK_RESULT) return ValidateResult(resulUpdate);
            }



            return StatusCode(HttpStatusCode.NoContent);


        }

        // DELETE: api/Report/5
        public IHttpActionResult Delete(int id)
        {

            return Content(HttpStatusCode.Forbidden, "Ningún reporte puede ser eliminado");
            if (id < 1) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString());


            Flags.ErrorFlag resultDelete = _ReportDataContext.Delete(id, out string error);


            if (resultDelete != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.NotModified, resultDelete.ToString());



            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
