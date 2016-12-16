namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class next : DbMigration
    {
        public override void Up()
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
            
            AddColumn("dbo.ChangelogEntries", "CreatedByUser", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChangelogEntries", "CreatedByUser");
            DropTable("dbo.ProvisionInstances");
        }
    }
}
