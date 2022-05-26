using DumyReportes.Data;
using DumyReportes.Filters;
using DumyReportes.Models;
using DumyReportes.Util;
using System;
using System.Collections.Generic;
using System.Data;
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
        [HttpGet]
        [Route("~/api/User/all")]
        [IdentityBasicAuthentication]
        public IHttpActionResult GetAll()
        {

            UserIdentiy userIdentiy = HttpContext.Current.User.Identity as UserIdentiy;
            if (userIdentiy == null || !userIdentiy.IsAuthenticated | userIdentiy.user.AccessLevel < Flags.AccessLevel.SUPERADMIN)
            {
                return Content(HttpStatusCode.Unauthorized, "No está autorizado para acceder.");
            }

            Flags.ErrorFlag result = _UserDataContext.GetAll(out List<IReportObject> users, out string error);

            return Ok(new
            {
                users,
                error = result.ToString() + error

            });

        }



        /*  public IHttpActionResult Get(int id)
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
                          user = user *//*as User*//*,

                      }
                  );


          }*/



        private static int HIDING_FACTOR = 54 * 13 * 4;

        // Cuando hace login
        [HttpGet]
        [IdentityBasicAuthentication]
        public IHttpActionResult Get()
        {

            UserIdentiy genericIdentity = HttpContext.Current.User.Identity as UserIdentiy;

            if (genericIdentity == null || !genericIdentity.IsAuthenticated
                || genericIdentity.user == null || genericIdentity.user.AccessLevel < Flags.AccessLevel.SUPERADMIN)
                return Content(HttpStatusCode.Unauthorized, "No tiene permisos para realizar esta acción.");



            return Ok(new
            {
                token = genericIdentity.user.CurrentToke,
                levelUser = (int)genericIdentity.user.AccessLevel * HIDING_FACTOR

            }); ;

        }


        [HttpPost]
        /*   [AllowAnonymous]*/


        [Route("~/api/User/V ")]
        public IHttpActionResult OK(int a)
        {
            return Ok("ok");
        }

        // POST: api/User
        [HttpPost]
        [IdentityBasicAuthentication]

        public IHttpActionResult Post(/*[FromBody]*/User user)
        {

            UserIdentiy genericIdentity = HttpContext.Current.User.Identity as UserIdentiy;

            if (genericIdentity == null || !genericIdentity.IsAuthenticated) return Unauthorized();

            if (genericIdentity.user == null || genericIdentity.user.AccessLevel < Flags.AccessLevel.SUPERADMIN)
                return Content(HttpStatusCode.Unauthorized, "No tiene permisos para crear un usuario.");

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
        [IdentityBasicAuthentication]

        public IHttpActionResult Put(int id, [FromBody] User user)
        {

            bool isValid = user.Validate();
            if (!isValid) return Content(HttpStatusCode.BadRequest, Flags.ErrorFlag.ERROR_INVALID_OBJECT.ToString());
            user.IdUser = id;

            UserIdentiy identity = (UserIdentiy)HttpContext.Current.User.Identity;
            if (identity == null || !identity.IsAuthenticated || identity.user == null || identity.user.AccessLevel < Flags.AccessLevel.SUPERADMIN)
            {
                return Content(HttpStatusCode.Unauthorized, "No está autorizado para realizar está acción.");
            }

            Flags.ErrorFlag result = _UserDataContext.Update(user, out string error);
            HttpStatusCode resultCode;


            switch (result)
            {

                case Flags.ErrorFlag.ERROR_NO_UPDATED_RECORDS:
                    resultCode = HttpStatusCode.NotModified;
                    break;
                case Flags.ErrorFlag.ERROR_OK_RESULT:
                    resultCode = HttpStatusCode.OK;
                    break;
                case Flags.ErrorFlag.ERROR_CONFLICT_CANT_DELETE:
                    resultCode = HttpStatusCode.Conflict;
                    break;
                case Flags.ErrorFlag.ERROR_DATABASE:
                    resultCode = HttpStatusCode.NotModified;
                    break;
                default:
                    resultCode = HttpStatusCode.InternalServerError;
                    break;

            }

            return Content(resultCode, error);




        }

        [HttpDelete]
        [IdentityBasicAuthentication]
        public IHttpActionResult Delete(int id)
        {

            UserIdentiy identity = (UserIdentiy)HttpContext.Current.User.Identity;
            if (identity == null || identity.IsAuthenticated || identity.user == null || identity.user.AccessLevel < Flags.AccessLevel.SUPERADMIN) return Content(HttpStatusCode.Unauthorized, "No está autorizado para realizar esta acción.");


            Flags.ErrorFlag errorFlag = _UserDataContext.Delete(id, out string error);

            HttpStatusCode resultCode;
            switch (errorFlag)
            {
                case Flags.ErrorFlag.ERROR_OK_RESULT:
                    resultCode = HttpStatusCode.OK;
                    break;
                case Flags.ErrorFlag.ERROR_CONFLICT_CANT_DELETE:
                    resultCode = HttpStatusCode.Conflict;
                    break;
                case Flags.ErrorFlag.ERROR_DATABASE:
                    resultCode = HttpStatusCode.NotModified;
                    break;
                default:
                    resultCode = HttpStatusCode.InternalServerError; //Flags.ErrorFlag.UNKNOWN;
                    break;
            }

            return Content(resultCode, error);

            //Flags.ErrorFlag result = _UserDataContext.Delete(id, out string error);

        }
    }
}
