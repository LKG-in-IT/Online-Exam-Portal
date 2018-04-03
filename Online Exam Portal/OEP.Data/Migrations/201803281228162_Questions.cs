namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Questions : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Question = c.String(maxLength: 250),
                        OptionA = c.String(maxLength: 250),
                        OptionB = c.String(maxLength: 250),
                        OptionC = c.String(maxLength: 250),
                        OptionD = c.String(maxLength: 250),
                        Answer = c.String(maxLength: 250),
                        Status = c.Boolean(nullable: false),
                        CreatedDate = c.DateTime(nullable: false),
                        UpdatedDate = c.DateTime(nullable: false),
                        UserId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Questions");
        }
    }
}
