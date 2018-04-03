namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserEducationDetails : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserEducationDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        EducationDetailsId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.EducationDetails", t => t.EducationDetailsId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.EducationDetailsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserEducationDetails", "EducationDetailsId", "dbo.EducationDetails");
            DropForeignKey("dbo.UserEducationDetails", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserEducationDetails", new[] { "EducationDetailsId" });
            DropIndex("dbo.UserEducationDetails", new[] { "ApplicationUserId" });
            DropTable("dbo.UserEducationDetails");
        }
    }
}
