using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace DumyReportes.Models
{
    [DataContract]
    public class Evidence
    {
        [Key]
        [DataMember]
        public int IdEvidence { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string Path { get; set; }

        public Evidence(int idEvidence, string fileName, string patch)
        {
            IdEvidence = idEvidence;
            FileName = fileName;
            Path = patch;
        }
    }
}