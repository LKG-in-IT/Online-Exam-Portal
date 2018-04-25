namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedPackageIdToUsers : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "PackageId", c => c.Int(nullable: false, defaultValue: 1));
            AddColumn("dbo.AspNetUsers", "StartDate", c => c.DateTime(nullable: false));
            CreateIndex("dbo.AspNetUsers", "PackageId");
            AddForeignKey("dbo.AspNetUsers", "PackageId", "dbo.Package", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "PackageId", "dbo.Package");
            DropIndex("dbo.AspNetUsers", new[] { "PackageId" });
            DropColumn("dbo.AspNetUsers", "StartDate");
            DropColumn("dbo.AspNetUsers", "PackageId");
        }
    }
}
