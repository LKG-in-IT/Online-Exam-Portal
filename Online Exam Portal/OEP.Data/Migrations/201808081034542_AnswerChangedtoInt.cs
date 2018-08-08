namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AnswerChangedtoInt : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Questions", "Answer", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Questions", "Answer", c => c.String(maxLength: 250));
        }
    }
}
