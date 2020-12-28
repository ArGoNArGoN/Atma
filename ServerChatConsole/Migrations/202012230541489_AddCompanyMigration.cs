namespace ServerChatConsole.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCompanyMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chat",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDServer = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Name = c.String(nullable: false),
                        MaxCountUser = c.Int(),
                        Info = c.String(),
                        Number = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Server", t => t.IDServer)
                .Index(t => t.IDServer);
            
            CreateTable(
                "dbo.EventLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDServer = c.Int(nullable: false),
                        IDUser = c.Int(),
                        IDChat = c.Int(),
                        IDTextChat = c.Int(),
                        IDRole = c.Int(),
                        IDMessage = c.Int(),
                        IDOpinion = c.Int(),
                        Action = c.Int(nullable: false),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Message", t => t.IDMessage)
                .ForeignKey("dbo.Role", t => t.IDRole)
                .ForeignKey("dbo.Server", t => t.IDServer)
                .ForeignKey("dbo.Opinion", t => t.IDOpinion)
                .ForeignKey("dbo.User", t => t.IDUser)
                .ForeignKey("dbo.TextChat", t => t.IDTextChat)
                .ForeignKey("dbo.Chat", t => t.IDChat)
                .Index(t => t.IDServer)
                .Index(t => t.IDUser)
                .Index(t => t.IDChat)
                .Index(t => t.IDTextChat)
                .Index(t => t.IDRole)
                .Index(t => t.IDMessage)
                .Index(t => t.IDOpinion);
            
            CreateTable(
                "dbo.Message",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDServerUser = c.Int(nullable: false),
                        IDTextChat = c.Int(nullable: false),
                        Text = c.String(),
                        PinnedMessage = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                        File = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.ServerUser", t => t.IDServerUser)
                .ForeignKey("dbo.TextChat", t => t.IDTextChat)
                .Index(t => t.IDServerUser)
                .Index(t => t.IDTextChat);
            
            CreateTable(
                "dbo.ServerUser",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDUser = c.Int(nullable: false),
                        IDRole = c.Int(),
                        IDServer = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Name = c.String(maxLength: 15),
                        Status2 = c.String(maxLength: 100),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.IDUser)
                .ForeignKey("dbo.Server", t => t.IDServer)
                .ForeignKey("dbo.Role", t => t.IDRole)
                .Index(t => t.IDUser)
                .Index(t => t.IDRole)
                .Index(t => t.IDServer);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDServer = c.Int(nullable: false),
                        Name = c.String(),
                        Info = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Server", t => t.IDServer)
                .Index(t => t.IDServer);
            
            CreateTable(
                "dbo.RightRole",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDRole = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Priority = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Role", t => t.IDRole)
                .Index(t => t.IDRole);
            
            CreateTable(
                "dbo.Server",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        Language = c.String(),
                        Info = c.String(maxLength: 500),
                        Icon = c.Binary(storeType: "image"),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Opinion",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDUser = c.Int(nullable: false),
                        IDServer = c.Int(nullable: false),
                        Mark = c.Int(nullable: false),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.IDUser)
                .ForeignKey("dbo.Server", t => t.IDServer)
                .Index(t => t.IDUser)
                .Index(t => t.IDServer);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        RealName = c.String(nullable: false),
                        Icon = c.Binary(),
                        Status = c.Int(nullable: false),
                        DateReg = c.DateTime(nullable: false, storeType: "date"),
                        DateOfBirht = c.DateTime(storeType: "date"),
                        Password = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Request",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDUser = c.Int(nullable: false),
                        IDFriend = c.Int(nullable: false),
                        UserRequest = c.Boolean(nullable: false),
                        FriendRequest = c.Boolean(nullable: false),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.IDUser)
                .ForeignKey("dbo.User", t => t.IDFriend)
                .Index(t => t.IDUser)
                .Index(t => t.IDFriend);
            
            CreateTable(
                "dbo.UserLog",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDUser = c.Int(nullable: false),
                        IDServer = c.Int(),
                        Action = c.Int(nullable: false),
                        Message = c.String(),
                        Date = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.User", t => t.IDUser)
                .ForeignKey("dbo.Server", t => t.IDServer)
                .Index(t => t.IDUser)
                .Index(t => t.IDServer);
            
            CreateTable(
                "dbo.TextChat",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        IDServer = c.Int(nullable: false),
                        Name = c.String(),
                        Info = c.String(),
                        Number = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Server", t => t.IDServer)
                .Index(t => t.IDServer);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EventLog", "IDChat", "dbo.Chat");
            DropForeignKey("dbo.ServerUser", "IDRole", "dbo.Role");
            DropForeignKey("dbo.UserLog", "IDServer", "dbo.Server");
            DropForeignKey("dbo.TextChat", "IDServer", "dbo.Server");
            DropForeignKey("dbo.Message", "IDTextChat", "dbo.TextChat");
            DropForeignKey("dbo.EventLog", "IDTextChat", "dbo.TextChat");
            DropForeignKey("dbo.ServerUser", "IDServer", "dbo.Server");
            DropForeignKey("dbo.Role", "IDServer", "dbo.Server");
            DropForeignKey("dbo.Opinion", "IDServer", "dbo.Server");
            DropForeignKey("dbo.UserLog", "IDUser", "dbo.User");
            DropForeignKey("dbo.ServerUser", "IDUser", "dbo.User");
            DropForeignKey("dbo.Request", "IDFriend", "dbo.User");
            DropForeignKey("dbo.Request", "IDUser", "dbo.User");
            DropForeignKey("dbo.Opinion", "IDUser", "dbo.User");
            DropForeignKey("dbo.EventLog", "IDUser", "dbo.User");
            DropForeignKey("dbo.EventLog", "IDOpinion", "dbo.Opinion");
            DropForeignKey("dbo.EventLog", "IDServer", "dbo.Server");
            DropForeignKey("dbo.Chat", "IDServer", "dbo.Server");
            DropForeignKey("dbo.RightRole", "IDRole", "dbo.Role");
            DropForeignKey("dbo.EventLog", "IDRole", "dbo.Role");
            DropForeignKey("dbo.Message", "IDServerUser", "dbo.ServerUser");
            DropForeignKey("dbo.EventLog", "IDMessage", "dbo.Message");
            DropIndex("dbo.TextChat", new[] { "IDServer" });
            DropIndex("dbo.UserLog", new[] { "IDServer" });
            DropIndex("dbo.UserLog", new[] { "IDUser" });
            DropIndex("dbo.Request", new[] { "IDFriend" });
            DropIndex("dbo.Request", new[] { "IDUser" });
            DropIndex("dbo.Opinion", new[] { "IDServer" });
            DropIndex("dbo.Opinion", new[] { "IDUser" });
            DropIndex("dbo.RightRole", new[] { "IDRole" });
            DropIndex("dbo.Role", new[] { "IDServer" });
            DropIndex("dbo.ServerUser", new[] { "IDServer" });
            DropIndex("dbo.ServerUser", new[] { "IDRole" });
            DropIndex("dbo.ServerUser", new[] { "IDUser" });
            DropIndex("dbo.Message", new[] { "IDTextChat" });
            DropIndex("dbo.Message", new[] { "IDServerUser" });
            DropIndex("dbo.EventLog", new[] { "IDOpinion" });
            DropIndex("dbo.EventLog", new[] { "IDMessage" });
            DropIndex("dbo.EventLog", new[] { "IDRole" });
            DropIndex("dbo.EventLog", new[] { "IDTextChat" });
            DropIndex("dbo.EventLog", new[] { "IDChat" });
            DropIndex("dbo.EventLog", new[] { "IDUser" });
            DropIndex("dbo.EventLog", new[] { "IDServer" });
            DropIndex("dbo.Chat", new[] { "IDServer" });
            DropTable("dbo.TextChat");
            DropTable("dbo.UserLog");
            DropTable("dbo.Request");
            DropTable("dbo.User");
            DropTable("dbo.Opinion");
            DropTable("dbo.Server");
            DropTable("dbo.RightRole");
            DropTable("dbo.Role");
            DropTable("dbo.ServerUser");
            DropTable("dbo.Message");
            DropTable("dbo.EventLog");
            DropTable("dbo.Chat");
        }
    }
}
