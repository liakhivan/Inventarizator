namespace InventarizatorLI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationStorageDBTwo : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.IngredientStatistics", "TypeEvent", c => c.Int(nullable: false));
            AlterColumn("dbo.ProductStatistics", "TypeEvent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProductStatistics", "TypeEvent", c => c.String());
            AlterColumn("dbo.IngredientStatistics", "TypeEvent", c => c.String());
        }
    }
}
