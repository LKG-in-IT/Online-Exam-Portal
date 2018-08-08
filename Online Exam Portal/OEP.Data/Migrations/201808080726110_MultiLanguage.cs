namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MultiLanguage : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.QuestionsLocalized",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(maxLength: 250),
                        OptionA = c.String(maxLength: 250),
                        OptionB = c.String(maxLength: 250),
                        OptionC = c.String(maxLength: 250),
                        OptionD = c.String(maxLength: 250),
                        LanguageId = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Languages", t => t.LanguageId, cascadeDelete: true)
                .Index(t => t.LanguageId);
            
            CreateTable(
                "dbo.Languages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 250),
                        Published = c.Boolean(nullable: false),
                        DisplayOrder = c.Int(nullable: false),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UpdatedDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        UserId = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.QuestionsLocalized", "LanguageId", "dbo.Languages");
            DropIndex("dbo.QuestionsLocalized", new[] { "LanguageId" });
            DropTable("dbo.Languages");
            DropTable("dbo.QuestionsLocalized");
        }
    }
}
