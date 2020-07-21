namespace InventarizatorLI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSecurityTableMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Securities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pass = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Securities");
        }
    }
}
