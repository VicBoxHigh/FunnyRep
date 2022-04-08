﻿using DumyReportes.Data;
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

        public IHttpActionResult Get()
        {
            return StatusCode(HttpStatusCode.NotImplemented);

            ErrorFlag resultGet = _ReportDtlContext.GetAll(out List<IReportObject> reportObjects, out string error);

            if (resultGet != ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.Conflict, resultGet.ToString());

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
        public IHttpActionResult Get([FromUri] int idRH)
        {
            if (idRH < 1) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString());


            UserIdentiy user = HttpContext.Current.User.Identity as UserIdentiy;

            if (user == null || !user.IsAuthenticated) return Unauthorized();

            Flags.ErrorFlag resultGet = _ReportDtlContext.GetAll(idRH, out List<IReportObject> reportObjects, out string error);

            if (resultGet == ErrorFlag.ERROR_NOT_EXISTS) return StatusCode(HttpStatusCode.NoContent);
            if (resultGet != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.Conflict, resultGet.ToString());

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

            if (!reportDtlEntry.Validate()) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_OBJECT.ToString());

            ErrorFlag resultCreateEvidence = EvidenceHelper.CreateEvidenceImg(reportDtlEntry);

            if (resultCreateEvidence != ErrorFlag.ERROR_OK_RESULT && resultCreateEvidence != ErrorFlag.ERROR_NO_FILE_TO_WRITE) return ValidateResult(resultCreateEvidence);

            Flags.ErrorFlag resultCreate = _ReportDtlContext.Create(reportDtlEntry, out string error);


            if (resultCreate != Flags.ErrorFlag.ERROR_OK_RESULT) return ValidateResult(resultCreate);


            return StatusCode(HttpStatusCode.Created);

        }

        private IHttpActionResult ValidateResult(ErrorFlag resultCreateEvidence)
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
                    
                default:
                    result = BadRequest("Error desconocido");
                    break;
            }

            return result;

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
