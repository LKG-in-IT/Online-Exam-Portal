namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserEducationDetailsOne : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserEducationDetails", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserEducationDetails", new[] { "ApplicationUserId" });
            AlterColumn("dbo.Category", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.SubCategory", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.EducationType", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.EducationDetails", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.EducationYearDetails", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Package", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Exam", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.ExamType", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.ExamQuestion", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.Questions", "UserId", c => c.String(nullable: false));
            AlterColumn("dbo.UserEducationDetails", "ApplicationUserId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.UserEducationDetails", "ApplicationUserId");
            AddForeignKey("dbo.UserEducationDetails", "ApplicationUserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            DropColumn("dbo.PackageSelected", "Status");
            DropColumn("dbo.PackageSelected", "CreatedDate");
            DropColumn("dbo.PackageSelected", "UpdatedDate");
            DropColumn("dbo.PackageSelected", "UserId");
            DropColumn("dbo.UserEducationDetails", "Status");
            DropColumn("dbo.UserEducationDetails", "CreatedDate");
            DropColumn("dbo.UserEducationDetails", "UpdatedDate");
            DropColumn("dbo.UserEducationDetails", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserEducationDetails", "UserId", c => c.String());
            AddColumn("dbo.UserEducationDetails", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserEducationDetails", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.UserEducationDetails", "Status", c => c.Boolean(nullable: false));
            AddColumn("dbo.PackageSelected", "UserId", c => c.String());
            AddColumn("dbo.PackageSelected", "UpdatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PackageSelected", "CreatedDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.PackageSelected", "Status", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.UserEducationDetails", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserEducationDetails", new[] { "ApplicationUserId" });
            AlterColumn("dbo.UserEducationDetails", "ApplicationUserId", c => c.String(maxLength: 128));
            AlterColumn("dbo.Questions", "UserId", c => c.String());
            AlterColumn("dbo.ExamQuestion", "UserId", c => c.String());
            AlterColumn("dbo.ExamType", "UserId", c => c.String());
            AlterColumn("dbo.Exam", "UserId", c => c.String());
            AlterColumn("dbo.Package", "UserId", c => c.String());
            AlterColumn("dbo.EducationYearDetails", "UserId", c => c.String());
            AlterColumn("dbo.EducationDetails", "UserId", c => c.String());
            AlterColumn("dbo.EducationType", "UserId", c => c.String());
            AlterColumn("dbo.SubCategory", "UserId", c => c.String());
            AlterColumn("dbo.Category", "UserId", c => c.String());
            CreateIndex("dbo.UserEducationDetails", "ApplicationUserId");
            AddForeignKey("dbo.UserEducationDetails", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
