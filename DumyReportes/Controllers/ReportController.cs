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

        // GET: api/Report //get all reports //get report header data only
        [Route("~/api/Report/all")]
        public IHttpActionResult Get()
        {



            return StatusCode(HttpStatusCode.NotImplemented);
        }



        //isowner TRUE FALSE
        //idUser  3088 3088
        //__________________
        //  Assigned | Created
        //So, he function can get all the reports, by owner or not, depending on the IdUser
        
        public IHttpActionResult Get(bool isOwner,int idUser)
        {

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
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Report/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Report/5
        public void Delete(int id)
        {
        }
    }
}
