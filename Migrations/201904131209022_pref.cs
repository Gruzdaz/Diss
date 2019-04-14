namespace Diss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pref : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chatrooms", "HomeTeamRoom", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chatrooms", "HomeTeamRoom");
        }
    }
}
