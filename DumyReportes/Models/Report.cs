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

    public class Report : ValidateModel, IReportObject
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
        public List<ReportDtlEntry> ReportUpdates { get; set; }

        [DataMember]
        public string Title { get; set; }
        
        [DataMember]
        public string Description { get; set; }


        public Report(int idReport, int idUserWhoNotified, Location Location, ReportStatus currentStat, DateTime dTCreation, List<ReportDtlEntry> reportUpdates,string title, string description)
        {
            IdReport = idReport;
            IdUserWhoNotified = idUserWhoNotified;
            this.Location = Location;
            CurrentStat = currentStat;
            DTCreation = dTCreation;
            this.ReportUpdates = reportUpdates;
            this.Title = title;
            this.Description = description;
        }
        public Report(int idReport, int idUserWhoNotified, Location Location, ReportStatus currentStat, DateTime dTCreation, string title, string descripion)
        :this(idReport, idUserWhoNotified, Location, currentStat, dTCreation, new List<ReportDtlEntry>(),title, descripion)
        {
   
            


        }

        public override bool Validate()
        {
            

            ValidateResult =  IdReport > 0 & Title.Length < 50 & IdUserWhoNotified > 0 & Location != null & DTCreation != null;            




            return ValidateResult;

             
        }
    }
}