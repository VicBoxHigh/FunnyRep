using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DumyReportes.Models
{
    [DataContract]
    public class ReportUpdate
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


    }
}