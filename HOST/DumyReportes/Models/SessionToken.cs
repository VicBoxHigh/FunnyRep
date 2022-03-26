using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DumyReportes.Models
{
    public class SessionToken
    {

        public long IdToken { get; private set; }

        public string Token { get; private set; }

        public int Level { get; private set; }

        public DateTime CreationDT { get; private set; }

        public DateTime ExpirationDT { get; private set; }

        public int IdUser { get; private set; }




    }
}