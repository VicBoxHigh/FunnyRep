using DumyReportes.Data;
using DumyReportes.Filters;
using DumyReportes.Models;
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
    [IdentityBasicAuthentication]
    public class ReportController : ApiController
    {



        //WhoNotifiedReports and OwnerReports

        private readonly ReportDataContext _ReportDataContext = new ReportDataContext();

        /* // GET: api/Report //get ALL reports //get report header data only
         [Route("~/api/Report/all")]
         public IHttpActionResult Get()
         {



             return StatusCode(HttpStatusCode.NotImplemented);
         }*/



        //isowner TRUE FALSE
        //idUser  3088 3088
        //__________________
        //  Assigned | Created
        //So, the function can get all the header reports , by owner or not, depending on the IdUser

        ///Como se implementó autenticación, ya es posible hacerlo si estos parametros.

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



            // if (idUser < 1) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString());

            Flags.ErrorFlag result =
                _ReportDataContext.GetAllBy(
                    isOwner: user.AccessLevel != Flags.AccessLevel.PUBLIC,
                    idUser: user.IdUser,
                    reportObjects: out List<IReportObject> reportObjs,
                    error: out string error
                    );

            if (result != Flags.ErrorFlag.ERROR_OK_RESULT)
                return StatusCode(HttpStatusCode.NotFound);

            if (reportObjs.Count == 0) return StatusCode(HttpStatusCode.NoContent);

            return Ok(
                new
                {
                    reports = reportObjs
                }
            );


        }

        //Get specific Header report by ID, detail report data
        //hould report have the title?
        // GET: api/Report/5
        public IHttpActionResult Get(int id)
        {
            if (id < 1) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString());

            Flags.ErrorFlag resultGetReporDetail = _ReportDataContext.Get(id, out IReportObject reportObj, out string error);


            if (resultGetReporDetail != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.Conflict, resultGetReporDetail.ToString());


            return Ok(
                new
                {
                    report = reportObj
                }
                );


        }


        //Get reports ByOwnerId
        //Get reports ByWhoNotifier


        //Crea ReportHeader
        // POST: api/Report
        public IHttpActionResult Post([FromBody] Report report)
        {
            if (report == null) return BadRequest("Objeto nulo");
            if (!report.Validate()) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_OBJECT.ToString());

            string fileName = Guid.NewGuid().ToString() + ".png";
            string path = $"C:\\imgs\\";
            File.WriteAllBytes(path + fileName, Convert.FromBase64String(report.Pic64));

            report.FileNameEvidence = fileName;
            report.PathEvidence = path;

            Flags.ErrorFlag resultCreation = _ReportDataContext.Create(report, out string error);



            if (resultCreation != Flags.ErrorFlag.ERROR_OK_RESULT)
            {
                /*HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.NotModified)
                {
                    Content = new StringContent("No hubo cambios."),
                    ReasonPhrase = resultCreation.ToString()
                };

                throw new HttpResponseException(httpResponseMessage);*/
                return StatusCode(HttpStatusCode.NotModified);

                //return Content<string>(HttpStatusCode.NotModified, resultCreation.ToString());

            }

            return StatusCode(HttpStatusCode.Created);


        }


        // PUT: api/Report/5
        public IHttpActionResult Put(int id, [FromBody] Report report)
        {
            if (!report.Validate()) return BadRequest(Flags.ErrorFlag.ERROR_VALIDATION_ENTITY.ToString());
            if (id != report.IdReport) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString());

            Flags.ErrorFlag resulUpdate = _ReportDataContext.Update(report, out string error);

            if (resulUpdate != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.Conflict, resulUpdate.ToString());



            return StatusCode(HttpStatusCode.NoContent);


        }

        // DELETE: api/Report/5
        public IHttpActionResult Delete(int id)
        {
            if (id < 1) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString());


            Flags.ErrorFlag resultDelete = _ReportDataContext.Delete(id, out string error);


            if (resultDelete != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.NotModified, resultDelete.ToString());



            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
