namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveDefaultFieldUniqueInLanguageTable : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Languages", new[] { "Default" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Languages", "Default", unique: true);
        }
    }
}
