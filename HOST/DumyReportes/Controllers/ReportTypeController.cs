using DumyReportes.Data;
using DumyReportes.Filters;
using DumyReportes.Flags;
using DumyReportes.Models;
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
    public class ReportTypeController : ApiController
    {
        // GET: api/ReportType
        ReportTypeContext _ReportTypeContext = new ReportTypeContext();

        public IHttpActionResult Get()
        {

            UserIdentiy genericIdentity = HttpContext.Current.User.Identity as UserIdentiy;

            if (genericIdentity == null || !genericIdentity.IsAuthenticated) return Unauthorized();

            User user = genericIdentity.user;

            Flags.ErrorFlag errorFlag = _ReportTypeContext.GetAll(out Dictionary<int,string> reportTypes, out string error);



            if (errorFlag == ErrorFlag.ERROR_OK_RESULT) return Ok(new
            {
                reportTypes
            });
            else
            {
                return Content(HttpStatusCode.NotFound, error);

            };


        }

 


    }
}
