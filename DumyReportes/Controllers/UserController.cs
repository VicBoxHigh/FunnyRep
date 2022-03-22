﻿using DumyReportes.Data;
using DumyReportes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DumyReportes.Controllers
{
    public class UserController : ApiController
    {
        // GET: api/User
        [Route("~/api/User/all")]
        public /*IEnumerable<string>*/ IHttpActionResult Get()
        {

            Flags.ErrorFlag result = UserData.GetAllUsers(out List<User> users, out string error);

            return Ok(new
            {
                users = users.ToList(),
                errorCode = result.ToString()

            });

        }

        // GET: api/User/5
        /// [Route("~/api/User/{authorId:int}/books")]
        public IHttpActionResult Get(int id)
        {
            if (id < 1) return BadRequest("ID no válido");

            Flags.ErrorFlag result = UserData.GetUser(id, out User user, out string error);

            if (result == Flags.ErrorFlag.ERROR_RECORD_NOT_EXISTS)
            {
                return Content(HttpStatusCode.NotFound, result.ToString());
            
            }

            return
                Ok(
                    new
                    {
                        resultCode = result.ToString(),
                        errorMsg = error,
                        user = user,

                    }
                );


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
            Flags.ErrorFlag resultCreate = UserData.createUser(user, out string error);

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

            Flags.ErrorFlag result = UserData.UpdateUser(user, out string error);

            if (result == Flags.ErrorFlag.ERROR_NO_UPDATED_RECORDS)
                return Content(HttpStatusCode.NoContent, result.ToString());


            return Ok();

        }

        [HttpDelete]
        // DELETE: api/User/5
        public IHttpActionResult Delete(int id)
        {

            Flags.ErrorFlag result = UserData.DeleteUser(id, out string error);


            return Ok();


        }
    }
}
