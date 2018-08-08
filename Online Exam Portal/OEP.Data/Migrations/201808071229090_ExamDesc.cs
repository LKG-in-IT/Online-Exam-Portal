namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ExamDesc : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Exam", "Description", c => c.String(maxLength: 250));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Exam", "Description");
        }
    }
}
