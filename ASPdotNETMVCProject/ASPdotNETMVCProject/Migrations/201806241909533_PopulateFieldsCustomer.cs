namespace ASPdotNETMVCProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulateFieldsCustomer : DbMigration
    {
        public override void Up()
        {
           /* Sql("INSERT INTO Customers" +
                         "(ID, FirstName, LastName, Address, PhoneNumber)" +
                         " VALUES(1, 'John', 'Smith', '243 boul. Lasalle, Montreal, QC, H4G3S2', 5145367723)");
            Sql("INSERT INTO Customers" +
                               "(ID, FirstName, LastName, Address, PhoneNumber)" +
                               " VALUES(2, 'Alex', 'Doe', '523 rue. Gordon, Montreal, QC, H4G2S6', 514756453)");*/
        }
        
        public override void Down()
        {
        }
    }
}
