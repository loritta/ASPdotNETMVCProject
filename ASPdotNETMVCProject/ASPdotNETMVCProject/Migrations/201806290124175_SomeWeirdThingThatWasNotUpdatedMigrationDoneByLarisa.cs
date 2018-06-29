namespace ASPdotNETMVCProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeWeirdThingThatWasNotUpdatedMigrationDoneByLarisa : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        CustomerID = c.Int(nullable: false),
                        GarageID = c.Int(nullable: false),
                        Title = c.String(),
                        Contents = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Customers", t => t.CustomerID, cascadeDelete: true)
                .ForeignKey("dbo.Garages", t => t.GarageID, cascadeDelete: true)
                .Index(t => t.CustomerID)
                .Index(t => t.GarageID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "GarageID", "dbo.Garages");
            DropForeignKey("dbo.Messages", "CustomerID", "dbo.Customers");
            DropIndex("dbo.Messages", new[] { "GarageID" });
            DropIndex("dbo.Messages", new[] { "CustomerID" });
            DropTable("dbo.Messages");
        }
    }
}
