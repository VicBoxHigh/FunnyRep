using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace DumyReportes.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IHttpActionResult /*BadRequestResult*/ Get()
        {
            return null;
            return NotFound();
            return Ok(new { a = "1" });
            return StatusCode(HttpStatusCode.Created);

            return BadRequest(); 
           /*/* return ((ApiController)this).*/
            // return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
             
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
