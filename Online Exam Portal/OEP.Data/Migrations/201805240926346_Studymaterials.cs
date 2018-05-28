namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Studymaterials : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StudyMaterial",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        SubcategoryId = c.Int(nullable: false),
                        FilePath = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.SubCategory", t => t.SubcategoryId, cascadeDelete: true)
                .Index(t => t.SubcategoryId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudyMaterial", "SubcategoryId", "dbo.SubCategory");
            DropIndex("dbo.StudyMaterial", new[] { "SubcategoryId" });
            DropTable("dbo.StudyMaterial");
        }
    }
}
