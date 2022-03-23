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
        //So, the function can get all the reports, by owner or not, depending on the IdUser

        public IHttpActionResult Get(bool isOwner, int idUser)
        {
            if (idUser < 1) return BadRequest("ID no válido");

            Flags.ErrorFlag result =
                _ReportDataContext.GetAllBy(isOwner, idUser, out List<IReportObject> reportObjs, out string error) :
                
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

        //Get specific report by ID, detail report data
        // GET: api/Report/5
        public string Get(int id)
        {
            return "value";
        }


        //Get reports ByOwnerId
        //Get reports ByWhoNotifier



        // POST: api/Report
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Report/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/Report/5
        public void Delete(int id)
        {
        }
    }
}
