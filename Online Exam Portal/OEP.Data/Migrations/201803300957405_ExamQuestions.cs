namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamQuestions : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ExamTypes", newName: "ExamType");
            CreateTable(
                "dbo.ExamQuestion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ExamId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Exam", t => t.ExamId, cascadeDelete: true)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.ExamId)
                .Index(t => t.QuestionId);
            
            AlterColumn("dbo.ExamType", "Name", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ExamQuestion", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.ExamQuestion", "ExamId", "dbo.Exam");
            DropIndex("dbo.ExamQuestion", new[] { "QuestionId" });
            DropIndex("dbo.ExamQuestion", new[] { "ExamId" });
            AlterColumn("dbo.ExamType", "Name", c => c.String());
            DropTable("dbo.ExamQuestion");
            RenameTable(name: "dbo.ExamType", newName: "ExamTypes");
        }
    }
}
