namespace BulbDigitalChangelog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agencies", "Rank", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Agencies", "Rank");
        }
    }
}
