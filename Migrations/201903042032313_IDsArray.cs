namespace Diss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IDsArray : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Events", "ChatID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Events", "ChatID", c => c.Int(nullable: false));
        }
    }
}
