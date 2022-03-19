using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DumyReportes.Models
{
    public class Location
    {

        public int IdLocation { get; private set; }
        public string Description { get; private set; }

        public int lat { get; private set; }
        public int lon { get; private set; }



    }
}