namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChangelogEntries", "FrameworkKey", c => c.Int(nullable: false));
            DropColumn("dbo.ChangelogEntries", "ProjectKey");
            DropColumn("dbo.ChangelogEntries", "HasBeenProvisioned");
            DropTable("dbo.ProvisionInstances");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.ProvisionInstances",
                c => new
                    {
                        ProvisionInstanceKey = c.Int(nullable: false, identity: true),
                        DateProvisioned = c.DateTime(nullable: false),
                        Provisioner = c.String(),
                        AgencyKey = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ProvisionInstanceKey);
            
            AddColumn("dbo.ChangelogEntries", "HasBeenProvisioned", c => c.Boolean(nullable: false));
            AddColumn("dbo.ChangelogEntries", "ProjectKey", c => c.Int(nullable: false));
            DropColumn("dbo.ChangelogEntries", "FrameworkKey");
        }
    }
}
