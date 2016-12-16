namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _9 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ChangelogEntries", "FrameworkKey", "dbo.Frameworks");
            DropForeignKey("dbo.Releases", "FrameworkKey", "dbo.Frameworks");
            CreateIndex("dbo.ChangelogEntries", "ReleaseKey");
            AddForeignKey("dbo.ChangelogEntries", "ReleaseKey", "dbo.Releases", "ReleaseKey");
            AddForeignKey("dbo.ChangelogEntries", "FrameworkKey", "dbo.Frameworks", "FrameworkKey");
            AddForeignKey("dbo.Releases", "FrameworkKey", "dbo.Frameworks", "FrameworkKey");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Releases", "FrameworkKey", "dbo.Frameworks");
            DropForeignKey("dbo.ChangelogEntries", "FrameworkKey", "dbo.Frameworks");
            DropForeignKey("dbo.ChangelogEntries", "ReleaseKey", "dbo.Releases");
            DropIndex("dbo.ChangelogEntries", new[] { "ReleaseKey" });
            AddForeignKey("dbo.Releases", "FrameworkKey", "dbo.Frameworks", "FrameworkKey", cascadeDelete: true);
            AddForeignKey("dbo.ChangelogEntries", "FrameworkKey", "dbo.Frameworks", "FrameworkKey", cascadeDelete: true);
        }
    }
}
