namespace Booking.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataRecensione : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Recensione", "Data", c => c.DateTime(nullable: false, storeType: "date"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Recensione", "Data");
        }
    }
}
