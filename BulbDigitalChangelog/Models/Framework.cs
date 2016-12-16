using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BulbDigitalChangelog.Models
{
    public class Framework
    {
        [Key]
        public int FrameworkKey { get; set; }
        public int Version { get; set; }
        public string Name { get; set; }
    }
}