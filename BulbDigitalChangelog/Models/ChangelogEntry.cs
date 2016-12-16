using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BulbDigitalChangelog.Models
{
    public class ChangelogEntry
    {
        [Key]
        public int ChangelogEntryKey { get; set; }
        public string Description { get; set; }
        public DateTime DateLogged { get; set; }
        public int FrameworkKey { get; set; }
        public int ReleaseKey { get; set; }
        public string CreatedByUser { get; set; }

        [ForeignKey("FrameworkKey")]
        public Framework Framework { get; set; }

        [ForeignKey("ReleaseKey")]
        public Release Release { get; set; }

        public string Self
        {
            get
            {
                return string.Format(CultureInfo.CurrentCulture,
               "api/changelog/{0}", this.ChangelogEntryKey);
            }
            set { }
        }

    }
}