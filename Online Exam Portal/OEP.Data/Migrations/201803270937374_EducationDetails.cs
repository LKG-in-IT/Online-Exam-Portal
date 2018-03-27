namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EducationDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EducationType",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EducationDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EducationTypeId = c.Int(nullable: false),
                        InstituteName = c.String(maxLength: 1000),
                        YearFromId = c.Int(),
                        YearToId = c.Int(),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EducationType", t => t.EducationTypeId, cascadeDelete: true)
                .ForeignKey("dbo.EducationYearDetails", t => t.YearFromId)
                .ForeignKey("dbo.EducationYearDetails", t => t.YearToId)
                .Index(t => t.EducationTypeId)
                .Index(t => t.YearFromId)
                .Index(t => t.YearToId);
            
            CreateTable(
                "dbo.EducationYearDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Year = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EducationDetails", "YearToId", "dbo.EducationYearDetails");
            DropForeignKey("dbo.EducationDetails", "YearFromId", "dbo.EducationYearDetails");
            DropForeignKey("dbo.EducationDetails", "EducationTypeId", "dbo.EducationType");
            DropIndex("dbo.EducationDetails", new[] { "YearToId" });
            DropIndex("dbo.EducationDetails", new[] { "YearFromId" });
            DropIndex("dbo.EducationDetails", new[] { "EducationTypeId" });
            DropTable("dbo.EducationYearDetails");
            DropTable("dbo.EducationDetails");
            DropTable("dbo.EducationType");
        }
    }
}
