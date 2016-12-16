namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _7 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Releases");
            AlterColumn("dbo.Releases", "ReleaseKey", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Releases", "ReleaseKey");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.Releases");
            AlterColumn("dbo.Releases", "ReleaseKey", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Releases", new[] { "ReleaseKey", "Version" });
        }
    }
}
