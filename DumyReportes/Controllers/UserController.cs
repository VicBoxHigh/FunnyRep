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
    public class UserController : ApiController
    {
        // GET: api/User
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/User/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/User
        public int Post([FromBody]string numEmpleado,string userName, string pass, bool isEnable, int accessLevel)
        {

            User user = new User(
                    numEmpleado,
                    userName,
                    pass,
                    isEnable,
                    (Flags.AccessLevel)accessLevel
                
                );

            //Válida la data
            if (!user.Validate()) return (int)Flags.ErrorFlag.ERROR_RESULT;

            //Inserta en DB
            Flags.ErrorFlag resultCreate = ReportData.createUser(user, out string error);


            return (int)resultCreate;

        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/User/5
        public void Delete(int id)
        {
        }
    }
}
