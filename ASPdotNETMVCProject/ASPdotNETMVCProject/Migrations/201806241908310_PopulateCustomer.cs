namespace ASPdotNETMVCProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateCustomer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FirstName = c.String(),
                        LastName = c.String(),
                        Address = c.String(),
                        PhoneNumber = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Garages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Address = c.String(nullable: false, maxLength: 200),
                        PhoneNumber = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Services",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Description = c.String(),
                        Garage_ID = c.Int(),
                        Transaction_ID = c.Int(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Garages", t => t.Garage_ID)
                .ForeignKey("dbo.Transactions", t => t.Transaction_ID)
                .Index(t => t.Garage_ID)
                .Index(t => t.Transaction_ID);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        GarageID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Garages", t => t.GarageID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.GarageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Services", "Transaction_ID", "dbo.Transactions");
            DropForeignKey("dbo.Transactions", "GarageID", "dbo.Garages");
            DropForeignKey("dbo.Transactions", "CustomerID", "dbo.Customers");
            DropForeignKey("dbo.Services", "Garage_ID", "dbo.Garages");
            DropIndex("dbo.Transactions", new[] { "GarageID" });
            DropIndex("dbo.Transactions", new[] { "CustomerID" });
            DropIndex("dbo.Services", new[] { "Transaction_ID" });
            DropIndex("dbo.Services", new[] { "Garage_ID" });
            DropTable("dbo.Transactions");
            DropTable("dbo.Services");
            DropTable("dbo.Garages");
            DropTable("dbo.Customers");
        }
    }
}
