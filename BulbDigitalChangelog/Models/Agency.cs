using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BulbDigitalChangelog.Models
{
    public class Agency
    {
        [Key]
        public int AgencyKey { get; set; }
        public string Name { get; set; }
        public int? Rank { get; set; }
        public string Url { get; set; }
    }
}