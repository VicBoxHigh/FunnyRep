using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace DumyReportes.Models
{
    public class UserIdentiy : GenericIdentity
    {

        public User user { get; private set; }
        public UserIdentiy(User user , string authType) : base(user.UserName, authType)
        {
            this.user = user;

        }


    }
}