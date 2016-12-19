namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using BulbDigitalChangelog.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<BulbDigitalChangelog.Models.BulbDigitalChangelogContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BulbDigitalChangelog.Models.BulbDigitalChangelogContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //context.Frameworks.AddOrUpdate(
            //  c => c.Name,
            //  new Framework() { FrameworkKey = 1, Name = "Core", Version = 1 },
            //  new Framework() { FrameworkKey = 2, Name = "Agency", Version = 1 },
            //  new Framework() { FrameworkKey = 3, Name = "Inside", Version = 1 });

            //context.Releases.AddOrUpdate(r => r.FrameworkKey,
            //    new Release() { FrameworkKey = 1, Version = 1 },
            //    new Release() { FrameworkKey = 2, Version = 1 },
            //    new Release() { FrameworkKey = 3, Version = 1 }
            //    );

            context.Agencies.AddOrUpdate(a => a.Name,
                new Agency() { Name = "MVAA" },
                new Agency() { Name = "DHHS" }
                );

            //context.AgencyReleases.AddOrUpdate(a => a.AgencyReleaseKey,
            //    new AgencyRelease() { AgencyKey = 1, DateProvisioned = DateTime.Now, Provisioner = "test", ReleaseKey = 10 },
            //    new AgencyRelease() { AgencyKey = 1, DateProvisioned = DateTime.Now, Provisioner = "test2", ReleaseKey = 11 });
            //context.ChangelogEntries.AddOrUpdate(c => c.ChangelogEntryKey,
            //    new ChangelogEntry() { Description = "Changelog 1", FrameworkKey = 1, ReleaseKey = 10, DateLogged = DateTime.Now });

            //);
            //
        }
    }
}
