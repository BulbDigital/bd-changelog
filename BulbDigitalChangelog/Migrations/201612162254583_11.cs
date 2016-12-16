namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgencyReleases",
                c => new
                    {
                        AgencyReleaseKey = c.Int(nullable: false, identity: true),
                        DateProvisioned = c.DateTime(nullable: false),
                        Provisioner = c.String(),
                        AgencyKey = c.Int(nullable: false),
                        ReleaseKey = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AgencyReleaseKey)
                .ForeignKey("dbo.Agencies", t => t.AgencyKey)
                .ForeignKey("dbo.Releases", t => t.ReleaseKey)
                .Index(t => t.AgencyKey)
                .Index(t => t.ReleaseKey);
            
            AddColumn("dbo.Releases", "HasBeenPulled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AgencyReleases", "ReleaseKey", "dbo.Releases");
            DropForeignKey("dbo.AgencyReleases", "AgencyKey", "dbo.Agencies");
            DropIndex("dbo.AgencyReleases", new[] { "ReleaseKey" });
            DropIndex("dbo.AgencyReleases", new[] { "AgencyKey" });
            DropColumn("dbo.Releases", "HasBeenPulled");
            DropTable("dbo.AgencyReleases");
        }
    }
}
