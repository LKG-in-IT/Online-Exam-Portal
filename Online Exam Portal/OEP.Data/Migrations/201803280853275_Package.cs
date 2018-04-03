namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Package : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Package",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Details = c.String(),
                        Prize = c.Double(nullable: false),
                        Duration = c.String(),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Package");
        }
    }
}
