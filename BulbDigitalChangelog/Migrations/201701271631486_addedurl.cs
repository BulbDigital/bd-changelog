namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addedurl : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agencies", "Url", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agencies", "Url");
        }
    }
}
