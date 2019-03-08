namespace Diss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addChatroomId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chatrooms", "ChatID", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Chatrooms", "ChatID");
        }
    }
}
