using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DumyReportes.Models
{
    public abstract class IValidateModel
    {
        public bool ValidateResult { get; set; } = true;

        public abstract bool Validate();

    }
}
