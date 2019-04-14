namespace Diss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ELO : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chatrooms", "PeopleCount", c => c.Int(nullable: false));
            AddColumn("dbo.Chatrooms", "AverageELO", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "ELO", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ELO");
            DropColumn("dbo.Chatrooms", "AverageELO");
            DropColumn("dbo.Chatrooms", "PeopleCount");
        }
    }
}
