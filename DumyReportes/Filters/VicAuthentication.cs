using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DumyReportes.Filters
{
    public class VicAuthentication
    {
        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
            if (HttpContext.Current != null)
            {
                HttpContext.Current.User = principal;
            }
        }
    }
}