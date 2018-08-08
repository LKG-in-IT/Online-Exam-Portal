namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class QuestionToLocalizedItemsMapping : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.QuestionsLocalized", "QuestionsId", c => c.Int(nullable: false));
            CreateIndex("dbo.QuestionsLocalized", "QuestionsId");
            AddForeignKey("dbo.QuestionsLocalized", "QuestionsId", "dbo.Questions", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionsLocalized", "QuestionsId", "dbo.Questions");
            DropIndex("dbo.QuestionsLocalized", new[] { "QuestionsId" });
            DropColumn("dbo.QuestionsLocalized", "QuestionsId");
        }
    }
}
