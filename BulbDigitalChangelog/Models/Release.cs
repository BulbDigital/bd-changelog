using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BulbDigitalChangelog.Models
{
    public class Release
    {
        [Key]
        public int ReleaseKey { get; set; }
        public int FrameworkKey { get; set; }
        public int Version { get; set; }
        public bool HasBeenPulled { get; set; }

        [ForeignKey("FrameworkKey")]
        public Framework Framework { get; set; }
    }
}