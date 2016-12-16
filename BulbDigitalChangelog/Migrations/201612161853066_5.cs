namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _5 : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.ChangelogEntries", "FrameworkKey");
            AddForeignKey("dbo.ChangelogEntries", "FrameworkKey", "dbo.Frameworks", "FrameworkKey", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ChangelogEntries", "FrameworkKey", "dbo.Frameworks");
            DropIndex("dbo.ChangelogEntries", new[] { "FrameworkKey" });
        }
    }
}
