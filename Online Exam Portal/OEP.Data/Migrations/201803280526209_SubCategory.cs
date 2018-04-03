namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SubCategory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SubCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CategoryId = c.Int(nullable: false),
                        Name = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Category", t => t.CategoryId, cascadeDelete: true)
                .Index(t => t.CategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SubCategory", "CategoryId", "dbo.Category");
            DropIndex("dbo.SubCategory", new[] { "CategoryId" });
            DropTable("dbo.SubCategory");
        }
    }
}
