namespace Diss.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChatRooms : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chatrooms",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        EventID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Chatrooms");
        }
    }
}
