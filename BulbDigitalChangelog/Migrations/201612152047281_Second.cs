namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Second : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChangelogEntries", "Self", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ChangelogEntries", "Self");
        }
    }
}
