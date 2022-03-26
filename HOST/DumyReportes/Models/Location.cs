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

        public decimal lat { get; private set; }
        public decimal lon { get; private set; }

        public Location(int idLocation, string description, decimal lat, decimal lon)
        {
            IdLocation = idLocation;
            Description = description;
            this.lat = lat;
            this.lon = lon;
        }
    }
}