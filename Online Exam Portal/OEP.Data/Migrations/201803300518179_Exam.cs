namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Exam : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ExamType", newName: "ExamTypes");
            CreateTable(
                "dbo.Exam",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        ExamtypeId = c.Int(nullable: false),
                        SubcategoryId = c.Int(nullable: false),
                        Passmark = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.ExamTypes", t => t.ExamtypeId, cascadeDelete: true)
                .ForeignKey("dbo.SubCategory", t => t.SubcategoryId, cascadeDelete: true)
                .Index(t => t.ExamtypeId)
                .Index(t => t.SubcategoryId);
            
            AlterColumn("dbo.ExamTypes", "Name", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Exam", "SubcategoryId", "dbo.SubCategory");
            DropForeignKey("dbo.Exam", "ExamtypeId", "dbo.ExamTypes");
            DropIndex("dbo.Exam", new[] { "SubcategoryId" });
            DropIndex("dbo.Exam", new[] { "ExamtypeId" });
            AlterColumn("dbo.ExamTypes", "Name", c => c.String(maxLength: 250));
            DropTable("dbo.Exam");
            RenameTable(name: "dbo.ExamTypes", newName: "ExamType");
        }
    }
}
