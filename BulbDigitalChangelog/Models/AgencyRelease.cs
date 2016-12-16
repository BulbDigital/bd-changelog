using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BulbDigitalChangelog.Models
{
    public class AgencyRelease
    {
        [Key]
        public int AgencyReleaseKey { get; set; }
        public DateTime DateProvisioned { get; set; }
        public string Provisioner { get; set; }
        public int AgencyKey { get; set; }
        public int ReleaseKey { get; set; }

        [ForeignKey("AgencyKey")]
        public Agency Agency { get; set; }

        [ForeignKey("ReleaseKey")]
        public Release Release { get; set; }
    }
}