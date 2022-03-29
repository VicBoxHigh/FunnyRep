using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DumyReportes.Models
{
    [DataContract]
    public class ReportDtlEntry : ValidateModel, IReportObject
    {

        [Key]
        [DataMember]
        public int IdReportUpdate { get; set; }

        [DataMember]
        public int IdReport { get; set; }




        [DataMember]
        public string TitleUpdate { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public bool IsOwnerUpdate { get; set; }

        [DataMember]
        public DateTime DTUpdate { get; set; }

        [DataMember]
        public string FileNameEvidence { get; set; }

        [DataMember]
        public string PathEvidence { get; set; }

        public ReportDtlEntry()
        {

        }
 
        public override bool Validate()
        {

            ValidateResult =  TitleUpdate != null && Description != null &&
                TitleUpdate.Length < 45 & Description.Length < 512;

            return ValidateResult;

        }
    }
}