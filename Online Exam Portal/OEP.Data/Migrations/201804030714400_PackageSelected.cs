namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PackageSelected : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PackageSelected",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PackageId = c.Int(nullable: false),
                        Datefrom = c.DateTime(nullable: false),
                        Dateto = c.DateTime(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Package", t => t.PackageId, cascadeDelete: true)
                .Index(t => t.PackageId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PackageSelected", "PackageId", "dbo.Package");
            DropIndex("dbo.PackageSelected", new[] { "PackageId" });
            DropTable("dbo.PackageSelected");
        }
    }
}
