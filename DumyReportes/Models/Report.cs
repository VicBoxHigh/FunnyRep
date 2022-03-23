using DumyReportes.Flags;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DumyReportes.Models
{
    [DataContract]

    public class Report
    {
        [Key]
        [DataMember]
        public int IdReport { get; set; }
        [DataMember]
        public int IdUserWhoNotified { get; set; }
        [DataMember]
        public Location Location { get; set;  }
        [DataMember]
        public Flags.ReportStatus CurrentStat { get; set; }
        [DataMember]
        public DateTime DTCreation { get; set; }

        [DataMember]
        public List<ReportUpdate> ReportUpdates { get; set; }
        
        
        public Report(int idReport, int idUserWhoNotified, Location Location, ReportStatus currentStat, DateTime dTCreation, List<ReportUpdate> reportUpdates)
        {
            IdReport = idReport;
            IdUserWhoNotified = idUserWhoNotified;
            this.Location = Location;
            CurrentStat = currentStat;
            DTCreation = dTCreation;
            this.ReportUpdates = reportUpdates;
        }
        public Report(int idReport, int idUserWhoNotified, Location Location, ReportStatus currentStat, DateTime dTCreation)
        :this(idReport, idUserWhoNotified, Location, currentStat, dTCreation, new List<ReportUpdate>())
        {
   
            


        }

    }
}