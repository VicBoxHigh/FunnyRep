﻿using DumyReportes.Data;
using DumyReportes.Flags;
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

        private UserDataContext _UserDataContext = new UserDataContext();


        public LoginValidatorHelper(string userName, string password)
        {
            this.userName = userName;
            this.password = password;
        }

        internal ErrorFlag Validate(out User user, out string error)
        {

            //call db context
            ErrorFlag result = this._UserDataContext.CredentialsExist(this.userName, this.password, out user, out error);

            /*  if (result != ErrorFlag.ERROR_OK_RESULT && result != ErrorFlag.ERROR_RECORD_EXISTS)
                  throw new Exception(result.ToString());
  */


            return result;

        }
    }
}