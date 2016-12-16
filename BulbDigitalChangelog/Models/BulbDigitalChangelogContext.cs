using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BulbDigitalChangelog.Models
{
    public class BulbDigitalChangelogContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public BulbDigitalChangelogContext() : base("name=BulbDigitalChangelogContext")
        {
        }

        public System.Data.Entity.DbSet<BulbDigitalChangelog.Models.ChangelogEntry> ChangelogEntries { get; set; }

        public System.Data.Entity.DbSet<BulbDigitalChangelog.Models.Agency> Agencies { get; set; }

        public System.Data.Entity.DbSet<BulbDigitalChangelog.Models.Framework> Frameworks { get; set; }

        public System.Data.Entity.DbSet<BulbDigitalChangelog.Models.Release> Releases { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
        }

        public System.Data.Entity.DbSet<BulbDigitalChangelog.Models.AgencyRelease> AgencyReleases { get; set; }
    }
}
