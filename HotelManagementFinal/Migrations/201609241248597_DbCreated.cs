namespace HotelManagementFinal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DbCreated : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CheckIn",
                c => new
                    {
                        CheckInId = c.Int(nullable: false, identity: true),
                        ChekInDate = c.DateTime(nullable: false),
                        CheckOutDate = c.DateTime(nullable: false),
                        Staying = c.String(),
                        CustomerId = c.Int(nullable: false),
                        RoomTypeId = c.Int(nullable: false),
                        RoomId = c.Int(nullable: false),
                        TotalPrice = c.Decimal(precision: 18, scale: 2),
                        Paying = c.Decimal(precision: 18, scale: 2),
                        RemainigPrice = c.Decimal(precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.CheckInId)
                .ForeignKey("dbo.Customer", t => t.CustomerId)
                .ForeignKey("dbo.Room", t => t.RoomId)
                .ForeignKey("dbo.RoomType", t => t.RoomTypeId)
                .Index(t => t.CustomerId)
                .Index(t => t.RoomTypeId)
                .Index(t => t.RoomId);
            
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        CustomerName = c.String(nullable: false),
                        CustomerRegistrationNo = c.String(),
                        CustomerPhoneNo = c.String(nullable: false),
                        CustomerAddress = c.String(nullable: false),
                        IdentificationTypeId = c.Int(nullable: false),
                        CustomerNid = c.String(),
                        CustomerPassportNo = c.String(),
                    })
                .PrimaryKey(t => t.CustomerId)
                .ForeignKey("dbo.IdentificationType", t => t.IdentificationTypeId)
                .Index(t => t.IdentificationTypeId);
            
            CreateTable(
                "dbo.IdentificationType",
                c => new
                    {
                        IdentificationTypeId = c.Int(nullable: false, identity: true),
                        IdentificationTypeName = c.String(),
                    })
                .PrimaryKey(t => t.IdentificationTypeId);
            
            CreateTable(
                "dbo.Room",
                c => new
                    {
                        RoomId = c.Int(nullable: false, identity: true),
                        RoomName = c.String(nullable: false),
                        RoomTypeId = c.Int(nullable: false),
                        RoomPrice = c.Decimal(nullable: false, precision: 18, scale: 0),
                        RoomDescription = c.String(),
                        RoomStatus = c.Boolean(),
                    })
                .PrimaryKey(t => t.RoomId)
                .ForeignKey("dbo.RoomType", t => t.RoomTypeId)
                .Index(t => t.RoomTypeId);
            
            CreateTable(
                "dbo.RoomType",
                c => new
                    {
                        RoomTypeId = c.Int(nullable: false, identity: true),
                        RoomTypeName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RoomTypeId);
            
            CreateTable(
                "dbo.UserAccount",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        LastName = c.String(nullable: false, maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 50),
                        UserName = c.String(nullable: false, maxLength: 50),
                        Password = c.String(nullable: false, maxLength: 50),
                        ConfirmPassword = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Room", "RoomTypeId", "dbo.RoomType");
            DropForeignKey("dbo.CheckIn", "RoomTypeId", "dbo.RoomType");
            DropForeignKey("dbo.CheckIn", "RoomId", "dbo.Room");
            DropForeignKey("dbo.Customer", "IdentificationTypeId", "dbo.IdentificationType");
            DropForeignKey("dbo.CheckIn", "CustomerId", "dbo.Customer");
            DropIndex("dbo.Room", new[] { "RoomTypeId" });
            DropIndex("dbo.Customer", new[] { "IdentificationTypeId" });
            DropIndex("dbo.CheckIn", new[] { "RoomId" });
            DropIndex("dbo.CheckIn", new[] { "RoomTypeId" });
            DropIndex("dbo.CheckIn", new[] { "CustomerId" });
            DropTable("dbo.UserAccount");
            DropTable("dbo.RoomType");
            DropTable("dbo.Room");
            DropTable("dbo.IdentificationType");
            DropTable("dbo.Customer");
            DropTable("dbo.CheckIn");
        }
    }
}
