namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CategoryNew : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Category", "UserId", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Category", "UserId", c => c.Int(nullable: false));
        }
    }
}
