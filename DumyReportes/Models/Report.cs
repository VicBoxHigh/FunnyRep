using DumyReportes.Flags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DumyReportes.Models
{
    public class Report
    {
        [Key]
        public int IdReport { get; private set; }
        public int IdUserWhoNotified { get; private set; }

        public Location location { get; private set;  }

        public Flags.ErrorFlag CurrentStat { get; set; }

        public DateTime DTCreation { get; private set; }

        public Report(int idReport, int idUserWhoNotified, Location location, ErrorFlag currentStat, DateTime dTCreation)
        {
            IdReport = idReport;
            IdUserWhoNotified = idUserWhoNotified;
            this.location = location;
            CurrentStat = currentStat;
            DTCreation = dTCreation;
        }

    }
}