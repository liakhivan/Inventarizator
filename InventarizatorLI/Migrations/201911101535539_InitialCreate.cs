namespace InventarizatorLI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conteiners",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Ingredients",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IngredientsForProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductId = c.Int(nullable: false),
                        IngredientId = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .ForeignKey("dbo.Products", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.IngredientId);
            
            CreateTable(
                "dbo.Packages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IngredientId = c.Int(nullable: false),
                        Weight = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Ingredients", t => t.IngredientId, cascadeDelete: true)
                .Index(t => t.IngredientId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Packages", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.IngredientsForProducts", "ProductId", "dbo.Products");
            DropForeignKey("dbo.IngredientsForProducts", "IngredientId", "dbo.Ingredients");
            DropForeignKey("dbo.Conteiners", "ProductId", "dbo.Products");
            DropIndex("dbo.Packages", new[] { "IngredientId" });
            DropIndex("dbo.IngredientsForProducts", new[] { "IngredientId" });
            DropIndex("dbo.IngredientsForProducts", new[] { "ProductId" });
            DropIndex("dbo.Conteiners", new[] { "ProductId" });
            DropTable("dbo.Packages");
            DropTable("dbo.IngredientsForProducts");
            DropTable("dbo.Ingredients");
            DropTable("dbo.Products");
            DropTable("dbo.Conteiners");
        }
    }
}
