namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangedDurationType : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Package", "Duration", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Package", "Duration", c => c.String());
        }
    }
}
