namespace InventarizatorLI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeTareMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Tares", "Amount", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Tares", "Amount");
        }
    }
}
