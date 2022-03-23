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
    public class ReportController : ApiController
    {



        //WhoNotifiedReports and OwnerReports

        private readonly ReportDataContext _ReportDataContext = new ReportDataContext();

        // GET: api/Report //get ALL reports //get report header data only
        [Route("~/api/Report/all")]
        public IHttpActionResult Get()
        {



            return StatusCode(HttpStatusCode.NotImplemented);
        }



        //isowner TRUE FALSE
        //idUser  3088 3088
        //__________________
        //  Assigned | Created
        //So, the function can get all the header reports , by owner or not, depending on the IdUser

        public IHttpActionResult Get(bool isOwner, int idUser)
        {
            if (idUser < 1) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString());

            Flags.ErrorFlag result =
                _ReportDataContext.GetAllBy(isOwner, idUser, out List<IReportObject> reportObjs, out string error);
                
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

            Flags.ErrorFlag resultGetReporDetail = _ReportDataContext.Get(id, out IReportObject reportObj ,out string error);


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

            if (!report.Validate()) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_OBJECT.ToString());



            Flags.ErrorFlag resultCreation = _ReportDataContext.Create(report,out string error);

            if (resultCreation != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.NotModified, resultCreation.ToString());


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
            if (id < 1) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_ID.ToString()) ;


            Flags.ErrorFlag resultDelete = _ReportDataContext.Delete(id, out string error);


            if (resultDelete != Flags.ErrorFlag.ERROR_OK_RESULT) return Content(HttpStatusCode.NotModified, resultDelete.ToString());



            return StatusCode(HttpStatusCode.NoContent);

        }
    }
}
