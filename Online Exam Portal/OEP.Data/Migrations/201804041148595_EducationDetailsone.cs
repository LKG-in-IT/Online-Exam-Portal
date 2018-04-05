namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EducationDetailsone : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EducationDetails", "ApplicationUserID", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Package", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Category", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.SubCategory", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.EducationType", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.EducationDetails", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.EducationYearDetails", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Exam", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.ExamType", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.ExamQuestion", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Questions", "UserId", c => c.String(nullable: false));
            CreateIndex("dbo.EducationDetails", "ApplicationUserID");
            AddForeignKey("dbo.EducationDetails", "ApplicationUserID", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.PackageSelected", "Status");
            DropColumn("dbo.PackageSelected", "CreatedDate");
            DropColumn("dbo.PackageSelected", "UpdatedDate");
            DropColumn("dbo.PackageSelected", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PackageSelected", "UserId", c => c.String());
            AddColumn("dbo.PackageSelected", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PackageSelected", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PackageSelected", "Status", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.EducationDetails", "ApplicationUserID", "dbo.AspNetUsers");
            DropIndex("dbo.EducationDetails", new[] { "ApplicationUserID" });
            AlterColumn("dbo.Questions", "UserId", c => c.String());
            AlterColumn("dbo.ExamQuestion", "UserId", c => c.String());
            AlterColumn("dbo.ExamType", "UserId", c => c.String());
            AlterColumn("dbo.Exam", "UserId", c => c.String());
            AlterColumn("dbo.EducationYearDetails", "UserId", c => c.String());
            AlterColumn("dbo.EducationDetails", "UserId", c => c.String());
            AlterColumn("dbo.EducationType", "UserId", c => c.String());
            AlterColumn("dbo.SubCategory", "UserId", c => c.String());
            AlterColumn("dbo.Category", "UserId", c => c.String());
            AlterColumn("dbo.Package", "UserId", c => c.String());
            DropColumn("dbo.EducationDetails", "ApplicationUserID");
        }
    }
}
