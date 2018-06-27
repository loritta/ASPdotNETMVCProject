namespace ASPdotNETMVCProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SomeChangesToCustomers : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Customers", "FirstName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Customers", "LastName", c => c.String(nullable: false, maxLength: 200));
            AlterColumn("dbo.Customers", "Address", c => c.String(nullable: false, maxLength: 200));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Customers", "Address", c => c.String());
            AlterColumn("dbo.Customers", "LastName", c => c.String());
            AlterColumn("dbo.Customers", "FirstName", c => c.String());
        }
    }
}
