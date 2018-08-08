namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedDefaultFieldInLanguageTableMaditUnique : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Languages", "Default", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Languages", "Default", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Languages", new[] { "Default" });
            DropColumn("dbo.Languages", "Default");
        }
    }
}
