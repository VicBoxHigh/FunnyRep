using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DumyReportes.Models
{
    [DataContract]
    public class ReportDtlEntry: ValidateModel, IReportObject
    {

        [Key]
        [DataMember]
        public int IdReportUpdate { get; set; }


        [DataMember]
        public int IdReport { get; set; }

        [DataMember]
        public Evidence Evidence { get; set; }

        [DataMember]
        public string TitleUpdate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsOwnerUpdate { get; set; }

        public ReportDtlEntry(int idReportUpdate, int idReport, Evidence evidence, string titleUpdate, string description, bool isOwnerUpdate)
        {
            IdReportUpdate = idReportUpdate;
            IdReport = idReport;
            Evidence = evidence;
            TitleUpdate = titleUpdate;
            Description = description;
            IsOwnerUpdate = isOwnerUpdate;
        }

        public override bool Validate()
        {

            ValidateResult = IdReport > 0 & Evidence != null & TitleUpdate != null && Description != null && 
                TitleUpdate.Length < 45 & Description.Length < 512;

            return ValidateResult;

        }
    }
}