using DumyReportes.Data;
using DumyReportes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DumyReportes.Util
{
    public class LoginValidatorHelper
    {
        private string userName;
        private string password;

        private readonly UserDataContext _UserDataContext = new UserDataContext(); 


        public LoginValidatorHelper(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        internal bool Validate(out User user)
        {

            //call db context
            user = this._UserDataContext.CredentialsExist(this.userName, this.password);

            return user != null;

        }
    }
}