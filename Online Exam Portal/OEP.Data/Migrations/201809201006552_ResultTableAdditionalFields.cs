namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ResultTableAdditionalFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Result", "TotalQuestions", c => c.Int(nullable: false));
            AddColumn("dbo.Result", "TotalQuestionsAttended", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Result", "TotalQuestionsAttended");
            DropColumn("dbo.Result", "TotalQuestions");
        }
    }
}
