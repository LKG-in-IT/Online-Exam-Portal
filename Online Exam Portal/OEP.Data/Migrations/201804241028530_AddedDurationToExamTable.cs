namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDurationToExamTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exam", "Duration", c => c.Int(nullable: false));
            AddColumn("dbo.Exam", "AllowReAttempts", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exam", "AllowReAttempts");
            DropColumn("dbo.Exam", "Duration");
        }
    }
}
