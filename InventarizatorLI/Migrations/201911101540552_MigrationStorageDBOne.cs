namespace InventarizatorLI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MigrationStorageDBOne : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.IngredientStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdIngredient = c.Int(nullable: false),
                        TypeEvent = c.String(),
                        Weight = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Ingredient_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredients", t => t.Ingredient_Id)
                .Index(t => t.Ingredient_Id);
            
            CreateTable(
                "dbo.ProductStatistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdProduct = c.Int(nullable: false),
                        TypeEvent = c.String(),
                        Weight = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Product_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductStatistics", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.IngredientStatistics", "Ingredient_Id", "dbo.Ingredients");
            DropIndex("dbo.ProductStatistics", new[] { "Product_Id" });
            DropIndex("dbo.IngredientStatistics", new[] { "Ingredient_Id" });
            DropTable("dbo.ProductStatistics");
            DropTable("dbo.IngredientStatistics");
        }
    }
}
