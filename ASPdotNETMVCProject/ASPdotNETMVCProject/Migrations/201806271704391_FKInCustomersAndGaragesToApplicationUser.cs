namespace ASPdotNETMVCProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FKInCustomersAndGaragesToApplicationUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Customers", "ApplicationUserId", c => c.String(maxLength: 128));
            AddColumn("dbo.Garages", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Customers", "ApplicationUserId");
            CreateIndex("dbo.Garages", "ApplicationUserId");
            AddForeignKey("dbo.Customers", "ApplicationUserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Garages", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Garages", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Customers", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Garages", new[] { "ApplicationUserId" });
            DropIndex("dbo.Customers", new[] { "ApplicationUserId" });
            DropColumn("dbo.Garages", "ApplicationUserId");
            DropColumn("dbo.Customers", "ApplicationUserId");
        }
    }
}
