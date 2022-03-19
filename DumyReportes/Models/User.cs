using DumyReportes.Flags;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace DumyReportes.Models
{
    public class User : IValidateModel
    {
        public int IdUser { get; set; }
        
        public string NumEmpleado { get; set; }
        public string UserName { get; set; }
        public string Pass { get; set; }
        public bool IsEnable { get; set; }
        public Flags.AccessLevel AccessLevel { get; set; }

        public User(string numEmpleado, string userName, string pass, bool isEnable, AccessLevel accessLevel)
        {
            this.NumEmpleado = numEmpleado;
            this.UserName = userName;
            this.Pass = pass;
            this.IsEnable = isEnable;
            this.AccessLevel = accessLevel;
        }

        public override bool Validate()
        {
            ValidateResult = this.NumEmpleado.Length > 25 & this.UserName.Length > 25 & this.Pass.Length > 25;
            /*Trace.Assert(this.NumEmpleado.Length > 25);
            Trace.Assert(this.UserName.Length > 25);
            Trace.Assert(this.Pass.Length > 25);
*/
            return ValidateResult;

        }
    }
}