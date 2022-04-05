using DumyReportes.Flags;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DumyReportes.Models
{
    [DataContract]
    public class User : ValidateModel, IReportObject
    {
        [DataMember]
        public int IdUser { get; set; }
        [DataMember]
        public string NumEmpleado { get; set; }
        [DataMember]
        public string UserName { get; set; }
        [DataMember]
        public string Pass { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public Flags.AccessLevel AccessLevel { get; set; }

        public string CurrentToke { get; set; }

        public User() { }

        public User(string numEmpleado, string userName, string pass, bool isEnabled, AccessLevel accessLevel)
        {
            this.NumEmpleado = numEmpleado;
            this.UserName = userName;
            this.Pass = pass;
            this.IsEnabled = isEnabled;
            this.AccessLevel = accessLevel;
        }

        public override bool Validate()
        {
            ValidateResult =   this.NumEmpleado.Length < 25 & this.UserName.Length < 25 & this.Pass.Length < 25;
            /*Trace.Assert(this.NumEmpleado.Length > 25);
            Trace.Assert(this.UserName.Length > 25);
            Trace.Assert(this.Pass.Length > 25);
*/
            return ValidateResult;

        }

       
    }
}