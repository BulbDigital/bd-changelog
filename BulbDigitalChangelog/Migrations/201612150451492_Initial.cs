namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChangelogEntries",
                c => new
                    {
                        ChangelogEntryKey = c.Int(nullable: false, identity: true),
                        ProjectKey = c.Int(nullable: false),
                        Description = c.String(),
                        DateLogged = c.DateTime(nullable: false),
                        HasBeenProvisioned = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ChangelogEntryKey);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChangelogEntries");
        }
    }
}
