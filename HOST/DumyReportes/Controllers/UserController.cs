using DumyReportes.Data;
using DumyReportes.Filters;
using DumyReportes.Models;
using DumyReportes.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DumyReportes.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UserController : ApiController
    {

        private readonly UserDataContext _UserDataContext = new UserDataContext();
        // GET: api/User
        [Route("~/api/User/all")]
         public /*IEnumerable<string>*/ IHttpActionResult Get([FromBody] bool something)
        {

            Flags.ErrorFlag result = _UserDataContext.GetAll(out List<IReportObject> users, out string error);

            return Ok(new
            {
                users,
                errorCode = result.ToString()

            });

        }

        public IHttpActionResult Get(int id)
        {
            if (id < 1) return BadRequest("ID no válido");

           
            Flags.ErrorFlag result = _UserDataContext.Get(id, out  IReportObject user  , out string error);
            

            if (result == Flags.ErrorFlag.ERROR_NOT_EXISTS)
            {
                return Content(HttpStatusCode.NotFound, result.ToString());
            
            }

            return
                Ok(
                    new
                    {
                        resultCode = result.ToString(),
                        errorMsg = error,
                        user = user /*as User*/,

                    }
                );


        }



        private static int HIDING_FACTOR = 54 * 13 * 4;

        // [Route("~/api/User/ ")]
        [IdentityBasicAuthentication]
        [HttpGet]
        public IHttpActionResult Get(/*[FromUri]int numEmpleado, [FromUri] string pass*/)
        {
            //if (numEmpleado < 1 && !String.IsNullOrEmpty(pass)) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_CREDENTIALS.ToString());
            UserIdentiy genericIdentity = HttpContext.Current.User.Identity as UserIdentiy;

            if (genericIdentity == null || !genericIdentity.IsAuthenticated) return Unauthorized();



            return Ok(new
            {
                token = genericIdentity.user.CurrentToke,
                levelUser = (int)genericIdentity.user.AccessLevel * HIDING_FACTOR

            }); ;

        }

        /*
                [Route("~/api/User/LoginAdm/{numEmpleado:int}, {pass:string]}")]
                public IHttpActionResult Post([FromUri] string usuario, [FromUri] string pass)
                {
                    if (numEmpleado < 1 && !String.IsNullOrEmpty(pass)) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_CREDENTIALS.ToString());





                }
        */
        [HttpPost]
        /*   [AllowAnonymous]*/

         
         [Route("~/api/User/V ")]
        public IHttpActionResult OK(int a)
        {
            return Ok("ok");
        }

        // POST: api/User
     
        public IHttpActionResult Post(/*[FromBody]*/string numEmpleado, string userName, string pass, bool isEnable, int accessLevel)
        {

            User user = new User(
                    numEmpleado,
                    userName,
                    pass,
                    isEnable,
                    (Flags.AccessLevel)accessLevel

                );

            //Válida la data
            bool isValid = user.Validate();
            if (!isValid) return BadRequest(Flags.ErrorFlag.ERROR_INVALID_OBJECT.ToString()); 

            //Inserta en DB
            Flags.ErrorFlag resultCreate = _UserDataContext.Create(user, out string error);

            if (resultCreate != Flags.ErrorFlag.ERROR_OK_RESULT) return BadRequest(resultCreate.ToString());

            return StatusCode(HttpStatusCode.Created);// resultCreate.ToString();

        }

        //including user disabling 
        // PUT: api/User/5
        [HttpPut]
         
        public IHttpActionResult Put(int id, [FromBody] User user)
        {
            bool isValid = user.Validate();
            if (!isValid) return Content(HttpStatusCode.BadRequest, Flags.ErrorFlag.ERROR_INVALID_OBJECT.ToString());
            user.IdUser = id;

            Flags.ErrorFlag result = _UserDataContext.Update(user, out string error);

            if (result == Flags.ErrorFlag.ERROR_NO_UPDATED_RECORDS)
                return Content(HttpStatusCode.NoContent, result.ToString());


            return Ok();

        }

        [HttpDelete]
        // DELETE: api/User/5
        
        public IHttpActionResult Delete(int id)
        {

            Flags.ErrorFlag result = _UserDataContext.Delete(id, out string error);


            return Ok();


        }
    }
}
