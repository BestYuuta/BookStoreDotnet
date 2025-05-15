namespace BookStoreDotnet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRentalFeeToRentals : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Rentals", "RentalFee", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Rentals", "RentalFee");
        }
    }
}
