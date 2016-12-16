namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Agencies",
                c => new
                    {
                        AgencyKey = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.AgencyKey);
            
            CreateTable(
                "dbo.Frameworks",
                c => new
                    {
                        FrameworkKey = c.Int(nullable: false, identity: true),
                        Version = c.Int(nullable: false),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.FrameworkKey);
            
            CreateTable(
                "dbo.Releases",
                c => new
                    {
                        ReleaseKey = c.Int(nullable: false, identity: true),
                        FrameworkKey = c.Int(nullable: false),
                        Version = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReleaseKey)
                .ForeignKey("dbo.Frameworks", t => t.FrameworkKey, cascadeDelete: true)
                .Index(t => t.FrameworkKey);
            
            AddColumn("dbo.ChangelogEntries", "ReleaseKey", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Releases", "FrameworkKey", "dbo.Frameworks");
            DropIndex("dbo.Releases", new[] { "FrameworkKey" });
            DropColumn("dbo.ChangelogEntries", "ReleaseKey");
            DropTable("dbo.Releases");
            DropTable("dbo.Frameworks");
            DropTable("dbo.Agencies");
        }
    }
}
